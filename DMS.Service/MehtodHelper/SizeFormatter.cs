using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.MehtodHelper
{
    public static class SizeFormatter
    {
        public static string FormatSize(long bytes)
        {
            if (bytes < 1024) return $"{bytes} B";
            double kb = bytes / 1024.0;
            if (kb < 1024) return $"{Math.Round(kb, 2)} KB";
            double mb = kb / 1024.0;
            if (mb < 1024) return $"{Math.Round(mb, 2)} MB";
            double gb = mb / 1024.0;
            return $"{Math.Round(gb, 2)} GB";
        }
    }
}
