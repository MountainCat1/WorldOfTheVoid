using System.Text.Json.Nodes;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Entities.Orders;

namespace WorldOfTheVoid.Domain.OrderHanders.Abstractions;

public interface IOrderHandler
{
    public OrderType OrderName { get; }
    public Task ExecuteAsync(Character character, JsonNode data);
}