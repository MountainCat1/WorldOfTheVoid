using System.Text.Json.Nodes;
using WorldOfTheVoid.Domain.Entities.Orders;

namespace WorldOfTheVoid.Dtos;

public class AddOrderRequest
{
    public required OrderType Type { get; init; }
    public required JsonObject Data { get; init; }
}