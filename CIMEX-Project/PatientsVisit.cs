namespace CIMEX_Project;

public class PatientsVisit
{
    public string Name { get; set; }
    public DateTime DateOfVisit { get; set; }
    public int TimeWindow { get;  set; }
    public string Status { get; set; }
    
    
    // public bool DespenceMedication { get;  set; }
    
    public PatientsVisit(){}
      
    
    public PatientsVisit(string name, DateTime dateOfVisit)
    {
        Name = name;
        DateOfVisit = dateOfVisit;
    }

    public PatientsVisit(string name, DateTime dateOfVisit, int timeWindow, string status)
    {
        Name = name;
        DateOfVisit = dateOfVisit;
        TimeWindow = timeWindow;
        Status = status;
    }
}