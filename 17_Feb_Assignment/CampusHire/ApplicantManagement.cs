using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
namespace CampusHire
{
    public class ApplicantManagement
    {
        List<Applicant> applicants = new List<Applicant>();
        private readonly string filePath = "Applicant.json";
        public ApplicantManagement()
        {
            LoadFromFile();
        }
        public void AddApplicant(Applicant applicant)
        {
            applicants.Add(applicant);
            SaveToFile();
        }

        public void DisplayAll()
        {
            if (applicants.Count == 0)
            {
                Console.WriteLine("No records found.");
                return;
            }
            foreach (var a in applicants)
            {
                Console.WriteLine($"ID: {a.ApplicantId}, Name: {a.ApplicantName}, Current: {a.CurrentLocation}, Preferred: {a.PreferredLocation}, Skill: {a.CoreCompetency}, Year: {a.PassingYear}");
            }

        }

        public Applicant Search(string id)
        {
            return applicants.Find(s => s.ApplicantId == id);
        }
        public void Delete(string id)
        {
            Applicant app = Search(id);
            if (app != null)
            {
                applicants.Remove(app);
                SaveToFile();
                Console.WriteLine("Applicant Deleted Successfully");
            }
            else
            {
                Console.WriteLine("Applicant Not Found.");
            }
        }

        public void Update(string id, Applicant updatedApplicant)
        {
            var appli = Search(id);
            if (appli != null)
            {
                applicants.Remove(appli);
                applicants.Add(updatedApplicant);
                SaveToFile();
            }
            else
            {
                Console.WriteLine("Applicant Not Found.");
            }

        }

        private void SaveToFile()
        {
            var opt = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(applicants, opt);
            File.WriteAllText(filePath, json);
        }
        private void LoadFromFile()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                applicants = JsonSerializer.Deserialize<List<Applicant>>(json) ?? new List<Applicant>();
            }
            else
            {
                applicants = new List<Applicant>();
            }
        }

    }
}
