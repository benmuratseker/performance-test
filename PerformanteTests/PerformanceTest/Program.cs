using Microsoft.VisualBasic;
using NBomber.CSharp;

using var httpClient = new HttpClient();

var scenario = Scenario.Create("get_product_list", async context =>
{
    var response = await httpClient.GetAsync("https://localhost:7213/product?category=boots");

    return response.IsSuccessStatusCode
        ? Response.Ok()
        : Response.Fail();
})
.WithoutWarmUp()
.WithLoadSimulations(
    Simulation.Inject(
        rate: 100, 
        interval:TimeSpan.FromSeconds(1), 
        during: TimeSpan.FromSeconds(30)));
        
NBomberRunner.RegisterScenarios(scenario).Run();

//before running this app
//run the api with a configuration arg of Release mode
//dotnet run -c Release