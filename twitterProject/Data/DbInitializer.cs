using twitterProject.Models;
using System;
using System.Linq;

namespace twitterProject.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TwitterContext context)
        {
            context.Database.EnsureCreated();

            // Look for any users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
            new User{FirstName="Than",LastName="Thao", Email="thao@gmail.com", Password="1234",},
             new User{FirstName="Tejvir",LastName="Singh", Email="tej@gmail.com", Password="1234",},
              new User{FirstName="Waseq",LastName="Rahman", Email="waseq@gmail.com", Password="1234",},
                new User{FirstName="Atul",LastName="Rana", Email="atul@gmail.com", Password="1234",},
                new User{FirstName="Dhvanil",LastName="Sharma", Email="dhvanil@gmail.com", Password="1234",}
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();

            //var tweets = new Tweet[]
            //{
            //new Tweet{ Description = "Hello", CreatedDate = DateTime.Parse("2001-09-01"), User = users[0]},
            //new Tweet{ Description = "Hi", CreatedDate = DateTime.Parse("2001-09-01"), User = users[1]}
            //new Course{CourseID=4022,Title="Microeconomics",Credits=3},
            //new Course{CourseID=2042,Title="Literature",Credits=4}
            //};
            //foreach (Tweet c in tweets)
            //{
            //    context.Tweets.Add(c);
            //}
            //context.SaveChanges();
        }
    }
}