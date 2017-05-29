using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebGames.Models
{
    // Models used as parameters to AccountController actions.

    public class ChangePasswordBindingModel
    {
        [Required(ErrorMessage = "Ο υπάρχων Κωδικός είναι υποχρεωτικός.")]
        [DataType(DataType.Password)]
        [Display(Name = "Υπάρχων password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Ο νέος Κωδικός είναι υποχρεωτικός.")]
        [StringLength(100, ErrorMessage = "Ο {0} πρέπει να περιέχει τουλάχιστων {2} χαρακτήρες.", MinimumLength = 6)] // The {0} must be at least {2} characters long.
        [DataType(DataType.Password)]
        [Display(Name = "Νεο password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Επιβεβαίωση νέου κωδικού")]
        [Compare("NewPassword", ErrorMessage = "Ο νέος κωδικός δεν είναι ίδιος με την επιβεβαίωση κωδικού.")]// The new password and confirmation password do not match.
        public string ConfirmPassword { get; set; }
    }

    public class RegisterBindingModel
    {
        [Required(ErrorMessage = "To Email είναι υποχρεωτικό.")]
        [EmailAddress(ErrorMessage = "Μη έγκυρο Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ο Κωδικός είναι υποχρεωτικός.")]
        [StringLength(100, ErrorMessage = "Ο {0} πρέπει να περιέχει τουλάχιστων {2} χαρακτήρες.", MinimumLength = 6)] // The {0} must be at least {2} characters long.
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Επιβεβαίωση Password")]
        [Compare("Password", ErrorMessage = "Ο κωδικός δεν είναι ίδιος με την επιβεβαίωση κωδικού.")]// The password and confirmation password do not match.
        public string ConfirmPassword { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required(ErrorMessage = "Ο νέος Κωδικός είναι υποχρεωτικός.")]
        [StringLength(100, ErrorMessage = "Ο {0} πρέπει να περιέχει τουλάχιστων {2} χαρακτήρες.", MinimumLength = 6)] // The {0} must be at least {2} characters long.
        [DataType(DataType.Password)]
        [Display(Name = "Νεο password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Επιβεβαίωση νέου κωδικού")]
        [Compare("NewPassword", ErrorMessage = "Ο νέος κωδικός δεν είναι ίδιος με την επιβεβαίωση κωδικού.")]// The new password and confirmation password do not match.
        public string ConfirmPassword { get; set; }
    }
}
