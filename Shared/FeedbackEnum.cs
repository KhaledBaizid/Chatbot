namespace Shared;

public enum FeedbackEnum
{
    Posistive=1,
    Negative=2,
    Neutral=3
    
}
public static class FeedbackMessages
{
    public const string PositiveMessage = "Thank you for your positive feedback!";
    public const string NegativeMessage = "We're sorry to hear about your negative feedback.";
    public const string NeutralMessage = "Thank you for your feedback!";

}