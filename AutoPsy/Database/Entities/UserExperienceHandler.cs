using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.Database.Entities
{
    public class UserExperienceHandler
    {
        private UserExperience userExperience;

        public UserExperienceHandler(int userId)
        {
            userExperience = new UserExperience();
            userExperience.UserId = userId;
            userExperience.Diagnosis = "";
            userExperience.ListOfMedicine = new List<Medicine>();
        }

        public void AddNameOfClinic(string nameOfClinic)
        {
            userExperience.NameOfClinic = nameOfClinic;
        }

        public void AddTreatingDoctor(string doctor)
        {
            userExperience.TreatingDoctor = doctor;
        }

        public void AddAppointment(DateTime appointmentDate)
        {
            userExperience.Appointment = appointmentDate;
        }

        public void AddDiagnosis(string diagnosis)
        {
            userExperience.Diagnosis= diagnosis;
        }

        public void AddMedicine(string nameOfMedicine, double dosage)
        {
            Medicine medicine = new Medicine() { NameOfMedicine = nameOfMedicine, Dosage = dosage };
            userExperience.ListOfMedicine.Add(medicine);
        }

        public void AddScore(int score)
        {
            userExperience.Score = score;
        }

        public UserExperience GetUserExperience()
        {
            return userExperience;
        }

        public bool CheckCorrectness()
        {
            if (userExperience.Appointment != null && userExperience.Diagnosis != "") return true; else return false;
        }

        public void CreateUserExperienceInfo()
        {
            DatabaseConnector.GetDatabaseConnector().CreateAndInsertData(userExperience);
        }
    }
}
