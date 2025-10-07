using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using LearningApp.Client;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "LearningAppTheme";
    options.Duration = TimeSpan.FromDays(365);
});
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<LearningApp.Client.SvgDBService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient("LearningApp.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("LearningApp.Server"));
builder.Services.AddScoped<LearningApp.Client.SecurityService>();
builder.Services.AddScoped<AuthenticationStateProvider, LearningApp.Client.ApplicationAuthenticationStateProvider>();
var host = builder.Build();
await host.RunAsync();