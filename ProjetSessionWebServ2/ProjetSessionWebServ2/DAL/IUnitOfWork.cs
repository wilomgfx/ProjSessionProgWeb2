using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetSessionWebServ2.DAL
{
    interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
