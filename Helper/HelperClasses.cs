using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NHibernate.Impl;
using System.Security;

namespace webkom.Helper
{

    public static class HelperClasses
    {

    public static SecureString ConvertToSecureString(string password)
    {
      if (password == null)
        throw new ArgumentNullException("password");

      var securePassword = new SecureString();

      foreach (char c in password)
        securePassword.AppendChar(c);

      securePassword.MakeReadOnly();
      return securePassword;
    }

    public static string BuildPropertyAccess<T>(Expression<Func<T>> alias, string propertyName)
        {
            string aliasName = ExpressionProcessor.FindMemberExpression(alias.Body);

            return string.Format("{0}.{1}", aliasName, propertyName);
        }


        public static  bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static string UpperCaseFirst(this string s)
        {
            //if (string.IsNullOrEmpty(s))
            //{
            //    return string.Empty;
            //}
            //char[] a = s.ToCharArray();
            //a[0] = char.ToUpper(a[0]);
            //return new string(a);

            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            var split = s.Split('.');
            string izlaz = "";

            foreach (string s1 in split)
            {
                char[] a = s1.ToCharArray();
                a[0] = char.ToUpper(a[0]);
                if (izlaz != "")
                    izlaz = izlaz + ".";
                izlaz = izlaz + new string(a);
            }

            return izlaz;
        }
    }
}
