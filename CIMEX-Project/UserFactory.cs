namespace CIMEX_Project;

public class UserFactory
{
    public TeamMember CreateUserForStudy(string RoleInStudy, TeamMember user)
    {
        string name = user.Name;
        string surname = user.Surname;
        string eMail = user.Email;
        string role = user.Role;
        
        switch (RoleInStudy)
        {
            case "Principal":
                return new PrincipalInvestigator(name, surname, eMail, role);
            case "Investigator":
                return new Investigator(name, surname, eMail, role);
            case "Nurse":
                return new StudyNurse(name, surname, eMail, role);
            default:
                return new TeamMember(name, surname, eMail, role);
            
        }
    }
}