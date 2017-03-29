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

    */

    internal class Program
    {
        private static void Main(string[] args)
        {
            AsyncContext.Run(() => MainAsync(args));
        }

        static async void MainAsync(string[] args)
        {
            List<RandomUser> randomUsers = new List<RandomUser>();

            Console.WriteLine("Starting download of 5 users...");
            randomUsers.AddRange(await GetRandomUser());
            Console.WriteLine("Finished download of 5 users...");

            Console.WriteLine("Adding users to database...");
            AddThemToTheDatabase(randomUsers);
            Console.WriteLine("Finished adding users to database...");

            Console.WriteLine("Hit Enter when done...");
            Console.ReadLine();
        }

        static async Task<List<RandomUser>> GetRandomUser()
        {
            List<RandomUser> rando = new List<RandomUser>();

            try
            {
                using (var client = new HttpClient())
                {
                    //var task = client.GetAsync("https://randomuser.me/api/?results=5&inc=name,email,phone,cell&noinfo")
                    HttpResponseMessage response = await client.GetAsync("https://randomuser.me/api/?results=5&inc=name,email,phone,cell&noinfo");

                    response.EnsureSuccessStatusCode();
                    using (HttpContent content = response.Content)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();

                        RootObject root = (RootObject)JsonConvert.DeserializeObject(jsonString, typeof(RootObject));
                        root.results.ForEach(x => rando.Add(
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
            }
            catch (Exception ex) { }

            return rando;
        }

        private static void AddThemToTheDatabase(List<RandomUser> randoms)
        {
            using (RandomUserDbContext db = new RandomUserDbContext())
            {
                randoms.ForEach(x => db.RandomUsers.Add(x));
                db.SaveChanges();
            }
        }
    }
}
