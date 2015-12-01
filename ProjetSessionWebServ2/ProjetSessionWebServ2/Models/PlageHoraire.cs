using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class PlageHoraire
    {
        public int Id { get; set; }

        public DateTime DateEtHeureDebut { get; set; }

        public DateTime DateEtHeureFin { get; set; }


    }
}