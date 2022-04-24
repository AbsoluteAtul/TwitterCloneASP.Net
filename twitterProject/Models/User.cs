using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace twitterProject.Models
{
    [Table("User")]
    public class User
    {
        //If u put the name as Id then it will automatically detect it as a key.
        //Otherwise you will need to write the tag [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "First Name")]
        [Column("FirtName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Last Name")]
        [Column("LastName")]
        public string LastName { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        [Column("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "ImageUrl is required")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 5)]
        [Column("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        //Each user will have a collection of tweets, likes and follows.
        public ICollection<Tweet> Tweets { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Follow> Follows { get; set; }
    }
}