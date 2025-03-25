namespace CIMEX_Project;

public class Visit
{
    public string Name { get; set; }
    public DateTime DateOfVisit { get; set; }
    public int TimeWindow { get;  set; }
    public bool Completed { get; set; }
    public List<string> Manipulation { get;  set; }
    public bool DespenceMedication { get;  set; }

    public Visit(string name, DateTime dateOfVisit, int timeWindow, bool completed)
    {
        Name = name;
        DateOfVisit = dateOfVisit;
        TimeWindow = timeWindow;
        Completed = completed;
    }

    public Visit()
    {
        throw new NotImplementedException();
    }
}