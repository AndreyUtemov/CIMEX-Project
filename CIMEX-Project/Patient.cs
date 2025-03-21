namespace CIMEX_Project;

public class Patient : Person
{
    public string PatientId { get; private set; }
    public bool Included { get; private set; }
    
    public string StudyName { get; private set; }
    public DateTime NextVisit { get; private set; }

    public DateTime NextSheduledVisit { get; private set; }

    public Patient(string name, string surname, string patientId, bool included, string studyName, DateTime nextVisit, DateTime nextSheduledVisit) : base(name, surname)
    {
        PatientId = patientId;
        Included = included;
        StudyName = studyName;
        NextVisit = nextVisit;
        NextSheduledVisit = nextSheduledVisit;
    }
}