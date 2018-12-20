using UnityEngine;

namespace La.Cantina.Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Score", order = 1)]
    public class ScoreScriptableObject : ScriptableObject
    {
        public int value = 0;
    }
}