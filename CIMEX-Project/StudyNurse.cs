namespace CIMEX_Project;

public class StudyNurse : TeamMember
{
    public StudyNurse(string name, string surname, string email, string role) : base(name, surname, email, role)
    {
    }

    public override void AddNewPatient()
    {
        throw new NotImplementedException();
    }

    public override List<Patient> GetAllPatints()
    {
        throw new NotImplementedException();
    }

    public override Patient GetPatient()
    {
        throw new NotImplementedException();
    }

    public override List<TeamMember> GetTeamList()
    {
        throw new NotImplementedException();
    }

    public override List<Study> GetStudyList()
    {
        throw new NotImplementedException();
    }

    public override Study GetStudy()
    {
        throw new NotImplementedException();
    }

    public override void SetVisit()
    {
        throw new NotImplementedException();
    }
    

    public override void SignDocument()
    {
        throw new NotImplementedException();
    }
}