using System.Net.Http;
using System.Text;
using System.Text.Json;
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

    public async Task<List<Study>> GetAllStudy(string eMail)
    {
        Console.WriteLine($"We are here with {eMail}");
        try
        {
            string requestUrl = $"studies/{Uri.EscapeDataString(eMail)}";


            HttpResponseMessage responseMessage = await ApiClient.Instance.GetAsync(requestUrl);

            Console.WriteLine(responseMessage.ToString());

            Console.WriteLine($"Request answer {responseMessage.StatusCode}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                Console.WriteLine("Pechalka");
            }

            string jsonString = await responseMessage.Content.ReadAsStringAsync();

            Console.WriteLine($"Получен JSON: {jsonString}");

            List<Study>? studies = JsonSerializer.Deserialize<List<Study>?>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            foreach (var study in studies)
            {
                Console.WriteLine($"Name: {study.StudyName} Surname:  {study.FullName}  Role: {study.RoleOfUser}");
            }

            return studies;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> GetUserRoleInStudy(TeamMember user)
    {
        await _neo4jClient.Connect();
        await using var session = _neo4jClient.GetDriver().AsyncSession(); // Используем AsyncSession()
        bool isUserPI = false;

        try
        {
            var result = await session.RunAsync(
                "MATCH (s:Study)-[r:ASSIGNED_TO]-(t:TeamMember) WHERE t.email = $teamMemberEmail" +
                " RETURN r.isPrincipal AS isUserPI", new { teamMemberEmail = user.Email });
            await result.ForEachAsync(record => { isUserPI = record["isUserPI"].As<bool>(); });
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error fetching studies: {e.Message}");
        }

        return isUserPI;
    }

    public async Task<bool> CreateStudy(Study study)
    {
        
        try
        {
            var payload = new
            {
                Title = study.StudyName,
                FullName = study.FullName,
                PrincipalInvestigatorsEmail = study.PrincipalInvestigator.Email 
             };

            string json = JsonSerializer.Serialize(payload);
            Console.WriteLine(json.ToString());
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string requestUrl = "study-creator";
            HttpResponseMessage response = await ApiClient.Instance.PostAsync(requestUrl, content);

            Console.WriteLine($"Response: {response.StatusCode}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in CreateTeamMember: {ex.Message}");
            return false;
        }
    }

    public Study SetStudy(Study study)
    {
        throw new NotImplementedException();
    }

    public int DeleteStudy()
    {
        throw new NotImplementedException();
    }

    private class StudyCreator
    {
        public string Title { get; set; }
        public string FullName { get; set; }
        public string PrincipalInvestigatorsEmail { get; set; }
    }
}