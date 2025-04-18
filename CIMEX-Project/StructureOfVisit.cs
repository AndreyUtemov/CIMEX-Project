namespace CIMEX_Project;

public class StructureOfVisit
{
    public string Name { get; set; }
    public int TimeWindow { get; set; }
    public int PeriodAfterRandomization { get; set; }
    public List<string> Tasks { get; set; }

    public StructureOfVisit()
    {
    }

    public StructureOfVisit(string name, int timeWindow, int periodAfterRandomization, List<string> tasks)
    {
        Name = name;
        TimeWindow = timeWindow;
        PeriodAfterRandomization = periodAfterRandomization;
        Tasks = tasks;
    }
}