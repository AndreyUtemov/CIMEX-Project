namespace CIMEX_Project;

public class PrincipalInvestigator : Investigator, IPrincipalInvestigator
{
    public PrincipalInvestigator(string name, string surname, string email, string role) : base(name, surname, email, role)
    {
    }

    public void SetTeamMemeber()
    {
        
    }

    public void ProveDocument()
    {
        
    }

    public  void AddNewPatient()
    {
        base.AddNewPatient();
    }

    public List<Patient> GetAllPatints()
    {
        return base.GetAllPatints();
    }

    public  Patient GetPatient()
    {
        return base.GetPatient();
    }

    public  List<TeamMember> GetTeamList()
    {
        return base.GetTeamList();
    }
    

    public  Study GetStudy()
    {
        return base.GetStudy();
    }



    public  List<Study> GetStudyList()
    {
        return base.GetStudyList();
    }

    public  void SetVisit()
    {
        base.SetVisit();
    }

 

    public  void SignDocument()
    {
        base.SignDocument();
    }
}