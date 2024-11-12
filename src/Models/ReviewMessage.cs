namespace connect.Models;

public class ReviewMessage
{
    public required string ReviewId { get; set; }
    public required string ProductId { get; set; }
    public int ReviewScore { get; set; }
}