using Codxia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codxia.Core
{
    public interface IUnitOfWork : IDisposable
    {


        IUserRepository UsersManger { get; }
        Task<int> Complete();
    }
}
