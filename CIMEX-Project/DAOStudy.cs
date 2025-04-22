using Microsoft.AspNetCore.Mvc;

namespace CIMEX_Project;

public interface DAOStudy
{
    Task<List<Study>> GetAllUsersStudies(string eMail);
    
    Task<bool> CreateStudy(Study study);

    Study SetStudy(Study study);

    int DeleteStudy();

}