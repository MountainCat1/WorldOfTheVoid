using UnityEngine;
using WorldOfTheVoid.Domain.Entities;

public class OrderDisplayUI : MonoBehaviour
    {
        [SerializeField] private Transform ordersContainer;
        [SerializeField] private OrderEntryUI orderUIPrefab;
        
        [SerializeField] private WorldReceiver worldReceiver;
        
        private void Start()
        {
            worldReceiver.OnWorldReceived += OnWorldReceived;
        }

        private void OnWorldReceived(WorldDto worldDto)
        {
            foreach (Transform child in ordersContainer)
            {
                Destroy(child.gameObject);
            }

            var ownCharacters = worldDto.OwnCharacters;

            foreach (var character in ownCharacters)
            {
                foreach (var order in character.Orders)
                {
                    var orderUIInstance = Instantiate(orderUIPrefab, ordersContainer);
                    orderUIInstance.Initialize(order);
                }
            }
        }
    }
    