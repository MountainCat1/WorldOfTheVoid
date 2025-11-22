using Microsoft.Extensions.Logging;
using WorldOfTheVoid.Domain.OrderHanders;
using WorldOfTheVoid.Domain.OrderHanders.Abstractions;
using WorldOfTheVoid.Domain.Ports;
using WorldOfTheVoid.Interfaces;

namespace WorldOfTheVoid.Domain.PerioticTasks;

public class ExecuteOrdersTask : IPeriodicTask
{
    private readonly IWorldRepository _worldRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<ExecuteOrdersTask> _logger;
    private readonly IEnumerable<IOrderHandler> _orderHandlers;

    public ExecuteOrdersTask(
        IWorldRepository worldRepository,
        ILogger<ExecuteOrdersTask> logger,
        IOrderRepository orderRepository,
        IEnumerable<IOrderHandler> orderHandlers
    )
    {
        _worldRepository = worldRepository;
        _logger = logger;
        _orderRepository = orderRepository;
        _orderHandlers = orderHandlers;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var world = await _worldRepository.GetDefaultWorld();
        var orders = await _orderRepository.GetAllAsync();

        var groupedOrders = orders
            .GroupBy(o => o.CharacterId);

        foreach (var orderGroup in groupedOrders)
        {
            var characterId = orderGroup.Key;
            var character = world.Characters.FirstOrDefault(c => c.Id == characterId);
            if (character == null)
            {
                _logger.LogWarning("Character with ID {CharacterId} not found for order execution.", characterId);
                continue;
            }

            foreach (var order in orderGroup)
            {
                var handler = _orderHandlers.FirstOrDefault(h => h.OrderName == order.Type);
                if (handler == null)
                {
                    _logger.LogWarning("No handler found for order {OrderName}.", order.Type);
                    continue;
                }

                try
                {
                    var result = await handler.ExecuteAsync(character, order.Data);

                    if (!result.Continues)
                    {
                        _orderRepository.Remove(order);
                    }

                    _logger.LogInformation(
                        "Executed order {OrderName} for character {CharacterId}.",
                        order.Type,
                        characterId
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error executing order {OrderName} for character {CharacterId}.",
                        order.Type,
                        characterId
                    );
                }
                
                break; // We process only one order per character per execution cycle
            }
        }
    }
}