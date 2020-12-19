namespace Shared.Models
{
    public record Connect
    {
        public string Status { get; init; }
        public ConnectData Data { get; init; }
    }
}