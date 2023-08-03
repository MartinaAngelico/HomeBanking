using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Repositories
{
    public class ClientRepository: RepositoryBase<Client>, IClientRepository
                                //es una clase abstracta, el tipo de dato es client, implementa los metodos de IClienrepository
                                //: hereda de
    {
        public ClientRepository(HomeBankingContext repositoryContext) : base(repositoryContext) 
        {
        }
        public Client FindById(long id)
        {
            return FindByCondition(client => client.Id == id) //de los clientes quiero el cliente cuyo Id sea = al que recibo por parametro
                .Include(client => client.Accounts) //el include es un metodo.Le estoy diciendo "De los clientes, incluime las cuentas del cliente"
                .Include(client=> client.ClientLoans) //"De los clientes, incluime los prestamos de ellos"
                .ThenInclude(cl => cl.Loan) //"Por cada cliente Loan, incluime su respectivo prestamo
                .Include(client => client.Cards)
                .FirstOrDefault(); //quiero el primero (client)
        }
        public IEnumerable<Client> GetAllClients()
        {
            return FindAll() 
                .Include(client => client.Accounts)
                .Include(client=> client.ClientLoans)
                .ThenInclude(cl=> cl.Loan)
                .Include(client => client.Cards)
                .ToList(); //lo transforma en una lista
        }

        public void Save(Client client)
        {
            Create(client); //crea el cliente en la base de datos
            SaveChanges(); //guarda los cambios
        }

    }
}
