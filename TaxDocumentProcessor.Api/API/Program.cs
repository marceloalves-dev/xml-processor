using TaxDocumentProcessor.Application;
using TaxDocumentProcessor.API.Middleware;
using TaxDocumentProcessor.Domain.Repositories;
using TaxDocumentProcessor.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddResponseCompression(opts => opts.EnableForHttps = true);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "XML Processor",
        Version = "v1",
        Description = "REST API for processing Brazilian fiscal documents (NF-e, CT-e, NFS-e) from XML."
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var repository = scope.ServiceProvider.GetRequiredService<INotaFiscalRepository>();
    await repository.EnsureIndexesAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "XML Processor v1"));
}

app.UseExceptionHandler();
app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
