using NotinoAssignment.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();

var app = builder.Build();
app.SetupPipeline();

app.Run();