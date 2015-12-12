using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class TypeKiosque
    {
        public int Id { get; set; }
        [Display(Name = "Nom", ResourceType = typeof(GlobalRessources.TypeRes))]
        public string Nom { get; set; }
    }
}