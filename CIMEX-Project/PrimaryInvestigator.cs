namespace CIMEX_Project;

public class PrimaryInvestigator : Investigator
{
    public PrimaryInvestigator(string name, string surname, string email, string role) : base(name, surname, email, role)
    {
    }

    public void CreateStudy()
    {
        
    }

    public void SetTeamMemeber()
    {
        
    }

    public void ProveDocument()
    {
        
    }

    public override void AddNewPatient()
    {
        base.AddNewPatient();
    }

    public override List<Patient> GetAllPatints()
    {
        return base.GetAllPatints();
    }

    public override Patient GetPatient()
    {
        return base.GetPatient();
    }

    public override List<TeamMember> GetTeamList()
    {
        return base.GetTeamList();
    }
    

    public override Study GetStudy()
    {
        return base.GetStudy();
    }



    public override List<Study> GetStudyList()
    {
        return base.GetStudyList();
    }

    public override void SetVisit()
    {
        base.SetVisit();
    }

 

    public override void SignDocument()
    {
        base.SignDocument();
    }
}