using Shared.Models.Enums;

namespace Shared.Models
{
    public record ConnectData
    {
        public Player Color { get; init; }
        public string Token { get; init; }
    }
}