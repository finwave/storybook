using StoryBook.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
// BootstrapBlazor
builder.Services.AddBootstrapBlazor();
// Add localization services
builder.Services.AddLocalization();
builder.Services.AddControllers();

// Register OpenAI service as singleton
builder.Services.AddSingleton<AIService>();

// Add IHttpClientFactory to the container and set the name of the factory
// to "StoryBookAPI". The base address for API requests is also set.
var serverUri = builder.Configuration["ServerUri"] ?? throw new InvalidOperationException("ServerUri is not set");
builder.Services.AddHttpClient("StoryBookAPI", httpClient =>
{
  httpClient.BaseAddress = new Uri(serverUri);
});

var app = builder.Build();

string[] supportedCultures = ["en-US", "fi-FI"];
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
