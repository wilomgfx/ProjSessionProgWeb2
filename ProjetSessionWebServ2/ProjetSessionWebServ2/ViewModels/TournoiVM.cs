using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.ViewModels
{
    public class TournoiVM
    {
        public Tournoi Tournoi { get; set; }

        //public PlageHoraire PlageHoraire { get; set; }

        public int HeureDebut { get; set; }
        public int HeureFin { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateTournoi { get; set; }
        
        public TournoiVM()
        {

        }

        public TournoiVM(Tournoi pTournoi)
        {
            Tournoi = pTournoi;
            //PlageHoraire = pTournoi.PlageHoraire;
        }


    }
}