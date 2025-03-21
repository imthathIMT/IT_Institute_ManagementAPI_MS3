using IT_Institute_Management.Database;
using IT_Institute_Management.EmailSection.Models;
using IT_Institute_Management.EmailSection.Repo;
using IT_Institute_Management.EmailSection.Service;
using IT_Institute_Management.ImageService;
using IT_Institute_Management.IRepositories;
using IT_Institute_Management.IServices;
using IT_Institute_Management.PasswordService;
using IT_Institute_Management.Repositories;
using IT_Institute_Management.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Sample Data Initialization (this part seems fine)
SampleDataIntializer seeder = new SampleDataIntializer();
seeder.AddMasterAdminSampleData();
seeder.AddAdminSampleData();
seeder.AddStudentSampleData();
seeder.AddCourseSampleData();
seeder.AddEnrollmentSampleData();
seeder.AddPaymentSampleData();
seeder.AddAnnouncementSampleData();
seeder.AddContactUsSampleData();
seeder.AddNotificationSampleData();

//Email Template Initialization
EmailTemplateInitializer initializer = new EmailTemplateInitializer();
initializer?.InitializeEmailTemplates();

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    option.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});


// Register EmailConfig
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EMAIL_CONFIGURATION"));

// Register services
builder.Services.AddScoped<sendmailService>();
builder.Services.AddScoped<SendMailRepository>();
builder.Services.AddScoped<EmailServiceProvider>();

// Database Context
builder.Services.AddDbContext<InstituteDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DevHubDB")));

// Ensure EmailConfig is available as a singleton if needed
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<EmailConfig>>().Value);

// Register Repositories and Services
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IContactUsRepository, ContactUsRepository>();
builder.Services.AddScoped<IContactUsService, ContactUsService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStudentMessageRepository, StudentMessageRepository>();
builder.Services.AddScoped<IStudentMessageService, StudentMessageService>();
builder.Services.AddScoped<ISocialMediaLinksRepository, SocialMediaLinksRepository>();
builder.Services.AddScoped<ISocialMediaLinksService, SocialMediaLinksService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Configure Authentication (Fix the duplication here)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true, 
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero 
        };

        // Custom event for handling expired tokens
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception is SecurityTokenExpiredException)
                {
                    context.Response.StatusCode = 401; 
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync("{\"message\": \"Token has expired\"}");
                }
                return Task.CompletedTask;
            }
        };
    });

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
      name: "CORSOpenPolicy",
      builder => {
          builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
      });
});


// Add Authorization
builder.Services.AddAuthorization();

// Build the app
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Enable static files and CORS
app.UseStaticFiles();
app.UseCors("CORSOpenPolicy");

// Map controllers
app.MapControllers();

// Run the application
app.Run();
