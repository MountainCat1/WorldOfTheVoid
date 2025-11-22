using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Utilities
{
    public static class Vector3Extensions
    {
        // public static string ToBackendString(this Vector3 v)
        //     => $"{v.x},{v.y},{v.z}";
        public static JToken ToBackendString(this Vector3 v)
            => JsonService.SerializeToJToken(v);
    }
}