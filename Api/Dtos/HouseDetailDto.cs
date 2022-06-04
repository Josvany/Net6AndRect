// using System.ComponentModel.DataAnnotations;

public record HouseDetailDto(int Id, string? Address, string? Country,
    string? Description, int Price, string? Photo);