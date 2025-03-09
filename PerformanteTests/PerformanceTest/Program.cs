using NBomber.CSharp;
using NBomber.Http;
using NBomber.Http.CSharp;
using NBomber.Data;
using NBomber.Data.CSharp;

using var httpClient = new HttpClient();
IDataFeed<string> categories = DataFeed.Random(new List<string>{"all", "boots", "equip", "kayak"});

var scenario = Scenario.Create("get_product_list", async context =>
{
    #region Request test
    // var response = await httpClient.GetAsync("https://localhost:7213/product?category=boots");
    //
    // return response.IsSuccessStatusCode
    //     ? Response.Ok()
    //     : Response.Fail();
    #endregion

    #region Http Request Nbomber.Http
    // var request = Http.CreateRequest("GET","https://localhost:7213/product?category=boots");
    // return await Http.Send(httpClient, request);
    #endregion
    
    #region Data Nbomber.Data
    var category = categories.GetNextItem(context.ScenarioInfo);
    var request = Http.CreateRequest("GET",$"https://localhost:7213/product?category={category}");
    return await Http.Send(httpClient, request);
    #endregion
})
.WithoutWarmUp()
.WithLoadSimulations(
    // Simulation.Inject(
    //     // rate: 100, 
    //     rate: 400, 
    //     interval:TimeSpan.FromSeconds(1), 
    //     during: TimeSpan.FromSeconds(30))

    #region  Additional requests
    Simulation.RampingInject(rate:250,
        interval:TimeSpan.FromSeconds(1),
        during: TimeSpan.FromSeconds(30)),
    Simulation.RampingInject(rate: 100,
            interval: TimeSpan.FromSeconds(1),
            during: TimeSpan.FromSeconds(30))

    #endregion
);
        
// NBomberRunner.RegisterScenarios(scenario).Run();
// NBomberRunner.RegisterScenarios(scenario).("product_service").Run();
NBomberRunner.RegisterScenarios(scenario).WithWorkerPlugins(new HttpMetricsPlugin()).Run();

//before running this app
//run the api with a configuration arg of Release mode
//dotnet run -c Release