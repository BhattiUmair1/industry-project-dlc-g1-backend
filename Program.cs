

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


DateTime timerStarted = DateTime.Now;


// timer
app.MapGet("api/HPM/timer/{request}", async (string request) =>
{
    if (request == "start")
    {
        DateTime timerStarted = DateTime.Now;
        return Results.Ok(new { timer = "started" });
    }
    else if (request == "reset")
    {
        var timerReset = timerStarted.Subtract(timerStarted);
        return Results.Ok(new { timer = "Resetted", resetTime = timerReset });

    }
    else if (request == "time")
    {
        TimeSpan time = timerStarted.Subtract(DateTime.Now);
        return Results.Ok(new { currentTime = time });
    }
    return Results.BadRequest(new { request = "Please make sure the route is correct." });
});


// get current
app.MapGet("api/HPM/team/{team}/score", async (string team, int currentScore, IPlayerService service) =>
{
    if (team == "thuis")
    {

    }
    else if (team == "uit") {

    }

    return Results.BadRequest(new { request = "Please make sure that team name is a string and the score is of the type int." });

});

// create a set
//app.MapPost("/set", [Authorize] async (IValidator<Set> validator, IValidator<Studyset> validator1, Set set, ISetService service) =>
//{
//    set.ReviewedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
//    var result = validator.Validate(set);
//    foreach (var item in set.Studyset)
//    {
//        var result1 = validator1.Validate(item);
//        if (!result1.IsValid)
//        {
//            return Results.BadRequest(result1.Errors);
//        }
//    }
//    if (!result.IsValid)
//    {
//        return Results.BadRequest(result.Errors);
//    }
//    var addedSet = await service.AddSetAsync(set);
//    return Results.Created("/addset/{UserId}", new { respond = addedSet });
//});
//// edit a set
//app.MapPut("/set", [Authorize] async (IValidator<Set> validator, IValidator<Studyset> validator1, Set set, ISetService service) =>
//{
//    set.ReviewedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
//    var result = validator.Validate(set);
//    foreach (var item in set.Studyset)
//    {
//        var result1 = validator1.Validate(item);
//        if (!result1.IsValid)
//        {
//            return Results.BadRequest(result1.Errors);
//        }
//    }
//    if (!result.IsValid)
//    {
//        return Results.BadRequest(result.Errors);
//    }
//    var editedSet = await service.UpdateSetAsync(set);
//    return Results.Ok(editedSet);

//});
//// delete a set
//app.MapDelete("/set/{DocRef}", async (string DocRef, ISetService service) =>
//{
//    if (DocRef.Length < 32 || string.IsNullOrEmpty(DocRef))
//    {
//        return Results.BadRequest("Document reference is invalid");
//    }
//    await service.DeleteSetAsync(DocRef);
//    return Results.Ok();
//});

app.Run("http://0.0.0.0:3000");

//Hack voor het uitvoeren van een unit-test  
//app.Run();
//public partial class Program { }