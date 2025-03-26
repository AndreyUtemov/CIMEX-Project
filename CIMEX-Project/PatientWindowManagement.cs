using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace CIMEX_Project;

public class PatientWindowManagement
{

    private DaoVisitMongoDB _daoVisitMongoDb = new DaoVisitMongoDB();
    private DAOTeamMemeberNeo4j _daoTeamMemeberNeo4J = new DAOTeamMemeberNeo4j();
    private static TeamMember user;
    private static Patient patient;
    private List<Button> buttonList = new List<Button>();

    public void SetPatientWindow(Patient patientToSet, TeamMember userInThisStudy)
    {
        user = userInThisStudy;
        patient = patientToSet;
        CreateVisitButtons();
    }

    public string GetPatientName()
    {
        return  patient.Surname + "  " + patient.Name+ "  " + patient.PatientId;
    }

    public string GetStudyName()
    {
        return patient.StudyName;
    }

    public List<Button> GetVisitButtons()
    {
        return buttonList;
    }

    public async Task CreateVisitButtons()
    {
        List<Visit> visitList = await _daoVisitMongoDb.GetAllPatienVisits(patient.PatientId);
        buttonList = ButtonFactory.CreateVisitButtons(visitList, patient.NextVisit.DateOfVisit);
    }

    public async Task SetVisitData(Visit visit)
    {
        Dictionary<string, bool> manipulations = await _daoVisitMongoDb.GetManipulationsForSpecificPatient(patient.PatientId, visit.Name);
        buttonList = ButtonFactory.CreateManipulationList(manipulations);
    }
}