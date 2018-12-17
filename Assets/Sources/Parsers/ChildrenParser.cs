using Json.Parser;
using La.Cantina.Types;
using System.Collections.Generic;

namespace La.Cantina.Parsers
{
    public class ChildrenParser
    {
        private const string ROOT       = "Incident_Define";
        private const string ID         = "Incident_ID";
        private const string ID_CRC     = "Incident_ID_CRC";
        private const string NAME       = "Name";

        public static Dictionary<uint, IncidentConfig> ParseIncidents(JSON json)
        {
            Dictionary<uint, IncidentConfig>    incidents               = new Dictionary<uint, IncidentConfig>();
            List<JSON>	                        incidentConfigJSONs	    = json.ToJSONList(ROOT);
			int			                        incidentCount			= incidentConfigJSONs.Count;
			
            for (int i = 0; i < incidentCount; ++i)
            {
                IncidentConfig incidentConfig = ParseIncident(incidentConfigJSONs[i]);
                incidents.Add(incidentConfig.id, incidentConfig);
            }

            return incidents;
        }

        public static IncidentConfig ParseIncident(JSON json)
        {
            IncidentConfig config = new IncidentConfig(
                json.ToUInt(ID_CRC),
                json.ToString(ID),
                json.ToString(NAME)
            );

            return config;
        }
    }
}
