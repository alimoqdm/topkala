using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer
{
    public class AttributeGroup
    {
        [Key]
        public int AttributeGroupId { get; set; }
         [Display(Name ="دسته")]
        public int CategoryId { get; set; }
        public Category category { get; set; }

        [Display(Name ="گروه مشخصه")]  
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [MaxLength(250)]
        public string title { get; set; }
        public ICollection<Attributee> attributes { get; set; }

    }
}

