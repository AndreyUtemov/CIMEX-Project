using System.Reflection.Metadata;

namespace CIMEX_Project;

public class TeamMember : Person
{
    public string Email { get; private set; }
    public string Role { get; private set; }
    


    public TeamMember(string name, string surname, string email, string role) : base(name, surname)
    {
        Email = email;
        Role = role;
    }

    public void AddNewPatient(Patient patient)
    {
        DAOPatientNeo4j daoPatientNeo4J = new DAOPatientNeo4j();
        daoPatientNeo4J.CreatePatient(patient);
    }
    
    public async Task<List<Patient>> GetAllPatients(TeamMember user)
    {
        DAOPatientNeo4j daoPatientNeo4J = new DAOPatientNeo4j();
        List<Patient> patients = await daoPatientNeo4J.GetAllPatients(user);
        return patients;
    }

    public List<Patient> GetAllPatientsInStudy(string study, List<Patient> patients)
    {
        List<Patient> patientsInStudy = patients
            .Where(patient => patient.StudyName == study)
            .ToList();
        return patientsInStudy;
    }

    public  List<TeamMember> GetTeamList()
    {
        throw new NotImplementedException();
    }

    public async Task<List<Study>> GetAllStudy(TeamMember user)
    {
        DAOStudyNeo4j daoStudyNeo4J = new DAOStudyNeo4j();
        List<Study> studies = await daoStudyNeo4J.GetAllStudy(user);
        return studies;
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