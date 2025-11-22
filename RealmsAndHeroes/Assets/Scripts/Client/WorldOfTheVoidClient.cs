using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client;
using UnityEngine;
using UnityEngine.Networking;
using Utilities;
using WorldOfTheVoid.Domain.Entities;

public class WorldOfTheVoidClient : MonoBehaviour
{
    [SerializeField] private string baseUrl;

    public User User { get; private set; }
    
    private string _authToken;

    private UnityWebRequest CreateRequest(string url, string method, string jsonBody = null)
    {
        var request = new UnityWebRequest(url, method);

        if (!string.IsNullOrWhiteSpace(jsonBody))
        {
            var bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Content-Type", "application/json");

        if (!string.IsNullOrEmpty(_authToken))
        {
            request.SetRequestHeader("Authorization", $"Bearer {_authToken}");
        }

        return request;
    }

    private async Task<UnityWebRequest> SendAsync(UnityWebRequest request)
    {
        var operation = request.SendWebRequest();
        await operation;

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[WorldOfTheVoidClient] Request failed: {request.error}");
            return null;
        }

        return request;
    }

    public async Task Authenticate(string username, string password)
    {
        string url = $"{baseUrl}/api/Auth/login";

        var payload = new
        {
            Username = username,
            Password = password
        };

        string json = JsonService.Serialize(payload);

        var request = CreateRequest(url, UnityWebRequest.kHttpVerbPOST, json);
        var result = await SendAsync(request);

        if (result == null) return;

        try
        {
            var response = JsonService.Deserialize<LoginResponse>(result.downloadHandler.text);
            _authToken = response.Token;
            
            User = JwtUtils.DecodePayload<User>(_authToken);
            
            Debug.Log($"[WorldOfTheVoidClient] Authentication successful as {User.Username} ({User.Id})");
        }
        catch (Exception e)
        {
            Debug.LogError($"[WorldOfTheVoidClient] Login parse error: {e.Message}");
        }
    }

    public async Task<WorldDto> GetWorldStateAsync()
    {
        string url = $"{baseUrl}/api/World/state";

        var request = CreateRequest(url, UnityWebRequest.kHttpVerbGET);
        var result = await SendAsync(request);

        if (result == null) return null;

        try
        {
            return JsonService.Deserialize<WorldDto>(result.downloadHandler.text);
        }
        catch (Exception e)
        {
            Debug.LogError($"[WorldOfTheVoidClient] JSON parse error: {e.Message}");
            return null;
        }
    }

    public async Task<OrderDto> AddOrderToCharacterAsync(string ownCharacterId, AddOrderRequest request)
    {
        string url = $"{baseUrl}/api/characters/{ownCharacterId}/orders";

        string json = JsonService.Serialize(request);

        var webRequest = CreateRequest(url, UnityWebRequest.kHttpVerbPOST, json);
        var result = await SendAsync(webRequest);

        if (result == null) return null;
        
        try
        {
            var order = JsonService.Deserialize<OrderDto>(result.downloadHandler.text);
            Debug.Log($"[WorldOfTheVoidClient] Order added: {order.Id} of type {order.Type}");
            return order;
        }
        catch (Exception e)
        {
            Debug.LogError($"[WorldOfTheVoidClient] JSON parse error: {e.Message}");
            return null;
        }
    }
    
    public async Task<ICollection<OrderDto>> GetAllCharacterOrdersAsync(string characterId)
    {
        string url = $"{baseUrl}/api/characters/{characterId}/orders";

        var request = CreateRequest(url, UnityWebRequest.kHttpVerbGET);
        var result = await SendAsync(request);

        if (result == null) return null;

        try
        {
            return JsonService.Deserialize<ICollection<OrderDto>>(result.downloadHandler.text);
        }
        catch (Exception e)
        {
            Debug.LogError($"[WorldOfTheVoidClient] JSON parse error: {e.Message}");
            return null;
        }
    }
}
