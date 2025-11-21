using System;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Utilities
{
    public static class JwtUtils
    {
        public static Dictionary<string, object> DecodePayload(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("JWT token is null or empty");

            string[] parts = token.Split('.');
            if (parts.Length != 3)
                throw new ArgumentException("Invalid JWT format");

            string payload = parts[1];

            // Fix Base64 padding
            payload = payload.Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            byte[] jsonBytes = Convert.FromBase64String(payload);
            string json = Encoding.UTF8.GetString(jsonBytes);

            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
        
        public static T DecodePayload<T>(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("JWT token is null or empty");

            string[] parts = token.Split('.');
            if (parts.Length != 3)
                throw new ArgumentException("Invalid JWT format");

            string payload = parts[1];

            // Fix Base64 padding
            payload = payload.Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            byte[] jsonBytes = Convert.FromBase64String(payload);
            string json = Encoding.UTF8.GetString(jsonBytes);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}