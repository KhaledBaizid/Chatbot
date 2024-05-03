namespace Backend.Services;

public interface IEmbeddingProvider
{
    public LangChain.Providers.OpenAI.Gpt35Turbo16KModel GetModel();  
}