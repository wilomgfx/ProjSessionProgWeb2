using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ProjetSessionWebServ2.Models
{
    public class Partie
    {
        public int Id { get; set; }
        public enum NomRound { QuartFinal, DemiFinal, Final };
        public NomRound Round { get; set; }
        public List<Equipe> Equipes { get; set; }
        public string Gagnant { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "BeginDateAndHour", ResourceType = typeof(GlobalRessources.CongresRes))]
        public DateTime? DateEtHeureDebut { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "EndDateAndHour", ResourceType = typeof(GlobalRessources.CongresRes))]
        public DateTime? DateEtHeureFin { get; set; }
        //public virtual PlageHoraire PlageHoraire { get; set; }
        public virtual Tournoi Tournoi { get; set; }

        public bool Actif { get; set; }
    }
}