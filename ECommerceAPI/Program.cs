
using Domain.Entities;
using Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Data;
using Repository.Repositories.Abstracts;
using Repository.Repositories.Concrates;
using Service.Services.Abstracts;
using Service.Services.Concrates;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ECommerceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(opt => 
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            builder.Services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 6;
                opt.User.RequireUniqueEmail = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.AllowedForNewUsers = true;
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero 
                    };
                });

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.Configure<FileStorageSettings>(builder.Configuration.GetSection("FileStorage"));   

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();  
            builder.Services.AddScoped<IAccountService, AccountService>();  
            builder.Services.AddScoped<IFileService, FileService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");
            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();

           
            app.MapControllers();

            app.Run();
        }
    }
}
