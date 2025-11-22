namespace WorldOfTheVoid.Dtos;

public class ReplaceOrdersRequest
{
    public required List<OrderDto> Orders { get; init; }
}