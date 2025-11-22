using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;

namespace WorldOfTheVoid.Domain.Entities
{
    public class OwnCharacterDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }

        public string WorldId { get; set; }
        public string AccountId { get; set; }
        [CanBeNull] public ICollection<OrderDto> Orders { get; set; }
    }
}