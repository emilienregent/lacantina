using Json.Parser;
using La.Cantina.Manager;
using La.Cantina.Parsers;
using UnityEngine;

namespace La.Cantina.Controller
{
    public class GameController : MonoBehaviour
    {
        private const string GAME_DATA_PATH = "JSON/Game_Data";

        private void Awake()
        {
            UnityEngine.Assertions.Assert.IsNotNull(Resources.Load<TextAsset>(GAME_DATA_PATH), "Can't find game data file at path : " + GAME_DATA_PATH);

            TextAsset   data = Resources.Load<TextAsset>(GAME_DATA_PATH);
            JSON        json = JSON.CreateFromText(data.text);

            GameManager.instance.vegetableIdToConfig = FoodParser.ParseVegetables(json);
            GameManager.instance.incidentIdToConfig  = ChildrenParser.ParseIncidents(json);
            GameManager.instance.responseIdToConfig  = AdultParser.ParseResponses(json);

            GameManager.instance.SetReady();
        }
    }
}