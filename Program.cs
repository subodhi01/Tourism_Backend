using Microsoft.EntityFrameworkCore;
using TourismGalle.Services;
using TourismGalle.Models;
using TourismGalle.Data;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Angular development server
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

// Other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<EmailService>(); // Register Email Service
builder.Services.AddScoped<TourPackageRepository>();
builder.Services.AddScoped<PlaceService>();



var app = builder.Build();

// Use CORS policy
app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();