namespace CIMEX_Project;

public class Visit
{
    public string Name { get; set; }
    public DateTime DateOfVisit { get; set; }
    public int TimeWindow { get;  set; }
    public string Status { get; set; }
    public List<string> Tasks { get;  set; }
    // public bool DespenceMedication { get;  set; }
    public Visit(string name, DateTime dateOfVisit)
    {
        Name = name;
        DateOfVisit = dateOfVisit;
    }

    public Visit(string name, DateTime dateOfVisit, int timeWindow, string status)
    {
        Name = name;
        DateOfVisit = dateOfVisit;
        TimeWindow = timeWindow;
        Status = status;
    }
}