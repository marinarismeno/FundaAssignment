using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FundaAssignment.Core.Models
{
    public class FundaApiResponse
    {
        public List<Listing> Objects { get; set; } = new();
        public Paging Paging { get; set; } = new();
        public int TotaalAantalObjecten { get; set; }
    }

    public class Listing
    {
        public int MakelaarId { get; set; }
        public string MakelaarNaam { get; set; } = string.Empty;
    }

    public class Paging
    {
        public int AantalPaginas { get; set; }
        public int HuidigePagina { get; set; }
        public string VolgendeUrl { get; set; } = string.Empty; 
        public string VorigeUrl { get; set; } = string.Empty;
    }
}
