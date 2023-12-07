namespace priceNegotiationAPI.Models.Dto
{
    public class NegotiationDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public double ProposedPrice { get; set; }
        public bool Accepted { get; set; }
        public bool WasHandled { get; set; }
    }
}
