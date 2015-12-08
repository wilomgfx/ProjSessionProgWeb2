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
                if(this.plageHoraireRepository == null)
                {
                    this.plageHoraireRepository = new PlageHoraireRepository(context);
                }
                return plageHoraireRepository;
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
    }
}