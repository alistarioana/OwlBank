using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using OwlBank;
using OwlBank.Repository;
using OwlBank.Auth.Contracts.Interfaces;
using OwlBank.Services.Implementations;
using Microsoft.AspNetCore.Http.Features;
using OwlBank.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition =
        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddFluentValidation();
builder.Services.AddDependencies();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctx => { 
        ctx.ProblemDetails.Instance= $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";

        ctx.ProblemDetails.Extensions.TryAdd("requestId", ctx.HttpContext.TraceIdentifier);

        var activity = ctx.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        ctx.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);


    };
});

builder.Services.AddExceptionHandler<ProblemExceptionHandler>();
builder.AddNpgsqlDbContext<OwlBankDBContext>("OwlBankDb");

builder.Services.AddAuthServices(builder.Configuration);
builder.Services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Events ??= new JwtBearerEvents();
    options.Events.OnChallenge = async context =>
    {
        context.HandleResponse();

        context.Response.StatusCode =
            StatusCodes.Status401Unauthorized;

        var problem = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Detail = "Sesiunea ta a expirat. Te rugam sa te reconectezi.",
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.2",
            Instance = $"{context.Request.Method} {context.Request.Path}"
        };

        await context.Response.WriteAsJsonAsync(problem);
    };
});
// Register HttpClient for Auth service and the HTTP-based ILoginService implementation
builder.Services.AddHttpClient("Auth", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuthService:BaseUrl"] ?? "https://localhost:5002");
});

builder.Services.AddScoped<ILoginService, AuthHttpLoginService>();

var app = builder.Build();

app.UseExceptionHandler();
app.MapDefaultEndpoints();

app.UseSwagger(); 
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OwlBank API v1");
    c.EnablePersistAuthorization();  
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OwlBankDBContext>(); 
    db.Database.Migrate();
}

app.Run();