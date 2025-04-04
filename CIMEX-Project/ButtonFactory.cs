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
            var button = new Button()
            {
                Content = $"{patient.Surname}\n{patient.Name}\n\n{patient.NextVisit.DateOfVisit.ToString("dd.MM.yyyy")}",
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
            if (study.NeedAttention)
            {
                button.Foreground = new SolidColorBrush(Colors.OrangeRed);
            }

            buttonList.Add(button);
        }

        return buttonList;
    }

    public async Task<List<Button>> CreateVisitButtons(List<Visit> visits)
    {
        List<Button> buttonList = new List<Button>();
        foreach (Visit visit in visits)
        {
            string contentString;
            Console.WriteLine($"{visit.Name}  {visit.DateOfVisit.ToString("dd.MM.yyyy")}");
            contentString = visit.TimeWindow == 0
                ? $"{visit.Name}\n{visit.DateOfVisit.ToString("dd.MM.yyyy")}"
                : $"{visit.Name}\n{visit.DateOfVisit.AddDays(visit.TimeWindow).ToString("dd.MM.yyyy")}";
            var button = new Button
            {
                Content =  contentString,
                Style = (Style)Application.Current.Resources["Small_Button"],
                Tag = visit,
                Background = new SolidColorBrush(
                visit.Status == "completed" ? ((Color)ColorConverter.ConvertFromString("#0E239A")) :
                    visit.Status == "scheduled" ? ((Color)ColorConverter.ConvertFromString("#0085D4")) : ((Color)ColorConverter.ConvertFromString("#01B0FF"))
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
                        manipulation.Value? "#0E239A" : "#01B0FF")
                )
            };
            buttonlist.Add(button);
        }

        return buttonlist;
    }
    
}