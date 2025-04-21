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
            // Формируем URL для запроса
            string requestUrl = $"patient-visits/{patientId}";

            // Выполняем запрос через ApiClient
            HttpResponseMessage response = await ApiClient.Instance.GetAsync(requestUrl);

            // Логируем ответ
            Console.WriteLine($"Mongo response: {response.StatusCode}");

            // Проверка, успешен ли запрос
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API request failed with status code: {response.StatusCode}");
                return new List<PatientsVisit>(); // Возвращаем пустой список в случае ошибки
            }

            // Чтение и десериализация ответа
            string jsonString = await response.Content.ReadAsStringAsync();

            // Если ответ пустой, возвращаем пустой список
            if (string.IsNullOrEmpty(jsonString))
            {
                Console.WriteLine("API returned an empty response.");
                return new List<PatientsVisit>();
            }

            // Десериализация JSON в объект
            PatientVisitResponse patientVisitResponse = JsonSerializer.Deserialize<PatientVisitResponse>(jsonString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // Игнорирование регистра в именах свойств
                    AllowTrailingCommas = true // Допускаем запятые в конце JSON
                });

            // Проверка, есть ли визиты в ответе
            if (patientVisitResponse?.Visits == null || patientVisitResponse.Visits.Count == 0)
            {
                Console.WriteLine("No visits found in API response.");
                return new List<PatientsVisit>();
            }

            // Преобразование списка визитов в объект PatientsVisit
            return patientVisitResponse.Visits.ConvertAll(visitData => new PatientsVisit(
                visitData.Name ?? "Не указано", // Используем дефолтное значение, если имя визита пустое
                DateTime.TryParse(visitData.DateOfVisit, out DateTime date)
                    ? date
                    : DateTime.MinValue // Преобразование даты
            ));
        }
        catch (HttpRequestException ex)
        {
            // Ошибка при запросе к API (например, если сервер не доступен)
            throw new InvalidOperationException($"Ошибка при запросе к API: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            // Ошибка при десериализации (если структура JSON не соответствует ожидаемой)
            throw new InvalidOperationException($"Ошибка десериализации ответа API: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            // Обработка других ошибок
            throw new InvalidOperationException($"Неизвестная ошибка при получении визитов: {ex.Message}", ex);
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
            throw new InvalidOperationException($"Ошибка при запросе к API: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            // Ошибка при десериализации (если структура JSON не соответствует ожидаемой)
            throw new InvalidOperationException($"Ошибка десериализации ответа API: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            // Обработка других ошибок
            throw new InvalidOperationException($"Неизвестная ошибка при получении визитов: {ex.Message}", ex);
        }
    }

    public async Task<List<string>> GetVisitTasks(string studyName, string visitName)
    {
        Console.WriteLine("GetVisitTasks");

        try
        {
            // Формируем URL с query-параметрами
            string requestUrl =
                $"study-visits/tasks?studyTitle={Uri.EscapeDataString(studyName)}&visitName={Uri.EscapeDataString(visitName)}";
            HttpResponseMessage response = await ApiClient.Instance.GetAsync(requestUrl);
            Console.WriteLine($"API response: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API request failed with status code: {response.StatusCode}");
                return new List<string>(); // Возвращаем пустой список при ошибке
            }

            string jsonString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonString))
            {
                Console.WriteLine("API returned an empty response.");
                return new List<string>();
            }

            // Десериализуем ответ как List<string>
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
            throw new InvalidOperationException($"Ошибка при запросе к API: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Ошибка десериализации ответа API: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Неизвестная ошибка при получении задач: {ex.Message}", ex);
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
