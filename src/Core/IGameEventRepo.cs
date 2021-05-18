namespace Eumel.Core
{
    public interface IGameEventRepo
    {
        public void StoreEvent(GameEvent gameEvent);
        public GameProgress GetGameProgress(string gameUuid);
    }
}