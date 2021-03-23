using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Utils
{
    public class Helpers
    {


        //User Types
        public const int SiwesAdmin = 1;
        public const int SiwesSupervisor = 2;
        public const int Lecturer = 3;
        public const int Student = 4;
        public const int IndustrialSupervisor = 5;

        // Claim Types
        public const string ClaimType_UserType = "UserType";
        public const string ClaimType_UserEmail = "UserEmail";
    }
}
