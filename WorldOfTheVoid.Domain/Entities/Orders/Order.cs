using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace WorldOfTheVoid.Domain.Entities.Orders;

public class Order
{
    public EntityId Id { get; set; }
    
    public OrderType Type { get; set; }
    public JsonObject Data { get; set; }
    
    public EntityId CharacterId { get; internal set; }
    
    [JsonIgnore]
    public Character Character { get; internal set; }

    public static Order Create(OrderType orderType, JsonObject data)
    {
        return new Order
        {
            Id = EntityId.Create(EntityType.Order),
            Type = orderType,
            Data = data,
        };
    }
}