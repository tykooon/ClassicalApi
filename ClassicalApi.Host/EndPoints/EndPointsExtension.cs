using ClassicalApi.Core.Infrastructure;
using ClassicalApi.Core.Models;
using ClassicalApi.Host.Authentication;
using ClassicalApi.Host.Models;
using Microsoft.AspNetCore.Mvc;
namespace ClassicalApi.Host.EndPoints;

public static class EndPointsExtension
{
    public static void AddCommandEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/composers").AddEndpointFilter<ApiKeyEndpointFilter>();

        group.MapPost("", async (HttpContext ctx) =>
        {
            var composer = await ctx.Request.ReadFromJsonAsync<Composer>();
            if (composer != null)
            {
                using var context = app.Services.CreateScope();
                var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
                var newId = repo.AddNew(composer);
                return Results.Ok(newId);
            }
            return Results.BadRequest("Failed to Create");
        });

        group.MapPut("", async (HttpContext ctx) =>
        {
            var composer = await ctx.Request.ReadFromJsonAsync<Composer>();
            if (composer != null)
            {
                using var context = app.Services.CreateScope();
                var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
                repo.Update(composer);
            }
        });

        group.MapDelete("{id:int}", (int id) =>
        {
            using var context = app.Services.CreateScope();
            var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
            repo.Delete(id);
        });

        group.MapDelete("{id:int}/portrait", (int id) =>
        {
            using var context = app.Services.CreateScope();
            var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
            var composer = repo.GetById(id);
            if (composer != null)
            {
                repo.DeletePortrait(id);
            }
        });

        group.MapPost("{id:int}/portrait", async (int id, HttpContext ctx) =>
        {
            var load = await ctx.Request.ReadFromJsonAsync<AddPortraitRequest>();
            if (load != null)
            {
                var data = Convert.FromBase64String(load.ImageBase64);
                using var context = app.Services.CreateScope();
                var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
                repo.AddPortrait(id, data);
            }
        });

        app.MapDelete("/medialinks/{id:int}", (int id) =>
        {
            using var context = app.Services.CreateScope();
            var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
            var media = repo.GetMediaLinkById(id);
            if (media != null)
            {
                repo.DeleteMedia(id);
            }
        });

        app.MapPost("/medialinks", (AddMediaLinkRequest load, HttpContext ctx) =>
        {
            if (load != null)
            {
                using var context = app.Services.CreateScope();
                var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
                var data = new MediaLink()
                {
                    Title = load.Title,
                    Url = load.Url,
                };
                var composers = repo.GetAll().Where(c => load.ComposerIds.Contains(c.Id)).ToList();
                if (composers.Count == 0)
                {
                    return Results.BadRequest("No composers assigned to media");
                }
                data.Composers = composers;
                var result = repo.AddNewMedia(data);
                return Results.Ok(result);
            }
            return Results.BadRequest("Unable to parse request.");
        });
    }

    public static void AddQueryEndpoints(this WebApplication app)
    {
        app.MapGet("/", () =>
    new Dictionary<string, string>() { ["api-name"] = "classical-api", ["version"] = "0.0.1" });

        app.MapGet("/composers", () =>
        {
            using var context = app.Services.CreateScope();
            var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
            return repo.GetAll();
        });

        app.MapGet("/composers/{id:int}", (int id) =>
        {
            using var context = app.Services.CreateScope();
            var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
            return repo.GetById(id);
        });

        app.MapGet("/composers/{name}", (string name) =>
        {
            using var context = app.Services.CreateScope();
            var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
            return repo.GetByLastName(name);
        });

        app.MapGet("/composers/{id:int}/portrait", (int id) =>
        {
            using var context = app.Services.CreateScope();
            var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
            return Convert.ToBase64String(repo.GetPortrait(id) ?? []);
        });

        app.MapGet("/composers/search", (string query) =>
        {
            using var context = app.Services.CreateScope();
            var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
            return repo.Search(query);
        });

        app.MapGet("/medialinks", ([FromQuery] int composerId) =>
        {
            using var context = app.Services.CreateScope();
            var repo = context.ServiceProvider.GetRequiredService<IComposerRepository>();
            return repo.GetMediaLinks(composerId);
        });
    }
}
