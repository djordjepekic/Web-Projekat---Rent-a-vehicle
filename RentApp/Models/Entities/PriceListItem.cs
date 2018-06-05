using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("PriceListItems")]
    public class PriceListItem
    {
        [Key]
        public int PriceListItemId { get; set; }
        public float Price { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        [ForeignKey("PriceList")]
        public int PriceListId { get; set; }
    }
}