using System.DirectoryServices.ActiveDirectory;
using System.Windows.Controls;
using System.Windows.Documents;
using MaterialDesignThemes.Wpf;
using MongoDB.Driver.Linq;

namespace CIMEX_Project;

public class AllProgrammManagement
{
    private static ButtonFactory _buttonFactory = new ButtonFactory();
    private static TeamMember _user;
    private static Study actualStudy;
    private static bool isMainWindow = true;
    private List<Patient> _patients;
    private List<Study> _studies;
    private static readonly Neo4jClient _neo4jClient = Neo4jClient.Instance;

    public async Task SetUser(string eMail)
    {
        try
        {
            DAOTeamMemeberNeo4j _daoTeamMemeberNeo4J = new DAOTeamMemeberNeo4j();
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
        _studies = await _user.GetAllStudy(_user);
        _patients = await _user.GetAllPatients(_user);
        var separatedPatientLists = SeparatePatients(_patients);
        List<Button> studyButtons = CreateStudyButtons(_studies);
        List<Button> screenedPatientsButton = CreatePatientsButtons(separatedPatientLists.Screened);
        List<Button> includedPetientsButton = CreatePatientsButtons(separatedPatientLists.Included);
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

    public (List<Button> Screened, List<Button> Included) SetStudyWindow(Study study)
    {
        isMainWindow = false;
        actualStudy = study;
        List<Patient> studyPatients = _user.GetAllPatientsInStudy(study.StudyName, _patients);
        string userStatusInStudy = study.RoleOfUser;
        _user = CreateUserForStudy(userStatusInStudy);
        var separatedPatientLists = SeparatePatients(studyPatients);
        List<Button> screenedPatientsButton = CreatePatientsButtons(separatedPatientLists.Screened);
        List<Button> includedPetientsButton = CreatePatientsButtons(separatedPatientLists.Included);
        return (screenedPatientsButton, includedPetientsButton);
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

    private TeamMember CreateUserForStudy(string userRoleInStudy)
    {
        UserFactory userFactory = new UserFactory();
        TeamMember user;
        if (userRoleInStudy.Equals("Principal"))
        {
            user = userFactory.CreateUserForStudy(userRoleInStudy, _user);
        }
        else if (userRoleInStudy.Equals("Investigator"))
        {
            user = userFactory.CreateUserForStudy("Investigator", _user);
        }
        else
        {
            user = userFactory.CreateUserForStudy("Nurse", _user);
        }

        return user;
    }

    public TeamMember GetUserRoleForPatient(Patient patient)
    {
        Study study = _studies.Single(std => std.StudyName == patient.StudyName);
        CreateUserForStudy(study.RoleOfUser);
        return _user;
    }

    public TeamMember GetUser()
    {
        return _user;
    }

    public Study GetStudy()
    {
        return actualStudy;
    }
}