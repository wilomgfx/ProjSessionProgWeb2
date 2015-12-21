using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class EquipeAvancement
    {
        public int EquipeAvancementId { get; set; }

        public Equipe Equipe { get; set; }

        public int NbrDePoints { get; set; }

        public virtual Tournoi Tournoi { get; set; }
    }
}