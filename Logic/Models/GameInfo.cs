namespace Logic.Models
{
    public record GameInfo
    {
        public string Status { get; init; }
        public GameInfoData Data { get; init; }
    }
}