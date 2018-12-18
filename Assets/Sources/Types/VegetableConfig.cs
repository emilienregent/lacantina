namespace La.Cantina.Types
{
    public class VegetableConfig
    {
        private uint        _id             = uint.MinValue;
        private string      _referenceId    = string.Empty;
        private string      _name           = string.Empty;
        private string      _difficulty     = string.Empty;
        private float       _timeToIncident = 0f;
        private float       _timeToEat      = 0f;
        private int         _points         = 0;

        public uint        id               {get {return _id; } }
        public string      referenceId      {get {return _referenceId; } }
        public string      name             {get {return _name;} }
        public string      difficulty       {get {return _difficulty;} }
        public float       timeToIncident   {get {return _timeToIncident; } }
        public float       timeToEat        {get {return _timeToEat; } }
        public int         points           {get {return _points;} }

        public VegetableConfig(uint id, string reference, string name, string difficulty, float timeToIncident, float timeToEat, int points)
        {
            _id             = id;
            _referenceId    = reference;
            _name           = name;
            _difficulty     = difficulty;
            _timeToIncident = timeToIncident;
            _timeToEat      = timeToEat;
            _points         = points;
        }
    }
}