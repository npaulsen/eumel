namespace Server.Models
{
    public class PlayerInfo
    {
        public readonly string Name;
        public readonly bool IsHuman;

        public PlayerInfo(string name, bool isHuman)
        {
            Name = name;
            IsHuman = isHuman;
        }
    }
}