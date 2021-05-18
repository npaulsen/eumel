using System;
using System.Collections.Generic;

namespace Eumel.Core.Players
{
    public interface IPlayerFactory
    {
        bool CanCreatePlayerOfType(string type);
        IInvokablePlayer GetNewPlayerOfType(string type);
        IPlayerFactory RegisterOrOverrideCreator(string key, Func<IInvokablePlayer> creator);
    }

    public class PlayerFactory : IPlayerFactory
    {
        private readonly Dictionary<string, Func<IInvokablePlayer>> _creators;

        public PlayerFactory()
        {
            _creators = new Dictionary<string, Func<IInvokablePlayer>>();
            RegisterOrOverrideCreator(PlayerType.Human, () => null);
            RegisterOrOverrideCreator(PlayerType.Bot, () => new Opportunist());
        }

        public IPlayerFactory RegisterOrOverrideCreator(string key, Func<IInvokablePlayer> creator)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException(nameof(key));
            }
            if (creator is null)
            {
                throw new ArgumentException(nameof(creator));
            }
            _creators[key] = creator;
            return this;
        }

        public bool CanCreatePlayerOfType(string type) => _creators.ContainsKey(type);

        public IInvokablePlayer GetNewPlayerOfType(string type)
        {
            if (!CanCreatePlayerOfType(type))
            {
                throw new ArgumentException(nameof(type));
            }
            return _creators[type].Invoke();
        }
    }
}