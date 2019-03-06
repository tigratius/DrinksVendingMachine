using System;

namespace DrinksVendingMachine.Models.Classes.json
{
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
}