using Json.Parser;
using La.Cantina.Parsers;
using La.Cantina.Types;
using System.Collections.Generic;
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

            Dictionary<uint, VegetableConfig> vegetableIdToConfig = FoodParser.ParseVegetables(json);
            Dictionary<uint, IncidentConfig> incidentIdToConfig = ChildrenParser.ParseIncidents(json);
            Dictionary<uint, ResponseConfig> responseIdToConfig = AdultParser.ParseResponses(json);
        }
    }
}