using System.Collections.Immutable;
using Backend.EFCData;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using Shared;

namespace Backend.DataAccessObjects.ConversationDAO;

public class ConversationDAO : IConversationInterface
{
    private readonly DataContext _systemContext;
    private readonly IEmbeddingProvider _embeddingProvider;
    private readonly ILlmChainProvider _promptProvider;

    public ConversationDAO(DataContext systemContext, IEmbeddingProvider embeddingProvider, ILlmChainProvider promptProvider)
    {
        _systemContext = systemContext;
        _embeddingProvider = embeddingProvider;
        _promptProvider = promptProvider;
    }

    public  async Task<Chat_session> GetConversationByChatSessionIdAsync(int chatSessionId,string question,int timeOutSeconds)
    {
        try
        { 
            var embeddingQuestion = await _embeddingProvider.GetModel().EmbedQueryAsync(question);
            Vector embeddingVectorQuestion = new Vector(embeddingQuestion);
            //////////////////////////////////////////////////////////////
            string chunks = "";
            chunks =  await GetChunksByVector(embeddingVectorQuestion);
            
            string answer = "";
            
            if (chunks.Equals("") )
            {
                answer = "I am sorry, i can not find the answer to your question.";
              
            }
            else
            { 
                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(timeOutSeconds));
                var getAnswerTask = _promptProvider.GetMode().Llm.GenerateAsync(" this is the context:"+chunks+" .I will ask you but not find out of context, and this the question: "+question); 
              
                var completedTask = await Task.WhenAny(getAnswerTask , timeoutTask);
                
                if (completedTask == getAnswerTask)
                {
                    // Save changes completed within the timeout
                    await getAnswerTask;
                    answer = getAnswerTask.Result.ToString();
                }
                else
                {
                    // Timeout occurred
                    answer = "Getting answer took too long. Please try to ask again.";
                }
              
            }
              
            var conversation = new Conversation
            {
                ChatSessionId = chatSessionId,
                Question = question,
                Answer = answer,
                ConversationTime= DateTime.Now.ToUniversalTime(),
                Feedback = FeedbackEnum.Neutral.ToString()
            };
            await _systemContext.Conversations.AddAsync(conversation);
            await _systemContext.SaveChangesAsync();
           
        //    }
            var chatSession =  await _systemContext.Chat_sessions.Include(c => c.Conversations.OrderBy(con=>con.Id)).OrderByDescending(c=>c.Id).FirstAsync(c => c.Id == chatSessionId);
            return chatSession;
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        throw new NotImplementedException();
    }
    
    public async Task<string> GetChunksByVector(Vector vector)
    { string chunksText = "";
        if (_systemContext.Database.IsInMemory())
        {
            var chunks =  _systemContext.Chunks
                
                .AsEnumerable()
                .Select(x => new { Entity = x, Distance = CosineDistance(x.Embedding, vector) })

                .Where(x => x.Distance<0.22)
                .OrderBy(x => x.Distance)
                .Take(10)
                .ToList();
           
           // Console.WriteLine("-------------------------------------------------------------------");
          //  Console.WriteLine(chunks.ToArray());

            foreach (var chunk in chunks)
            { 
                // Console.WriteLine("-------------------------------------------------------------------");
              //  Console.WriteLine(chunk.Entity.Text);
                chunksText += chunk.Entity.Text;
            }
        }
        else
        {
            var chunks = await _systemContext.Chunks
                  .Select(x=>new{Entity = x, Distance = x.Embedding!.CosineDistance(vector) })
               
                .Where(x => x.Distance < 0.22)
                .OrderBy(x => x.Distance)
                .Take(10)
                .ToListAsync();
           
          //  Console.WriteLine("-------------------------------------------------------------------");
            foreach (var chunk in chunks)
            { 
                //Console.WriteLine("-------------------------------------------------------------------");
               // Console.WriteLine(chunk.Entity.Text);
                chunksText += chunk.Entity.Text;
            }
        }
     
     
        return chunksText;
    }
    
    public static double CosineDistance(Vector vector1, Vector vector2)
    {
        if (vector1 == null || vector2 == null)
        {
            throw new ArgumentNullException("Vectors cannot be null");
        }

        // Calculate dot product
        double dotProduct = DotProduct(vector1, vector2);

        // Calculate magnitudes
        double magnitude1 = Magnitude(vector1);
        double magnitude2 = Magnitude(vector2);

        // Calculate cosine distance
        return 1- ( dotProduct / (magnitude1 * magnitude2));
    }

    private static double DotProduct(Vector vector1, Vector vector2)
    {
        if (vector1.ToArray().Length!= vector2.ToArray().Length)
        {
            throw new ArgumentException("Vectors must have the same length");
        }

        double dotProduct = 0;
        for (int i = 0; i < vector1.ToArray().Length; i++)
        {
            dotProduct += vector1.ToArray()[i] * vector2.ToArray()[i];
        }
        return dotProduct;
    }

    private static double Magnitude(Vector vector)
    {
        double sumOfSquares = 0;
        foreach (double component in vector.ToArray())
        {
            sumOfSquares += component * component;
        }
        return Math.Sqrt(sumOfSquares);
    }

    public Task<List<Conversation>> GetConversationsByFeedbackAndByDateAsync(DateTime startDate, DateTime endDate, string feedback)
    {
        try
        {
            var startDateToUniversalTime = startDate.ToUniversalTime();
            var endDateToUniversalTime = endDate.ToUniversalTime();
            if (startDateToUniversalTime > endDateToUniversalTime)
            {
                throw new Exception("Start date cannot be greater than end date");
            }
            var conversations = _systemContext.Conversations.Where(c => c.Feedback == feedback && c.ConversationTime >= startDateToUniversalTime && c.ConversationTime <= endDateToUniversalTime).ToList();
            return Task.FromResult(conversations);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        throw new NotImplementedException();
    }
}