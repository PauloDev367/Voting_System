using System.Text.Json.Serialization;
using VotingSystem.Extensions;
using VotingSystem.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
});

builder.Services.AddSignalR();
builder.Services.AddSignalR(cfg=> {
    cfg.EnableDetailedErrors = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.ConfigureDbContext();
builder.ConfigureAppDependencies();
builder.ConfigureIdentity();
builder.ConfigureAppCors();

builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseCors();  

app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<VotingHub>("/votes");
app.MapHub<AdminVotingHub>("/votes/admin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

