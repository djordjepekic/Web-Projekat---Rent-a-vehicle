using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("Users")]
    public class AppUser
    {     
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        [Column("DateOfBirth", TypeName = "datetime2")]
        public DateTime DateOfBirth { get; set; }
        public string Image { get; set; }
        public bool Verified { get; set; }
        public bool CanCreateService { get; set; }
    }
}