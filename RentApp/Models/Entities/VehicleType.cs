using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("VehicleTypes")]
    public class VehicleType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
    }
}