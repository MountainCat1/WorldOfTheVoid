using System;
using System.Collections.Generic;

namespace WorldOfTheVoid.Domain.Entities
{
    public class WorldDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<CharacterDto> Characters { get; set; }
        public virtual ICollection<PlaceDto> Places { get; set; }
        
        public ICollection<string> GetAllEntitiesIds()
        {
            var entityIds = new List<string>();
        
            foreach (var character in Characters)
            {
                entityIds.Add(character.Id);
            }
        
            foreach (var place in Places)
            {
                entityIds.Add(place.Id);
            }
        
            return entityIds;
        }
    }
}