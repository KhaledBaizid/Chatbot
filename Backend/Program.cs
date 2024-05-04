using Backend.DataAccessObjects.Admin;
using Backend.DataAccessObjects.ChatSessionDAO;
using Backend.DataAccessObjects.LoginDAO;
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
builder.Services.AddScoped<ICredentialsInterface, CredentialsImplementation>();
builder.Services.AddScoped<ILoginInterface, LoginImplementation>();
builder.Services.AddScoped<IPDFInterface, PDFImplementation>();
builder.Services.AddScoped<IChatSessionInterface, ChatSessionImplementation>();

builder.Services.AddSingleton<IPromptProvider,PromptProvider>();
builder.Services.AddSingleton<IEmbeddingProvider,EmbeddingProvide>(provider =>
{
    // Retrieve API_KEY from configuration or wherever it's stored
    var apiKey = "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";
    return new EmbeddingProvide(apiKey);

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