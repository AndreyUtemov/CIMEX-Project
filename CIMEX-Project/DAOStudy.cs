using Microsoft.AspNetCore.Mvc;

namespace CIMEX_Project;

public interface DAOStudy
{
    Task<List<Study>> GetAllStudy(string eMail);
    
    Task<IActionResult> CreateStudy(Study study);

    Study SetStudy(Study study);

    int DeleteStudy();

}