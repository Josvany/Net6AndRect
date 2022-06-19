using Api.Data;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;

public static class WebApplicationBidsExtention
{
    public static void MapBidsEndPoints(this WebApplication app)
    {
        app.MapGet("house/{houseId:int}/bids", async (int houseId, IHouseRepository repoHouse, IBidRespositoty repoBids) =>
        {
            if (await repoHouse.Get(houseId) is null)
            {
                return Results.Problem($"House {houseId} not found", statusCode: StatusCodes.Status404NotFound);
            }
            var bids = await repoBids.Get(houseId);

            return Results.Ok(bids);
        }).ProducesProblem(StatusCodes.Status404NotFound).Produces(StatusCodes.Status200OK);

        app.MapPost("house/{houseId:int}/bids", async (int houseId, [FromBody]BidDto dto, IBidRespositoty repoBids) =>
        {
            if (dto.HouseId != houseId)
            {
                return Results.Problem("No match", statusCode: StatusCodes.Status400BadRequest);
            }

            if(!MiniValidator.TryValidate(dto, out var errors))
            {
                return Results.ValidationProblem(errors);
            }

            var newBids = await repoBids.Add(dto);

            return Results.Created($"hosuses/{newBids.HouseId}/bids", newBids);

        }).ProducesValidationProblem().ProducesProblem(StatusCodes.Status404NotFound).Produces<BidDto>(StatusCodes.Status201Created);
    }
}