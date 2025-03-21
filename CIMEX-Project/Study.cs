namespace CIMEX_Project;

public class Study
{
    public string StudyName { get; private set; }
    
    public string FullName { get; private set; }
    public List<Visit> VisitShedule { get; private set; }
    
    public bool NeedAtention { get; private set; }

    public Study(string studyName, string fullName, List<Visit> visitShedule, bool needAtention)
    {
        StudyName = studyName;
        FullName = fullName;
        VisitShedule = visitShedule;
        NeedAtention = needAtention;
    }

    public Study(string studyName, string fullName)
    {
        StudyName = studyName;
        FullName = fullName;
    }
}