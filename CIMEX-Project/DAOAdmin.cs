namespace CIMEX_Project;

public interface DAOAdmin
{
    Task<List<Study>> GetAllStudy();
    Task<List<TeamMember>> GetAllTeamMember();
}