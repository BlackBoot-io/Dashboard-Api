using BlackBoot.Api.Extentions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


#region Services
builder.Services.AddControllers();
builder.Services.AddBlockBootDbContext(configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region Application
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion

