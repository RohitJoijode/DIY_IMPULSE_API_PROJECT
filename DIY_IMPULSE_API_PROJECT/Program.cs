using DIY_IMPULSE_API_PROJECT.DBENGINE;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();

builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Here injected application DB context
builder.Services.AddDbContext<DBEngine>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
   
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "yourIssuer",
        ValidAudience = "yourAudience",
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("082F41538C1178DE768A9AC86291678D"))
    };
});





var aesKey = Encoding.UTF8.GetBytes("3dtdgruAMC2YRYmI"); // 16, 24, or 32 bytes
var aesIV = Encoding.UTF8.GetBytes("3dtdgruAMC2YRYmI");  // 16 bytes

//AddSingleton :- Registers a service as a singleton , which means a single instance of the service is created and and shared throughout the application's lifetime.
builder.Services.AddSingleton<DIY_IMPULSE_API_PROJECT.BAL.IRepository.IAESEncription>(new DIY_IMPULSE_API_PROJECT.BAL.Repository.AESEncription(aesKey, aesIV));

//Register a services with a scope lifetme which means a new instance is created for each request but shared within a single request.
builder.Services.AddScoped<DIY_IMPULSE_API_PROJECT.BAL.IRepository.IRSAHelperRepo,DIY_IMPULSE_API_PROJECT.BAL.Repository.RSAHelperRepo>();
builder.Services.AddScoped<DIY_IMPULSE_API_PROJECT.BAL.IRepository.ICommonRepo,DIY_IMPULSE_API_PROJECT.BAL.Repository.CommonRepo>();
builder.Services.AddScoped<DIY_IMPULSE_API_PROJECT.BAL.IRepository.IAuthServices,DIY_IMPULSE_API_PROJECT.BAL.Repository.AuthServices>();
builder.Services.AddScoped<DIY_IMPULSE_API_PROJECT.BAL.IRepository.ILogInRepository,DIY_IMPULSE_API_PROJECT.BAL.Repository.LogInRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
