using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace IdealMall.Entities
{
    public class TradeUser
    {
        [Display(Name = "First Name")]
        [UIHint("TradeTextBox")]
        [RegularExpression("([A-z]+)", ErrorMessage = ">> Only Alphabets")]
        [Required(ErrorMessage = ">> Required")]
        public String FirstName { get; set; }

        [Display(Name = "Last Name")]
        [UIHint("TradeTextBox")]
        [RegularExpression("([A-z]+)", ErrorMessage = ">> Only Alphabets")]
        [Required(ErrorMessage = ">> Required")]
        public String LastName { get; set; }

        [Display(Name = "Name of Business")]
        [UIHint("TradeTextBox")]
        [Required(ErrorMessage = ">> Required")]
        public String BusinessName { get; set; }

        [Display(Name = "Email Address")]
        [UIHint("TradeTextBox")]
        [Required(ErrorMessage = ">> Required")]
        [RegularExpression("[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})",
                           ErrorMessage = ">> Invalid Email Id")]
        public String EmailAddress { get; set; }

        [Display(Name = "Contact Number")]
        [UIHint("TradeTextBox")]
        [Required(ErrorMessage = ">> Required")]
        [RegularExpression("([0-9\\+-]+)", ErrorMessage = ">> Invalid Phone number")]
        public String ContactNumber { get; set; }

        [Display(Name = "Preferred User Name")]
        [UIHint("TradeTextBox")]
        [Required(ErrorMessage = ">> Required")]
        public String UserName { get; set; }

        [Display(Name = "Enter Password")]
        [UIHint("TradeTextBox")]
        [Required(ErrorMessage = ">> Required")]
        [DataType(DataType.Password)]
        public String TradePassword { get; set; }

        [Display(Name = "Enter Password")]
        [Required(ErrorMessage = ">> Required")]
        public String NewPassword { get; set; }
        [Display(Name = "Re-enter Password")]
        [Required(ErrorMessage = ">> Required")]
        public String ConfirmPassword { get; set; }

        [Display(Name = "Post Code")]        
        [UIHint("TradeTextBox")]
        [Required]
        public String PostCode { get; set; }
        public bool TradeRememberMe { get; set; }
    }
}
