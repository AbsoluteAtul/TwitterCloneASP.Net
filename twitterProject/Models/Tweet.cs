using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace twitterProject.Models
{
    public class Tweet
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Tweet description is required")]
        [StringLength(141, MinimumLength = 2)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        //Each tweet will have a user who posted it
        public User User { get; set; }
        //Each tweet will have a collection of likes
        public ICollection<Like> Likes { get; set; }
    }
}