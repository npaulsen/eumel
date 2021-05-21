namespace Eumel.Shared.HubInterface
{
    public class CurrentLobbyPlayersDto
    {
        /// <summary>
        /// The indices of players connected. 
        /// Can contain multiples, if there are multiple connections or a player.
        /// -1 are watchers.
        /// TODO: find an appropriate format for this :)
        /// </summary>
        public int[] PlayerConnections { get; set; }
    }
}