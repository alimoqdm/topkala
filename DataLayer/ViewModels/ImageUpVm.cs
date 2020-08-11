using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;


namespace DataLayer
{
    public class ImageUpVm
    {
        public IFormFile Photo { get; set; }
    }
}
