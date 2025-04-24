namespace CIMEX_Project;

public interface DAOPatient
{
    Task<List<Patient>> GetAllPatients(string eMail);

   Task<bool> CreatePatient(Patient patient, Investigator investigator);

    Patient SetPatient(Patient patient);

    int DeletePatient(Patient patient);

}