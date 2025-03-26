using MongoDB.Bson;
using MongoDB.Driver;


namespace CIMEX_Project;

public class DaoVisitMongoDB : IDaoVisit
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _database;

    public async Task<List<Visit>> GetAllPatienVisits(string patientID)
    {
        var mongoClient = MongoDBClient.Instance;

        try
        {
            mongoClient.Connect();
        }
        catch (Exception e)
        {
            // TODO exception handel
        }

        List<Visit> visits = new List<Visit>();
        try
        {
            // Подключаемся к коллекции Visit
            var collection = _database.GetCollection<Visit>("patient_visit_data");

            // Создаем фильтр для поиска по PatientID
            var filter = Builders<Visit>.Filter.Eq("PatientID", patientID);

            // Выполняем запрос в MongoDB, используя фильтр для коллекции Visit
            var result = await collection.Find(filter).ToListAsync();

            // Преобразуем результаты в список объектов Visit
            foreach (var visitData in result)
            {
                var visit = new Visit
                {
                    Name = visitData.Name,
                    DateOfVisit = visitData.DateOfVisit.ToLocalTime(),
                    TimeWindow = visitData.TimeWindow,
                    Completed = visitData.Completed
                };

                visits.Add(visit);
            }
        }
        catch (Exception e)
        {
            // TODO exception handel
        }

        return (List<Visit>)visits;
    }

    public async Task<Dictionary<string, bool>> GetManipulationsForSpecificPatient(string patientID, string visitName)
    {
        var mongoClient = MongoDBClient.Instance;

        try
        {
            mongoClient.Connect();
        }
        catch (Exception e)
        {
            // TODO exception handel
        }


        var collection = _database.GetCollection<Visit>("patient_specific_visit_data");

        // Создаем фильтр для поиска по PatientID
        var filter = Builders<Visit>.Filter.And(
            Builders<Visit>.Filter.Eq("PatientID", patientID),
            Builders<Visit>.Filter.Eq("visit_name", visitName)
        );

        // Выполняем запрос в MongoDB, используя фильтр для коллекции Visit
        var result = await collection.Find(filter).ToListAsync();
        Dictionary<string, bool> visits = new Dictionary<string, bool>();
        // Преобразуем результаты в список объектов Visit
        foreach (var visit in result)
        {
            // Проверяем, есть ли поле "manipulations" в документе
            if (visit.ToBsonDocument().Contains("manipulations"))
            {
                // Достаём манипуляции из BSON
                var manipulationsBson = visit.ToBsonDocument()["manipulations"].AsBsonDocument;

                // Преобразуем BSON в Dictionary<string, bool>
                foreach (var element in manipulationsBson.Elements)
                {
                    visits[element.Name] = element.Value.AsBoolean;
                }
            }
        }
        return visits;
    }
}