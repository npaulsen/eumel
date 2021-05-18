using System.ComponentModel.DataAnnotations.Schema;

namespace Eumel.Persistance.GameEvents
{
    public class PersistedEvent
    {
        public int Id { get; set; }

        // [Column(TypeName = "uuid")]
        public string GameUuid { get; set; }

        public int RoundIndex { get; set; }

        [Column(TypeName = "varchar(30)")]
        public PersistedEventType Type { get; set; }

        [Column(TypeName = "jsonb")]
        public string Payload { get; set; }
    }
}