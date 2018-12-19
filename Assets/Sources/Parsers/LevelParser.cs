using Json.Parser;
using La.Cantina.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La.Cantina.Parsers
{
    public class LevelParser
    {
        private const string ROOT               = "Level_define";
        private const string ID                 = "Level_id";
        private const string ID_CRC             = "Level_id_CRC";
        private const string NAME               = "battle_name";
        private const string TIME               = "time_sec";
        private const string CHILDREN_COUNT     = "children_per_player";
        private const string TABLE_COUNT        = "tables_per_player";
        private const string PLAYER_COUNT       = "max_amount_players";

        public static Dictionary<uint, LevelConfig> ParseLevels(JSON json)
        {
            Dictionary<uint, LevelConfig>   levels              = new Dictionary<uint, LevelConfig>();
            List<JSON>	                    levelConfigJSONs	= json.ToJSONList(ROOT);
			int			                    levelCount			= levelConfigJSONs.Count;
			
            for (int i = 0; i < levelCount; ++i)
            {
                LevelConfig levelConfig = ParseLevel(levelConfigJSONs[i]);
                levels.Add(levelConfig.id, levelConfig);
            }

            return levels;
        }

        public static LevelConfig ParseLevel(JSON json)
        {
            LevelConfig config = new LevelConfig(
                json.ToUInt(ID_CRC),
                json.ToString(ID),
                json.ToString(NAME),
                json.ToInt(TIME),
                json.ToInt(CHILDREN_COUNT),
                json.ToInt(TABLE_COUNT),
                json.ToInt(PLAYER_COUNT)
            );

            return config;
        }

        public static List<uint> ComputeLevelIds(Dictionary<uint, LevelConfig> levels)
        {
            List<uint> levelIds = new List<uint>();

            foreach (KeyValuePair<uint, LevelConfig> pair in levels)
            {
                levelIds.Add(pair.Key);
            }

            return levelIds;
        }
    }
}