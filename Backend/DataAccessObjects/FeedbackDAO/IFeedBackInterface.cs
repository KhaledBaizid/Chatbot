namespace Backend.DataAccessObjects.FeedbackDAO;

public interface IFeedBackInterface
{
    public Task<string> giveFeedback(int conversationId,string feedback);
}