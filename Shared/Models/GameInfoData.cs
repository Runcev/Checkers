using System.Text.Json.Serialization;
using Shared.Models.Enums;

namespace Shared.Models
{
    public record GameInfoData
    {
        public string Status { get; init; }
        [JsonPropertyName("whose_turn")]
        public Player WhoseTurn { get; init; }
        public Player? Winner { get; init; }
        public Square[] Board { get; init; }
        [JsonPropertyName("available_time")]
        public double AvailableTime { get; init; }
        [JsonPropertyName("is_started")]
        public bool IsStarted { get; init; }
        [JsonPropertyName("is_finished")]
        public bool IsFinished { get; init; }        
        [JsonPropertyName("last_move")]
        public LastMoveInfo LastMove { get; init; }
    }
}