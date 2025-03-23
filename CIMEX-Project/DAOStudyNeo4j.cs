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
            var result = await session.RunAsync("MATCH (s:Study)-[n]-(p) WHERE p.email = $teamMemberEmail" +
                                                " RETURN s.name AS studyName, s.fullname AS fullName", new {teamMemberEmail = user.Email});
            await result.ForEachAsync(record =>
            {
                var studyName = record["studyName"].As<string>();
                var fullName = record["fullName"].As<string>();

                Study study = new Study(studyName, fullName);
                
                studies.Add(study);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error fetching studies: {e.Message}");
        }

        return studies;
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