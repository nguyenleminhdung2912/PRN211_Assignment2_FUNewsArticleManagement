using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class SystemAccountDAO
    {
        public static SystemAccount GetAccountById(short? accountId)
        {
            using var db = new FunewsManagementFall2024Context();
            SystemAccount returnAccount 
                = db.SystemAccounts
                .Include(n => n.NewsArticles)
                .FirstOrDefault(c => c.AccountId.Equals(accountId));
            return returnAccount;
        }

        public static SystemAccount? CheckLogin(string email, string password)
        {
            // Kiểm tra xem người dùng có tồn tại không
            using var db = new FunewsManagementFall2024Context();
            var user = db.SystemAccounts.SingleOrDefault(u => u.AccountEmail == email && u.AccountPassword == password);

            // Nếu tìm thấy người dùng, trả về dữ liệu (đăng nhập thành công)
            if (user != null)
            {
                return user;
            }

            // Nếu không tìm thấy người dùng, trả về null (đăng nhập thất bại)
            return null;
        }

        public static void DeleteAccount(SystemAccount account)
        {
            try
            {
                using var context = new FunewsManagementFall2024Context();
                var new1 =
                    context.SystemAccounts.SingleOrDefault(c => c.AccountId == account.AccountId);
                context.SystemAccounts.Remove(new1);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<SystemAccount> GetSystemAccounts()
        {
            var list = new List<SystemAccount>();
            try
            {
                using var context = new FunewsManagementFall2024Context();
                list = context.SystemAccounts
                    .Include(n => n.NewsArticles)
                    .ToList();
            }
            catch (Exception ex) { }
            return list;
        }

        public static void SaveAccount(SystemAccount account)
        {
            try
            {
                using var context = new FunewsManagementFall2024Context();
                var maxId = context.SystemAccounts.Max(a => (int?)a.AccountId) ?? 0;
                account.AccountId = (short)(maxId + 1);
                context.SystemAccounts.Add(account);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateAccount(SystemAccount account)
        {
            try
            {
                using var context = new FunewsManagementFall2024Context();
                context.Entry<SystemAccount>(account).State
                    = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static SystemAccount GetSystemAccountByEmail(string accountEmail)
        {
            using var db = new FunewsManagementFall2024Context();
            SystemAccount returnAccount
                = db.SystemAccounts
                .Include(n => n.NewsArticles)
                .FirstOrDefault(c => c.AccountEmail.Equals(accountEmail));
            return returnAccount;
        }

        public static List<SystemAccount> SearchSystemAccounts(string name)
        {
            var list = new List<SystemAccount>();
            try
            {
                using var context = new FunewsManagementFall2024Context();
                list = context.SystemAccounts
                    .Where(s => s.AccountEmail.ToLower().Contains(name.ToLower()))
                    .ToList();
            }
            catch (Exception ex) { }
            return list;
        }
    }
}
