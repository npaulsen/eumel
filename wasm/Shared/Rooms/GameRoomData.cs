using System.ComponentModel.DataAnnotations;

namespace BlazorSignalRApp.Shared.Rooms
{
    public class GameRoomData
    {
        [Required]
        [StringLength(24)]
        public string Id { get; set; }

        [Required]
        [Range(3, 6)]
        public int PlayerCount { get; set; } = 3;
    }

}