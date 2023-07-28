using HomeBanking.Models;
using System.Collections.Generic;

namespace HomeBanking.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts(); //metodo que me traiga todos las cuentas
       // public IEnumerable<Account> Accounts { get; private set; }
        void Save(Account account);
        Account FindById(long id);
    }
}
