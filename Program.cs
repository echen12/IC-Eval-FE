using IC_Eval_FE;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);


builder.Services.AddSingleton(new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5050/data") // Fallback URL if config is missing
});


builder.Services.AddMudServices();


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();