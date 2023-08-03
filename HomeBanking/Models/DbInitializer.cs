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

                var account2 = context.Accounts.FirstOrDefault(c => c.Number == "002");
                if (account2 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction { AccountId= account2.Id, Amount = 89000, Date= DateTime.Now.AddHours(-5), Description = "Transferencia no recibida", Type = TransactionType.CREDIT.ToString() },
                        new Transaction { AccountId= account2.Id, Amount = 45000, Date= DateTime.Now.AddHours(-6), Description = "Compra en tienda mercado libre y envio realizado", Type = TransactionType.DEBIT.ToString() },
                        new Transaction { AccountId= account2.Id, Amount = -16500, Date= DateTime.Now.AddHours(-7), Description = "Compra en tienda Madame Tricots", Type = TransactionType.DEBIT.ToString() },

                    };
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }
            }

            if (!context.Loans.Any())
            {
                //creamos 3 prestamos Hipotecario, Personal y Automotriz
                var loans = new Loan[]
                {
                    new Loan {Name = "Hipotecario", MaxAmount = 500000, Payments = "12,24,36,48,60"},
                    new Loan {Name = "Personal", MaxAmount = 100000, Payments = "6,12,24"},
                    new Loan {Name = "Automotriz", MaxAmount = 300000, Payments = "6,12,24,36"},
                };
                foreach(Loan loan in loans)
                {
                    context.Loans.Add(loan);
                }
                context.SaveChanges();

                //ahora agregare los clientloan (prestamos del cliente)
                var client1 = context.Clients.FirstOrDefault(c => c.Email == "martiangelico@gmail.com");
                if (client1 != null)
                {
                    //usaremos los 3 tipos de prestamos
                    var loan1 = context.Loans.FirstOrDefault(I => I.Name == "Hipotecario");
                    if (loan1 != null) 
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 400000,
                            ClientId = client1.Id,
                            LoanId = loan1.Id,
                            Payments = "60"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }

                    var loan2 = context.Loans.FirstOrDefault(I => I.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 50000,
                            ClientId = client1.Id,
                            LoanId = loan2.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }

                    var loan3 = context.Loans.FirstOrDefault(I => I.Name == "Automotriz"); 
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 100000,
                            ClientId = client1.Id,
                            LoanId = loan3.Id,
                            Payments = "24"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }
                }
                context.SaveChanges();
                //cliente2
                var client2 = context.Clients.FirstOrDefault(c => c.Email == "franiriartb@gmail.com");
                if (client2 != null)
                {
                    //usaremos los 3 tipos de prestamos
                    var loan1 = context.Loans.FirstOrDefault(I => I.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 500000,
                            ClientId = client2.Id,
                            LoanId = loan1.Id,
                            Payments = "60"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }

                    var loan2 = context.Loans.FirstOrDefault(I => I.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 80000,
                            ClientId = client2.Id,
                            LoanId = loan2.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }

                    var loan3 = context.Loans.FirstOrDefault(I => I.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 200000,
                            ClientId = client2.Id,
                            LoanId = loan3.Id,
                            Payments = "24"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }
                }
                //guardo los prestamos
                context.SaveChanges();
            }
        }
    }
}