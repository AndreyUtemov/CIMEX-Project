using System.DirectoryServices.ActiveDirectory;
using System.Windows.Controls;
using System.Windows.Documents;
using MaterialDesignThemes.Wpf;

namespace CIMEX_Project;

public class AllProgrammManagement
{
    private static ButtonFactory _buttonFactory = new ButtonFactory();
    private static TeamMember _user;
    private static Study actualStudy;
    private static bool isMainWindow = true;
    private static readonly Neo4jClient _neo4jClient = Neo4jClient.Instance; // Используем singl.eton
    private static DAOPatientNeo4j _daoPatientNeo4J = new DAOPatientNeo4j(_neo4jClient);
    private static DAOStudyNeo4j _daoStudyNeo4J = new DAOStudyNeo4j(_neo4jClient);
    private static DAOTeamMemeberNeo4j _daoTeamMemeberNeo4J = new DAOTeamMemeberNeo4j();



    public async Task SetUser(string eMail)
    {
        try
        {
           _user = await _daoTeamMemeberNeo4J.GetTeamMemberByLogin(eMail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }

    public async Task<(List<Button> Studies, List<Button> Screened, List<Button> Included)> ProgrammStart()
    {
        
        List<Study> studyList = _user.GetAllStudy(_user);
        List<Patient> allPatientsList = _user.GetAllPatients(_user);
        var separatedPatientLists = SeparatePatients(allPatientsList);
        List<Button> studyButtons =  CreateStudyButtons(studyList);
        List<Button> screenedPatientsButton = CreatePatientsButtons(separatedPatientLists.Screened);
        List<Button> includedPetientsButton = CreatePatientsButtons(separatedPatientLists.Included);
        _user.Studies = studyList;
        
        return (studyButtons, screenedPatientsButton, includedPetientsButton);
    }


    private List<Button> CreateStudyButtons(List<Study> studyList)
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
        return _user.Name + " " + _user.Surname;
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
    public  SetStudyWindow(Study study)
    {
        isMainWindow = false;
        actualStudy = study;
        string userStatusInStudy = _user.UserStatusInStudy(study);
        CreateUserForStudy(userStatusInStudy);
        List<Patient> studyPatients = study.Patients;
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
        return (Included: includedPatients, Screened: screenedPatients);
    }

    private void CreateUserForStudy(string userRoleInStudy)
    {
        UserFactory userFactory = new UserFactory();
        if (userRoleInStudy.Equals("Principal"))
        {
            _user = userFactory.CreateUserForStudy(userRoleInStudy, _user);
        }
        else if (userRoleInStudy.Equals("Investigator"))
        {
            _user = userFactory.CreateUserForStudy("Investigator", _user);
        }
        else
        {
            _user = userFactory.CreateUserForStudy("Nurse", _user);
        }
    }
}