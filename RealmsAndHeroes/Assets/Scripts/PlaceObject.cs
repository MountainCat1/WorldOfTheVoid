using DefaultNamespace.Utilities;
using TMPro;
using UnityEngine;
using WorldOfTheVoid.Domain.Entities;

namespace DefaultNamespace
{
    public class PlaceObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameDisplay;
        
        public void Initialize(PlaceDto place)
        {
            nameDisplay.text = $"{place.Name} ({place.Population})";

            transform.position = place.Position.ToUnityVector3();
        }
    }
}