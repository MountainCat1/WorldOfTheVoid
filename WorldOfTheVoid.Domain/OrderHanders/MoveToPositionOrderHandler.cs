using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Entities.Orders;
using WorldOfTheVoid.Domain.OrderHanders.Abstractions;

namespace WorldOfTheVoid.Domain.OrderHanders;

public class MoveToPositionOrderHandler : IOrderHandler
{
    public OrderType OrderName => OrderType.MoveToPosition;

    private JsonSerializerOptions _jsonOptions;

    public MoveToPositionOrderHandler(JsonSerializerOptions jsonOptions)
    {
        _jsonOptions = jsonOptions;
    }

    public async Task ExecuteAsync(Character character, JsonNode data)
    {
        const float speed = 1;

        var targetPosition = data["TargetPosition"]!.Deserialize<Vector3>(_jsonOptions);
        
        var direction = Vector3.Normalize(targetPosition - character.Position);
        
        character.Position += direction * speed;
        
        await Task.CompletedTask;
    }
}