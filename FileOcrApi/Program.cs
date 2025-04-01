using Ocr;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<OcrOptions>(builder.Configuration.GetSection(OcrOptions.Ocr));

//Configure dependency injection
builder.Services.AddScoped<PdfOcrProcessor>();
builder.Services.AddScoped<ImageOcrProcessor>();
builder.Services.AddScoped<OcrProcessor>();

builder.Logging.AddSimpleConsole(i => i.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Enabled);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();