using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DefaultNamespace;
using DefaultNamespace.Utilities;
using UnityEngine;
using WorldOfTheVoid.Domain.Entities;

public class WorldReceiver : MonoBehaviour
{
    public event Action<WorldDto> OnWorldReceived;
    
    [SerializeField] private WorldOfTheVoidClient worldClient;

    [SerializeField] private string username = "admin";
    [SerializeField] private string password = "admin";
    
    [SerializeField] private float updateIntervalSeconds = 3f;
    
    private CancellationTokenSource _cts;
    
    private void OnEnable()
    {
        _cts = new CancellationTokenSource();
        _ = UpdateWorldLoopAsync(_cts.Token);
    }

    private void OnDisable()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
    
    private async Task UpdateWorldLoopAsync(CancellationToken ct)
    {
        await worldClient.Authenticate(username, password);
        
        while (!ct.IsCancellationRequested)
        {
            try
            {
                if(worldClient.User == null)
                {
                    Debug.Log("Not authenticated. Attempting to authenticate...");
                    await worldClient.Authenticate(username, password);
                    continue;
                }
                
                Debug.Log("Fetching world state...");
                var worldDto = await worldClient.GetWorldStateAsync();
                WorldState.State = worldDto;

                if (worldDto != null)
                    OnWorldReceived?.Invoke(worldDto);
                else
                    Debug.LogWarning("Received null world state.");

                await Task.Delay(Mathf.RoundToInt(updateIntervalSeconds * 1000), ct); // wait 3 seconds
            }
            catch (TaskCanceledException)
            {
                // graceful exit
                break;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                await Task.Delay(Mathf.RoundToInt(updateIntervalSeconds * 1000), ct);
            }
        }
    }

}