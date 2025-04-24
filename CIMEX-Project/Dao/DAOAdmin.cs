namespace CIMEX_Project;

public interface IDaoAdmin
{
    Task<List<Study>> GetAllStudy();
    Task<List<TeamMember>> GetAllTeamMember();
}