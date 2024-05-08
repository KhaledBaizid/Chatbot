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
    private readonly IPromptProvider _promptProvider;

    public ConversationDAO(DataContext systemContext, IEmbeddingProvider embeddingProvider, IPromptProvider promptProvider)
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
            {  var timeoutTask = Task.Delay(TimeSpan.FromSeconds(timeOutSeconds));
                var getAnswerTask = _promptProvider.GetModeLlmChain().Llm.GenerateAsync(" this is the context:"+chunks+" .I will ask you but not find out of context, and this the question: "+question); 
              //  var promptAnswer= await _promptProvider.GetModeLlmChain().Llm.GenerateAsync(" this is the context:"+chunks+" .I will ask you but not find out of context, and this the question: "+question);
              
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
               
              //  answer = promptAnswer.ToString();
              
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
            var chatSession = await _systemContext.Chat_sessions.Include(c => c.Conversations.OrderBy(con=>con.Id)).FirstAsync(c => c.Id == chatSessionId);
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
       
            var chunks = await _systemContext.Chunks
                .Select(x=>new{Entity = x, Distance = x.Embedding!.CosineDistance(vector) })
                .Where(x => x.Distance < 0.20)
                .OrderBy(x => x.Distance)
                .Take(10)
                .ToListAsync();
            foreach (var chunk in chunks)
            {  Console.WriteLine(chunk.Distance);
                chunksText += chunk.Entity.Text+" ";
            }
        return chunksText;
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