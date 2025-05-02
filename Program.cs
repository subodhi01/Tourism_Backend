using Microsoft.EntityFrameworkCore;
using TourismGalle.Services;
using TourismGalle.Models;
using TourismGalle.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add CORS policy (named "AllowFrontend")
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular dev server
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Only if using cookies or auth headers
    });
});

// Other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<TravelPlaceRepository>();
builder.Services.AddScoped<TourPackageRepository>();
// builder.Services.AddScoped<PlaceService>(); // Uncomment if needed

var app = builder.Build();

// ✅ Use CORS policy here
app.UseCors("AllowFrontend");

// Swagger (only in development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
