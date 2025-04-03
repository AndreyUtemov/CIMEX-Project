namespace CIMEX_Project;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

public class PatientWindowManagement
{
    private readonly DaoVisitMongoDb _daoVisitMongoDb = new DaoVisitMongoDb();
    private TeamMember user;
    private Patient patient;


    public async Task SetPatientWindow(Patient patientToSet, TeamMember userInThisStudy)
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

    public async Task<List<Button>> GetVisitButtons(Patient patient)
    {
        List<Visit> visitList = await _daoVisitMongoDb.GetPatientVisits(patient.PatientId);
        foreach (var visit in visitList)
        {
            Console.WriteLine($"Visit - {visit.Name}  Date - {visit.DateOfVisit} Status - {visit.Status}");
        }
        ButtonFactory buttonFactory = new ButtonFactory();
        List<Button> buttonList = await buttonFactory.CreateVisitButtons(visitList);
        foreach (var button in buttonList)
        {
            Console.WriteLine(button.Tag.ToString());
        }

        return buttonList;
    }
}
