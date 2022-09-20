using Dapr.Client;

// create web-app
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IFineCalculator, HardCodedFineCalculator>();

// builder.Services.AddHttpClient();
// builder.Services.AddSingleton<VehicleRegistrationService>();

var daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3602";
builder.Services.AddSingleton<VehicleRegistrationService>(_ => 
    new VehicleRegistrationService(
        DaprClient.CreateInvokeHttpClient(
            "vehicleregistrationservice", $"http://localhost:{daprHttpPort}"
        )
    )
);

builder.Services.AddControllers();

var app = builder.Build();

// configure web-app
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// configure routing
app.MapControllers();

// let's go!
app.Run();