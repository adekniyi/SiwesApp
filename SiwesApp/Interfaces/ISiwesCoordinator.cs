using SiwesApp.Dtos.All;
using SiwesApp.Dtos.SiwesCoOrdinatotDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Interfaces
{
    interface ISiwesCoordinator
    {
        public Task<ToRespond> CreateStudent(SiwesCoordinatorRequest studentRequest);

    }
}
