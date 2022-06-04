using Api.Data;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddDbContext<HouseDbContext>(opt => opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
builder.Services.AddScoped<IHouseRepository, HouseRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(conf => conf.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();


app.MapGet("/houses", (IHouseRepository repo) => repo.GetAll()).Produces<HouseDto>(StatusCodes.Status200OK);
app.MapGet("/houses/{id:int}", async (int id, IHouseRepository repo) =>
{
    var houseDetail = await repo.Get(id);
    if (houseDetail is null)
    {
        return Results.Problem($"House with Id : {id} not found.", statusCode:StatusCodes.Status404NotFound);
    }

    return Results.Ok(houseDetail);
}).ProducesProblem(StatusCodes.Status404NotFound).Produces<HouseDetailDto>(StatusCodes.Status200OK);

app.Run();

