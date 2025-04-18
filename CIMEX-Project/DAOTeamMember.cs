namespace CIMEX_Project;

public interface DAOTeamMember

{
    Task<bool> CheckUserPassword(string eMail, string password);
    Task<List<TeamMember>> GetAllTeamMembers(Study study);

    Task<bool> IsUserPI(Study study, TeamMember user);

    Task<TeamMember> GetTeamMember(string eMail);

    Task<bool>CreateTeamMemeber(TeamMember teamMember, string password);

    void SetInvestigator(Investigator investigator);

    int DeleteInvestigator(Investigator investigator);
}