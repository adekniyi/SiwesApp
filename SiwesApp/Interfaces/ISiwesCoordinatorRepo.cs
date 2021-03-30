using SiwesApp.Dtos.All;
using SiwesApp.Dtos.SiwesCoOrdinatotDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Interfaces
{
    public interface ISiwesCoordinatorRepo
    {
        public Task<ToRespond> CreateSiwesCo(SiwesCoordinatorRequest siwesCoordinatorRequest);

    }
}
