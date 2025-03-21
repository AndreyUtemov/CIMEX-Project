using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIMEX_Project;

public class DAOPatientNeo4j : DAOPatientManagement
{
    private readonly HttpClient _httpClient;
    private DAOPatientManagement _daoPatientManagementImplementation;

    public DAOPatientNeo4j()
    {
        _httpClient = new HttpClient();
    }


    public async Task<List<Patient>> GetAllPatients(TeamMember user)
    {
        try
        {
            var url = "http://localhost:5000/api/restapipatientmanagementneo4j";
        
            // Сериализация объекта user в JSON
            if (user != null)
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

                // Отправка POST-запроса с телом
                var response = await _httpClient.PostAsync(url, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    // Десериализация ответа в список пациентов
                    var content = await response.Content.ReadAsStringAsync();
                    var patientList = JsonSerializer.Deserialize<List<Patient>>(content);

                    return patientList;
                }
                else
                {
                    // Обработка ошибки, если запрос не успешен
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public Task<IActionResult> GetAllPatients()
    {
        return _daoPatientManagementImplementation.GetAllPatients();
    }

    public Task<IActionResult> GetAllPatientsInStudy(Study study)
    {
        throw new NotImplementedException();
    }

    public Task CreatePatient(Patient patient)
    {
        throw new NotImplementedException();
    }

    public Patient SetPatient(Patient patient)
    {
        throw new NotImplementedException();
    }

    public int DeletePatient(Patient patient)
    {
        throw new NotImplementedException();
    }
}