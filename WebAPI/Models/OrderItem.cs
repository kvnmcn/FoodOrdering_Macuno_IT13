using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebAPI.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("menu_item_id")]

        public int MenuItemId { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        [NotMapped]
        public decimal Subtotal => Quantity * Price;
    }
}
