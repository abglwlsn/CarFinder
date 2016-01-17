using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarFinder.Models
{
    public partial class Recalls
    {
        [StringLength(50)]
        public string ModelYear { get; set; }

        [StringLength(50)]
        public string Make { get; set;}

        [StringLength(50)]
        public string Model { get; set;}

        [StringLength(50)]
        public string Manufacturer { get; set;}

        [StringLength(50)]
        public string NHTSACampaignNumber { get; set;}

        [StringLength(50)]
        public string NHTSAActionNumber { get; set; }

        [StringLength(50)]
        public string ReportReceivedDate { get; set; }

        [StringLength(50)]
        public string Component { get; set; }

        [StringLength(500)]
        public string Summary { get; set; }

        [StringLength(200)]
        public string Conequence { get; set; }

        [StringLength(200)]
        public string Remedy { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

    }
}