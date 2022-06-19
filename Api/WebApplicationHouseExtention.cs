using Api.Data;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;

public static class WebApplicationHouseExtention
{
    public static void MapHouseEndPoints(this WebApplication app)
    {
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
        }).ProducesValidationProblem().Produces<HouseDetailDto>(StatusCodes.Status201Created);

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
    }
}