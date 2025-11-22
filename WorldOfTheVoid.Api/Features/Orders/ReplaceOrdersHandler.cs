using System.Text.Json.Nodes;
using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Entities.Orders;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Dtos;
using WorldOfTheVoid.Errors;
using WorldOfTheVoid.Interfaces;

namespace WorldOfTheVoid.Features.Orders;

public class ReplaceOrdersCommand : ICommand<ICollection<OrderDto>>
{
    public required ICollection<OrderDto> Orders { get; init; }
    public required EntityId CharacterId { get; init; }
}

public class ReplaceOrdersHandler : ICommandHandler<ReplaceOrdersCommand, ICollection<OrderDto>>
{
    private readonly ICharacterRepository _characterRepository;

    public ReplaceOrdersHandler(ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task<ICollection<OrderDto>> Handle(ReplaceOrdersCommand command)
    {
        var character = await _characterRepository.GetById(command.CharacterId);
        if (character == null)
        {
            throw new EntityNotFound<Character>(command.CharacterId);
        }
        
        var newOrders = new List<Order>();

        foreach (var createCharacterRequest in command.Orders)
        {
            var order = Order.Create(createCharacterRequest.Type, createCharacterRequest.Data);
            
            newOrders.Add(order);
        }
        
        character.Orders.Clear();
        foreach (var order in newOrders)
        {
            character.AddOrder(order);
        }

        await _characterRepository.SaveChangesAsync();

        return newOrders.Select(OrderDto.Create).ToList();
    }
}