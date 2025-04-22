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
                        document.Add(new Paragraph($"Visit {patientsVisit.Name}"));
                        document.Add(new Paragraph($"Visit Date: {patientsVisit.DateOfVisit:dd.MM.yyyy}"));
                        document.Add(new Paragraph($"Assigned Investigator: {patientsVisit.AssignedInvestigator.Surname} {patientsVisit.AssignedInvestigator.Name}"));
                        document.Add(
                            new Paragraph($"Patient: {patient.PatientHospitalId} {patient.Surname} {patient.Name}"));
                        document.Add(new Paragraph("Scheduled manipulations"));
                        foreach (var task in patientsVisit.Tasks)
                        {
                            document.Add(new Paragraph(task));
                        }
                        document.Close();
                    }
                }

                // Получаем байты PDF из MemoryStream
                byte[] pdfBytes = memoryStream.ToArray();

                // Создаем временный файл с уникальным именем
                string tempFilePath = Path.Combine(Path.GetTempPath(), $"temp_pdf_{Guid.NewGuid()}.pdf");

                try
                {
                    // Сохраняем PDF во временный файл
                    await File.WriteAllBytesAsync(tempFilePath, pdfBytes);
                    Console.WriteLine($"Temporary PDF file created at: {tempFilePath}");

                    // Открываем PDF в стандартной программе
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = tempFilePath,
                        UseShellExecute = true
                    };

                    Process.Start(startInfo);

                    // Информируем, что файл не будет удалён
                    Console.WriteLine("PDF открыт. Файл не будет удалён автоматически. Он находится во временной папке.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to open or process the PDF: " + ex.Message);
                }
            }
        }
    }
}