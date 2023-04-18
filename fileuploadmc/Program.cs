using fileuploadmc.GrpcControllers;
using fileuploadmc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
//builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddScoped<IFileHandler, FileHandlerHelper>();
builder.Services.AddScoped<IExcelReader, ExcelReader>();
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    // Communication with gRPC endpoints must be made through a gRPC client.
    // To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909
    endpoints.MapGrpcService<GreeterService>();

});
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapControllers();


app.Run();
