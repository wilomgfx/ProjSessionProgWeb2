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
        public Taille TailleSection { get; set; }
        public Salle Salle { get; set; }
        public Nullable<int> EvenementId { get; set; }
        public Evenement Evenement { get; set; }
        public Dimension Dimension { get; set; }
        //public virtual Dimension Sections { get; set; } <--- this should not exists, it fked up the db :(
    }
}