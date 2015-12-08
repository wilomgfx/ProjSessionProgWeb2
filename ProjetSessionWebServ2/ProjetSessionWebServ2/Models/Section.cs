using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public enum Taille {Petite, Moyenne, Grand, Tres_Grand }
        public Nullable<int> EvenementId { get; set; }
        public Evenement Evenement { get; set; }
        public virtual Dimension Dimension { get; set; }
    }
}