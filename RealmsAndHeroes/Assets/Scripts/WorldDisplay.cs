using System;
using System.Collections.Generic;
using System.Linq;
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
    private Dictionary<string, GameObject> _instantiatedObjects = new();

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
                WorldState.State = worldDto;

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
        
        var allEntitiesIds = world.GetAllEntitiesIds();
        
        // Clear existing place objects
        foreach (var (entityId, go) in _instantiatedObjects.ToList())
        {
            var existsInWorld = allEntitiesIds.Contains(entityId);
            if (existsInWorld) continue;
            
            Debug.Log($"Removing object with EntityId: {entityId}");
            Destroy(go);
        }

        foreach (var place in world.Places)
        {
            if (!_instantiatedObjects.TryGetValue(place.Id, out var placeObjectGo))
            {
                placeObjectGo = Instantiate(placeObjectPrefab, containerParent).gameObject;
             _instantiatedObjects.Add(place.Id, placeObjectGo);
            }
            
            placeObjectGo.GetComponent<PlaceObject>().Initialize(place);
        }

        foreach (var character in world.Characters)
        {
            if (!_instantiatedObjects.TryGetValue(character.Id, out var characterObjectGo))
            {
                characterObjectGo = Instantiate(characterObjectPrefab,  containerParent).gameObject;
                _instantiatedObjects.Add(character.Id, characterObjectGo);
            }
            
            
            var own = worldClient.User != null && character.AccountId == worldClient.User.Id;
            
            characterObjectGo.GetComponent<CharacterObject>().Initialize(character, own);
        }
    }
}