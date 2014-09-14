using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace VDolgah.Models
{
    public class AccountChecker
    {
        public DbEntities db = DbEntities.Instance;
        user u;
        Random rand = new Random(DateTime.Now.Millisecond);

        public AccountChecker(user u)
        {
            this.u = u;
        }

        //проверка на сущ. пользователя
        public string CheckEmail(bool login)
        {
            if (db.users.Where((x) => x.email == u.email).ToList().Count != 0 && !login)
                return "Пользователь с таким email уже существует";
            if (db.users.Where((x) => x.login == u.login).ToList().Count != 0 && !login)
                return "Пользователь с таким логином уже существует";
            if (u.password_hash != u.confirm_password && !login)
                return "Пароли не совпадают";
            return null;
        }

        public bool CheckLogin()
        {
            return db.users.Where((x) => x.login == u.login).ToList().Count != 0;
        }

        //проверка при входе
        public string CheckData(bool login)
        {
            if (!login)
            {
                u.login = u.email;
                if(!CheckLogin())
                    return "Пользователь не существует";
                u.salt = db.users.Where((x) => x.login == u.login).ToList().First().salt;
                u.password_hash = CreateMD5Hash();
                if (u.password_hash != db.users.Where((x) => x.login == u.login).ToList().First().password_hash)
                    return "Неправильный логин или пароль";
                else
                    return null;
            }
            else
                if (CheckEmail(true) == null && db.users.Where((x) => x.email == u.email).Count() > 0)
            {
                u.salt = db.users.Where((x) => x.email == u.email).ToList().First().salt;
                u.password_hash = CreateMD5Hash();
                if (u.password_hash != db.users.Where((x) => x.email == u.email).ToList().First().password_hash)
                    return "Неправильный логин или пароль";
                else
                    return null;
            }
            else
                return "Пользователь не существует";
        }

        public user GetUser()
        {
            if (u.login == u.email)
                return db.users.Where((x) => x.login == u.login).ToList().First();
            else
                return db.users.Where((x) => x.email == u.email).ToList().First();
        }

        public string CreateMD5Hash()
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(u.password_hash + u.salt);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public string GenerateSalt()
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < 10; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rand.NextDouble() + 65)));
                builder.Append(ch);
            }
            u.salt = builder.ToString();
            return builder.ToString();
        }

        public string getLastIP()
        {
            foreach (var addr in Dns.GetHostEntry(string.Empty).AddressList)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                    return addr.MapToIPv4().ToString();
            }
            return String.Empty;
        }
    }
}