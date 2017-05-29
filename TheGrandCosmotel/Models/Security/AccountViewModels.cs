using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebGames.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "To Email είναι υποχρεωτικό.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessage = "To Email είναι υποχρεωτικό.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Το UserName είναι υποχρεωτικό.")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ο Κωδικός είναι υποχρεωτικός.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Να με θυμάσαι?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Το UserName είναι υποχρεωτικό.")]
        [DataType(DataType.Text, ErrorMessage="Λάθος UserName - Μπορεί να περιέχει λατινικούς χαρακτήρες και αριθμούς")]
        [Display(Name = "UserName - Χρησιμοποιήστε κάτι διαφορετικό από το email σας")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ο Κωδικός είναι υποχρεωτικός.")]
        [StringLength(100, ErrorMessage = "Ο {0} πρέπει να περιέχει τουλάχιστων {2} χαρακτήρες.", MinimumLength = 6)] // The {0} must be at least {2} characters long.
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Επιβεβαίωση Password")]
        [Compare("Password", ErrorMessage = "Ο κωδικός δεν είναι ίδιος με την επιβεβαίωση κωδικού.")]// The password and confirmation password do not match.
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Το Ονοματεπώνυμο είναι υποχρεωτικός.")]
        [DataType(DataType.Text)]
        [Display(Name = "Ονοματεπώνυμο")]
        public string FullName { get; set; }

        [Required(ErrorMessage ="To Email είναι υποχρεωτικό.")]
        [EmailAddress(ErrorMessage = "Μη έγκυρο Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Το Κατάστημα είναι υποχρεωτικό.")]
        [DataType(DataType.Text)]
        [Display(Name = "Κατάστημα")]
        public string Shop { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Οικογενειακή κατάσταση")]
        public string MaritalStatus { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Χόμπυ")]
        public string Hobby { get; set; }

        [Required(ErrorMessage = "Το Εικονίδιο χρήστη είναι υποχρεωτικό.")]
        [DataType(DataType.Text)]
        [Display(Name = "Εικονίδιο χρήστη")]
        public string Avatar { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Το UserName είναι υποχρεωτικό.")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ο Κωδικός είναι υποχρεωτικός.")]
        [StringLength(100, ErrorMessage = "Ο {0} πρέπει να περιέχει τουλάχιστων {2} χαρακτήρες.", MinimumLength = 6)] // The {0} must be at least {2} characters long.
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Επιβεβαίωση Password")]
        [Compare("Password", ErrorMessage = "Ο κωδικός δεν είναι ίδιος με την επιβεβαίωση κωδικού.")]// The password and confirmation password do not match.
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "To Email είναι υποχρεωτικό.")]
        [EmailAddress(ErrorMessage = "Μη έγκυρο Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
