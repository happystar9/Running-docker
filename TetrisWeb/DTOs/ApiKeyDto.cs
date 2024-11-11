namespace TetrisWeb.DTOs;

public class ApiKeyDto
{
    public int Id { get; set; }

    public int? PlayerId { get; set; }

    public string Key { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime? ExpiredOn { get; set; }
    public bool IsExpired => ExpiredOn.HasValue && ExpiredOn <= DateTime.Now;

}
