using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer
{
   public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Display(Name ="نام محصول")] 
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [MaxLength(250)]
        public string Name { get; set; }
        [Display(Name = "نام انگلیسی محصول")]
     
        [MaxLength(250)]
        public string EnglishName { get; set; }
        [Display(Name = "برند")]
        [MaxLength(250)]
        public string Brand { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
    
        public int Price { get; set; }

        [Display(Name = "قیمت با تخفیف")]
      
        public int PriceWithReduction { get; set; }

        [Display(Name = "تخفیف")]
        public int Reduction{ get; set; }

        [Display(Name = "محصول ویژه")]
        public bool IsSpecial { get; set; }

        [Display(Name = "فروشنده")]
        public string Seller { get; set; } = "تاپ کالا";


        [Display(Name = "کلمات کلیدی")]
        public string Tags { get; set; }

        [Display(Name = "نقد و بررسی")]
        [DataType(DataType.MultilineText)]
        public string Review { get; set; }

         [Display(Name ="بازدید")]
        public int Visit { get; set; }

         [Display(Name ="تعداد فروش")]
        public int CountSell { get; set; }


        [Display(Name ="تاریخ ایجاد")]
        public DateTime CreatDateTime { get; set; }

        public string UserLike { get; set; }

        [Display(Name = "گروه محصول ")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }


        [Display(Name = "برند")]
        public int BrandId { get; set; }
        public Brand Brands { get; set; }



        public ICollection<Properties> properties { get; set; }
        public ICollection<ProductColor> productColors { get; set; }
        public ICollection<ProductPathImage>  productPathImages { get; set; }
        public ICollection<Comment>  comments { get; set; }
        public ICollection<Question>  questions { get; set; }
 
        public ICollection<ProductAttribute>  productAttributes { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }


        public Product()
        {
            productPathImages = new List<ProductPathImage>();
        }
    }
}
