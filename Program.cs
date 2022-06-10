var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<IUserInfoRepository, UserInfoRepository>();
//builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<SetValidator>());

FirebaseApp.Create(new AppOptions
{
  Credential = GoogleCredential.FromFile(@"./Firebase/project-dlc-firebase-adminsdk.json")
});

var ValidIssuer = builder.Configuration.GetSection("Jwt:Firebase:ValidIssuer");
var ValidAudience = builder.Configuration.GetSection("Jwt:Firebase:ValidAudience");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
  opt.Authority = ValidIssuer.Value;
  opt.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidIssuer = ValidIssuer.Value,
    ValidAudience = ValidAudience.Value,
    ValidateIssuerSigningKey = true,
  };
});

var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/", () =>
{
  return new { data = "working" };
});

#region GET

app.MapGet("/api/HPM/players", (IPlayerService playerService) =>
{
  return playerService.GetPlayers();
});

#endregion

#region POST

#endregion

#region UPDATE

#endregion

#region DELETE

#endregion


app.Run("http://0.0.0.0:3000");

