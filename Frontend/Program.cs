using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend;
using Frontend.Authentication;
using Frontend.Services;
using Frontend.Services.ChatSessionServices;
using Frontend.Services.ConversationServices;
using Frontend.Services.CredentalServices;
using Frontend.Services.FeedbackServices;
using Frontend.Services.LoginServices;
using Frontend.Services.UploadFileServices;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
// builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri("http://localhost:5157/")});
builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri("https://khaledchatbotbackend.azurewebsites.net/")});

builder.Services.AddScoped<AuthenticationStateProvider, SimpleAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthManager, AuthManagerImpl>();
builder.Services.AddScoped<ICredentialService, CredentialService>();
builder.Services.AddScoped<IUploadFileService, UploadFileService>();
builder.Services.AddScoped<IChatSessionService, ChatSessionService>();
builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddSweetAlert2();
builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();