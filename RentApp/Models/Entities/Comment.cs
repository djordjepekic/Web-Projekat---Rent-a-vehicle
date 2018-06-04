using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public int Rating { get; set; }
        public string CommentText { get; set; }

        [ForeignKey("User")]
        public int ClientId{ get; set; }
    }
}