namespace CIMEX_Project;

public class Visit
{
    public string Name { get; set; }
    public DateTime DateOfVisit { get; set; }
    public int TimeWindow { get;  set; }
    public string Status { get; set; }
    public List<string> Manipulation { get;  set; }
    public bool DespenceMedication { get;  set; }

    public Visit(string name, DateTime dateOfVisit, string status)
    {
        Name = name;
        DateOfVisit = dateOfVisit;
        Status = status;
    }

    public Visit()
    {
        throw new NotImplementedException();
    }
}