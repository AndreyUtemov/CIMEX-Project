namespace CIMEX_Project;

public class Study
{
    public string StudyName { get; private set; }
    public string FullName { get; private set; }
    public string RoleOfUser { get; private set; }
    public bool NeedAttention { get; private set; }

    

    public Study(string studyName, string fullName, string roleOfUser, bool needAttention)
    {
        StudyName = studyName;
        FullName = fullName;
        RoleOfUser = roleOfUser;
        NeedAttention = needAttention;
    }
}