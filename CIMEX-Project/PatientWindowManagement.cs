using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace CIMEX_Project;

public class PatientWindowManagement
{

    private DaoVisitMongoDB _daoVisitMongoDb = new DaoVisitMongoDB();
    private static TeamMember user;
    private static Patient patient;
    private List<Button> buttonList = new List<Button>();

    public void SetPatientWindow(TeamMember userInThisStudy, Patient patientToSet)
    {
        user = userInThisStudy;
        patient = patientToSet;
        CreateVisitButtons();
    }

    public string GetPatientName()
    {
        return patient.PatientId + "/" + patient.Surname + " " + patient.NextVisit;
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
        buttonList = ButtonFactory.CreateVisitButtons(visitList, patient.NextVisit);
    }
        

}