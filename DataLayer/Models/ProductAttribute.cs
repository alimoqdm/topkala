using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer
{
    public class ProductAttribute
    {
        [Key]
        public int ProductAttributeId { get; set; }
        [Display(Name =" مشخصه")]
      
        public int AttributeId { get; set; }
        public Attributee attribute { get; set; }
        [Display(Name = " محصول")]
      
        public int ProductId { get; set; }
        
        public Product product { get; set; }

        [Display(Name = "مقدار مشخصه")]
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [MaxLength(600)]
        public string Value { get; set; }

    }
}
