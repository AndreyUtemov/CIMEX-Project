namespace CIMEX_Project;

public class Patient : Person
{
    public string PatientId { get; private set; }
    public string Status { get; private set; }
    public Visit NextVisit { get; private set; }
    public bool Included { get; private set; }

    public Patient(string name, string surname, string patientId, string status, Visit nextVisit, bool included) : base(name, surname)
    {
        PatientId = patientId;
        Status = status;
        NextVisit = nextVisit;
        Included = included;
    }
}