using Microsoft.EntityFrameworkCore;
using DOCSeal.Infrastructure.DataContext;
using DOCSeal.Infrastructure.DataContext.Exceptions;
using DOCSeal.Infrastructure.Security;
using DOCSeal.Infrastructure.Services.EmailService;
using DOCSeal.Infrastructure.Services.VerificationCode;
using DOCSeal.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "appsettings.Secrets.json", 
    optional: false,
    reloadOnChange: true);

#region Database

try 
{
    if (string.IsNullOrEmpty(builder.Configuration.GetValue<string>("db:host")))
        throw new DbBuildValueException("db:host");
    if (string.IsNullOrEmpty(builder.Configuration.GetValue<string>("db:name")))
        throw new DbBuildValueException("db:name");
    if (string.IsNullOrEmpty(builder.Configuration.GetValue<string>("db:login")))
        throw new DbBuildValueException("db:login");
    if (string.IsNullOrEmpty(builder.Configuration.GetValue<string>("db:password")))
        throw new DbBuildValueException("db:password");

    
    var dbConnString = $"Host={builder.Configuration.GetValue<string>("db:host")};" +
                       $"Port={builder.Configuration.GetValue("db:port", 5432)};" +
                       $"Database={builder.Configuration.GetValue<string>("db:name")};" +
                       $"Username={builder.Configuration.GetValue<string>("db:login")};" +
                       $"Password={builder.Configuration.GetValue<string>("db:password")};";

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(dbConnString));
}
catch (DbBuildValueException)
{
    throw; 
}
catch (Exception ex)
{
    throw new DbBuildAnyException(ex);
}

#endregion

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

var salt = builder.Configuration.GetValue<string>("salt") 
           ?? throw new InvalidOperationException("Соль не найдена!");

builder.Services.AddSingleton<IPasswordHasher>(new PasswordHasher(salt));

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddScoped<IVerificationCodeService,VerificationCodeWorker>();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));




// Add services to the container.


builder.Services.AddControllers();


builder.Services.AddOpenApi();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();