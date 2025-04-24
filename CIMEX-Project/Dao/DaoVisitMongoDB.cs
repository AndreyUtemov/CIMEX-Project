using System.Text;

namespace CIMEX_Project;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class DaoVisitMongoDb
{
    public async Task<List<PatientsVisit>> GetPatientVisits(string patientId)
    {
        Console.WriteLine("Start GetPatientVisits");
        try
        {
            string requestUrl = $"patient-visits/{patientId}";
            
            HttpResponseMessage response = await ApiClient.Instance.GetAsync(requestUrl);

          
            Console.WriteLine($"Mongo response: {response.StatusCode}");
           
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API request failed with status code: {response.StatusCode}");
                return new List<PatientsVisit>(); 
            }
            
            string jsonString = await response.Content.ReadAsStringAsync();
            
            if (string.IsNullOrEmpty(jsonString))
            {
                Console.WriteLine("API returned an empty response.");
                return new List<PatientsVisit>();
            }

            PatientVisitResponse patientVisitResponse = JsonSerializer.Deserialize<PatientVisitResponse>(jsonString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, 
                    AllowTrailingCommas = true 
                });

           
            if (patientVisitResponse?.Visits == null || patientVisitResponse.Visits.Count == 0)
            {
                Console.WriteLine("No visits found in API response.");
                return new List<PatientsVisit>();
            }
            return patientVisitResponse.Visits.ConvertAll(visitData => new PatientsVisit(
                visitData.Name ?? "Не указано", 
                DateTime.TryParse(visitData.DateOfVisit, out DateTime date)
                    ? date
                    : DateTime.MinValue 
            ));
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Error while requesting API: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Error deserializing API response: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unknown error while receiving visits: {ex.Message}", ex);
        }
    }

    public async Task<List<StructureOfVisit>> GetStructureOfAllVisits(string studyName)
    {
        Console.WriteLine("GetStructureOfVisits");

        try
        {
            string requestUrl = $"study-visits/{studyName}";
            HttpResponseMessage response = await ApiClient.Instance.GetAsync(requestUrl);
            Console.WriteLine($"Mongo response: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API request failed with status code: {response.StatusCode}");
                return new List<StructureOfVisit>();
            }

            string jsonString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonString))
            {
                Console.WriteLine("API returned an empty response.");
                return new List<StructureOfVisit>();
            }

            PatientVisitResponse patientVisitResponse = JsonSerializer.Deserialize<PatientVisitResponse>(jsonString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true
                });

            if (patientVisitResponse?.Visits == null || patientVisitResponse.Visits.Count == 0)
            {
                Console.WriteLine("No visits found in API response.");
                return new List<StructureOfVisit>();
            }

            return patientVisitResponse.Visits.ConvertAll<StructureOfVisit>(visitData => new StructureOfVisit
            (
                visitData.Name,
                visitData.TimeWindow, // Время визита
                visitData.PeriodAfterRandomization,
                visitData.Tasks
            ));
        }
        catch (HttpRequestException ex)
        {
            // Ошибка при запросе к API (например, если сервер не доступен)
            throw new InvalidOperationException($"Error while requesting API: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            // Ошибка при десериализации (если структура JSON не соответствует ожидаемой)
            throw new InvalidOperationException($"Error deserializing API response: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
           
            throw new InvalidOperationException($"Unknown error while retrieving tasks: {ex.Message}", ex);
        }
    }

    public async Task<List<string>> GetVisitTasks(string studyName, string visitName)
    {
        Console.WriteLine("GetVisitTasks");

        try
        {
           
            string requestUrl =
                $"study-visits/tasks?studyTitle={Uri.EscapeDataString(studyName)}&visitName={Uri.EscapeDataString(visitName)}";
            HttpResponseMessage response = await ApiClient.Instance.GetAsync(requestUrl);
            Console.WriteLine($"API response: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API request failed with status code: {response.StatusCode}");
                return new List<string>(); 
            }

            string jsonString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonString))
            {
                Console.WriteLine("API returned an empty response.");
                return new List<string>();
            }

            List<string> tasks = JsonSerializer.Deserialize<List<string>>(jsonString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true
                });

            if (tasks == null || tasks.Count == 0)
            {
                Console.WriteLine("No tasks found in API response.");
                return new List<string>();
            }

            return tasks;
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Error while requesting API:: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Error deserializing API response: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unknown error while retrieving tasks:: {ex.Message}", ex);
        }
    }

    public async Task<bool> PostNewStudyStructure(Study study)
    {
        Console.WriteLine("Started Create Study Mongo");
        try
        {
          
            var visits = new List<BsonVisit>();

            foreach (var visit in study.VisitsStructure)
            {
                var bsonVisit = new BsonVisit
                {
                    Name = visit.Name,
                    PeriodAfterRandomization = visit.PeriodAfterRandomization,
                    Tasks = visit.Tasks,
                    TimeWindow = visit.TimeWindow
                };
            
                visits.Add(bsonVisit);
            }

            
            var studyPayload = new BsonStudy
            {
                Title = study.StudyName,
                Fullname = study.FullName,
                Visits = visits
            };

           
            string json = JsonSerializer.Serialize(studyPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

          
            string requestUrl = "study-visits";
            HttpResponseMessage response = await ApiClient.Instance.PostAsync(requestUrl, content);

            Console.WriteLine($"Response: {response.StatusCode}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in CreateStudyStructure: {ex.Message}");
            return false;
        }
    }
}

public class PatientVisitResponse
{
    public string Id { get; set; }
    public string PatientId { get; set; }
    public List<BsonVisit> Visits { get; set; } = new();
}

public class BsonVisit
{
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string Name { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("date")]
    public string DateOfVisit { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("windowDays")]
    public int TimeWindow { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("intervalFromRandomization")]
    public int PeriodAfterRandomization { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("tasks")]
    public List<string> Tasks { get; set; }
}

public class BsonStudy
{
    [System.Text.Json.Serialization.JsonPropertyName("title")]
    public string Title { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("fullname")]
    public string Fullname { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("visits")]
    public List<BsonVisit> Visits { get; set; }
}