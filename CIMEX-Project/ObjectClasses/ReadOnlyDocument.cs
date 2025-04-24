namespace CIMEX_Project;

public class ReadOnlyDocument : StudyDocument
{
    public void ShowDocument()
    {
        throw new NotImplementedException();
    }

    public bool CanBeChanged()
    {
        return false;
    }

    public bool CanBeSigned()
    {
        return false;
    }

    public bool WasProved()
    {
        return false;
    }
}