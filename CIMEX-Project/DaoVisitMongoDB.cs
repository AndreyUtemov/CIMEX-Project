namespace CIMEX_Project;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class DaoVisitMongoDb
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://localhost:7031/api/Study";

    public DaoVisitMongoDb()
    {
        try
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl),
                Timeout = TimeSpan.FromSeconds(30) // Добавляем таймаут для запросов
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Не удалось инициализировать HTTP-клиент.", ex);
        }
    }

    public async Task<List<Visit>> GetPatientVisits(string patientId)
    {
        if (string.IsNullOrEmpty(patientId))
            throw new ArgumentNullException(nameof(patientId), "Идентификатор пациента не может быть пустым.");

        try
        {
            string requestUrl = $"{BaseUrl}/patient-visits/{patientId}";
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                // Если API вернул ошибку, возвращаем пустой список
                return new List<Visit>();
            }

            string jsonString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonString))
            {
                return new List<Visit>(); // Пустой ответ от API
            }

            PatientVisitResponse patientVisitResponse = JsonSerializer.Deserialize<PatientVisitResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true // Дополнительная гибкость для JSON
            });

            if (patientVisitResponse?.Visits == null)
            {
                return new List<Visit>(); // Нет данных о визитах
            }

            return patientVisitResponse.Visits.ConvertAll(visitData => new Visit
            {
                Name = visitData.Name ?? "Не указано",
                DateOfVisit = DateTime.TryParse(visitData.DateOfVisit, out DateTime date) ? date : DateTime.MinValue,
                Status = visitData.Status ?? "Неизвестно"
            });
        }
        catch (HttpRequestException ex)
        {
            // Ошибка сети (например, API не запущен)
            throw new InvalidOperationException($"Ошибка при запросе к API: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            // Ошибка десериализации (JSON не соответствует структуре)
            throw new InvalidOperationException($"Ошибка десериализации ответа API: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            // Прочие ошибки
            throw new InvalidOperationException($"Неизвестная ошибка при получении визитов: {ex.Message}", ex);
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

    [System.Text.Json.Serialization.JsonPropertyName("status")]
    public string Status { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("tasks")]
    public List<BsonTask> Tasks { get; set; } = new();
}

public class BsonTask
{
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string Name { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("status")]
    public string Status { get; set; }
}

// Предполагаемый класс Visit (определен где-то в проекте)
// public class Visit
// {
//     public string Name { get; set; }
//     public DateTime DateOfVisit { get; set; }
//     public string Status { get; set; }
// }