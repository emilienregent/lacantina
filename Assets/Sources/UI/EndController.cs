using La.Cantina.Data;
using La.Cantina.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace La.Cantina.UI
{
    public class EndController : MonoBehaviour
    {
        // TODO: Reference EndPlayerController for both winner and losers

        [SerializeField] private EndPlayerController[]      _players    = new EndPlayerController[4];
        [SerializeField] private ScoreScriptableObject[]    _scores     = new ScoreScriptableObject[4];
        [SerializeField] private Color[]                    _colors     = new Color[4];
        [SerializeField] private SettingsScriptableObject   _settings   = null;

        private void Start()
        {
            Dictionary<int, int> playerIndexToScore = new Dictionary<int, int>();

            // TODO: Call SetPlayer method for each player and disable others
            for (int i = 0; i < _settings.numPlayers; ++i)
            {
                playerIndexToScore[i] = _scores[i].value;
            }

            List<KeyValuePair<int, int>> sortedList = playerIndexToScore.ToList();

            sortedList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            sortedList.Reverse();

            for (int i = 0; i < _players.Length; ++i)
            {
                if (i < _settings.numPlayers)
                {
                    _players[i].SetPlayer(sortedList[i].Key + 1, sortedList[i].Value, _colors[sortedList[i].Key]);
                }
                else
                {
                    _players[i].DisablePlayer();
                }
            }
        }

        private void Update()
        {
            // TODO: Check player input to load the start scene to play again

            for (int i = 0; i < _settings.numPlayers; ++i)
            {
                if (Input.GetButtonDown("Action1_P" + (i + 1)))
                {
                    SceneManager.LoadScene((int)SceneEnum.START);
                }
            }
            
        }
    }
}