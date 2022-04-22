namespace twitterProject.Models
{
    public class Like
    {
        public int TweetID { get; set; }
        public int UserID { get; set; }
        public Tweet Tweet { get; set; }
        public User User { get; set; }
    }
}