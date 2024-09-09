using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity.Products
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? IdCategory { get; set; }
        public Guid? IdUnit { get; set; }
        public Guid? IdSuplier { get; set; }
        public string? CategoryName { get; set; }
        public string? UnitName { get; set; }
        public string? SuplierName { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string? ModifyBy { get; set; }
        public bool? IsActived { get; set; } = true;
    }
}
