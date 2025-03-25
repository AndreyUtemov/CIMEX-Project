using MongoDB.Bson;
using MongoDB.Driver;

namespace CIMEX_Project;

public class MongoDBClient
{
    private static readonly Lazy<MongoDBClient> _instance = new(() => new MongoDBClient());

    private IMongoClient? _client;
    private IMongoDatabase? _database;
    
    private readonly string _connectionString = "mongodb://localhost:27017";
    private readonly string _databaseName = "cimex";

    private MongoDBClient() { }

    public static MongoDBClient Instance => _instance.Value;

    public void Connect()
    {
        if (_client != null)
        {
            Console.WriteLine("Already connected to MongoDB");
            return;
        }

        _client = new MongoClient(_connectionString);
        _database = _client.GetDatabase(_databaseName);

        Console.WriteLine("MongoDB connection successful");
    }

    public void Disconnect()
    {
        if (_client != null)
        {
            _client = null;
            _database = null;
            Console.WriteLine("MongoDB connection closed");
        }
        else
        {
            Console.WriteLine("No opened MongoDB connection");
        }
    }

    public IMongoCollection<BsonDocument> GetCollection(string collectionName)
    {
        if (_database == null)
        {
            throw new InvalidOperationException("Connection failed");
        }
        return _database.GetCollection<BsonDocument>(collectionName);
    }
}

