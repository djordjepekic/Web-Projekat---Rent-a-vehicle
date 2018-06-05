using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class AppUser
    {     
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Adress { get; set; }
        [Column("DateOfBirth", TypeName = "datetime2")]
        public DateTime DateOfBirth { get; set; }
        public string Image { get; set; }
        public bool Verified { get; set; }
        public bool CanCreateService { get; set; }
    }
}