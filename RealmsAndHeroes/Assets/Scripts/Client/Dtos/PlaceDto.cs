using System;
using System.Numerics;

namespace WorldOfTheVoid.Domain.Entities
{
    public class PlaceDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }
        public Vector3 Position { get; set; }
    
        public string WorldId { get; set; }
    }
}