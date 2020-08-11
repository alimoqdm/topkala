using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer
{
   public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        [Display(Name = "محصول")]
        public int ProductId { get; set; }
        public Product product { get; set; }
        [Display(Name = "سوال")]  
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [MaxLength(500)]
        public string ask { get; set; }
        [Display(Name = "پاسخ")]
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [MaxLength(500)]
        public string Answer { get; set; }
    }
}
