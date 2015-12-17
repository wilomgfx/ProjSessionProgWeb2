using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Section
    {
        public int Id { get; set; }
        [Display(Name = "Nom", ResourceType = typeof(GlobalRessources.SalleSectionRes))]
        public string Nom { get; set; }
        [Display(Name = "Taille", ResourceType = typeof(GlobalRessources.SalleSectionRes))]
        public Taille TailleSection { get; set; }
        [Display(Name = "Salle", ResourceType = typeof(GlobalRessources.SalleSectionRes))]
        public Salle Salle { get; set; }
        public Nullable<int> EvenementId { get; set; }
        [Display(Name = "Evenement", ResourceType = typeof(GlobalRessources.SalleSectionRes))]
        public Evenement Evenement { get; set; }
        public Dimension Dimension { get; set; }
        //public virtual Dimension Sections { get; set; } <--- this should not exists, it fked up the db :(
    }
}