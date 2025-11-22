using System;
using TMPro;
using UnityEngine;
using Utilities;
using WorldOfTheVoid.Domain.Entities;

public class WorldDataDisplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private WorldReceiver worldReceiver;

    private void Start()
    {
        worldReceiver.OnWorldReceived += OnWorldReceived;
    }

    private void OnWorldReceived(WorldDto world)
    {
        text.text = JsonService.Serialize(world);
    }
}
