using BusinessObjects;
using DataAccessObjects;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        public SystemAccount CheckLogin(string email, string password)
        => SystemAccountDAO.CheckLogin(email, password);

        public void DeleteAccount(SystemAccount account)
        => SystemAccountDAO.DeleteAccount(account);

        public SystemAccount GetSystemAccountByEmail(string accountEmail)
        => SystemAccountDAO.GetSystemAccountByEmail(accountEmail);

        public SystemAccount GetSystemAccountById(short? accountId)
        => SystemAccountDAO.GetAccountById(accountId);

        public List<SystemAccount> GetSystemAccounts()
        => SystemAccountDAO.GetSystemAccounts();

        public void SaveAccount(SystemAccount account)
        => SystemAccountDAO.SaveAccount(account);

        public List<SystemAccount> SearchSystemAccounts(String name)
        => SystemAccountDAO.SearchSystemAccounts(name);

        public void UpdateAccount(SystemAccount account)
        => SystemAccountDAO.UpdateAccount(account);
    }
}
