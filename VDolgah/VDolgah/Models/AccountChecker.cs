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
        public DBEntities db = new DBEntities();
        string email;
        string password;
        string confirm;
        string salt;
        Random rand = new Random(DateTime.Now.Millisecond);

        public AccountChecker(string email, string password, string confirm)
        {
            this.confirm = confirm;
            this.email = email;
            this.password = password;
        }

        //проверка на сущ. пользователя
        public string CheckEmail()
        {
            if (db.users.Where((x) => x.email == this.email).ToList().Count != 0)
                return "Пользователь с таким email уже существует";
            if (password != confirm)
                return "Пароли не совпадают";
            return null;
        }

        public user GetUser()
        {
            return db.users.Where((x) => x.email == this.email).ToList().First();
        }

        //проверка при входе
        public string CheckData()
        {
            if (CheckEmail() == null)
            {
                salt = db.users.Where((x) => x.email == this.email).ToList().First().salt;
                password = CreateMD5Hash();
                if (password != db.users.Where((x) => x.email == this.email).ToList().First().password_hesh)
                    return "Неправильный логин или пароль";
                else
                    return null;
            }
            else
                return "Пользователь не существует";
        }

        public string CreateMD5Hash()
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password + salt);
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
            salt = builder.ToString();
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