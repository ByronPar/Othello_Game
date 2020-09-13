using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Othello_Game.Models
{
    public class ArchivoModel
    {
        [Required]
        [DisplayName("s")]
        public HttpPostedFileBase Archivo { get; set; }
    }
}