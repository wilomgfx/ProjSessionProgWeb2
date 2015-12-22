using ProjetSessionWebServ2.DAL;
using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ProjetSessionWebServ2.DAL
{
    public class UnitOfWork : IUnitOfWork
    {

        
        public ApplicationDbContext context = new ApplicationDbContext();

        private TransactionRepository transactionRepository;

        public TransactionRepository TransactionRepository
        {
            get
            {
                if (this.transactionRepository == null)
                {
                    this.transactionRepository = new TransactionRepository(context);
                }
                return transactionRepository;
            }
        }

        private TypeConferenceRepository typeConferenceRepository;

        public TypeConferenceRepository TypeConferenceRepository
        {
            get
            {
                if (this.typeConferenceRepository == null)
                {
                    this.typeConferenceRepository = new TypeConferenceRepository(context);
                }
                return typeConferenceRepository;
            }
        }

        private CongresRepository congresRepository;

        public CongresRepository CongresRepository
        {
            get
            {
                if (this.congresRepository == null)
                {
                    this.congresRepository = new CongresRepository(context);
                }
                return congresRepository;
            }
        }

        private SalleRepository salleRepository;

        public SalleRepository SalleRepository
        {
            get
            {
                if (this.salleRepository == null)
                {
                    this.salleRepository = new SalleRepository(context);
                }
                return salleRepository;
            }
        }

        private SectionRepository sectionRepository;

        public SectionRepository SectionRepository
        {
            get
            {
                if (this.sectionRepository == null)
                {
                    this.sectionRepository = new SectionRepository(context);
                }
                return sectionRepository;
            }
        }

        private DimensionRepository dimensionRepository;

        public DimensionRepository DimensionRepository
        {
            get
            {
                if (this.dimensionRepository == null)
                {
                    this.dimensionRepository = new DimensionRepository(context);
                }
                return dimensionRepository;
            }
        }
        private ConferenceRepository conferenceRepository;

        public ConferenceRepository ConferenceRepository
        {
            get
            {

                if (this.conferenceRepository == null)
                {
                    this.conferenceRepository = new ConferenceRepository(context);
                }
                return conferenceRepository;
            }
        }

        private EvenementRepository evenementRepository;

        public EvenementRepository EvenementRepository
        {
            get
            {

                if (this.evenementRepository == null)
                {
                    this.evenementRepository = new EvenementRepository(context);
                }
                return evenementRepository;
            }
        }

        private TournoiRepository tournoiRepository;

        public TournoiRepository TournoiRepository
        {
            get
            {

                if (this.tournoiRepository == null)
                {
                    this.tournoiRepository = new TournoiRepository(context);
                }
                return tournoiRepository;
            }
        }

        private KiosqueRepository kiosqueRepository;

        public KiosqueRepository KiosqueRepository
        {
            get
            {

                if (this.kiosqueRepository == null)
                {
                    this.kiosqueRepository = new KiosqueRepository(context);
                }
                return kiosqueRepository;
            }
        }

        private TypeKiosqueRepository typeKiosqueRepository;

        public TypeKiosqueRepository TypeKiosqueRepository
        {
            get
            {

                if (this.typeKiosqueRepository == null)
                {
                    this.typeKiosqueRepository = new TypeKiosqueRepository(context);
                }
                return typeKiosqueRepository;
            }
        }

        private TypeTournoiRepository typeTournoiRepository;

        public TypeTournoiRepository TypeTournoiRepository
        {
            get
            {
                if (this.typeTournoiRepository == null)
                {
                    this.typeTournoiRepository = new TypeTournoiRepository(context);
                }
                return typeTournoiRepository;
            }
        }

        private SpectacleRepository spectacleRepository;
       // private SpectacleRepository spectacleRepository;

         public SpectacleRepository SpectacleRepository
         {
             get
             {

                 if (this.spectacleRepository == null)
                 {
                     this.spectacleRepository = new SpectacleRepository(context);
                 }
                 return spectacleRepository;
             }
         }

        private TypeSpectacleRepository typeSpectacleRepository;

        public TypeSpectacleRepository TypeSpectacleRepository
        {
            get
            {

                if (this.typeSpectacleRepository == null)
                {
                    this.typeSpectacleRepository = new TypeSpectacleRepository(context);
                }
                return typeSpectacleRepository;
            }
        }


        private PlageHoraireRepository plageHoraireRepository;

        public PlageHoraireRepository PlageHoraireRepository
        {
            get
            {
                if (this.plageHoraireRepository == null)
                {
                    this.plageHoraireRepository = new PlageHoraireRepository(context);
                }
                return plageHoraireRepository;
            }
        }

        private EquipeRepository equipeRepository;

        public EquipeRepository EquipeRepository
        {
            get
            {

                if (this.equipeRepository == null)
                {
                    this.equipeRepository = new EquipeRepository(context);
                }
                return equipeRepository;
            }
        }

        private EquipeAvancementRepository equipeAvancementRepository;

        public EquipeAvancementRepository EquipeAvancementRepository
        {
            get
            {

                if (this.equipeAvancementRepository == null)
                {
                    this.equipeAvancementRepository = new EquipeAvancementRepository(context);
                }
                return equipeAvancementRepository;
            }
        }

        private PartieRepository partieRepository;

        public PartieRepository PartieRepository
        {
            get
            {

                if (this.partieRepository == null)
                {
                    this.partieRepository = new PartieRepository(context);
                }
                return partieRepository;
            }
        }

        //private ExempleRepo exempleRepository;

        //public ExempleRepo ExempleReposiroy
        //{
        //    get
        //    {

        //        if (this.exempleRepository == null)
        //        {
        //            this.exempleRepository = new ExempleRepo(context);
        //        }
        //        return exempleRepository;
        //    }
        //}

        public void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {

                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors

                        .SelectMany(x => x.ValidationErrors)

                        .Select(x => x.ErrorMessage);
                // Join the list to a single string.

                var fullErrorMessage = string.Join("; ", errorMessages);
                // Combine the original exception message with the new one.

                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.

                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

            }
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsRoomAvailableForTime(Evenement ev, List<PlageHoraire> plages)
        {
            // Aller chercher les evenements du meme congrès qui se passe dans la même pièce.
            List<Evenement> eventsInSameRoom = this.EvenementRepository.ObtenirEvenements().Where(e => ev.Congres.Id == e.Congres.Id && e.Salle.NoSalle.Equals(ev.Salle.NoSalle)).ToList();

            // Assignation des plages horaire de l'évènement à vérifier à une variable temporaire.
            List<PlageHoraire> timeForEvent = plages;

            // Pour chaque evenement qui se passe dans la même pièce...
            foreach (Evenement eve in eventsInSameRoom)
            {
                // On vérifis chaque plage horaire...
                foreach (PlageHoraire time in eve.PlageHoraires)
                {
                    // Pour voir si il y a conflit avec l'évènement.
                    foreach (PlageHoraire timeInCurrentEvent in timeForEvent)
                    {
                        // Si la date de début ou la date de fin d'une plage horaire de l'évènement courant
                        // se trouve encapsuler dans une plage horaire d'un autre évènement, il y a présence de conflit.
                        //if((timeInCurrentEvent.DateEtHeureDebut >= time.DateEtHeureDebut &&
                        //    timeInCurrentEvent.DateEtHeureDebut < time.DateEtHeureFin) ||
                        //    (timeInCurrentEvent.DateEtHeureFin > time.DateEtHeureDebut &&
                        //    timeInCurrentEvent.DateEtHeureFin < time.DateEtHeureFin))
                        //{
                        //    // Il y a conflit.
                        //    return false;
                        //}

                        // Si la date de début ou la date de fin d'une plage horaire de l'évènement courant
                        // se trouve encapsuler dans une plage horaire d'un autre évènement, il y a présence de conflit.
                        if(timeInCurrentEvent.DateEtHeureDebut == time.DateEtHeureDebut)
                        {
                            // Il y a conflit.
                            return false;
                        }
                        if((timeInCurrentEvent.DateEtHeureFin > time.DateEtHeureDebut) &&
                            (timeInCurrentEvent.DateEtHeureFin <= time.DateEtHeureFin))
                        {
                            // Il y a conflit.
                            return false;
                        }
                        if ((timeInCurrentEvent.DateEtHeureDebut > time.DateEtHeureDebut) &&
                            (timeInCurrentEvent.DateEtHeureDebut <= time.DateEtHeureFin))
                        {
                            // Il y a conflit.
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}