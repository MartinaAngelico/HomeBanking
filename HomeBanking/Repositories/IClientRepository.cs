using HomeBanking.Models;
using System.Collections.Generic;

namespace HomeBanking.Repositories
{
    public interface IClientRepository
    {
            IEnumerable<Client> GetAllClients(); //metodo que me traiga todos los clintes
            void Save(Client client); 
            Client FindById(long id);
    }
}
