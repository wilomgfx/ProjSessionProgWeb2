using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.ViewModels
{
    public class Tournoi_EquipesPointages
    {
        public Tournoi Tournoi { get; set; }
        public List<EquipeAvancement> Equipes { get; set; }
    }
}