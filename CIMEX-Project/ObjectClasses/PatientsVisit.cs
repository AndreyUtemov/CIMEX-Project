using MongoDB.Driver.Linq;

namespace CIMEX_Project;

public class PatientsVisit
{
    public string Name { get; set; }
    public DateTime DateOfVisit { get; set; }
    public int TimeWindow { get; set; }
    public bool IsScheduled { get; set; }
    public List<string> Tasks { get; set; }
    public Investigator AssignedInvestigator { get; set; }


    // public bool DespenceMedication { get;  set; }

    public PatientsVisit()
    {
    }


    public PatientsVisit(string name, DateTime dateOfVisit)
    {
        Name = name;
        DateOfVisit = dateOfVisit;
    }

    public PatientsVisit(string name, DateTime dateOfVisit, int timeWindow, bool isScheduled)
    {
        Name = name;
        DateOfVisit = dateOfVisit;
        TimeWindow = timeWindow;
        IsScheduled = isScheduled;
    }

    public async Task<(PatientsVisit NewPatientVisit, DateTime StartDate, DateTime EndDate)> CreateVisitData(Patient patient,
        PatientsVisit actualVisit, List<StructureOfVisit> structureOfVisits)
    {
        DaoVisitMongoDb daoVisitMongoDb = new DaoVisitMongoDb();
        Console.WriteLine("Creating of visit  list in CreateVisitData");
        List<PatientsVisit> patientVisitList = await daoVisitMongoDb.GetPatientVisits(patient.PatientHospitalId);
        int indexOfActualVisit = structureOfVisits.FindIndex(s => s.Name.Equals(actualVisit.Name));
        var newVisitStructure = structureOfVisits[indexOfActualVisit + 1];
        var newVisit = patientVisitList[indexOfActualVisit + 1];
        if (indexOfActualVisit > 0)
        {
            return (NewPatientVisit: newVisit,  StartDate: newVisit.DateOfVisit.AddDays(-newVisitStructure.TimeWindow),
                newVisit.DateOfVisit.AddDays(newVisitStructure.TimeWindow));
        }
        else
        {
            return (NewPatientVisit: newVisit, StartDate: DateTime.Today.AddDays(1),
                EndDate: newVisit.DateOfVisit);
        }
    }
}