namespace CIMEX_Project;

public class Patient : Person
{
    public string PatientHospitalId { get;  set; }
    public string PatientStudyId { get; set; }
    public string StudyName { get;  set; }
    public string Status { get;  set; }
    public PatientsVisit NextPatientsVisit { get;  set; }

    public Patient() : base("", "")
    {
    }

    public Patient(string name, string surname, string patientHospitalId, string studyName, string status, PatientsVisit nextPatientsVisit) : base(name, surname)
    {
        PatientHospitalId = patientHospitalId;
        StudyName = studyName;
        Status = status;
        NextPatientsVisit = nextPatientsVisit;
    }

  
    public async Task CreateNewVisit(Patient patient, PatientsVisit patientsVisit)
    {
        DAOPatientNeo4j daoPatientNeo4J = new DAOPatientNeo4j();
        daoPatientNeo4J.CreateNewVisit(patient, patientsVisit);
    }
}