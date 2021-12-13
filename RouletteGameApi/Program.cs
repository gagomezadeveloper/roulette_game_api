using System.Text;
using RouletteGameApi.Data;
using RouletteGameApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var settings = builder.Configuration.GetSection("RouletteStoreDatabase");
builder.Services.Configure<StoreDatabaseSettings>(settings);
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddScoped<IPlayersService, PlayersService>();
builder.Services.AddScoped<IRoulettesService, RoulettesService>();
builder.Services.AddSingleton<PlayersService>();
builder.Services.AddSingleton<RoulettesService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                    .GetBytes(builder.Configuration.GetSection("RouletteStoreDatabase:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateLifetime = true, //In order to validate expired tokens
                    ClockSkew = TimeSpan.Zero //In order to remove the time gap
                };
            });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
