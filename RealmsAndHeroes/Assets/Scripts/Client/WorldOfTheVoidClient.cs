using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using WorldOfTheVoid.Domain.Entities;
using Newtonsoft.Json;
using Utilities;

public class WorldOfTheVoidClient : MonoBehaviour
{
    [SerializeField] private string baseUrl;

    public async Task<WorldDto> GetWorldStateAsync()
    {
        string url = $"{baseUrl}/api/World/state";

        using UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Accept", "application/json");

        var operation = request.SendWebRequest();
        await operation;

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[WorldOfTheVoidClient] Request failed: {request.error}");
            return null;
        }

        try
        {
            Debug.Log($"{request.downloadHandler.text}");
            return JsonService.Deserialize<WorldDto>(request.downloadHandler.text);
        }
        catch (Exception e)
        {
            Debug.LogError($"[WorldOfTheVoidClient] JSON parse error: {e.Message}");
            return null;
        }
    }
}