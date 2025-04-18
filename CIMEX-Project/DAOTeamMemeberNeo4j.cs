using System.ComponentModel.DataAnnotations;
using System.DirectoryServices.ActiveDirectory;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Documents;
using Neo4j.Driver;

namespace CIMEX_Project;

public class DAOTeamMemeberNeo4j : DAOTeamMember
 {
    private static readonly Neo4jClient _neo4JClient = Neo4jClient.Instance;
    private DAOTeamMember _daoTeamMemberImplementation;
    
    public async Task<bool> CheckUserPassword(string eMail, string password)
    {
        await _neo4JClient.Connect();
        await using var session = _neo4JClient.GetDriver().AsyncSession();
        bool accessApproved = false;
        try
        {
            var result = await session.RunAsync(
                "MATCH (t:TeamMember)-[:HAS_PASSWORD]->(p:Password) WHERE t.email = $eMail RETURN p.password AS password",
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
    
    public async Task<TeamMember> GetTeamMember(string eMail)
    {
      try
        {
            string requestUrl = $"team/{eMail}";
            HttpResponseMessage responseMessage = await ApiClient.Instance.GetAsync(requestUrl);
            Console.WriteLine($"Request answer {responseMessage.StatusCode}");
            if(!responseMessage.IsSuccessStatusCode)
            {
                Console.WriteLine("Pechalka");
            }
           
            string jsonString = await responseMessage.Content.ReadAsStringAsync();
           
            Console.WriteLine($"Recived JSON: {jsonString}");
            TeamMember? teamMember = JsonSerializer.Deserialize<TeamMember>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            teamMember.Email = eMail;
            return teamMember;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<List<TeamMember>> GetAllTeamMembers(Study study)
    {
        await _neo4JClient.Connect();
        await using var session = _neo4JClient.GetDriver().AsyncSession();
        List<TeamMember> teamMembers = new List<TeamMember>();
        string name;
        string surname;
        string email;
        string role;
        try
        {
            var result = await session.RunAsync(
                "MATCH(t:TeamMember)-[r:ASSIGNED_TO]->(s:Study) WHERE s.name = $studyName" +
                "RETURN t.name AS Name, t.surname AS Surname, r.roleInStudy AS Role, t.email AS Email"
                , new { studyName = study.StudyName });
            await result.ForEachAsync(record =>
            {
                name = record["Name"].As<string>();
                surname = record["Surname"].As<string>();
                role = record["Role"].As<string>();
                email = record["Email"].As<string>();

                TeamMember teamMember = new TeamMember(name, surname, email, role);
                teamMembers.Add(teamMember);
            });
            return teamMembers;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> IsUserPI(Study study, TeamMember user)
    {
        // MATCH (e:Employee)-[c]-(s:Study) WHERE s.name = "ACTIVE" RETURN e
        await _neo4JClient.Connect();
        await using var session = _neo4JClient.GetDriver().AsyncSession();
        bool isPI = false;
        try
        {
            var result = await session.RunAsync("MATCH (t:TeamMember)-[r:ASSIGNED_TO]->(s:Study) WHERE  t.email = $userEmail " +
                                                "AND s.name = $studyName RETURN  r.isPrincipal AS isUserPI", 
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


    public async Task<bool> CreateTeamMemeber(TeamMember teamMember, string password)
    {
        Console.WriteLine("Started CreateTeamMemeber");
        try
        {
            Console.WriteLine($"name {teamMember.Name} surname {teamMember.Surname} email {teamMember.Email} role {teamMember.Role} password {password}");
            // Создаём объект для API, который включает пароль
            var payload = new
            {
                Name = teamMember.Name,
                Surname = teamMember.Surname,
                Email = teamMember.Email,
                Role = teamMember.Role,
                Password = password
            };

            string json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string requestUrl = "team-member-creator";
            HttpResponseMessage response = await ApiClient.Instance.PostAsync(requestUrl, content);

            Console.WriteLine($"Response: {response.StatusCode}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in CreateTeamMemeber: {ex.Message}");
            return false;
        }
    }



    public void SetInvestigator(Investigator investigator)
    {
        throw new NotImplementedException();
    }

    public int DeleteInvestigator(Investigator investigator)
    {
        throw new NotImplementedException();
    }
    
    private class TeamMemberInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
    }
}
