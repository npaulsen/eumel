using System.ComponentModel.DataAnnotations;

namespace BlazorSignalRApp.Shared.Rooms
{
    public class GameRoomData
    {
        public const int MinPlayers = 3;
        public const int MaxPlayers = 6;

        [Required]
        [StringLength(24)]
        public string Id { get; set; }

        [Required]
        [Range(3, 6)]
        public int PlayerCount { get; set; } = 3;

        [Required]
        [MinLength(MinPlayers)]
        [MaxLength(MaxPlayers)]
        public GamePlayerData[] Players { get; set; }
    }

    public class GamePlayerData
    {
        [Required]
        [StringLength(24)]
        public string Name { get; set; }

        public bool IsHuman { get; set; }

    }

}