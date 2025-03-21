using Microsoft.AspNetCore.Mvc;

namespace CIMEX_Project;

public interface DAOPatient
{
   Task<List<Patient>> GetAllPatients(TeamMember user);

   Task<IActionResult> GetAllPatientsInStudy([FromBody] Study study);

    Task CreatePatient(Patient patient);

    Patient SetPatient(Patient patient);

    int DeletePatient(Patient patient);

}