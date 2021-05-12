using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EkiHire.Core.Model
{
    public class SearchVM
    {
        public string SearchText { get; set; }
        public List<string> Keywords { get; set; }
        public long? SubcategoryId { get; set; }
        public long? CategoryId { get; set; }
    }
}
