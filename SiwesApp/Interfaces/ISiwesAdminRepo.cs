using SiwesApp.Dtos.All;
using SiwesApp.Dtos.SiwesAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Interfaces
{
    public interface ISiwesAdminRepo
    {
        public Task<ToRespond> CreateSiwesAdmin(SiwesAdminRequest siwesAdminRequest);
    }
}
