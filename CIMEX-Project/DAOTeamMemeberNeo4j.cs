using Neo4j.Driver;

namespace CIMEX_Project;

public class DAOTeamMemeberNeo4j : DAOTeamMember
 {
    private readonly Neo4jClient _neo4JClient;

    public DAOTeamMemeberNeo4j()
    {
        _neo4JClient = Neo4jClient.Instance;
    }
    
    public async Task<bool> CheckUserPassword(string eMail, string password)
    {
        await _neo4JClient.Connect();
        await using var session = _neo4JClient.GetDriver().AsyncSession();
        bool accessApproved = false;
        try
        {
            var result = await session.RunAsync(
                "MATCH (t:TeamMember) WHERE t.email = $eMail RETURN t.crypto AS password",
                new { eMail = eMail });
            await result.ForEachAsync(record =>
            {
                var correctPassword = record["password"].As<string>();
                if (correctPassword == password)
                {
                    accessApproved = true;
                }
            });
            return accessApproved;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
    public async Task<TeamMember> GetTeamMemberByLogin(string eMail)
    {
        await _neo4JClient.Connect();
        await using var session = _neo4JClient.GetDriver().AsyncSession();
        var name = "";
        var surname = "";
        var role = "";
        try
        {
            var result = await session.RunAsync("MATCH (t:TeamMember) WHERE t.email = $eMail " +
                                                "RETURN t.name AS name, t.surname AS surname, t.role AS role"
                , new { eMail = eMail });
            await result.ForEachAsync(record =>
            {
                name = record["name"].As<string>();
                surname = record["surname"].As<string>();
                role = record["role"].As<string>();

            });
            return new TeamMember(name, surname, eMail, role);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public List<TeamMember> GetALlTeamMembers()
    {
        // MATCH (n:Employee) RETURN 
        throw new NotImplementedException();
    }

    public async Task<bool> IsUserPI(Study study, TeamMember user)
    {
        // MATCH (e:Employee)-[c]-(s:Study) WHERE s.name = "ACTIVE" RETURN e
        await _neo4JClient.Connect();
        await using var session = _neo4JClient.GetDriver().AsyncSession();
        bool isPI = false;
        try
        {
            var result = await session.RunAsync("MATCH (t:Team_Member)-[r:ASSIGNED_TO]->(s:Study) WHERE  t.email = $userEmail " +
                                                "AND s.name = $studyName RETURN  r.isPI AS isUserPI", 
                                                new {userEmail = user.Email, studyName = study.StudyName});
            await result.ForEachAsync(record =>
            {
                isPI = record["isUserPI"].As<bool>();
                
                
            });
            return isPI;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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