using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Dtos;
using WorldOfTheVoid.Errors;
using WorldOfTheVoid.Interfaces;

namespace WorldOfTheVoid.Features.Orders;

public class GetCharacterOrdersQuery : IQuery<ICollection<OrderDto>>
{
    public required EntityId CharacterId { get; init; }
}


public class GetCharacterOrdersHandler : IQueryHandler<GetCharacterOrdersQuery, ICollection<OrderDto>>
{
    private readonly ICharacterRepository _characterRepository;

    public GetCharacterOrdersHandler(ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task<ICollection<OrderDto>> Handle(GetCharacterOrdersQuery query)
    {
        var character = await _characterRepository.GetById(query.CharacterId);
        
        if(character == null)
        {
            throw new EntityNotFound<Character>(query.CharacterId);
        }
        
        return character.Orders.Select(OrderDto.Create).ToList();
    }
}