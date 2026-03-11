using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundaAssignment.Core.Models
{
    public class MakelaarListingCount
    {
        public int MakelaarId { get; set; }
        public string MakelaarNaam { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
