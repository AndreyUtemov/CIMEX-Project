using System.Reflection.Metadata;

namespace CIMEX_Project;

public abstract class TeamMember : Person
{
    public string Email { get; private set; }
    
    public string Role { get; private set; }


    protected TeamMember(string name, string surname, string email, string role) : base(name, surname)
    {
        Email = email;
        Role = role;
    }


    public abstract void AddNewPatient();

    public abstract List<Patient> GetAllPatints();

    public abstract Patient GetPatient();

    public abstract List<TeamMember> GetTeamList();

    public abstract List<Study> GetStudyList();

    public abstract Study GetStudy();

    public abstract void SetVisit();

  

    public abstract void SignDocument();

}