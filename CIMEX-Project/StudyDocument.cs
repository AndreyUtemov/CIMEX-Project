namespace CIMEX_Project;

public interface StudyDocument
{
    void ShowDocument();
    bool CanBeChanged();
    bool CanBeSigned();
    bool WasProved();
}