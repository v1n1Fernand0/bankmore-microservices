using BankMore.Transferencia.Infrastructure.Config;
using BankMore.Transferencia.Application.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Transferência API",
        Version = "v1",
        Description = "Serviço de transferências bancárias da plataforma BankMore"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe: Bearer {seu_token_jwt}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme, Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.Authority = builder.Configuration["Jwt:Authority"];
        opt.Audience = builder.Configuration["Jwt:Audience"];
        opt.RequireHttpsMetadata = false; 
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransferenciaInfrastructure(builder.Configuration);
builder.Services.AddTransferenciaApplication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Transferência API v1");
        options.RoutePrefix = string.Empty; 
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => Results.Ok("Transferência API rodando!"))
   .WithOpenApi();

app.MapControllers();

app.Run();
