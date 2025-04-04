using Microsoft.AspNetCore.Mvc;

namespace CIMEX_Project;

public interface DAOPatient
{
   Task<List<Patient>> GetAllPatients(TeamMember user);

   Task<int> CreatePatient(Patient patient);

    Patient SetPatient(Patient patient);

    int DeletePatient(Patient patient);

}