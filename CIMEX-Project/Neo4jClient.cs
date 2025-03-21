namespace CIMEX_Project;

using System;
using Neo4j.Driver;
using System.Threading.Tasks;

public sealed class Neo4jClient : IAsyncDisposable
{
    private static readonly Lazy<Neo4jClient> _instance = new(() => new Neo4jClient());
    private IDriver? _driver;
    
    private readonly string _uri = "neo4j+s://63368ae2.databases.neo4j.io";
    private readonly string _user = "neo4j";
    private readonly string _password = "aSLSOOhvBOnr8h8gHs6Hj0XWrOI_lZwiR8xZTCPpoWE";
    
    private Neo4jClient() { }

    public static Neo4jClient Instance => _instance.Value;

    public async Task Connect()
    {
        if (_driver != null)
        {
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            Console.WriteLine("Connected with Neo4j");
            return;
        }

        _driver = GraphDatabase.Driver(_uri, AuthTokens.Basic(_user, _password));
        try
        {
            await _driver.VerifyConnectivityAsync();
            Console.WriteLine("Connection Neo4j successful");

        }
        catch (Exception e)
        {
           Console.WriteLine($"Connection error {e.Message}");
        } 
    }

    public async Task Disconnect()
    {

        if (_driver != null)
        {
            await _driver.DisposeAsync();
            _driver = null;
            Console.WriteLine("Neo4j Connection closed");
        }
        else
        {
            Console.WriteLine("No opened Neo4j connection");
        }
    }

    public async ValueTask DisposeAsync()
    {
        await Disconnect();
    }

    public IDriver GetDriver() => _driver ?? throw new InvalidOperationException("Connection failed");

}