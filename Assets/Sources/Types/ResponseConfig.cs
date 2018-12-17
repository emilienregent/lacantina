using System.Collections.Generic;

namespace La.Cantina.Types
{
    public class ResponseConfig
    {
        private uint                    _id                 = uint.MinValue;
        private string                  _referenceId        = string.Empty;
        private string                  _name               = string.Empty;
        private float                   _time               = 0f;
        private int                     _points             = 0;
        private Dictionary<uint, bool>  _incidentIdToResult = new Dictionary<uint, bool>();

        public uint                     id                  {get {return _id; } }
        public string                   referenceId         {get {return _referenceId; } }
        public string                   name                {get {return _name;} }
        public float                    time                {get {return _time;} }
        public int                      points              {get {return _points;} }
        public Dictionary<uint, bool>   incidentIdToResult  { get { return _incidentIdToResult; } }

        public ResponseConfig(uint id, string reference, string name, float time, int points)
        {
            _id             = id;
            _referenceId    = reference;
            _name           = name;
            _time           = time;
            _points         = points;
        }
    }
}