using Eumel.Core.GameSeriesEvents;

namespace Eumel.Core
{
    public interface IGameEventRepo
    {
        public void StoreEvent(GameEvent gameEvent);
        public void StoreSeriesEvent(GameSeriesEvent gameEvent);
        public void DeleteOutdatedEvents(string gameUuid);
        public GameProgress GetGameProgress(string gameUuid);
    }
}