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
            if (account.Id == 0) //si el id es 0 es una nueva cuenta
            {
                Create(account);
            }
            else
            {
                Update(account);
            }
            SaveChanges();
        }

        public IEnumerable<Account> GetAccountByClient(long clientId)
        {
            return FindByCondition(account => account.ClientId == clientId)
            .Include(account => account.Transactions)
            .ToList();
        }

        public Account FindByNumber(string number)
        {
            return FindByCondition(account => account.Number.ToUpper() == number.ToUpper())
            .Include(account => account.Transactions)
            .FirstOrDefault();
        }

        public IEnumerable<Account> GetAccountsByClient(long clientId)
        {
            throw new System.NotImplementedException();
        }
    }
}
