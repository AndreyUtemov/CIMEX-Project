using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

namespace CIMEX_Project;

public class DAOPatientNeo4j : DAOPatient
{
    private readonly Neo4jClient _neo4jClient;
    private DAOPatient _daoPatientImplementation;

    public DAOPatientNeo4j(Neo4jClient neo4jClient) // Внедряем зависимость
    {
        _neo4jClient = neo4jClient;
    }

    public DAOPatientNeo4j()
    {
    }

    public async Task<List<Patient>> GetAllPatients(string eMail)
    {
        Console.WriteLine("Get all patients started");

        try
        {
            string requestUrl = $"patients/team/{eMail}";


            HttpResponseMessage responseMessage = await ApiClient.Instance.GetAsync(requestUrl);

            Console.WriteLine(responseMessage.ToString());

            Console.WriteLine($"Request answer {responseMessage.StatusCode}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                Console.WriteLine("Pechalka");
            }

            string jsonString = await responseMessage.Content.ReadAsStringAsync();

            Console.WriteLine($"Получен JSON: {jsonString}");

            List<Patient>? patients = JsonSerializer.Deserialize<List<Patient>?>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });


            foreach (var patient in patients)
            {
                Console.WriteLine(
                    $"Name: {patient.StudyName} Surname:  {patient.Surname}  Role: {patient.PatientStudyId}" +
                    $"Study {patient.StudyName} Visit{patient.NextPatientsVisit.Name}  {patient.NextPatientsVisit.DateOfVisit.ToString("dd.MM.yyyy")}");
            }

            return patients;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<int> CreatePatient(Patient patient)
    {
        await _neo4jClient.Connect();
        await using var session = _neo4jClient.GetDriver().AsyncSession(); // Используем AsyncSession()

        try
        {
            // CREATE запрос для добавления пациента, исследования и посещения
            var query = @"
        CREATE (p:Patient {name: $name, surname: $surname, id: $patientCid})
        CREATE (s:Study {name: $studyName})
        CREATE (v:Visit {visitDate: $visitDate, visitName: $nextVisit})
        CREATE (p)-[:ENROLLED_IN {patientStatus: 'screening'}]->(s)
        CREATE (p)-[:SCHEDULED]->(v)
        RETURN COUNT(p) AS addedCount";

            // Выполнение запроса с параметрами
            var result = await session.RunAsync(query, new
            {
                name = patient.Name,
                surname = patient.Surname,
                patientCid = patient.PatientHospitalId,
                studyName = patient.StudyName,
                visitDate = patient.NextPatientsVisit.DateOfVisit,
                nextVisit = patient.NextPatientsVisit.Name
            });

            // Извлечение результата из запроса
            var addedCount = await result.SingleAsync(record => record["addedCount"].As<int>());

            return addedCount; // Возвращаем количество добавленных пациентов
        }
        catch (Exception ex)
        {
            // Обработка ошибок
            Console.WriteLine($"Error while creating patient: {ex.Message}");
            return 0; // Возвращаем 0 в случае ошибки
        }
    }

    public Patient SetPatient(Patient patient)
    {
        throw new NotImplementedException();
    }

    public int DeletePatient(Patient patient)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateNewVisit(Patient patient, PatientsVisit patientsVisit)
    {
        Console.WriteLine("Started CreateNewVisit");
        try
        {
            var payload = new
            {
                Patient = patient.PatientHospitalId,
                Investigator = patientsVisit.AssignedInvestigator.Email,
                VisitName = patientsVisit.Name,
                VisitDate = patientsVisit.DateOfVisit.ToString("yyyy-MM-dd")
            };

            string json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string requestUrl = "patient-visit";
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
}