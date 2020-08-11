using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer
{
    public enum Level
    {
        Level1, Level2, Level3, Level4
    }
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
      
        [Display(Name ="نام گروه")] 
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [MaxLength(250)]
        public string Name { get; set; }

        [Display(Name = "سطح")]
        public Level Level { get; set; }

        public Category Parent { get; set; }
        [Display(Name = "پدر")]
        public int? ParentId { get; set; }

        public ICollection<Category> Categories { get; set; }

        public ICollection<Product> products { get; set; }
        public ICollection<AttributeGroup> AttributeGroups { get; set; }
        public ICollection<SilderImage> SilderImage { get; set; }
        public ICollection<Brand> Brands { get; set; }


        public Category()
        {
            Categories = new List<Category>();
        }

       
    }
 
}
