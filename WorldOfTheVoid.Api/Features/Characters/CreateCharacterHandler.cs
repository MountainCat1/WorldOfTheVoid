using WorldOfTheVoid.Domain;
using WorldOfTheVoid.Domain.Entities;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Dtos;
using WorldOfTheVoid.Errors;
using WorldOfTheVoid.Interfaces;
using WorldOfTheVoid.Domain.Entities;

namespace WorldOfTheVoid.Features.Characters;

public class CreateCharacterCommand : ICommand<CharacterDto>
{
    public required EntityId AccountId { get; set; }
    public required string Name { get; set; }
}

public class CreateCharacterHandler : ICommandHandler<CreateCharacterCommand, CharacterDto>
{
    private IAccountRepository _accountRepository;
    private IWorldRepository _worldRepository;

    public CreateCharacterHandler(IAccountRepository accountRepository, IWorldRepository worldRepository)
    {
        _accountRepository = accountRepository;
        _worldRepository = worldRepository;
    }

    public async Task<CharacterDto> Handle(CreateCharacterCommand command)
    {
        var account = await _accountRepository.GetAccountByIdAsync(command.AccountId);

        var world = await _worldRepository.GetDefaultWorld();

        if (account is null)
            throw new EntityNotFound<Account>();
        
        var character = Character.Create(account, command.Name);
        
        world.Characters.Add(character);

        await _worldRepository.SaveChangesAsync();
        
        return CharacterDto.Create(character);
    }
}