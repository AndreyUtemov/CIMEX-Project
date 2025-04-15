namespace CIMEX_Project;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

public class PatientWindowManagement
{
    private readonly DaoVisitMongoDb _daoVisitMongoDb = new DaoVisitMongoDb();
   
    public async Task<List<Button>> GetVisitButtons(Patient patient)
    {
        Console.WriteLine("Creating of visit buttons list in GetVisitButtons  PatientWindowManagement");
        List<Visit> visitList = await _daoVisitMongoDb.GetPatientVisits(patient.PatientHospitalId);
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
