using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("PriceListItems")]
    public class PriceListItem
    {
        public int Id { get; set; }
        public float Price { get; set; }

        [ForeignKey("PriceList")]
        public int? PriceListId { get; set; }
        public virtual PriceList PriceList{ get; set; }
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}