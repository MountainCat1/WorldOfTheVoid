using System.Text.Json.Nodes;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Entities.Orders;

namespace WorldOfTheVoid.Domain.OrderHanders.Abstractions;

public interface IOrderHandler
{
    public OrderType OrderName { get; }
    public Task<OrderHandleResult> ExecuteAsync(Character character, JsonNode data);
}

public record OrderHandleResult
{
    public bool Continues { get; set; }
    public static OrderHandleResult Completed => new OrderHandleResult { Continues = false };

    public static OrderHandleResult InProgress => new OrderHandleResult { Continues = true };
}