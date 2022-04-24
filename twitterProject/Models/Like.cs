namespace twitterProject.Models
{
    public class Like
    {
        public int TweetID { get; set; }
        public int UserID { get; set; }

        //Each like will have a User who posted the like and a tweet on which the like is posted
        public Tweet Tweet { get; set; }
        public User User { get; set; }
    }
}