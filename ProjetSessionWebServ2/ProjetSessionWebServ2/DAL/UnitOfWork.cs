using ProjetSessionWebServ2.DAL;
using ProjetSessionWebServ2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace GestionPhotoImmobilier.DAL
{
    public class UnitOfWork : IUnitOfWork
    {

        
        private ApplicationDbContext context = new ApplicationDbContext();
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