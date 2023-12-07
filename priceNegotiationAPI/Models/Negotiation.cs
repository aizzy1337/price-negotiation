using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace priceNegotiationAPI.Models
{
    public class Negotiation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double ProposedPrice { get; set; }
        public bool Accepted { get; set; }
        public bool WasHandled { get; set; }

        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
