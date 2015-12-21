using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Equipe
    {
        public int Id { get; set; }

        [Display(Name = "NomEquipe", ResourceType = typeof(GlobalRessources.TournoiRes))]
        public string Nom { get; set; }

        [Display(Name = "Equipes", ResourceType = typeof(GlobalRessources.TournoiRes))]
        public List<ApplicationUser> Joueurs { get; set; }

        public virtual List<Partie> Parties { get; set; }

        //public int PointageTotal { get; set; }
    }
}