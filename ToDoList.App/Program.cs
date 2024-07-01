using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ToDoList.App;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// configura injecció de serveis HTTP en Blazor. l'escope (entorn), es crea una nova instància de
// HttpClient, una direcció Base, creant una nova instància de URI, on host es troba en una BDcontext de la memoriaRAM
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<LocalStorageAccessor>();

await builder.Build().RunAsync();
