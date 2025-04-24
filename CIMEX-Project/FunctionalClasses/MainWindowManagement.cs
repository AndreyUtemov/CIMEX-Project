using System.DirectoryServices.ActiveDirectory;
using System.Windows.Controls;
using System.Windows.Documents;
using MaterialDesignThemes.Wpf;
using MongoDB.Driver.Linq;

namespace CIMEX_Project;

public class MainWindowManagement
{
    private static ButtonFactory _buttonFactory = new ButtonFactory();
    private static TeamMember _user;
    private static Study actualStudy;
    private static bool isMainWindow = true;
    private List<Patient> _patients;
    private List<Study> _studies;
   
    public async Task SetUser(string eMail)
    {
        Console.WriteLine($"{eMail} is here");
        try
        {
            DAOTeamMemeberNeo4j _daoTeamMemeberNeo4J = new DAOTeamMemeberNeo4j();
            _user = await _daoTeamMemeberNeo4J.GetTeamMember(eMail);
            Console.WriteLine(_user.Surname);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
    }

    public async Task<(List<Button> Studies, List<Button> Screened, List<Button> Included)> ProgrammStart()
    {
        Console.WriteLine($"We are in Program start for {_user.Surname}");
        _studies = await _user.GetAllStudy(_user.Email);
        foreach (Study study in _studies)
        {
            Console.WriteLine($"check list {study.StudyName}");
        }
        List<Button> studyButtons = CreateStudyButtons(_studies);
        foreach (var but in studyButtons)
        {
            Console.WriteLine(but.Content.ToString());
        }
        var unsortedPatients = await _user.GetAllPatients(_user);
        _patients = unsortedPatients.OrderBy(p => p.NextPatientsVisit.DateOfVisit).ToList();
        var separatedPatientLists = SeparatePatients(_patients);
        
       List<Button> screenedPatientsButton = CreatePatientsButtons(separatedPatientLists.Screened);
        List<Button> includedPetientsButton = CreatePatientsButtons(separatedPatientLists.Included);
        return (studyButtons, screenedPatientsButton, includedPetientsButton);
    }

    private List<Button> CreateStudyButtons(List<Study> studyList)
    {
        Console.WriteLine("Study Buttons Creating");
        
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
        return $"{_user.Name}  {_user.Surname}\n{_user.Role}";
    }

    public string GetRightTitle()
    {
        if (isMainWindow)
        {
            return "";
        }
        else
        {
            return $"{actualStudy.StudyName}\nyour role in study:\n{actualStudy.RoleOfUser}\n";
        }
    }

    public (List<Button> Screened, List<Button> Included) SetStudyWindow(Study study)
    {
        isMainWindow = false;
        actualStudy = study;
        Console.WriteLine($"Chosen study {study.StudyName} role {study.RoleOfUser} ");
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
            if (patient.NextPatientsVisit.Name == "Screening")
            {
                screenedPatients.Add(patient);
            }
            else
            {
                includedPatients.Add(patient);
            }
        }

        return (Included: includedPatients, Screened: screenedPatients);
    }

    private TeamMember CreateUserForStudy(string userRoleInStudy)
    {
        TeamMemberFactory teamMemberFactory = new TeamMemberFactory();
        TeamMember user;
        if (userRoleInStudy.Equals("Principal Investigator"))
        {
            user = teamMemberFactory.CreateUserForStudy(userRoleInStudy, _user);
        }
        else if (userRoleInStudy.Equals("Investigator"))
        {
            user = teamMemberFactory.CreateUserForStudy("Investigator", _user);
        }
        else
        {
            user = teamMemberFactory.CreateUserForStudy("Nurse", _user);
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