using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Interfaces;

namespace WorldOfTheVoid.Features;

public class GrowPopulationPeriodicTask : IPeriodicTask
{
    private readonly IWorldRepository _worldRepository;

    public GrowPopulationPeriodicTask(IWorldRepository worldRepository)
    {
        _worldRepository = worldRepository;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var world = await _worldRepository.GetDefaultWorld();
        
        var places = world.Places;

        foreach (var place in places)
        {
            place.Population += (int)(place.Population * 0.05); // Grow population by 5%
        }
    }
}