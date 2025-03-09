using Microsoft.VisualBasic;
using NBomber.CSharp;
using NBomber.Http;
using NBomber.Http.FSharp;

using var httpClient = new HttpClient();

var scenario = Scenario.Create("get_product_list", async context =>
{
    #region Request test
    // var response = await httpClient.GetAsync("https://localhost:7213/product?category=boots");
    //
    // return response.IsSuccessStatusCode
    //     ? Response.Ok()
    //     : Response.Fail();
    #endregion
    
    var request = Http.createRequest("GET","https://localhost:7213/product?category=boots");
    return await Http.send(httpClient, request);
})
.WithoutWarmUp()
.WithLoadSimulations(
    Simulation.Inject(
        rate: 100, 
        interval:TimeSpan.FromSeconds(1), 
        during: TimeSpan.FromSeconds(30)));
        
// NBomberRunner.RegisterScenarios(scenario).Run();
// NBomberRunner.RegisterScenarios(scenario).("product_service").Run();
NBomberRunner.RegisterScenarios(scenario).WithWorkerPlugins(new HttpMetricsPlugin()).Run();

//before running this app
//run the api with a configuration arg of Release mode
//dotnet run -c Release