using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

namespace CIMEX_Project
{
    namespace Controllers
    {
        [ApiController]
        [Route("api[controller]")]
        public class RestApiPatientManagementNeo4j : ControllerBase
        {
            private readonly Neo4jClient _neo4JClient;

            public RestApiPatientManagementNeo4j()
            {
                _neo4JClient = Neo4jClient.Instance;
            }

            private async Task Connect() => await _neo4JClient.Connect();

            private async Task Disconnect() => await _neo4JClient.Connect();

            [HttpGet]
            public async Task<IActionResult> GetAllPatients([FromBody] TeamMember teamMember)
            {
                await Connect();
                var session = _neo4JClient.GetDriver().AsyncSession();
                try
                {
                    var result = await session.RunAsync
                    ("MATCH (p:Patient)-[r:PARTICIPATES_IN]->(s:Study)-[c2]-(t:TeamMember)" +
                     "WHERE t.email = \"$teamMemberEmail\" RETURN p, r.included AS isIncluded, s.name AS studyName",
                        new { teamMemberEmail = teamMember.Email });
                    var patientList = new List<Patient>();
                    await result.ForEachAsync(record =>
                    {
                        var patientNode = record["p"].As<INode>();
                        var isIncluded = record["isIncluded"].As<bool>();
                        var studyName = record["studyName"].As<string>();

                        var patient = new Patient(
                            patientNode.Properties["name"].As<string>(),
                            patientNode.Properties["surname"].As<string>(),
                            patientNode.Properties["patientId"].As<string>(),
                            isIncluded,
                            studyName,
                            patientNode.Properties["nextVisit"].As<DateTime>(),
                            patientNode.Properties["nextSheduledVisit"].As<DateTime>()
                        );
                        patientList.Add(patient);
                    });
                    return Ok(patientList);
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
                finally
                {
                    await Disconnect();
                }
            }


            public Task<IActionResult> GetAllPatients()
            {
                throw new NotImplementedException();
            }

            public async Task<IActionResult> GetAllPatientsInStudy([FromBody] Study study)
            {
                await Connect();
                var session = _neo4JClient.GetDriver().AsyncSession();
                try
                {
                    var result = await session.RunAsync
                    ("MATCH (p:Patient)-[r:PARTICIPATES_IN]->(s:Study) WHERE s.name = \"$studyName\" RETURN p, r.included AS isIncluded",
                        new { studyName = study.StudyName });
                    var patientList = new List<Patient>();
                    await result.ForEachAsync(record =>
                    {
                        var patientNode = record["p"].As<INode>();
                        var isIncluded = record["isIncluded"].As<bool>();
                        var patient = new Patient(
                            patientNode.Properties["name"].As<string>(),
                            patientNode.Properties["surname"].As<string>(),
                            patientNode.Properties["patientID"].As<string>(),
                            isIncluded,
                            study.StudyName,
                            patientNode.Properties["nextVisit"].As<DateTime>(),
                            patientNode.Properties["nextSheduledVisit"].As<DateTime>()
                        );
                        patientList.Add(patient);
                    });
                    return Ok(patientList);
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
                finally
                {
                    await Disconnect();
                }
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
    }
}