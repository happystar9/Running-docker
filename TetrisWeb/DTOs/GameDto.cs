using TetrisWeb.ApiServices;

namespace TetrisWeb.DTOs
{
    public class GameDto
    {
        public int Id { get; set; }

        public string CreatedByAuthId { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime? StopTime { get; set; }

        public int? PlayerCount { get; set; }

        public List<GameSessionService> Sessions { get; set; }
    }
}
