using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Usuário API",
        Version = "v1",
        Description = "Serviço de usuários da plataforma BankMore"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Usuário API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.MapGet("/", () => "Usuário API rodando!").WithOpenApi();

app.Run();
