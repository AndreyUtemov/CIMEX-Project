namespace CIMEX_Project;

public class DAOTeamMemeberNeo4j : DAOTeamMember
{
    public List<TeamMember> GetALlTeamMembers()
    {
        // MATCH (n:Employee) RETURN 
        throw new NotImplementedException();
    }

    public List<TeamMember> GetTeamMembersByStudy(Study study)
    {
        // MATCH (e:Employee)-[c]-(s:Study) WHERE s.name = "ACTIVE" RETURN e
        throw new NotImplementedException();
    }

    public TeamMember GetTeamMemberByLogin(string eMail)
    {
        // MATCH (e:Employee) WHERE e.email = "david.fischer@cimex.at" RETURN e 
        throw new NotImplementedException();
    }

    public void CreateInvestigator(Investigator investigator)
    {
        throw new NotImplementedException();
    }

    public void SetInvestigator(Investigator investigator)
    {
        throw new NotImplementedException();
    }

    public int DeleteInvestigator(Investigator investigator)
    {
        throw new NotImplementedException();
    }
}