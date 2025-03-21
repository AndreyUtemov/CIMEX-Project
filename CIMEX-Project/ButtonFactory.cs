using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CIMEX_Project;

public class ButtonFactory
{
    public List<Button> CreatePatientButtons(List<Patient> patientList, int visitWindow)
    {
        List<Button> buttonList = new List<Button>();

        foreach (Patient patient in patientList)
        {
            string visitdate = CreateDateString(patient.NextSheduledVisit, visitWindow);
            var button = new Button()
            {
                Content = $"{patient.PatientId}\n{patient.Name}\n\n{visitdate}",
                Style = (Style)Application.Current.Resources["Big_Button"],
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01B0FF")),
                Tag = patient.PatientId
            };
            if (DateTime.Now > patient.NextSheduledVisit)
            {
                button.Foreground = new SolidColorBrush(Colors.OrangeRed);
            }

            buttonList.Add(button);
        }

        return buttonList;
    }

    public List<Button> CreateStudyButtons(List<Study> studyList)
    {
        List<Button> buttonList = new List<Button>();

        foreach (Study study in studyList)
        {
            var button = new Button()
            {
                Content = $"{study.StudyName}\n{study.FullName}",
                Style = (Style)Application.Current.Resources["Small_Button"],
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0085D4")),
                Tag = study.StudyName
            };
            if (study.NeedAtention)
            {
                button.Foreground = new SolidColorBrush(Colors.OrangeRed);
            }
            buttonList.Add(button);
        }

        return buttonList;
    }

    private string CreateDateString(DateTime date, int window)
    {
        if (window == 0)
        {
            return "Visit date\n" + date.ToString("dd.mm.yyyy");
        }
        else
        {
            return "Visit date\n" + date.AddDays(window).ToString("dd.mm.yyyy") + "\n" +
                   date.AddDays(-1 * window).ToString("dd.mm.yyyy");
        }
    }
}