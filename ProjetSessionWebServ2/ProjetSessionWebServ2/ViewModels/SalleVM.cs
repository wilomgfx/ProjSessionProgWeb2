using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.ViewModels
{
    public class SalleVM
    {
        public Salle Salle { get; set; }

        public double CalculerAirSalle(Salle salle)
        {
            double largeur = salle.Dimension.Largeur;
            double longueur = salle.Dimension.Longueur;
            double airCarre = largeur * longueur;
            return airCarre;
        }

        public bool PossibleAjouterSection(Salle salle,Section section)
        {
            double airCarreSalle = this.CalculerAirSalle(salle);
            double largeurSection = section.Dimension.Largeur;
            double longueurSection = section.Dimension.Longueur;
            double airCarreSection = largeurSection * longueurSection;

            if (airCarreSalle > airCarreSection)
                return true;

            return true;
        }

        public double NombreAirCarreRestante(Salle salle)
        {
            double airCarreSalle = this.CalculerAirSalle(salle);
            double airOccuper = 0;
            Section[] tabSection = salle.Sections.ToArray();
            foreach(Section section in tabSection)
            {
                double largeurSection = section.Dimension.Largeur;
                double longueurSection = section.Dimension.Longueur;
                double airCarreSection = largeurSection * longueurSection;
                airOccuper += airCarreSection;
            }

            double airRestante = airCarreSalle - airOccuper;
            return airRestante;
        }
    }
}