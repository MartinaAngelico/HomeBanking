using System.Linq;

namespace HomeBanking.Models
{
    public class DbInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if(!context.Clients.Any())
            {
                //creamos datos de prueba
                var clients = new Client[]
                {
                    new Client {
                         FirstName = "Martina",
                         LastName = "Angelico",
                         Email = "martiangelico@gmail.com",
                         Password = "marti1324",
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

            context.SaveChanges();
        }
    }
}
