using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGames.Helpers
{
    public class Base64Helper
    {
        public static string GetBase64(string value)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(value);
            var base64Value = System.Convert.ToBase64String(plainTextBytes);
            return base64Value;
        }
    }
}