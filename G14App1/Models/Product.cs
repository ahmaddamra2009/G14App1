using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace G14App1.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [DisplayName("Product Name")]
        [Required(ErrorMessage ="Enter Product Name")]
        public string ProductName { get; set; }
        [Required]
        [Range(100,500,ErrorMessage ="Range Between 100 - 500")]
        public decimal Price { get; set; }
        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Enter Category Name")]
        public string CategoryName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Url)]
        public string WebUrl { get; set; }

    }
}
