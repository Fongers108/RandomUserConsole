using Newtonsoft.Json;
using Nito.AsyncEx;
using RandomUserConsole.Context;
using RandomUserConsole.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RandomUserConsole
{
    /*  Source: https://randomuser.me/
        1. pull down 5 users using HttpClient class, 
        2. store the results of their name, email, phone, cell into an entity framework code first database 
        3. all of this should be done asynchronously using Async/await style where applicable and 
        4. in a console application.

        Added package Entity Framework
        Added package Nito.AsyncEx
        Added package Newtonsoft.Json

        Enabled Migrations

        There are all sorts of additional small details not included in the code. For example, using try/catch for 
        exception handling and better separation ofconcerns. I thought about adding the users to the DB directly from
        the GetRandomUsers call but decided to separate that out in this case.
    */

    internal class Program
    {
        private static void Main(string[] args)
        {
            AsyncContext.Run(() => MainAsync(args));
        }

        static async void MainAsync(string[] args)
        {   
            // informt the user and start our work
            Console.WriteLine("Starting download 5 users...");
            DownloadAndAddUsers(await GetRandomUsers());
            Console.WriteLine("Finished download of 5 users...");
            
            // prompt user to close the console
            Console.WriteLine("Hit Enter when done...");
            Console.ReadLine();
        }

        // The workhorse to download the users
        static async Task<List<RandomUser>> GetRandomUsers()
        {
            List<RandomUser> randomUsers = new List<RandomUser>();

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://randomuser.me/api/?results=5&inc=name,email,phone,cell&noinfo");

                response.EnsureSuccessStatusCode();
                using (HttpContent content = response.Content)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    RootObject root = (RootObject)JsonConvert.DeserializeObject(jsonString, typeof(RootObject));
                    root.results.ForEach(x => randomUsers.Add(
                        new RandomUser()
                        {
                            FullName = string.Format("{0} {1} {2}", x.name.title, x.name.first, x.name.last),
                            Email = x.Email,
                            Phone = x.Phone,
                            Cell = x.Cell
                        }
                        ));
                }
            }

            return randomUsers;
        }

        // Add the list of users to the database.
        private static void DownloadAndAddUsers(List<RandomUser> randoms)
        {
            using (RandomUserDbContext db = new RandomUserDbContext())
            {
                randoms.ForEach(x => db.RandomUsers.Add(x));
                db.SaveChanges();
            }
        }
    }
}
