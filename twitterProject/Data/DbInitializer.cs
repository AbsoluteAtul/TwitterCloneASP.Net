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

            //Add users on the time of DB creation
            var users = new User[]
            {
            new User{FirstName="Than",LastName="Thao", Email="thao@gmail.com", ImageUrl="https://avatars.githubusercontent.com/u/56978370?v=4", Password="12345",},
             new User{FirstName="Tejvir",LastName="Singh", Email="tej@gmail.com",ImageUrl="https://avatars.githubusercontent.com/u/62151509?v=4" , Password="12345",},
              new User{FirstName="Waseq",LastName="Rahman", Email="waseq@gmail.com", ImageUrl="https://avatars.githubusercontent.com/u/56893007?v=4", Password="12345",},
                new User{FirstName="Atul",LastName="Rana", Email="atul@gmail.com", ImageUrl="https://avatars.githubusercontent.com/u/88677125?v=4", Password="12345",},
                new User{FirstName="Dhvanil",LastName="Sharma", Email="dhvanil@gmail.com", ImageUrl="https://avatars.githubusercontent.com/u/80846153?v=4", Password="12345",}
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