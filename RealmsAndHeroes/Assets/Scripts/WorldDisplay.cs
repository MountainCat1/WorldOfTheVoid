using System;
using System.Threading;
using System.Threading.Tasks;
using DefaultNamespace;
using DefaultNamespace.Utilities;
using UnityEngine;
using WorldOfTheVoid.Domain.Entities;

public class WorldDisplay : MonoBehaviour
{
    [SerializeField] private WorldOfTheVoidClient worldClient;
    
    [SerializeField] private PlaceObject placeObjectPrefab;
    [SerializeField] private CharacterObject characterObjectPrefab;
    
    [SerializeField] private Transform containerParent;

    [SerializeField] private string username = "admin";
    [SerializeField] private string password = "admin";
    
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
                Debug.Log("Fetching world state...");
                var worldDto = await worldClient.GetWorldStateAsync();

                if (worldDto != null)
                    OnWorldReceived(worldDto);
                else
                    Debug.LogWarning("Received null world state.");

                await Task.Delay(3000, ct); // wait 3 seconds
            }
            catch (TaskCanceledException)
            {
                // graceful exit
                break;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error fetching world state: {ex.Message}");
                await Task.Delay(3000, ct);
            }
        }
    }

    private void OnWorldReceived(WorldDto world)
    {
        Debug.Log($"World received: {world.Name} with {world.Places.Count} places.");
        
        // Clear existing place objects
        foreach (Transform child in containerParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var place in world.Places)
        {
            var placeObject = Instantiate(placeObjectPrefab, place.Position.ToUnityVector3(), Quaternion.identity, containerParent);
            placeObject.Initialize(place);
        }

        foreach (var character in world.Characters)
        {
            var characterObject = Instantiate(characterObjectPrefab, character.Position.ToUnityVector3(), Quaternion.identity, containerParent);
            
            var own = worldClient.User != null && character.AccountId == worldClient.User.Id;
            characterObject.Initialize(character, own);
        }
    }
}