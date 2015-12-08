using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime DateAchat { get; set; }
        public string TypeAchat { get; set; }
        public decimal Montant { get; set; }
    }
}