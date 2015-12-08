using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.ViewModels
{
    public class TournoiVM
    {
        public Tournoi Tournoi { get; set; }

        public PlageHoraire PlageHoraire { get; set; }
        
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