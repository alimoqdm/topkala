using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer
{
   public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Display(Name = "محصول")]
       
        public int ProductId { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string Name { get; set; }

        [Display(Name = "نظر")]
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [MaxLength(500)]
        public string Text { get; set; }
        

        [Display(Name = "تاریخ ثبت")]
        [DisplayFormat(DataFormatString = "{0: yyyy/mm/dd}")]
        public DateTime CreatDate { get; set; }

        public Product product { get; set; }
    }
}
