namespace La.Cantina.Types
{
    public class LevelConfig
    {
        private uint        _id                     = uint.MinValue;
        private string      _referenceId            = string.Empty;
        private string      _name                   = string.Empty;
        private int         _time                   = 0;
        private int         _children_per_player    = 0;
        private int         _tables_per_player      = 0;
        private int         _max_amount_players     = 0;

        public uint         id                       { get { return _id; } }
        public string       referenceId              { get { return _referenceId; } }
        public string       name                     { get { return _name;} }
        public int          time                     { get { return _time;} }
        public int          children_per_player      { get { return _children_per_player; } }
        public int          tables_per_player        { get { return _tables_per_player; } }
        public int          max_amount_players       { get { return _max_amount_players; } }

        public LevelConfig(uint id, string reference, string name, int time, int children_per_player, int tables_per_player, int max_amount_players)
        {
            _id                     = id;
            _referenceId            = reference;
            _name                   = name;
            _time                   = time;
            _children_per_player    = children_per_player;
            _tables_per_player      = tables_per_player;
            _max_amount_players     = max_amount_players;
        }
    }
}