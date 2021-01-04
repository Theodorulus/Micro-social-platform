using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DAW_project.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Grupul trebuie sa aiba nume!")]
        public string GroupName { get; set; }

        public DateTime DateCreated { get; set; }
        
        public string GroupAdmin_Id { get; set; }

        //public virtual ApplicationUser GroupAdmin { get; set; }

        public virtual ICollection<Post> GroupPosts { get; set; }

        public virtual ICollection<ApplicationUser> GroupUsers { get; set; }
    }
}