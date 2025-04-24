using System;
using System.Diagnostics;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace CIMEX_Project
{
    public class PdfHandler
    {
        public async Task PrintVisit(PatientsVisit patientsVisit, Patient patient)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (PdfWriter writer = new PdfWriter(memoryStream))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        Document document = new Document(pdf);
                        document.Add(new Paragraph($"Visit {patient.StudyName}").SetFontSize(32));
                        document.Add(new Paragraph($"Visit {patientsVisit.Name}").SetFontSize(24));
                        document.Add(new Paragraph($"Visit Date: {patientsVisit.DateOfVisit:dd.MM.yyyy}").SetFontSize(24));
                        document.Add(new Paragraph($"Assigned Investigator: {patientsVisit.AssignedInvestigator.Surname} {patientsVisit.AssignedInvestigator.Name}").SetFontSize(24));
                        document.Add(
                            new Paragraph($"Patient: {patient.PatientHospitalId} {patient.Surname} {patient.Name}:").SetFontSize(24));
                        document.Add(new Paragraph("Scheduled manipulations").SetFontSize(18));
                        foreach (var task in patientsVisit.Tasks)
                        {
                            document.Add(new Paragraph($"   -{task}"));
                        }
                        document.Close();
                    }
                }

            
                byte[] pdfBytes = memoryStream.ToArray();

              
                string tempFilePath = Path.Combine(Path.GetTempPath(), $"temp_pdf_{Guid.NewGuid()}.pdf");

                try
                {
                  
                    await File.WriteAllBytesAsync(tempFilePath, pdfBytes);
                    Console.WriteLine($"Temporary PDF file created at: {tempFilePath}");

                  
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = tempFilePath,
                        UseShellExecute = true
                    };

                    Process.Start(startInfo);

               
                    Console.WriteLine("PDF is open. The file will not be deleted automatically. It is located in a temporary folder..");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to open or process the PDF: " + ex.Message);
                }
            }
        }
    }
}