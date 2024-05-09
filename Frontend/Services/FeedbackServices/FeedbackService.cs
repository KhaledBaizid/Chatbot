using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Shared;

namespace Frontend.Services.FeedbackServices;

public class FeedbackService : IFeedbackService
{
    private readonly HttpClient httpClient;

    public FeedbackService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<string> GiveFeedbackAsync(int? conversationId, FeedbackEnum feedback)
    {
        //   var response = await httpClient.PutAsJsonAsync<string>($"/Feedback?conversationId={conversationId}&feedback={feedback}");
        
        try
        {
            // Construct the URL with the conversationId and feedback parameters
            var url = $"/Feedback?conversationId={conversationId}&feedback={feedback}";

            // Send the HTTP PUT request
            var response = await httpClient.PutAsync(url, null);

            // Check if the request was successful
            response.EnsureSuccessStatusCode();

            // Return the response as a string
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            // Handle the exception here. You might want to log it or take other actions.
            // For now, rethrowing the exception is used for demonstration purposes.
            throw new Exception("Error while giving feedback", ex);
        }
    }

    public Task<List<Conversation>> GetConversationsByFeedbackAndByDateAsync(DateTime startDate, DateTime endDate,
        string feedback)
    {
        throw new NotImplementedException();
    }
}