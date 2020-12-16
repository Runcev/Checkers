using Shared.Models.Enums;

namespace Shared.Models
{
    public record Square
    {
        public Player Color { get; init; }
        public int Row { get; init; }
        public int Column { get; init; }
        public bool King { get; init; }
        public int Position { get; init; }
    }
}