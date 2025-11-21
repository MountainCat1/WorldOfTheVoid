using DefaultNamespace.Utilities;
using TMPro;
using UnityEngine;
using WorldOfTheVoid.Domain.Entities;

namespace DefaultNamespace
{
    public class CharacterObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameDisplay;
        
        public void Initialize(CharacterDto character, bool own)
        {
            nameDisplay.text = $"{character.Name}";
            nameDisplay.color = own ? Color.green : Color.white;

            transform.position = character.Position.ToUnityVector3();
        }
    }
}