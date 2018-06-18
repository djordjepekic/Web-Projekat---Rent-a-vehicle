using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("Vehicles")]
    public class Vehicle 
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public bool Available { get; set; }
        public string Image { get; set; }

        [ForeignKey("VehicleType")]
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}