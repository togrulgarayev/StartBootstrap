using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StartBootstrap.Data.ViewModels.Account
{
    public class RegisterViewModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Password),Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
    }
}
