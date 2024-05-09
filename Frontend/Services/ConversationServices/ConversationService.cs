using System.Net.Http.Json;
using Shared;

namespace Frontend.Services.ConversationServices;

public class ConversationService : IConversationService
{
    
    private readonly HttpClient httpClient;

    public ConversationService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    

    public async Task<Chat_session> GetConversationByChatSessionId(int chatSessionId, string question)
    {
        var response = await httpClient.GetFromJsonAsync<Chat_session>($"/Conversation?chatSessionId={chatSessionId}&question={question}");
        return response;
    }
}