using System.Collections.ObjectModel;
using System.Windows;

namespace CIMEX_Project;

public partial class StudyCreation : Window
{
    private PrincipalInvestigator _principalInvestigator;
    private List<StructureOfVisit> _visits;
    public ObservableCollection<string> VisitNames { get; set; }
    public StudyCreation()
    {
        InitializeComponent();
        _visits = new List<StructureOfVisit>(); // Инициализация
        VisitNames = new ObservableCollection<string>();
        VisitList.ItemsSource = VisitNames;
        DataContext = this;
    }

    private async void CreateStudy(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleBox.Text) || string.IsNullOrWhiteSpace(FullNameBox.Text) ||
            _visits == null || _visits.Count == 0
            || _principalInvestigator == null)
        {
            MessageBox.Show("Enter all data", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else
        {
            Study study = new Study
            {
                StudyName = TitleBox.Text,
                FullName = FullNameBox.Text,
                PrincipalInvestigator = _principalInvestigator,
                VisitsStructure = _visits
            };
            DAOStudyNeo4j daoStudyNeo4J = new DAOStudyNeo4j();
            Console.WriteLine("Try to create sudy");
            bool studyCreated = await daoStudyNeo4J.CreateStudy(study);
            if (studyCreated)
            {
                MessageBox.Show($"Study {study.StudyName} created");
            }
_principalInvestigator.SendAppointment(_principalInvestigator, study);
            this.DialogResult = true;
            this.Close();
        }
    }

    private void CreatePrincipalInvestigator(object sender, RoutedEventArgs e)
    {
        var form = new TeamMemberInputForm(true);
        if (form.ShowDialog() == true)
        {
           _principalInvestigator = new PrincipalInvestigator(form.Result.Name,
                form.Result.Surname, form.Result.Email, "Medical Doctor");
            
            PrincipalInvestigatorData.Text = $"Principal Investigator:\n" +
                                             $"Dr.{_principalInvestigator.Surname} {_principalInvestigator.Name} " +
                                             $"e-Mail {_principalInvestigator.Email}";
        }
        
    }

    private void AddVisit(object sender, RoutedEventArgs e)
    {
        var form = new VisitAddition(VisitNames.Count());
        if (form.ShowDialog() == true)
        {
            _visits.Add(form.Result);
            VisitNames.Add(form.Result.Name);
        }
    }
}