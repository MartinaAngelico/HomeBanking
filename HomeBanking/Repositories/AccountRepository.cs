using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Account FindById(long id)
        {
            return FindByCondition(account => account.Id == id) //de las cuentas quiero la cuenta cuyo Id sea = al que recibo por parametro
               .Include(account => account.Transactions) //el include es un metodo.Le estoy diciendo "De los clientes, incluime las cuentas del cliente"
               .FirstOrDefault(); //quiero el primero (client)
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll()
                .Include(account => account.Transactions)
                .ToList(); //lo transforma en una lista
        }

        public void Save(Account account)
        {
            Create(account); //crea el cliente en la base de datos
            SaveChanges(); //guarda los cambios
        }
    }
}
