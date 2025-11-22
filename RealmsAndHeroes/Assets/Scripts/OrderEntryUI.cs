using System.Linq;
using TMPro;
using UnityEngine;
using WorldOfTheVoid.Domain.Entities;

public class OrderEntryUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text orderTypeText;
        [SerializeField] private TMP_Text orderDataText;
        
        public void Initialize(OrderDto order)
        {
            orderTypeText.text = order.Type.ToString();
            orderDataText.text = string.Join(", ", order.Data.Select<object, string>(kv => $"{kv}"));
        }
    }