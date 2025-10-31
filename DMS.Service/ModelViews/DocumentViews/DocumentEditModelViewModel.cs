using Microsoft.AspNetCore.Http;
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
        public IFormFile? File { get; set; }
    }
}
