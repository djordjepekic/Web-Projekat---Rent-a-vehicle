using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("Offices")]
    public class Office
    {		    
        public int Id { get; set; }
        public string Image { get; set; }
        public string Adress { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }

        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}