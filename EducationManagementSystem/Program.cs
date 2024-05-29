using EducationManagementSystem.Infrastructure;
using EducationManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TimestampInterceptor>();
builder.Services.AddDbContext<AppDbContext>((provider, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database") ?? throw new Exception());
    options.AddInterceptors(provider.GetRequiredService<TimestampInterceptor>());
});
builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<MarkService>();
builder.Services.AddScoped<ScheduleService>();
builder.Services.AddSingleton<ImportService>();
builder.Services.AddSingleton<KeycloakService>();
builder.Services.AddSingleton<RequestTimeMiddleware>();
builder.Services.AddHttpClient();
builder.Services.AddQuartz(x =>
{
    x.UsePersistentStore(c =>
        c.UsePostgres(builder.Configuration.GetConnectionString("Quartz") ?? throw new Exception()));
});
builder.Services.AddQuartzHostedService();
var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
await dbContext.Database.EnsureCreatedAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<RequestTimeMiddleware>();
app.MapControllers();

await app.RunAsync();