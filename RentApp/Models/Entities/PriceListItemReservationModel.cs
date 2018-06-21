using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class PriceListItemReservationModel
    {
        public int PriceListId { get; set; }
        public int VehicleId { get; set; }
    }
}