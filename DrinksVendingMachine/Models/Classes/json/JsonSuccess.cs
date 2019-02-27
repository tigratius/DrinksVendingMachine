using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinksVendingMachine.Models.Classes.json
{
    public class JsonSuccess : JsonBase
    {
        public JsonSuccess()
        {
            success = true;
            title = "Успешная операция";
            message = "ok";
        }

        public JsonSuccess(string message)
            : this()
        {
            this.message = message;
        }
    }
}