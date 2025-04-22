using System.Text;

namespace CIMEX_Project;

public class Investigator : TeamMember, IInvestigator
{
    public Investigator()
    {
    }

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

    public async Task SendReminder(Investigator investigator, Patient patient, StructureOfVisit structureOfVisit)
    {
        DAOTeamMemeberNeo4j daoTeamMemeberNeo4J = new DAOTeamMemeberNeo4j();
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var task in structureOfVisit.Tasks)
        {
            stringBuilder.AppendLine(task);
        }
        string tasks = stringBuilder.ToString();

        string textOfEmail = $"Dear Dr.{investigator.Surname},\n " +
                             $"As part of Study {patient.StudyName}, you are scheduled to perform Visit {structureOfVisit.Name} " +
                             $"for Patient {patient.Surname} {patient.Name} on {patient.NextPatientsVisit.DateOfVisit:dd.MM.yyyy}" +
                             $".During this visit, the following procedures are to be carried out:\n" +
                             $"{tasks}\nThis message was generated automatically by the CIMEX system.\nNo reply is required.Kind regards,\nCIMEX Team";
        string subject =
            $"{patient.StudyName} Visit {patient.NextPatientsVisit.Name} {patient.NextPatientsVisit.DateOfVisit:dd.MM.yyyy}";
        await daoTeamMemeberNeo4J.SendReminder(investigator.Email, subject, textOfEmail);

    }
}