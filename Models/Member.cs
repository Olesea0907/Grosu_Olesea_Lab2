using System.ComponentModel.DataAnnotations;

namespace Grosu_Olesea_Lab2.Models
{
    public class Member
    {
        public int ID { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s-]*$", ErrorMessage = "Prenumele trebuie să înceapă cu majusculă.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Prenumele trebuie să aibă între 3 și 30 de caractere.")]
        public string? FirstName { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s-]*$", ErrorMessage = "Numele trebuie să înceapă cu majusculă.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Numele trebuie să aibă între 3 și 30 de caractere.")]
        public string? LastName { get; set; }

        [StringLength(70, ErrorMessage = "Adresa nu poate depăși 70 de caractere.")]
        public string? Adress { get; set; }

        [Required(ErrorMessage = "Email-ul este obligatoriu.")]
        [EmailAddress(ErrorMessage = "Introduceți un email valid.")]
        public string Email { get; set; }

        [RegularExpression(@"^\(?([0-9]{4})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Telefonul trebuie să fie de forma '0722-123-123', '0722.123.123' sau '0722 123 123'.")]
        public string? Phone { get; set; }

        [Display(Name = "Full Name")]
        public string? FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public ICollection<Borrowing>? Borrowings { get; set; }
    }
}
