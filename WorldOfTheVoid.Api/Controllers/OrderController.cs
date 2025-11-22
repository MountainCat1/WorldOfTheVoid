using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Dtos;
using WorldOfTheVoid.Features.Orders;

namespace WorldOfTheVoid.Controllers;

[ApiController]
[Authorize]
[Route("api/characters/{characterId}/orders")]
public class OrderController : ControllerBase
{
    private AddOrderHandler _addOrderHandler;
    private ReplaceOrdersHandler _replaceOrdersHandler;

    public OrderController(AddOrderHandler addOrderHandler, ReplaceOrdersHandler replaceOrdersHandler)
    {
        _addOrderHandler = addOrderHandler;
        _replaceOrdersHandler = replaceOrdersHandler;
    }


    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] AddOrderRequest request, [FromRoute] EntityId characterId)
    {
        var command = new AddOrderCommand
        {
            Type = request.Type,
            Data = request.Data,
            CharacterId = characterId
        };

        var result = await _addOrderHandler.Handle(command);

        return CreatedAtAction(nameof(GetOrderById), new { characterId = characterId, orderId = result.Id }, result);
    }

    [HttpPut(":replace")]
    [ProducesResponseType<ICollection<OrderDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReplaceOrder(
        [FromBody] ReplaceOrdersRequest request,
        [FromRoute] EntityId characterId
    )
    {
        var command = new ReplaceOrdersCommand
        {
            CharacterId = characterId,
            Orders = request.Orders
        };

        var result = await _replaceOrdersHandler.Handle(command);

        return Ok(result);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderById([FromRoute] EntityId characterId, [FromRoute] EntityId orderId)
    {
        return StatusCode(501); // Not Implemented
    }
}