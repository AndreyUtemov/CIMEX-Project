namespace CIMEX_Project;

public interface DAOTeamMember

{
    List<TeamMember> GetALlTeamMembers();

    List<TeamMember> GetTeamMembersByStudy(Study study);

    TeamMember GetTeamMemberByLogin(string eMail);

    void CreateInvestigator<>(Investigator investigator);

    void SetInvestigator(Investigator investigator);

    int DeleteInvestigator(Investigator investigator);
}