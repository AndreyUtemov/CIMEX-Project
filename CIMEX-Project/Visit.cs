namespace CIMEX_Project;

public class Visit
{
    private string name;
    private int sheduleAfterLastVisit;
    private int timeWindow;
    private List<string> manipulation;
    private bool despenceMedication;

    public Visit(string name, int sheduleAfterLastVisit, int timeWindow, List<string> manipulation, bool despenceMedication)
    {
        this.name = name;
        this.sheduleAfterLastVisit = sheduleAfterLastVisit;
        this.timeWindow = timeWindow;
        this.manipulation = manipulation;
        this.despenceMedication = despenceMedication;
    }
}