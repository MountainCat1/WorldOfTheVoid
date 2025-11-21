using System;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Utilities;
using WorldOfTheVoid.Domain.Entities;

public class Commander : MonoBehaviour
{
    [SerializeField] private WorldOfTheVoidClient worldClient;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            IssueMoveCommand(mouseWorldPosition);
        }
    }

    private void IssueMoveCommand(Vector3 mouseWorldPosition)
    {
        var request = new AddOrderRequest()
        {
            Type = OrderType.MoveToPosition,
            Data = new()
            {
                { "TargetPosition", JsonService.DeserializeToJToken(mouseWorldPosition) }
            }

        };

        var world = WorldState.State;

        var ownCharacters = world
            .Characters
            .Where(c => c.AccountId == worldClient.User.Id);


        foreach (var ownCharacter in ownCharacters)
        {
            _ = worldClient.AddOrderToCharacterAsync(ownCharacter.Id, request);
        }
    }
}