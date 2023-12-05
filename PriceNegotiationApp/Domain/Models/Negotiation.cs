namespace Domain.Models;

public class Negotiation
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal ProposedPrice { get; set; }
    public int NegotiationAttempts { get; set; }
    public bool IsAccepted { get; set; } = false;
    public bool IsWaitingForResponse { get; set; } = false;
}
