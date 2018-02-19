using System;
using System.ComponentModel.DataAnnotations;

namespace AzR.Core.HelperModels
{
    public class ReportHeader
    {
        public long Id { get; set; }

        public string Name { get; set; }
        [StringLength(128)]
        public string Code { get; set; }
        [StringLength(128)]
        public string UniqueName { get; set; }
        public string ReportName { get; set; }
        public string Slogan { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        [StringLength(500)]
        public string Logo { get; set; }
        [StringLength(500)]
        public string ImageUrl { get; set; }
        [StringLength(50)]
        public string MimeType { get; set; }
        [StringLength(128)]
        public string Email { get; set; }

        [StringLength(128)]
        public string Email2 { get; set; }
        [StringLength(128)]
        public string Mobile { get; set; }

        [StringLength(128)]
        public string Mobile2 { get; set; }
        public bool? HasSignatory { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}