using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

namespace CIMEX_Project;

public class DAOStudyNeo4j : DAOStudy
{
    private readonly Neo4jClient _neo4jClient;

    public DAOStudyNeo4j(Neo4jClient neo4jClient) // Внедряем зависимость
    {
        _neo4jClient = neo4jClient;
    }

    public DAOStudyNeo4j()
    {
    }

    public async Task<List<Study>> GetAllStudy(TeamMember user)
    {
        await _neo4jClient.Connect();
        await using var session = _neo4jClient.GetDriver().AsyncSession(); // Используем AsyncSession()
        var studies = new List<Study>();

        try
        {
            var result = await session.RunAsync("MATCH (s:Study)-[r:ASSIGNED_TO]-(t:TeamMember) WHERE t.email = $teamMemberEmail" +
                                                " RETURN s.title AS studyName, s.fullName AS fullName, r.role AS roleInStudy," +
                                                "r.needAttention AS needAttention ", new {teamMemberEmail = user.Email});
            await result.ForEachAsync(record =>
            {
                var studyName = record["studyName"].As<string>();
                var fullName = record["fullName"].As<string>();
                var roleInStudy = record["roleInStudy"].As<string>();
                var needAttention = true;
                // var needAttention = record["needAttention"].As<bool>();

                Study study = new Study(studyName, fullName, roleInStudy, needAttention);
                
                studies.Add(study);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error fetching studies: {e.Message}");
        }

        return studies;
    }
    
    public async Task<bool> GetUserRoleInStudy(TeamMember user)
    {
        await _neo4jClient.Connect();
        await using var session = _neo4jClient.GetDriver().AsyncSession(); // Используем AsyncSession()
        bool isUserPI = false;

        try
        {
            var result = await session.RunAsync("MATCH (s:Study)-[r:ASSIGNED_TO]-(t:TeamMember) WHERE t.email = $teamMemberEmail" +
                                                " RETURN r.isPrincipal AS isUserPI", new {teamMemberEmail = user.Email});
            await result.ForEachAsync(record =>
            {
                isUserPI = record["isUserPI"].As<bool>();
            });
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error fetching studies: {e.Message}");
        }

        return isUserPI;
    }
    
    public Task<IActionResult> CreateStudy(Study study)
    {
        throw new NotImplementedException();
    }

    public Study SetStudy(Study study)
    {
        throw new NotImplementedException();
    }

    public int DeleteStudy()
    {
        throw new NotImplementedException();
    }
}