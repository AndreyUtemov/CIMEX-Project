namespace CIMEX_Project;

public abstract class  Person
{
   public string Name { get; set;}
   public string Surname { get; set;}

   protected Person(string name, string surname)
   {
      Name = name;
      Surname = surname;
  
   }
   
}