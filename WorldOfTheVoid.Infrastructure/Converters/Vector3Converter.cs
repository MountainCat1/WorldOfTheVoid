using System.Numerics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class Vector3Converter : ValueConverter<Vector3, string>
{
    public Vector3Converter() : base(
        v => $"{v.X},{v.Y},{v.Z}",          // to database
        v => Parse(v))                      // from database
    { }

    private static Vector3 Parse(string value)
    {
        var parts = value.Split(',');
        return new Vector3(
            float.Parse(parts[0]),
            float.Parse(parts[1]),
            float.Parse(parts[2]));
    }
}