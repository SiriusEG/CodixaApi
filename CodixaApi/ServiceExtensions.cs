using Codixa.Core.Interfaces;
using Codixa.Core.Models.UserModels;
using CodixaApi.Services;
using Codxia.Core;
using Codxia.Core.Interfaces;
using Codxia.EF;
using Codxia.EF.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace CodxiaApi
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Controllers and Endpoints API Explorer
            services.AddControllers();
            services.AddAuthorization();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Register DBContext for Dependency Injection
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("defaultConnection"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            // Register UnitOfWork for Dependency Injection
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Register Repositories for Dependency Injection
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IAdminDashboardService, AdminDashboardService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ISectionService, SectionService>();
            services.AddTransient<ILessonService, LessonService>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IInstructorService, InstructorService>();
            services.AddTransient<ICourseFeedbackService, CourseFeedbackService>();
            services.AddTransient<ICourseProgresService, CourseProgresService>();

         
       

            // Add Identity
            services.AddIdentity<AppUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AppDbContext>();

            // CORS Configuration
            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    // Uncomment if you need to specify an origin
                    // policyBuilder.WithOrigins("https://team-protifolio-vsdj.vercel.app").AllowAnyHeader().AllowAnyMethod();
                });
            });

            // JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });

            //services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                // Define security schemes for Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Add security requirements to the API
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
            });
        }
    }
}
