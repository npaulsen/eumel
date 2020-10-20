namespace EumelCore
{
    public class MadeGuess : Move
    {
        public readonly int Count;

        public MadeGuess(PlayerInfo player, int count) : base(player)
        {
            Count = count;
        }
    }

}