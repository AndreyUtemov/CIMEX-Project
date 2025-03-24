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

    public async Task<List<Patient>> GetAllPatients(TeamMember user)
    {
        await _neo4jClient.Connect();
        await using var session = _neo4jClient.GetDriver().AsyncSession(); // Используем AsyncSession()
        var patients = new List<Patient>();
        try
        {
            var result = await session.RunAsync
            ("MATCH (p:Patient)-[r:PARTICIPATES]->(s:Study)-[c2]-(t)" +
             "WHERE t.email = $teamMemberEmail RETURN p.name AS name, p.surname AS surname, p.patientId AS patientId, p.nextVisit AS nextVisit," +
             " p.window AS window, r.included AS isIncluded, s.name AS studyName",
                new { teamMemberEmail = user.Email });
           
            await result.ForEachAsync(record =>
            {
                var name = record["name"].As<string>();
                var surname = record["surname"].As<string>();
                var patientId = record["patientId"].As<string>();
                var nextVisit = record["nextVisit"].As<DateTime>();
                var isIncluded = record["isIncluded"].As<bool>();
                var studyName = record["studyName"].As<string>();
                var window = record["window"].As<int>();

                var patient = new Patient(
                    name, surname, patientId, isIncluded, studyName, nextVisit,  window
                );
                patients.Add(patient);
            });
            
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error fetching studies: {e.Message}");
        }
       

        return patients;
    }


    public async Task<List<Patient>> GetAllPatientsInStudy(Study study)
    {
        // MATCH (p:Patient)-[r]-(s:Study) WHERE s.name = "ACTIVE" RETURN p
        await _neo4jClient.Connect();
        await using var session = _neo4jClient.GetDriver().AsyncSession(); // Используем AsyncSession()
        var patients = new List<Patient>();
        try
        {
            var result = await session.RunAsync
            ("MATCH (p:Patient)-[r:PARTICIPATES]->(s:Study)" +
             "WHERE s.name = $studyName RETURN p.name AS name, p.surname AS surname, p.patientId AS patientId, p.nextVisit AS nextVisit," +
             " p.window AS window, r.included AS isIncluded",
                new { studyName = study.StudyName });
           
            await result.ForEachAsync(record =>
            {
                var name = record["name"].As<string>();
                var surname = record["surname"].As<string>();
                var patientId = record["patientId"].As<string>();
                var nextVisit = record["nextVisit"].As<DateTime>();
                var isIncluded = record["isIncluded"].As<bool>();
                var window = record["window"].As<int>();

                var patient = new Patient(
                    name, surname, patientId, isIncluded, study.StudyName, nextVisit,  window
                );
                patients.Add(patient);
            });
            
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error fetching studies: {e.Message}");
        }

        return patients;
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