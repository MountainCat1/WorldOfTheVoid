using System.Numerics;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Interfaces;

namespace WorldOfTheVoid.Domain.PerioticTasks;

public class MoveCharactersTask : IPeriodicTask
{
    private IWorldRepository _worldRepository;

    public MoveCharactersTask(IWorldRepository worldRepository)
    {
        _worldRepository = worldRepository;
    }


    public async Task ExecuteAsync(CancellationToken ct)
    {
        var world = await _worldRepository.GetDefaultWorld();

        var characters = world.Characters;

        var rng = new Random();
        
        foreach (var character in characters)
        {
            character.Position += new Vector3(rng.NextSingle() - 0.5f, rng.NextSingle() - 0.5f, rng.NextSingle() - 0.5f);
        }
    }
}