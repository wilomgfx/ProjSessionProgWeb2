using System.Collections.Generic;
namespace ProjetSessionWebServ2.Models
{
    public class Partie
    {
        public int Id { get; set; }
        public enum NomRound { QuartFinal, DemiFinal, Final };
        public List<Equipe> Equipes { get; set; }
        public string Gagnant { get; set; }
        public virtual PlageHoraire PlageHoraire { get; set; }
    }
}