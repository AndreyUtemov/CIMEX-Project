namespace CIMEX_Project;

public interface IDaoVisit
{
    Task<List<PatientsVisit>> GetAllPatienVisits(string PatientID);
}