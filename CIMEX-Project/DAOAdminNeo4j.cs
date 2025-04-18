using System.Net.Http;
using System.Text.Json;

namespace CIMEX_Project;

public class DAOAdminNeo4j : DAOAdmin
{
    public async Task<List<Study>> GetAllStudy()
    {
        Console.WriteLine($"GetallStudy< in AdminDao started");
        try
        {
            string requestUrl = "studies";

            HttpResponseMessage responseMessage = await ApiClient.Instance.GetAsync(requestUrl);

            Console.WriteLine(responseMessage.ToString());

            Console.WriteLine($"Request answer {responseMessage.StatusCode}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                Console.WriteLine("No responce");
            }

            string jsonString = await responseMessage.Content.ReadAsStringAsync();

            Console.WriteLine($"JSON responced: {jsonString}");

            List<Study>? studies = JsonSerializer.Deserialize<List<Study>?>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            foreach (var study in studies)
            {
                Console.WriteLine($"Name: {study.StudyName} Fullname:  {study.FullName}");
            }

            return studies;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<List<TeamMember>> GetAllTeamMember()
    {
        Console.WriteLine($"GetAllyTeamMemeber in AdminDao started");
        try
        {
            string requestUrl = "team";

            HttpResponseMessage responseMessage = await ApiClient.Instance.GetAsync(requestUrl);

            Console.WriteLine(responseMessage.ToString());

            Console.WriteLine($"Request answer {responseMessage.StatusCode}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                Console.WriteLine("No responce");
            }

            string jsonString = await responseMessage.Content.ReadAsStringAsync();

            Console.WriteLine($"JSON responced: {jsonString}");

            List<TeamMember>? team = JsonSerializer.Deserialize<List<TeamMember>?>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            foreach (var teamMember in team)
            {
                Console.WriteLine($"email: {teamMember.Email} Surname:  {teamMember.Surname}");
            }

            return team;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}