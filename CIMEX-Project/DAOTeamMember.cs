namespace CIMEX_Project;

public interface DAOTeamMember

{
    Task<bool> CheckUserPassword(string eMail, string password);
    List<TeamMember> GetALlTeamMembers();

    Task<bool> IsUserPI(Study study, TeamMember user);

    Task<TeamMember> GetTeamMemberByLogin(string eMail);

    void CreateInvestigator(Investigator investigator);

    void SetInvestigator(Investigator investigator);

    int DeleteInvestigator(Investigator investigator);
}