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
    private static DAOInvestigatorNeo4j _daoInvestigatorNeo4J= new DAOInvestigatorNeo4j();
    private static DAOPatientNeo4j _daoPatientNeo4J= new DAOPatientNeo4j();
    private static DAOStudyNeo4j _daoStudyNeo4J = new DAOStudyNeo4j();
    


    public void ProgrammStart(string eMail)
    {
        user = _daoInvestigatorNeo4J.GetInvestigator(eMail);
        isMainWindow = true;
        studyList = user.GetStudyList();
        List<Patient> allPatientsList = _daoPatientNeo4J.GetAllPatients(user);
    }

    public List<Button> CreateUpperRowButtons()
    {
        List<Button> buttonList = _buttonFactory.CreateStudyButtons(studyList);
        return buttonList;
    }

    public List<Button> CreateMiddleRowButtons()
    {
        List<Button> buttonList = _buttonFactory.CreatePatientButtons(patienteList);

        return buttonList;
    }

    public List<Button> CreateBottomRowButtons()
    {
        List<Button> buttonList = _buttonFactory.CreatePatientButtons(patienteList);

        return buttonList;
    }
    
}