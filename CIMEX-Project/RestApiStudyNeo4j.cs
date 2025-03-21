using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

namespace CIMEX_Project

{
    namespace Controllers
    {
        [ApiController]
        [Route("api[controller]")]
        public class RestApiStudyNeo4j : ControllerBase
        {
            private readonly Neo4jClient _neo4jClient;

            public RestApiStudyNeo4j()
            {
                _neo4jClient = Neo4jClient.Instance;
            }

            private async Task Connect() => await _neo4jClient.Connect();

            private async Task Disconnect() => await _neo4jClient.Connect();

            [HttpGet]
            public async Task<IActionResult> GetAllStudy()
            {
                await Connect();
                var session = _neo4jClient.GetDriver().AsyncSession();
                try
                {
                    var result =
                        await session.RunAsync("MATCH (s:Study) RETURN s.name AS studyName, s.fullname AS fullName");

                    var studyList = new List<Study>();
                    await result.ForEachAsync(record =>
                    {
                        var study = new Study
                        (
                            record["studyName"].As<string>(),
                            record["fullName"].As<string>()
                        );
                        studyList.Add(study);
                    });
                    return Ok(studyList);
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
            
            [HttpPost]

            public async Task<IActionResult> CreateStudy([FromBody] Study study)
            {
                await Connect();
                var session = _neo4jClient.GetDriver().AsyncSession();

                try
                {
                    var result = await session.RunAsync("CREATE (s:Study{name: $studyName, fullName: $fullName) RETURN s.name AS studyName",
                        new { studyName = study.StudyName, fullName = study.FullName });
                    var newStudyName = await result.SingleAsync(record => record["studyName"].As<string>());
                    return Ok(newStudyName);
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
            }

        }
    }
}