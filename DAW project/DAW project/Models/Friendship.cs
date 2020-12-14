using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DAW_project.Models
{
    public class Friendship
    {
        [Key]
        public int FriendshipId { get; set; }
        //[Column(Order = 1), Key]
        //public string User1_Id { get; set; }
        public virtual ApplicationUser User1 { get; set; }

        //[Column(Order = 2), Key]
        //public string User2_Id { get; set; }
        public virtual ApplicationUser User2 { get; set; }

        public DateTime RequestTime { get; set; }

        public bool Accepted { get; set; }
    }
}