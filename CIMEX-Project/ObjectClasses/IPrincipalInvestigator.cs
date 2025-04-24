namespace CIMEX_Project;

public interface IPrincipalInvestigator
{
    public void SetTeamMemeber(TeamMember teamMember, Study study);

    public void WithdrawTeamMember(string email, Study study);

    public void ProveDocument();
 
}