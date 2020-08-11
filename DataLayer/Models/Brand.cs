using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }

        public Category  Category { get; set; }
        [Display(Name = "گروه برند ")]
        public int CategoryId { get; set; }

        [Display(Name = "نام برند")]
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [MaxLength(250)]
        public string Value { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
