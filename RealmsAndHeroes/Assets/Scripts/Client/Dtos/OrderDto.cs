using Newtonsoft.Json.Linq;

namespace WorldOfTheVoid.Domain.Entities
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public JObject Data { get; set; }
    }
}