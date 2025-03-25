namespace CIMEX_Project;

public interface IDaoVisit
{
    Task<List<Visit>> GetAllPatienVisits(string PatientID);
}