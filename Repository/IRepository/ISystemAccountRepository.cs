using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface ISystemAccountRepository
    {
        SystemAccount GetSystemAccountById(short? accountId);
        SystemAccount GetSystemAccountByEmail(string accountEmail);

        SystemAccount CheckLogin(string email, string password);

        List<SystemAccount> GetSystemAccounts();
        void UpdateAccount(SystemAccount account);
        void DeleteAccount(SystemAccount account);
        void SaveAccount(SystemAccount account);
        List<SystemAccount> SearchSystemAccounts(String name);
    }
}
