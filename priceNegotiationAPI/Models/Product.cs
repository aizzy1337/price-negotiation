using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace priceNegotiationAPI.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public double Price { get; set; }

        public ICollection<Negotiation> Negotiations { get; } = new List<Negotiation>();

        public DateTime CreatedDate { get; set; }
    }
}
