using StoryBook.Components;

var builder = WebApplication.CreateBuilder(args);

// Add localization services
builder.Services.AddLocalization();
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
// BootstrapBlazor
builder.Services.AddBootstrapBlazor();

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
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
