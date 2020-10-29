namespace EumelCore
{
    public class EumelRoundSettings
    {
        public readonly int StartingPlayerIndex;

        public readonly int TricksToPlay;

        public EumelRoundSettings(int startingPlayerIndex, int tricksToPlay)
        {
            StartingPlayerIndex = startingPlayerIndex;
            TricksToPlay = tricksToPlay;
        }
    }
}