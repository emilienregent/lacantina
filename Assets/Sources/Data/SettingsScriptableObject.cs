using UnityEngine;

namespace La.Cantina.Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Settings", order = 2)]
    public class SettingsScriptableObject : ScriptableObject
    {
        public int numPlayers = 1;
    }
}
