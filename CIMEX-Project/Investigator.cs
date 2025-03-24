namespace CIMEX_Project;

public class Investigator : TeamMember, IInvestigator
{
    public Investigator(string name, string surname, string email, string role) : base(name, surname, email, role)
    {
    }

    public void conductVisit()
    {
        
    }

    public void registerSAE()
    {
        
    }
        
        
    public  void AddNewPatient()
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

    public  Study GetStudy()
    {
        throw new NotImplementedException();
    }
    

    public List<Study> GetStudyList()
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