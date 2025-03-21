using System.Reflection.Metadata;

namespace CIMEX_Project;

public class TeamMember : Person
{
    public string Email { get; private set; }
    
    public string Role { get; private set; }


    protected TeamMember(string name, string surname, string email, string role) : base(name, surname)
    {
        Email = email;
        Role = role;
    }


    public void AddNewPatient()
    {
        throw new NotImplementedException();
    }

    
    public  List<Patient> GetAllPatints()
    {
        throw new NotImplementedException();
    }

    public  Patient GetPatient()
    {
        throw new NotImplementedException();
    }

    public  List<TeamMember> GetTeamList()
    {
        throw new NotImplementedException();
    }

    public  List<Study> GetStudyList()
    {
        throw new NotImplementedException();
    }

    public  Study GetStudy()
    {
        throw new NotImplementedException();
    }

    public void SetVisit()
    {
        throw new NotImplementedException();
    }


    public  void SignDocument()
    {
        throw new NotImplementedException();
    }
}