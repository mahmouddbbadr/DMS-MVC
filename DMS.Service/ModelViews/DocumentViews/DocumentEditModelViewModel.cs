using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.ModelViews.DocumentViews
{
    public class DocumentEditModelViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [ValidateNever]
        public string OwnerId { get; set; }
        public IFormFile? File { get; set; }
    }
}
