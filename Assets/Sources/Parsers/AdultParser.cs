using Json.Parser;
using La.Cantina.Types;
using System.Collections.Generic;

namespace La.Cantina.Parsers
{
    public class AdultParser
    {
        private const string ROOT_DEFINE    = "Response_Define";
        private const string ROOT_CALC      = "Response_Calc";
        private const string ID             = "Response_ID";
        private const string ID_CRC         = "Response_ID_CRC";
        private const string NAME           = "Name";
        private const string DIFFICULTY     = "Difficulty";
        private const string TIME           = "Time";
        private const string POINTS         = "Points";

        private const string CALC_RESPONSE_REF  = "Response_CRC";
        private const string CALC_INCIDENT_REF  = "Incident_CRC";
        private const string CALC_RESULT        = "Positive_Result";

        public static Dictionary<uint, ResponseConfig> ParseResponses(JSON json)
        {
            Dictionary<uint, ResponseConfig>    responses               = new Dictionary<uint, ResponseConfig>();
            List<JSON>	                        responseConfigJSONs	    = json.ToJSONList(ROOT_DEFINE);
            List<JSON>	                        responseCalcJSONs       = json.ToJSONList(ROOT_CALC);
			int			                        responseCount			= responseConfigJSONs.Count;
			
            for (int i = 0; i < responseCount; ++i)
            {
                ResponseConfig response = ParseResponse(responseConfigJSONs[i]);

                CalculateResponseToIncident(responseCalcJSONs, response);

                responses.Add(response.id, response);
            }

            return responses;
        }

        private static ResponseConfig ParseResponse(JSON json)
        {
            ResponseConfig config = new ResponseConfig(
                json.ToUInt(ID_CRC),
                json.ToString(ID),
                json.ToString(NAME),
                json.ToFloat(TIME),
                json.ToInt(POINTS)
            );

            return config;
        }

        private static void CalculateResponseToIncident(List<JSON> jsons, ResponseConfig response)
        {
            int count = jsons.Count;

            for (int i = 0; i < count; ++i)
            {
                if (response.id == jsons[i].ToUInt(CALC_RESPONSE_REF))
                {
                    response.incidentIdToResult.Add(jsons[i].ToUInt(CALC_INCIDENT_REF), jsons[i].ToBoolean(CALC_RESULT));
                }
            }
        }
    }
}