using System.Net.Http.Json;
using Shared;

namespace Frontend.Services;

public class ChatSessionService :   IChatSessionService
{
    private readonly HttpClient httpClient;

    public ChatSessionService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    
    public async Task<int> StartChatSessionAsync()
    {
        try
        {
            

            var response = await httpClient.PostAsJsonAsync("/ChatSession", new { });

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return int.Parse(responseContent);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception(errorContent);
            }
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public Task<Chat_session> getChatSessionById(int chatSessionId)
    {
        throw new NotImplementedException();
    }
}