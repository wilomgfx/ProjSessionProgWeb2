using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Equipe
    {
        public int Id { get; set; }

        public string Nom { get; set; }

        public List<ApplicationUser> Joueurs { get; set; }

        public int PointageTotal { get; set; }
    }
}