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
             "WHERE t.email = $teamMemberEmail AND r.patientStatus <> 'ended'  " +
             "RETURN p.name AS name, p.surname AS surname, p.id AS patientId, r.plannedVisitDate AS nextVisit, r.visitName AS visit, s.name AS studyName, r.patientStatus AS status",
                new { teamMemberEmail = user.Email });

            await result.ForEachAsync(record =>
            {
                var name = record["name"].As<string>();
                var surname = record["surname"].As<string>();
                var patientId = record["patientId"].As<string>();
                var nextVisit = record["nextVisit"].As<DateTime>();
                var status = record["status"].As<string>();
                var visitName = record["visit"].As<string>();
                var studyName = record["studyName"].As<string>();

                var visit = new Visit(visitName, nextVisit, "scheduled");

                var patient = new Patient(
                    name, surname, patientId, status, studyName, visit
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
            ("MATCH (p:Patient)-[r:ENROLLED_IN]->(s:Study) WHERE s.name = $studyName AND r.patientStatus  <> 'ended' RETURN p.name AS name, " +
             "p.surname AS surname, p.id AS patientId, r.plannedVisitDate AS nextVisit, r.visitName AS visit, r.patientStatus AS status",
                new { studyName = study.StudyName });

            await result.ForEachAsync(record =>
            {
                var name = record["name"].As<string>();
                var surname = record["surname"].As<string>();
                var patientId = record["patientId"].As<string>();
                var nextVisit = record["nextVisit"].As<DateTime>();
                var status = record["status"].As<string>();
               
                var visitName = record["visit"].As<string>();

                var visit = new Visit(visitName, nextVisit, "scheduled");

                var patient = new Patient(
                    name, surname, patientId, status, study.StudyName, visit
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