using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer
{
    public class SilderImage
    {
        [Key]
        public int SilderId { get; set; }
         [Display(Name ="عنوان عکس")]
        public int CategoryId { get; set; }
        public Category category { get; set; }
         [Display(Name ="عکس")]   
        
        public string Image { get; set; }
    }
}
