using Microsoft.AspNetCore.Mvc;

namespace CIMEX_Project;

public interface DAOStudy
{
    Task<IActionResult> GetAllStudy();
    
    Task<IActionResult> CreateStudy(Study study);

    Study SetStudy(Study study);

    int DeleteStudy();

}