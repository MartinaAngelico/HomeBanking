using System;
using System.Linq;

namespace HomeBanking.Models
{
    public class DbInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                //creamos datos de prueba
                var clients = new Client[]
                {
                    new Client {
                         FirstName = "Martina",
                         LastName = "Angelico",
                         Email = "martiangelico@gmail.com",
                         Password = "marti132456",
                    },
                    new Client {
                         FirstName = "Francisco",
                         LastName = "Iriart",
                         Email = "franiriartb@gmail.com",
                         Password = "fran1324",
                    },
                };

                foreach (var client in clients)
                {
                    context.Clients.Add(client); //le paso el nombre de la variable
                }
            }


            //context.SaveChanges();

            if (!context.Accounts.Any())
            {
                var accountMarti = context.Clients.FirstOrDefault(c => c.Email == "martiangelico@gmail.com");
                if (accountMarti != null)
                {
                    var Accounts = new Account[]
                    {
                        new Account{ClientId=accountMarti.Id, CreationDate=DateTime.Now, Number=string.Empty, Balance= 1000},
                        new Account{ClientId= accountMarti.Id, CreationDate=DateTime.Now, Number="uno", Balance= 2000},

                    };

                    foreach (Account account in Accounts)
                    {
                        context.Accounts.Add(account);
                    }

                }
                var accountFran = context.Clients.FirstOrDefault(c => c.Email == "franiriartb@gmail.com");
                if (accountFran != null)
                {
                    var Accounts = new Account[]
                    {
                        new Account{ClientId=accountFran.Id, CreationDate=DateTime.Now, Number="tres", Balance= 1001},
                        new Account{ClientId= accountFran.Id, CreationDate=DateTime.Now, Number="uno", Balance= 2002},

                    };

                    foreach (Account account in Accounts)
                    {
                        context.Accounts.Add(account);
                    }
                }
                context.SaveChanges();
            }
        }
    }
}