using System.Collections.Immutable;
using Backend.EFCData;
using Backend.Services;
using LangChain.Memory;
using LangChain.Prompts;
using LangChain.Schema;
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
    private readonly ILlmChainProvider _llmChainProvider;
   List<string> l = new List<string>();
    public ConversationDAO(DataContext systemContext, IEmbeddingProvider embeddingProvider, ILlmChainProvider llmChainProvider)
    {
        _systemContext = systemContext;
        _embeddingProvider = embeddingProvider;
        _llmChainProvider = llmChainProvider;
    }

    public  async Task<Chat_session> GetConversationByChatSessionIdAsync(int chatSessionId,string question,int timeOutSeconds)
    {
        try
        { 
            // get the embedding of the question
            var embeddingQuestion = await _embeddingProvider.GetModel().EmbedQueryAsync(question);
         
            Vector embeddingVectorQuestion = new Vector(embeddingQuestion);
           
            string Context = "";
           
            // find the context of the question by calling the GetChunksByVector method to get the chunks that are similar to the question
            Context =  await GetChunksByVector(embeddingVectorQuestion);
            
            string answer = "";
            
            if (Context.Equals("") )
            {
                answer = "I am sorry, i can not find the answer to your question.";
              
            }
            else
            {  var timeoutTask = Task.Delay(TimeSpan.FromSeconds(timeOutSeconds));
               
              // use llmChain provider to generate the answer
                var getAnswerTask = _llmChainProvider.GetMode().Llm.GenerateAsync(" Use the following pieces of context to answer the question at the end.\n     If the answer is not in context then just say that you don't know, don't try to make up an answer. this is the context:\n "+Context+".\n . and this the question: \n"+question);
               
                Console.WriteLine(getAnswerTask.Result.Usage.Time.TotalSeconds);
              
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

    private async Task<string> GetChunksByVector(Vector vector)
    { 
        string chunksText = "";
       
            var chunks = await _systemContext.Chunks
                .Select(x=>new{Entity = x, Distance = x.Embedding!.CosineDistance(vector) })
                .Where(x => x.Distance < 0.25)
                .OrderBy(x => x.Distance)
                .Take(15)
                .ToListAsync();
            foreach (var chunk in chunks)
            {  
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