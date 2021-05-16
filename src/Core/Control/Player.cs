using Eumel.Core.Players;

namespace Eumel.Core
{
    public class Player
    {
        public readonly PlayerInfo Info;
        public readonly IInvokablePlayer Invocable;

        protected Player(PlayerInfo info, IInvokablePlayer invokable)
        {
            Info = info;
            Invocable = invokable;
        }

        public static Player CreateHuman(string name) => new Player(new PlayerInfo(name, PlayerType.Human), null);
        public static Player CreateBot(string name) => new Player(new PlayerInfo(name, PlayerType.Human), new Opportunist());
        public static Player CreateCustom(string name, IInvokablePlayer invokable) => new Player(new PlayerInfo(name, PlayerType.Bot), invokable);
    }
}