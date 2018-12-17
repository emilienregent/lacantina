namespace La.Cantina.Types
{
    public class IncidentConfig
    {
        private uint        _id             = uint.MinValue;
        private string      _referenceId    = string.Empty;
        private string      _name           = string.Empty;

        public uint        id               {get {return _id; } }
        public string      referenceId      {get {return _referenceId; } }
        public string      name             {get {return _name;} }

        public IncidentConfig(uint id, string reference, string name)
        {
            _id             = id;
            _referenceId    = reference;
            _name           = name;
        }
    }
}