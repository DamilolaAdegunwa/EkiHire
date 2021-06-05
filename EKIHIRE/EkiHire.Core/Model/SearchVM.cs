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
        public long? AdId { get; set; }
        //min_amount, max_amount, locations and properties
        public decimal? min_amount { get; set; }
        public decimal? max_amount { get; set; }
        public string StateId { get; set; }
        public string LGA { get; set; }
        public int Ad { get; set; }
    }
}
