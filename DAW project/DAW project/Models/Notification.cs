using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DAW_project.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        
        public string Text { get; set; }

        [ForeignKey("Admin")]
        public string Admin_Id { get; set; }
        public virtual ApplicationUser Admin { get; set; }

        [ForeignKey("Post")]
        public int Post_Id { get; set; }
        public virtual Post Post { get; set; }

        public DateTime RequestTime { get; set; }
    }
}