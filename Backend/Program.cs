using Backend.DataAccessObjects.Admin;
using Backend.DataAccessObjects.AuthenticationDAO;
using Backend.DataAccessObjects.ChatSessionDAO;
using Backend.DataAccessObjects.ConversationDAO;
using Backend.DataAccessObjects.FeedbackDAO;
using Backend.DataAccessObjects.AuthenticationDAO;
using Backend.DataAccessObjects.PdfDAO;
using Backend.EFCData;
using Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<ICredentialsInterface, CredentialsDAO>();
builder.Services.AddScoped<IAuthenticationInterface, AuthenticationDao>();
builder.Services.AddScoped<IPDFInterface, PDFDAO>();
builder.Services.AddScoped<IChatSessionInterface, ChatSessionDAO>();
builder.Services.AddScoped<IConversationInterface, ConversationDAO>();
builder.Services.AddScoped<IFeedBackInterface, FeedbackDAO>();

builder.Services.AddSingleton<ILlmChainProvider,LlmChainProvider>();
builder.Services.AddSingleton<IEmbeddingProvider,EmbeddingProvider>(provider =>
{
    // Retrieve API_KEY from configuration or wherever it's stored
    var apiKey = "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";
    return new EmbeddingProvider(apiKey);

});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();