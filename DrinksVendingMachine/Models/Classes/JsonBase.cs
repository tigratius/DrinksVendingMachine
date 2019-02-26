using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinksVendingMachine.Models.Classes
{

    public abstract class JsonBase
    {
        public bool success;
        public string title;
        public string message;
    }

    public class JsonError : JsonBase
    {
        public JsonError(string title, string message)
        {
            success = false;
            this.title = title;
            this.message = message;
        }

        public JsonError(string message)
            : this("Ошибка!", message)
        {
        }

        public JsonError(Exception exception)
            : this(exception.Message)
        {
        }
    }

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