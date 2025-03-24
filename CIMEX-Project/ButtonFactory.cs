using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CIMEX_Project;

public class ButtonFactory
{
    public List<Button> CreatePatientButtons(List<Patient> patientList)
    {
        List<Button> buttonList = new List<Button>();
        foreach (Patient patient in patientList)
        {
            string visitDate = CreateDateString(patient.NextVisit, patient.TimeWindow);
            var button = new Button()
            {
                Content = $"{patient.Surname}\n{patient.Name}\n\n{visitDate}",
                Style = (Style)Application.Current.Resources["Big_Button"],
                Tag = patient
            };
            if (DateTime.Now > patient.NextVisit)
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
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0E239A")),
                Tag = study
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
            return "Visit date\n" + date.ToString("dd.MM.yyyy");
        }
        else
        {
            return "Visit date\n" + date.AddDays(window).ToString("dd.MM.yyyy") + "\n" +
                   date.AddDays(-1 * window).ToString("dd.MM.yyyy");
        }
    }
}