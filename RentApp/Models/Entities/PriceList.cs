using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("PriceLists")]
    public class PriceList
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public AppUser User { get; set; }
        [Column("TimeOfReservation", TypeName = "datetime2")]
        public DateTime TimeOfReservation { get; set; }
        [Column("TimeToReturn", TypeName = "datetime2")]
        public DateTime TimeToReturn { get; set; }

        //[InverseProperty("PriceListTakeOffice")]
        [ForeignKey("TakeOffice")]
        public int? TakeOfficeId { get; set; }
        public virtual Office TakeOffice { get; set; }
        //[InverseProperty("PriceListReturnOffice")]
        [ForeignKey("ReturnOffice")]
        public int? ReturnOfficeId { get; set; }
        public virtual Office ReturnOffice { get; set; }

    }
}