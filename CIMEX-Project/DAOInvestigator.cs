namespace CIMEX_Project;

public interface DAOInvestigator

{
    List<Investigator> GetALlInvestigators();

    Investigator GetInvestigator(string eMail);

    Investigator CreateInvestigator(Investigator investigator);

    Investigator SetInvestigator(Investigator investigator);

    int DeleteInvestigator(Investigator investigator);
}