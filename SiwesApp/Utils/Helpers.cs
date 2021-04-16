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
        public const int SiwesCoordinator = 2;
        public const int Lecturer = 3;
        public const int Student = 4;
        public const int IndustrialSupervisor = 5;

        //Eligibility Status 
        public const int Pending = 1;
        public const int Eligible = 2;
        public const int Rejected = 3;

        // STATUS MESSAGES
        public const string StatusMessageSuccess = "Request Successful";
        public const string StatusMessageMailFailure = "Object could not send Mail";
        public const string StatusMessageNotFound = "Object was not Found";
        public const string StatusMessageObjectNull = "Object is Empty";
        public const string StatusMessageSaveError = "Object was unable to Save";
        public const string StatusMessageObjectExists = "Object already Exists";
        public const string StatusMessageBadRequest = "Bad Request";
        public const string StatusMessageSignInError = "The User was unable to SignIn";
        public const string StatusMessageEmailAlreadyConfirmed = "The Email Address has already been Confirmed by TestMi";
        public const string StatusMessageInvalidUserType = "The User Type is Invalid";
        public const string StatusMessageRoleAssignmentError = "Unable to Assign Role";
        public const string StatusMessageUnauthorized = "Unauthorized access";
        public const string StatusMessageLockedOut = "This Account Has Been Deactivated";

        //STATUS CODES
        public const int Success = 0;
        public const int MailFailure = 1;
        public const int NotFound = 2;
        public const int ObjectNull = 3;
        public const int SaveError = 4;
        public const int SaveNoRowAffected = 5;
        public const int NotSucceeded = 6;
        public const int ObjectExists = 7;
        public const int BadRequest = 8;
        public const int SignInError = 9;
        public const int EmailAlreadyConfirmed = 10;
        public const int InvalidUserType = 11;
        public const int RoleAssignmentError = 12;
        public const int HasSubmitted = 13;
        public const int Unauthorized = 14; 
        public const int LockedOut = 15; 
        public const int InvalidFileSize = 16; 
        public const int CloudinaryFileDeleteError = 17; 
        public const int PreviousPasswordStorageError = 18;
        public const int NewPasswordError = 19;

        // User Roles
        public const string SiwesAdminRole = "SiwesAdmin";
        public const string SiwesCoordinatorRole = "SiwesCoordinator";
        public const string LecturerRole = "Lecturer";
        public const string StudentRole = "Student";
        public const string IndustrialSupervisorRole = "IndustrialSupervisor";

        // Claim Types
        public const string ClaimType_UserType = "UserType";
        public const string ClaimType_UserEmail = "UserEmail";
        public const string ClaimType_StudentId = "StudentId";

    }
}
