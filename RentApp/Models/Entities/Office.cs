using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("Offices")]
    public class Office
    {		    
		[Key]
        public int OfficeId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Adress { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }

        [ForeignKey("Service")]
        public int SerivceId { get; set; }
    }
}