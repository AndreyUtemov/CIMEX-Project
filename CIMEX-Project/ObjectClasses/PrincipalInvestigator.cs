namespace CIMEX_Project;

public class PrincipalInvestigator : Investigator, IPrincipalInvestigator
{
    public PrincipalInvestigator()
    {
    }

    public PrincipalInvestigator(string name, string surname, string email, string role) : base(name, surname, email, role)
    {
    }

    public void SetTeamMemeber(TeamMember teamMember, Study study)
    {
        
    }

    public void WithdrawTeamMember(string email, Study study)
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

    public async Task SendAppointment(PrincipalInvestigator principalInvestigator, Study study)
    {
        string subject = $"Appointment as Principal Investigator for {study.StudyName}";
        string mailContent =
            $"Dear Dr.{principalInvestigator.Surname},\nWe are pleased to inform you that you have been selected" +
            $" as the Principal Investigator for the {study.StudyName} study. " +
            $"Kindly remember to include the research team members.\n" +
            "This message was automatically generated in CIMEX, and no response is required.\n" +
            "Thank you,\n CIMEX-team";
        DAOTeamMemeberNeo4j daoTeamMemeberNeo4J = new DAOTeamMemeberNeo4j();
        await daoTeamMemeberNeo4J.SendReminder(principalInvestigator.Email, subject, mailContent);
    }
}