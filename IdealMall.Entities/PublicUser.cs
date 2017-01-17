using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace IdealMall.Entities
{
    public class PublicUser
    {
        [Display(Name = "First Name")]
        [UIHint("PublicTextBox")]
        [RegularExpression("([A-z]+)", ErrorMessage = ">> Only Alphabets")]
        [Required(ErrorMessage = ">> Required")]
        public String FirstName { get; set; }

        [Display(Name = "Last Name")]
        [UIHint("PublicTextBox")]
        [RegularExpression("([A-z]+)", ErrorMessage = ">> Only Alphabets")]
        [Required(ErrorMessage = ">> Required")]
        public String LastName { get; set; }

        [Display(Name = "Name of Business")]
        [UIHint("PublicTextBox")]
        [Required(ErrorMessage = ">> Required")]
        public String BusinessName { get; set; }

        [Display(Name = "Email Address")]
        [UIHint("PublicTextBox")]
        [Required(ErrorMessage = ">> Required")]
        [RegularExpression("[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})",
                           ErrorMessage = ">> Invalid Email Id")]
        public String EmailAddress { get; set; }

        [Display(Name = "Contact Number")]
        [UIHint("PublicTextBox")]
        [Required(ErrorMessage = ">> Required")]
        [RegularExpression("([0-9\\+-]+)", ErrorMessage = ">> Invalid Phone number")]
        public String ContactNumber { get; set; }

        [Display(Name = "Post Code")]
        [UIHint("PublicTextBox")]
        [Required(ErrorMessage = ">> Required")]
        public String PostCode { get; set; }

        [Display(Name = "Enter Password")]
        [UIHint("PublicTextBox")]
        [Required(ErrorMessage = ">> Required")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Display(Name = "Enter Password")]
        [Required(ErrorMessage = ">> Required")]
        public String NewPassword { get; set; }
        [Display(Name = "Re-enter Password")]
        [UIHint("PublicTextBox")]
        [Required(ErrorMessage = ">> Required")]
        [DataType(DataType.Password)]        
        public String ConfirmPassword { get; set; }

   public String UserName { get; set; }
   
        public bool RememberMe { get; set;}
    }
}
