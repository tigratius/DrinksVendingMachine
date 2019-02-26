using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinksVendingMachine.Models.Classes
{
    public static class ValidatingTokens
    {
        private static readonly string TokenId = System.Configuration.ConfigurationManager.AppSettings["token"];

        public static bool IsTokenValid(string token)
        {
            return token == TokenId;
        }

        public static string GetToken()
        {
            return TokenId;
        }
    }
}