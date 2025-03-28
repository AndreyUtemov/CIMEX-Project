using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace CIMEX_Project;

public class ButtonFactory
{
    public List<Button> CreatePatientButtons(List<Patient> patientList)
    {
        List<Button> buttonList = new List<Button>();
        foreach (Patient patient in patientList)
        {
            string visitDate = CreateDateString(patient.NextVisit.DateOfVisit, patient.NextVisit.TimeWindow);
            var button = new Button()
            {
                Content = $"{patient.Surname}\n{patient.Name}\n\n{visitDate}",
                Style = (Style)Application.Current.Resources["Big_Button"],
                Tag = patient
            };
            if (DateTime.Now > patient.NextVisit.DateOfVisit)
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

    public static List<Button> CreateVisitButtons(List<Visit> visits, DateTime actualVisit)
    {
        List<Button> buttonList = new List<Button>();
        foreach (Visit visit in visits)
        {
            var button = new Button
            {
                Content = $"{visit.Name}\n{visit.DateOfVisit}",
                Style = (Style)Application.Current.Resources["Small_Button"],
                Tag = visit,
                Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(
                visit.DateOfVisit < actualVisit ? "#0E239A" :
                visit.DateOfVisit == actualVisit ? "#0085D4" : "#01B0FF")
                )
            };
            buttonList.Add(button);
        }
        return buttonList;
    }

    public static List<Button> CreateManipulationList(Dictionary<string, bool> manipulations)
    {
        List<Button> buttonlist = new List<Button>();
        foreach (var manipulation in manipulations)
        {
            var button = new Button
            {
                Content = manipulation.Key,
                Style = (Style)Application.Current.Resources["Small_Button"],
                Tag = manipulation.Key,
                Background = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString(
                        manipulation.Value == true ? "#0E239A" : "#01B0FF")
                )
            };
            buttonlist.Add(button);
        }

        return buttonlist;
    }

    private string CreateDateString(DateTime date, int window)
    {
        if (window == 0)
        {
            return "Visit date\n" + date.ToString("dd.MM.yyyy");
        }
        else
        {
            return "Visit date\n" + date.AddDays(-1 * window).ToString("dd.MM.yyyy") + "\n" +
                   date.AddDays(window).ToString("dd.MM.yyyy");
        }
    }
}