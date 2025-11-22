using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using WorldOfTheVoid.Domain.Entities;

public class WorldDisplay : MonoBehaviour
{
    [SerializeField] private WorldReceiver worldReceiver;
    [SerializeField] private WorldOfTheVoidClient worldClient;

    [SerializeField] private PlaceObject placeObjectPrefab;
    [SerializeField] private CharacterObject characterObjectPrefab;
    
    [SerializeField] private Transform containerParent;
    private Dictionary<string, GameObject> _instantiatedObjects = new();


    private void Start()
    {
        worldReceiver.OnWorldReceived += OnWorldReceived;
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