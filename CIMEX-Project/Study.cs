namespace CIMEX_Project;

public class Study
{
    public string StudyName { get;  set; }
    public string FullName { get;  set; }
    public string RoleOfUser { get;  set; }
    public PrincipalInvestigator PrincipalInvestigator { get; set; }
    public bool NeedAttention { get;  set; }
    public List<PatientsVisit> VisitsSchedule { get; set; }
    public List<StructureOfVisit> VisitsStructure { get; set; }

    public Study()
    {
    }

    public Study(string studyName, string fullName, string roleOfUser, bool needAttention)
    {
        StudyName = studyName;
        FullName = fullName;
        RoleOfUser = roleOfUser;
        NeedAttention = needAttention;
    }
}