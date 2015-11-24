using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Congres
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Adresse { get; set; }

        public string Nom { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        public virtual List<ApplicationUser> Users { get; set; }

        public virtual List<Tournoi> Tournois { get; set; }

        public virtual List<Evenement> Evenements { get; set; }

        public virtual List<Spectacle> Spectacles { get; set; }

        public virtual List<Kiosque> Kiosques { get; set; }

        public virtual List<PlageHoraire> PlageHoraires { get; set; }

        public virtual List<Transaction> Transactions { get; set; }

    }
}