using System.DirectoryServices.ActiveDirectory;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CIMEX_Project;

public class MainWindowManagement
{
    private static ButtonFactory _buttonFactory = new ButtonFactory();
    private static TeamMember user;
    private static Study actualStudy;
    private static bool isMainWindow = true;
    private static readonly Neo4jClient _neo4jClient = Neo4jClient.Instance; // Используем singleton
    private static DAOPatientNeo4j _daoPatientNeo4J = new DAOPatientNeo4j(_neo4jClient);
    private static DAOStudyNeo4j _daoStudyNeo4J = new DAOStudyNeo4j(_neo4jClient);
    private static DAOTeamMemeberNeo4j _daoTeamMemeberNeo4J = new DAOTeamMemeberNeo4j();
    private static List<Button> studyButtons = new List<Button>();
    private static List<Button> screenedButtons = new List<Button>();
    private static List<Button> includedButtons = new List<Button>();


    public async Task<bool> CheckUser(string login, string password)
    {
        try
        {
            bool accessApproved = await _daoTeamMemeberNeo4J.CheckUserPassword(login, password);
            return accessApproved;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task ProgrammStart(string eMail)
    {
        user = await _daoTeamMemeberNeo4J.GetTeamMemberByLogin(eMail);
        List<Study> studyList = await _daoStudyNeo4J.GetAllStudy(user);
        List<Patient> allPatientsList = await _daoPatientNeo4J.GetAllPatients(user);
        var separatedPatientLists = SeparatePatients(allPatientsList);
        studyButtons = await CreateStudyButtons(studyList);
    }


    public async Task<List<Button>> CreateStudyButtons(List<Study> studyList)
    {
        List<Button> buttonList = _buttonFactory.CreateStudyButtons(studyList);
        return buttonList;
    }

    public List<Button> CreatePatientsButtons(List<Patient> patients)
    {
        List<Button> buttonList = _buttonFactory.CreatePatientButtons(patients);

        return buttonList;
    }


    public string GetLeftTitle()
    {
        return user.Name + " " + user.Surname;
    }

    public string GetRightTitle()
    {
        if (isMainWindow)
        {
            return "";
        }
        else
        {
            return actualStudy.StudyName + "\n" + actualStudy.FullName;
        }
    }

    public (List<Button> Studies, List<Button> Screened, List<Button> Included) GetMainButtons()
    {
        return (Studies: studyButtons, Screened: screenedButtons, Included: includedButtons);
    }

    public async Task SetStudyWindow(Study study)
    {
        isMainWindow = false;
        actualStudy = study;
        bool isUserPI = await _daoTeamMemeberNeo4J.IsUserPI(study, user);
        CreateUserForStudy(isUserPI);
        List<Patient> studyPatients = await _daoPatientNeo4J.GetAllPatientsInStudy(study);
        SeparatePatients(studyPatients);
    }

    private (List<Patient> Included, List<Patient> Screened) SeparatePatients(List<Patient> allPatientsList)
    {
        List<Patient> includedPatients = new List<Patient>();
        List<Patient> screenedPatients = new List<Patient>();
        foreach (Patient patient in allPatientsList)
        {
            if (patient.Status == "screening")
            {
                includedPatients.Add(patient);
            }
            else 
            {
                screenedPatients.Add(patient);
            }
        }

        screenedButtons = CreatePatientsButtons(screenedPatients);
        includedButtons = CreatePatientsButtons(includedPatients);

        return (Included: includedPatients, Screened: screenedPatients);
    }

    private void CreateUserForStudy(bool isUserPI)
    {
        UserFactory userFactory = new UserFactory();
        if (isUserPI)
        {
            user = userFactory.CreateUserForStudy("Principal", user);
        }
        else if (user.Role == "Doctor")
        {
            user = userFactory.CreateUserForStudy("Investigator", user);
        }
        else
        {
            user = userFactory.CreateUserForStudy("Nurse", user);
        }
    }

    public async Task<TeamMember> GetUser(Patient patient)
    {
        bool userRole = await _daoStudyNeo4J.GetUserRoleInStudy(user);
        CreateUserForStudy(userRole);
        return user;
    }
    
}