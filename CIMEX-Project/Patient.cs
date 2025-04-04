namespace CIMEX_Project;

public class Patient : Person
{
    public string PatientHospitalId { get; private set; }
    public string PatientStudyId { get; private set; }
    public string StudyName { get; private set; }
    public string Status { get; private set; }
    public Visit NextVisit { get;  set; }

    public Patient(string name, string surname, string patientHospitalId, string patientStudyId, string studyName, string status, Visit nextVisit) : base(name, surname)
    {
        PatientHospitalId = patientHospitalId;
        PatientStudyId = patientStudyId;
        StudyName = studyName;
        Status = status;
        NextVisit = nextVisit;
    }
}