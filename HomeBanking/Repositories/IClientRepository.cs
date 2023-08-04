using HomeBanking.Models;
using System.Collections.Generic;

namespace HomeBanking.Repositories
{
    public interface IClientRepository
    {
            IEnumerable<Client> GetAllClients(); //metodo que me traiga todos los clintes
            void Save(Client client); 
            Client FindById(long id);
            Client FindByEmail(string email); //retorne un cliente pasándole como parámetro su email
    }
}
