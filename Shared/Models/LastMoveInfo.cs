using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Shared.Models.Enums;

namespace Shared.Models
{
    public record LastMoveInfo
    {
        public Player Player { get; init; }

        private List<List<int>> _lastMoves;
        
        public List<(int from, int to)> Moves { get; private set; }

        [JsonPropertyName("last_moves")]
        public List<List<int>> LastMoves
        {
            set
            {
                _lastMoves = value;
                Moves = value?.Select(move => (move.First(), move.Last())).ToList() ?? new List<(int from, int to)>();
            }
            get => _lastMoves;
        }
    }
}