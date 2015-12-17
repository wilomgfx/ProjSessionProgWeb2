using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Congres
    {
        public int Id { get; set; }

        public string Description { get; set; }

        [Display(Name = "Address", ResourceType = typeof(GlobalRessources.CongresRes))]
        public string Adresse { get; set; }

        [Display(Name = "Name", ResourceType = typeof(GlobalRessources.CongresRes))]
        public string Nom { get; set; }

        [Display(Name = "Active", ResourceType = typeof(GlobalRessources.CongresRes))]
        public bool Actif { get; set; }

        [Display(Name = "StartDate", ResourceType = typeof(GlobalRessources.CongresRes))]
        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; }

        [Display(Name = "EndDate", ResourceType = typeof(GlobalRessources.CongresRes))]
        [DataType(DataType.Date)]
        public DateTime DateFin { get; set; }

        public virtual List<ApplicationUser> Users { get; set; }

        public virtual List<Evenement> Evenements { get; set; }

        public virtual List<PlageHoraire> PlageHoraires { get; set; }

        public virtual List<Transaction> Transactions { get; set; }

    }
}