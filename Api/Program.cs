using Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using MiniValidation;

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

app.MapPost("/houses",async ([FromBody]HouseDetailDto dto, IHouseRepository repo) =>
{
    if (!MiniValidator.TryValidate(dto, out var errors))
    {
        return Results.ValidationProblem(errors);
    }

    var newHouse = await repo.Add(dto);
    return Results.Created($"/houses/{newHouse.Id}", newHouse);
}).Produces<HouseDetailDto>(StatusCodes.Status201Created).ProducesValidationProblem();

app.MapPut("/houses",async ([FromBody]HouseDetailDto dto, IHouseRepository repo) =>
{
    if (!MiniValidator.TryValidate(dto, out var errors))
    {
        return Results.ValidationProblem(errors);
    }

    if (await repo.Get(dto.Id) is null)
    {
        return Results.Problem($"House {dto.Id} not found", statusCode: StatusCodes.Status404NotFound);
    }

    var updateHouse = await repo.Update(dto);


    return Results.Ok(updateHouse);
}).ProducesProblem(StatusCodes.Status404NotFound).Produces<HouseDetailDto>(StatusCodes.Status200OK).ProducesValidationProblem();


app.MapDelete("/houses/{id:int}", async (int id, IHouseRepository repo) =>
{
    if (await repo.Get(id) is null)
    {
        return Results.Problem($"House {id} not found", statusCode: StatusCodes.Status404NotFound);
    }
    await repo.Delete(id);

    return Results.Ok();
    
}).ProducesProblem(StatusCodes.Status404NotFound).Produces<HouseDetailDto>(StatusCodes.Status200OK);

app.Run();

