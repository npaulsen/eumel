using System.ComponentModel.DataAnnotations.Schema;

namespace Eumel.Persistance.GameEvents
{
    public class PersistedSeriesEvent
    {
        public int Id { get; set; }

        // [Column(TypeName = "uuid")]
        public string GameUuid { get; set; }

        [Column(TypeName = "varchar(30)")]
        public PersistedSeriesEventType Type { get; set; }

        [Column(TypeName = "jsonb")]
        public string Payload { get; set; }
    }
}