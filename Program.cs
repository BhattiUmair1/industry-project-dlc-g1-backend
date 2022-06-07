

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddTransient<ISetService, SetService>();
builder.Services.AddTransient<IUserInfoRepository, UserInfoRepository>();
builder.Services.AddTransient<IFolderService, FolderService>();
builder.Services.AddGraphQLServer().AddQueryType<Queries>().ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true).AddMutationType<Mutation>();
builder.Services.AddGraphQLServer().AddAuthorization();
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<SetValidator>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<StudysetValidator>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<FolderValidator>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<SetRefValidator>());

FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.FromFile(@"./Firebase/Firebasestudyapp-343918-firebase-adminsdk-cm4kr-2069e8f542.json")
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
app.MapGraphQL();


app.MapGet("/", () =>
{
    return new { data = "working" };
});


DateTime timerStarted = DateTime.Now;


// test timer
app.MapGet("api/HPM/timer/{request}", (string request) =>
{
    if (request == "start")
    {
        DateTime timerStarted = DateTime.Now;
        Console.WriteLine("The current time is {0}", timerStarted);
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
        Console.WriteLine("Current time: " + time);
        return Results.Ok(new { curerntTime = time });
    }
    return Results.BadRequest(new { request = "Please make sure the route is correct." });
});


// get current
app.MapGet("api/HPM/{DocRef}", [Authorize] async (string DocRef, ISetService service) =>
{
    // respository
    if (DocRef.Length < 32 || string.IsNullOrEmpty(DocRef))
    {
        return Results.BadRequest("DocRef is invalid");
    }
    var userInfo = await service.GetSetAsync(DocRef);
    return Results.Ok(userInfo);

});

// create a set
app.MapPost("/set", [Authorize] async (IValidator<Set> validator, IValidator<Studyset> validator1, Set set, ISetService service) =>
{
    set.ReviewedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
    var result = validator.Validate(set);
    foreach (var item in set.Studyset)
    {
        var result1 = validator1.Validate(item);
        if (!result1.IsValid)
        {
            return Results.BadRequest(result1.Errors);
        }
    }
    if (!result.IsValid)
    {
        return Results.BadRequest(result.Errors);
    }
    var addedSet = await service.AddSetAsync(set);
    return Results.Created("/addset/{UserId}", new { respond = addedSet });
});
// edit a set
app.MapPut("/set", [Authorize] async (IValidator<Set> validator, IValidator<Studyset> validator1, Set set, ISetService service) =>
{
    set.ReviewedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
    var result = validator.Validate(set);
    foreach (var item in set.Studyset)
    {
        var result1 = validator1.Validate(item);
        if (!result1.IsValid)
        {
            return Results.BadRequest(result1.Errors);
        }
    }
    if (!result.IsValid)
    {
        return Results.BadRequest(result.Errors);
    }
    var editedSet = await service.UpdateSetAsync(set);
    return Results.Ok(editedSet);

});
// delete a set
app.MapDelete("/set/{DocRef}", async (string DocRef, ISetService service) =>
{
    if (DocRef.Length < 32 || string.IsNullOrEmpty(DocRef))
    {
        return Results.BadRequest("Document reference is invalid");
    }
    await service.DeleteSetAsync(DocRef);
    return Results.Ok();
});


// read a folder
app.MapGet("/folder/{DocRef}", [Authorize] async (string DocRef, IFolderService service) =>
{
    // respository
    if (DocRef.Length < 32 || string.IsNullOrEmpty(DocRef))
    {
        return Results.BadRequest("DocRef is invalid");
    }
    var userInfo = await service.GetFolderAsync(DocRef);
    return Results.Ok(userInfo);

});

// read folders
app.MapGet("/folders/{UserId}", [Authorize] async (string UserId, IFolderService service) =>
{
    if (UserId.Length < 28 || UserId == "" || UserId == null)
    {
        return Results.BadRequest("Userid is invalid");
    }
    var folders = await service.GetFoldersAsync(UserId);
    return Results.Ok(folders);
});

// create a folder
app.MapPost("/folder", [Authorize] async (IValidator<Folder> validator, IValidator<SetRef> validator1, Folder folder, IFolderService service) =>
{
    var result = validator.Validate(folder);
    foreach (var item in folder.SetRefs!)
    {
        var result1 = validator1.Validate(item);
        if (!result1.IsValid)
        {
            return Results.BadRequest(result1.Errors);
        }
    }

    if (!result.IsValid)
    {
        return Results.BadRequest(result.Errors);
    }
    var addedFolder = await service.AddFolderAsync(folder);
    return Results.Created("/folder", addedFolder);
});
// edit a folder
app.MapPut("/folder", [Authorize] async (IValidator<Folder> validator, IValidator<SetRef> validator1, Folder folder, IFolderService service) =>
{
    var result = validator.Validate(folder);

    foreach (var item in folder.SetRefs!)
    {
        var result1 = validator1.Validate(item);
        if (!result1.IsValid)
        {
            return Results.BadRequest(result1.Errors);
        }
    }

    if (!result.IsValid)
    {
        return Results.BadRequest(result.Errors);
    }
    var editedFolder = await service.UpdateFolderAsync(folder);
    return Results.Ok(editedFolder);
});
// delete a folder
app.MapDelete("/folder/{DocRef}", async (string DocRef, IFolderService service) =>
{
    if (DocRef.Length < 32 || DocRef == "" || DocRef == null)
    {
        return Results.BadRequest("Document reference is invalid");
    }
    await service.DeleteFolderAsync(DocRef);
    return Results.Ok();
});



app.Run("http://0.0.0.0:3000");

//Hack voor het uitvoeren van een unit-test  
//app.Run();
//public partial class Program { }