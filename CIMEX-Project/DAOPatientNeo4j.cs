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
            ("MATCH (p:Patient)-[r:ENROLLED_IN]->(s:Study)<-[:ASSIGNED_TO]-(t:TeamMember) " +
             "MATCH (p)-[:SCHEDULED]-(v:Visit)" +
             "WHERE t.email = $teamMemberEmail AND r.patientStatus <> 'ended'  " +
             "RETURN p.name AS name, p.surname AS surname, p.cid AS patientCid, p.sid AS patientSid," +
             " v.visitDate AS visitDate, v.visitName AS nextVisit, s.name AS studyName, r.patientStatus AS status",
                new { teamMemberEmail = user.Email });

            await result.ForEachAsync(record =>
            {
                var name = record["name"].As<string>();
                var surname = record["surname"].As<string>();
                var patientCid = record["patientCid"].As<string>();
                var patientSid = record["patientSid"].As<string>();
                var nextVisit = record["nextVisit"].As<string>();
                var visitDate = record["visitDate"].As<DateTime>();
                var status = record["status"].As<string>();
                var studyName = record["studyName"].As<string>();

                var visit = new Visit(nextVisit, visitDate);

                var patient = new Patient(
                    name, surname, patientCid, patientSid, studyName, status, visit
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