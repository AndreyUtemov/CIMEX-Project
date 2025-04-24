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

    public DAOPatientNeo4j(Neo4jClient neo4jClient) 
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

           
            List<Patient>? patients = JsonSerializer.Deserialize<List<Patient>?>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            return patients;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> CreatePatient(Patient patient, Investigator investigator)
    {
        Console.WriteLine("Started CreateNewVisit");
        try
        {
            var payload = new
            {
                Name = patient.Name,
                Surname = patient.Surname,
                PatientHospitalId = patient.PatientHospitalId,
                PatientStudyId = "0",
                StudyName  = patient.StudyName,
                Status = "Screened",
                NextPatientsVisit = new
                {
                    Name = patient.NextPatientsVisit.Name,
                    DateOfVisit = patient.NextPatientsVisit.DateOfVisit.ToString("yyyy-MM-dd"),
                    InvestigatorsEmail = investigator.Email}
            };

            string json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string requestUrl = "patient-creator";
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
            HttpResponseMessage response = await ApiClient.Instance.PatchAsync(requestUrl, content);

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