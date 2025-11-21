
using Newtonsoft.Json.Linq;

namespace WorldOfTheVoid.Domain.Entities
{
    public class AddOrderRequest
    {
        public OrderType Type { get; set; }
        public JObject Data { get; set; }
    }
}