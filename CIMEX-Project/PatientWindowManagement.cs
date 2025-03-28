namespace CIMEX_Project;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

public class PatientWindowManagement
{
    private readonly DaoVisitMongoDb _daoVisitMongoDb = new DaoVisitMongoDb();
    private static TeamMember user;
    private static Patient patient;
    private List<Button> buttonList;

    public void SetPatientWindow(Patient patientToSet, TeamMember userInThisStudy)
    {
        user = userInThisStudy;
        patient = patientToSet;
    }

    public string GetPatientName()
    {
        return $"{patient.Surname} {patient.Name} {patient.PatientId}";
    }

    public string GetStudyName()
    {
        return patient.StudyName;
    }

    public List<Button> GetVisitButtons()
    {
        return buttonList ?? new List<Button>();
    }

    public async Task CreateVisitButtons()
    {
        List<Visit> visitList = await _daoVisitMongoDb.GetPatientVisits(patient.PatientId);
        buttonList = ButtonFactory.CreateVisitButtons(visitList, patient.NextVisit?.DateOfVisit ?? DateTime.MinValue);
    }
}