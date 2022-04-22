﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace twitterProject.Models
{
    public class Tweet
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Tweet description is required")]
        [StringLength(140, MinimumLength = 2)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public User User { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}