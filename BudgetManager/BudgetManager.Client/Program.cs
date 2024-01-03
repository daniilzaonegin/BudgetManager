using BudgetManager.Client;
using BudgetManager.Client.Services;
using BudgetManager.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddFluentUIComponents();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
Uri backendApiUri = builder.Configuration.GetValue<Uri>("backendApiUri") ?? throw new ApplicationException("backendUri is not configured");
builder.Services.AddHttpClient<IApiClient,ApiClient>(c => c.BaseAddress = backendApiUri);
await builder.Build().RunAsync();
