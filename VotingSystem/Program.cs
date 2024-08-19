using VotingSystem.Extensions;
using VotingSystem.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});



builder.Services.AddSignalR();
builder.Services.AddSignalR(cfg=> {
    cfg.EnableDetailedErrors = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureDbContext();
builder.ConfigureAppDependencies();
builder.ConfigureIdentity();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.MapHub<VotingHub>("/votes");
app.MapControllers();

app.Run();
