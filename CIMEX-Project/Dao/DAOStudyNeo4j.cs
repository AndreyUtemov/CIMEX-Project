using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace CIMEX_Project;

public class DAOStudyNeo4j : DAOStudy
{

    public DAOStudyNeo4j()
    {
    }

    public async Task<List<Study>> GetAllUsersStudies(string eMail)
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