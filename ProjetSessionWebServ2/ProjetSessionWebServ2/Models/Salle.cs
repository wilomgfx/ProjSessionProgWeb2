using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Salle
    {
        public int Id { get; set; }
        [Display(Name = "NoSalle", ResourceType = typeof(GlobalRessources.SalleSectionRes))]
        public string NoSalle { get; set; }
        [Display(Name = "Sections", ResourceType = typeof(GlobalRessources.SalleSectionRes))]
        public virtual List<Section> Sections { get; set; }

        public virtual Dimension Dimension { get; set; }
        [Display(Name = "Taille", ResourceType = typeof(GlobalRessources.SalleSectionRes))]
        public Taille TailleSalle { get; set;}
    }
}