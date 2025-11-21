using System.Text.Json.Nodes;
using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Domain.Entities.Orders;

namespace WorldOfTheVoid.Dtos;

public class OrderDto
{
    public required EntityId Id { get; init; }
    public required string Type { get; init; }
    public required JsonObject Data { get; init; }
    
    
    public static OrderDto Create(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            Type = order.Type.ToString(),
            Data = order.Data,
        };
    }
}