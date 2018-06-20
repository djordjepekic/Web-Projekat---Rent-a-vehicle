using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class ReservationModel
    {
        public string UserName { get; set; }
        public int VehicleId { get; set; }
        public DateTime TimeOfReservation { get; set; }
        public DateTime TimeToReturn { get; set; }
        public int TakeOfficeId { get; set; }
        public int ReturnOfficeId { get; set; }
    }
}