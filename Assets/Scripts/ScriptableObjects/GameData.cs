using UnityEngine;

namespace ScriptableObjects
{
    [System.Serializable]
    [CreateAssetMenu]
    public class GameData : ScriptableObject
    {
        public string selectedCategoryName;
        public BoardData selectedBoardData;
        
    }
}
