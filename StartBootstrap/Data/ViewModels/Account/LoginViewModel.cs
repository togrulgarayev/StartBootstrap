using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StartBootstrap.Data.ViewModels.Account
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
