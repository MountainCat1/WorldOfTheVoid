using System;
using System.Numerics;

namespace WorldOfTheVoid.Domain.Entities
{
    public class CharacterDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }

        public string WorldId { get; set; }
        public string AccountId { get; set; }
    }
}