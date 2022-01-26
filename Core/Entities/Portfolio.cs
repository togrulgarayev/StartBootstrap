
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class Portfolio
    {
        public int Id { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }


        public string ModalTitle { get; set; }
        public string ModalDescription { get; set; }
        public string Text { get; set; }

    }
}
