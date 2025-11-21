using System.Text.Json.Nodes;
using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Entities.Orders;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Dtos;
using WorldOfTheVoid.Errors;
using WorldOfTheVoid.Interfaces;

namespace WorldOfTheVoid.Features.Orders;

public class AddOrderCommand : ICommand<OrderDto>
{
    public required OrderType Type { get; init; }
    public required JsonObject Data { get; init; }
    public required EntityId CharacterId { get; init; }
}

public class AddOrderHandler : ICommandHandler<AddOrderCommand, OrderDto>
{
    private readonly IWorldRepository _worldRepository;

    public AddOrderHandler(IWorldRepository worldRepository)
    {
        _worldRepository = worldRepository;
    }

    public async Task<OrderDto> Handle(AddOrderCommand command)
    {
        var world = await _worldRepository.GetDefaultWorld();
        
        var character = world.Characters.FirstOrDefault(x => x.Id == command.CharacterId);
        if (character == null)
        {
            throw new EntityNotFound<Character>(command.CharacterId);
        }
        
        var order = Order.Create(command.Type, command.Data);
        
        character.AddOrder(order);

        await _worldRepository.SaveChangesAsync();

        return OrderDto.Create(order);
    }
}