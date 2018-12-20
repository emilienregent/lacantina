using Json.Parser;
using La.Cantina.Data;
using La.Cantina.Enums;
using La.Cantina.Manager;
using La.Cantina.Parsers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace La.Cantina.Controller
{
    public class GameController : MonoBehaviour
    {
        private const string GAME_DATA_PATH = "JSON/Game_Data";
        private float _elapsedTime = 0f;

        [SerializeField] private SettingsScriptableObject _settings = null;

        private void Awake()
        {
            UnityEngine.Assertions.Assert.IsNotNull(Resources.Load<TextAsset>(GAME_DATA_PATH), "Can't find game data file at path : " + GAME_DATA_PATH);

            TextAsset   data = Resources.Load<TextAsset>(GAME_DATA_PATH);
            JSON        json = JSON.CreateFromText(data.text);

            GameManager.instance.vegetableIdToConfig    = FoodParser.ParseVegetables(json);
            GameManager.instance.incidentIdToConfig     = ChildrenParser.ParseIncidents(json);
            GameManager.instance.responseIdToConfig     = AdultParser.ParseResponses(json);
            GameManager.instance.levelIdToConfig        = LevelParser.ParseLevels(json);
            GameManager.instance.levelIds               = LevelParser.ComputeLevelIds(GameManager.instance.levelIdToConfig);

            GameManager.instance.TimerEnded += OnTimerEnded;

            GameManager.instance.SetReady(_settings);
        }

        private void Update()
        {
            if (GameManager.instance.isReady == true)
            {
                // Each second
                _elapsedTime += Time.deltaTime;

                if (_elapsedTime >= 1f)
                {
                    _elapsedTime = _elapsedTime % 1f;

                    GameManager.instance.UpdateTimer();
                }
            }
        }

        private void OnTimerEnded(object sender, int elapsedTime)
        {
            SceneManager.LoadScene((int)SceneEnum.END);
        }

        private void OnDestroy()
        {
            GameManager.Destroy();
        }
    }
}