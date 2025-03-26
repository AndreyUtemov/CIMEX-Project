namespace CIMEX_Project;

public class Patient : Person
{
    public string PatientId { get; private set; }
    public string Status { get; private set; }
    public string StudyName { get; private set; }
    public Visit NextVisit { get; private set; }

    public Patient(string name, string surname, string patientId, string status, string studyName, Visit nextVisit) : base(name, surname)
    {
        PatientId = patientId;
        Status = status;
        StudyName = studyName;
        NextVisit = nextVisit;
    }
}