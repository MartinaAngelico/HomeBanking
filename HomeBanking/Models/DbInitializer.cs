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
                context.SaveChanges();
            }

            if (!context.Accounts.Any())
            {
                var accountMarti = context.Clients.FirstOrDefault(c => c.Email == "martiangelico@gmail.com");
                if (accountMarti != null)
                {
                    var Accounts = new Account[]
                    {
                        new Account{ClientId=accountMarti.Id, CreationDate=DateTime.Now, Number="001", Balance= 1000},
                        new Account{ClientId= accountMarti.Id, CreationDate=DateTime.Now, Number="002", Balance= 2000},

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
                        new Account{ClientId=accountFran.Id, CreationDate=DateTime.Now, Number="003", Balance= 1001},
                        new Account{ClientId= accountFran.Id, CreationDate=DateTime.Now, Number="004", Balance= 2002},

                    };

                    foreach (Account account in Accounts)
                    {
                        context.Accounts.Add(account);
                    }
                }
                context.SaveChanges();
            }

            if (!context.Transactions.Any())
            {
                var account1 = context.Accounts.FirstOrDefault(c => c.Number == "001");
                if (account1 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction { AccountId= account1.Id, Amount = 10000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia reccibida", Type = TransactionType.CREDIT.ToString() },
                        new Transaction { AccountId= account1.Id, Amount = -2000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre", Type = TransactionType.DEBIT.ToString() },
                        new Transaction { AccountId= account1.Id, Amount = -3000, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda xxxx", Type = TransactionType.DEBIT.ToString() },

                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }

                var account3 = context.Accounts.FirstOrDefault(c => c.Number == "002");
                if (account3 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction { AccountId= account3.Id, Amount = 89000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia no recibida", Type = TransactionType.CREDIT.ToString() },
                        new Transaction { AccountId= account3.Id, Amount = 45000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre y envio realizado", Type = TransactionType.DEBIT.ToString() },
                        new Transaction { AccountId= account3.Id, Amount = -16500, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda Madame Tricots", Type = TransactionType.DEBIT.ToString() },

                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}