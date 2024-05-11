using ClassicalApi.Core.Infrastructure;
using ClassicalApi.Host;
using ClassicalApi.Host.EndPoints;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("private-settings.json");

builder.Services.AddConfiguredDbContext(builder.Configuration);

builder.Services.AddResponseCaching();

builder.Services.AddCors(
    options => options.AddPolicy("CorsPolicy", opt => opt
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()));

builder.Services.AddScoped<IComposerRepository, ComposerRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseSwagger();
app.UseSwaggerUI();

app.UseForwardedHeaders(new ForwardedHeadersOptions()
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseResponseCaching();

app.AddQueryEndpoints();
app.AddCommandEndpoints();

app.Run();