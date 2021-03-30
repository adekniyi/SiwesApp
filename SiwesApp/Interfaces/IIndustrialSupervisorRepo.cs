using SiwesApp.Dtos.All;
using SiwesApp.Dtos.IndustrialSupervisorDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Interfaces
{
    public interface IIndustrialSupervisorRepo
    {
        public Task<ToRespond> CreateIndustrialSupervisor(IndustrialSupervisorRequest industrialSupervisorRequest);

    }
}
