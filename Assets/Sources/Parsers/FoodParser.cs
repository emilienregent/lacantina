using Json.Parser;
using La.Cantina.Types;
using System.Collections.Generic;

namespace La.Cantina.Parsers
{
    public class FoodParser
    {
        private const string ROOT               = "Food_Define";
        private const string VEGETABLE_ID       = "Vegetable_ID";
        private const string VEGETABLE_ID_CRC   = "Vegetable_ID_CRC";
        private const string NAME               = "Name";
        private const string DIFFICULTY         = "Difficulty";
        private const string TIME_TO_INCIDENT   = "time_to_incident";
        private const string TIME_TO_EAT        = "time_to_eat";
        private const string POINTS             = "Points";

        public static Dictionary<uint, VegetableConfig> ParseVegetables(JSON json)
        {
            Dictionary<uint, VegetableConfig>   vegetables              = new Dictionary<uint, VegetableConfig>();
            List<JSON>	                        vegetableConfigJSONs	= json.ToJSONList(ROOT);
			int			                        vegetableCount			= vegetableConfigJSONs.Count;
			
            for (int i = 0; i < vegetableCount; ++i)
            {
                VegetableConfig vegetableConfig = ParseVegetable(vegetableConfigJSONs[i]);
                vegetables.Add(vegetableConfig.id, vegetableConfig);
            }

            return vegetables;
        }

        public static VegetableConfig ParseVegetable(JSON json)
        {
            VegetableConfig config = new VegetableConfig(
                json.ToUInt(VEGETABLE_ID_CRC),
                json.ToString(VEGETABLE_ID),
                json.ToString(NAME),
                json.ToString(DIFFICULTY),
                json.ToFloat(TIME_TO_INCIDENT),
                json.ToFloat(TIME_TO_EAT),
                json.ToInt(POINTS)
            );

            return config;
        }
    }
}
