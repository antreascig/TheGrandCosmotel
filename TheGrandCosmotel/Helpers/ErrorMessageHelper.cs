using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGames.Helpers
{
    public class ErrorMessageHelper
    {
        public static List<string> FixErrors(IEnumerable<string> Errors)
        {
            var fixedErrors = new List<string>();
            foreach (var error in Errors)
            {
                var msg = error;
                if (msg.Contains("is invalid, can only contain letters or digits."))
                {
                    msg = "Λάθος UserName - Μπορεί να περιέχει λατινικούς χαρακτήρες και αριθμούς";
                }
                else if (msg.Contains("email") && msg.Contains("already taken"))
                {
                    msg = $"Το Email χρησιμοποιείτε ήδη.";
                }
                fixedErrors.Add(msg);
            }

            return fixedErrors;
        }
    }
}