namespace CIMEX_Project;

public class Study
{
    public string StudyName { get; private set; }
    public string FullName { get; private set; }
    public string RoleOfUser { get; private set; }
    public bool NeedAtention { get; private set; }
    public Dictionary<string, List<Visit>> VisitShcedule { get; set;}
    public List<Patient> Patients { get; set; }

 
    

    public Study(string studyName, string fullName, string roleOfUser, bool needAtention)
    {
        StudyName = studyName;
        FullName = fullName;
        RoleOfUser = roleOfUser;
        NeedAtention = needAtention;
    }
}