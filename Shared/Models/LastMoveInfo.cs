using System.Collections.Generic;
using System.Text.Json.Serialization;
using Shared.Models.Enums;

namespace Shared.Models
{
    public record LastMoveInfo
    {
        public Player Player { get; init; }
        [JsonPropertyName("last_moves")]
        public List<List<int>> LastMoves { get; init; }
    }
}