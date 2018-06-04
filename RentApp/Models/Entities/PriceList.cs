using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("PriceLists")]
    public class PriceList
    {
		[Key]
        public int PriceListId { get; set; }

        [Column("TimeOfReservation", TypeName = "datetime2")]
        public DateTime TimeOfReservation { get; set; }
        [Column("TimeToReturn", TypeName = "datetime2")]
        public DateTime TimeToReturn { get; set; }
    }
}