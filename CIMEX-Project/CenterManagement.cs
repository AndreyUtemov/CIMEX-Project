using System.Windows.Controls;
using System.Windows.Documents;

namespace CIMEX_Project;

public class CenterManagement
{
    private static ButtonFactory _buttonFactory = new ButtonFactory();
    private static TeamMember user;
    private static List<Study> studyList;
    private static Study actualStudy = null;
    private static bool isMainWindow;
    private static DAOPatientNeo4j _daoPatientNeo4J= new DAOPatientNeo4j();
    private static DAOStudyNeo4j _daoStudyNeo4J = new DAOStudyNeo4j();
    private static List<Patient> includedPatients = new List<Patient>();
    private static List<Patient> screenedPatients = new List<Patient>();
    


    public async void ProgrammStart(string eMail)
    {
        // user = _daoInvestigatorNeo4J.GetInvestigator(eMail);
        isMainWindow = true;
        studyList = user.GetStudyList();
        studyList = await _daoStudyNeo4J.GetAllStudy(user);
        List<Patient> allPatientsList = await _daoPatientNeo4J.GetAllPatients(user);

        foreach (Patient patient in allPatientsList)
        {
            if (patient.Included)
            {
                includedPatients.Add(patient);
            }
            else
            {
                screenedPatients.Add(patient);
            }
        }
    }

    public List<Button> CreateUpperRowButtons()
    {
        List<Button> buttonList = _buttonFactory.CreateStudyButtons(studyList);
        return buttonList;
    }

    public List<Button> CreateMiddleRowButtons()
    {
        List<Button> buttonList = _buttonFactory.CreatePatientButtons(includedPatients, 0);

        return buttonList;
    }

    public List<Button> CreateBottomRowButtons()
    {
        List<Button> buttonList = _buttonFactory.CreatePatientButtons(screenedPatients, 2);

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
    
    
}