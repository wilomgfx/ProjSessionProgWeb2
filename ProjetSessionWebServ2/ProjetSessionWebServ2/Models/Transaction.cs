using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Display(Name = "DateBought", ResourceType = typeof(GlobalRessources.CongresRes))]
        public DateTime DateAchat { get; set; }

        [Display(Name = "BoughtType", ResourceType = typeof(GlobalRessources.CongresRes))]
        public string TypeAchat { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(GlobalRessources.CongresRes))]
        public decimal Montant { get; set; }
    }
}