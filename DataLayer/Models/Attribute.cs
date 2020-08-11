using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer
{
    

    public class Attributee
    {
        [Key]
        public int AttributeId { get; set; }
         [Display(Name =" گروه مشخصه")]
        public int AttributeGroupId { get; set; }
        public AttributeGroup attibuteGroup { get; set; }

        [Display(Name = "عنوان مشخصه")]   
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [MaxLength(250)]
        public string title { get; set; }

        public ICollection<ProductAttribute> productAttribute { get; set; }





      //  [Display(Name = "محصول ")]
    //    public int ProductId { get; set; }
    //    public Product product { get; set; }

    }
}
