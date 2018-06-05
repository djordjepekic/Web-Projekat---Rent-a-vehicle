using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string CommentText { get; set; }

        [ForeignKey("User")]
        public int ClientId{ get; set; }
        public AppUser User { get; set; }
    }
}