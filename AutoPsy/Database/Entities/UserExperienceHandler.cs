using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AutoPsy.Database.Entities
{
    public class UserExperienceHandler
    {
        private UserExperience userExperience;
        private ObservableCollection<Database.Entities.Medicine> listOfMedicine;

        public UserExperienceHandler(int userId)
        {
            userExperience = new UserExperience();
            userExperience.UserId = userId;
            userExperience.Diagnosis = "";
            listOfMedicine = new ObservableCollection<Medicine>();
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
            userExperience.Diagnosis = diagnosis;
        }

        public void AddMedicine()
        {
            if (listOfMedicine.Count == 0) {
                listOfMedicine.Add(new Medicine() { NameOfMedicine = AutoPsy.Resources.UserExperienceDefault.NameOfMedicine, Dosage = Double.Parse(AutoPsy.Resources.UserExperienceDefault.Dosage) });
                return;
            }

            var item = listOfMedicine.Last();

            if (!item.NameOfMedicine.Equals(AutoPsy.Resources.UserExperienceDefault.NameOfMedicine) && item.NameOfMedicine != "")
                listOfMedicine.Add(new Medicine() { NameOfMedicine = AutoPsy.Resources.UserExperienceDefault.NameOfMedicine, Dosage = Double.Parse(AutoPsy.Resources.UserExperienceDefault.Dosage) });         
        }

        public void SetCurrentMedicineName(string name)
        {
            listOfMedicine.Last().NameOfMedicine = name;
        }

        public void SetCurrentMedicineDosage(string dosage)
        {
            try
            {
                listOfMedicine.Last().Dosage = Double.Parse(dosage);
            }
            catch
            {
                throw new Exception();
            }
        }

        public ObservableCollection<Medicine> GetMedicine()
        {
            return listOfMedicine;
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
            if (userExperience.Appointment != null && userExperience.Diagnosis != "" && userExperience.Diagnosis != AutoPsy.Resources.UserExperienceDefault.Diagnosis) return true; else return false;
        }

        public void CreateUserExperienceInfo()
        {
            string codifiedMedicine = "";
            foreach (Medicine medicine in listOfMedicine)
            {
                string temp = String.Join("/", medicine.NameOfMedicine, medicine.Dosage);
                codifiedMedicine += String.Concat(temp, "\\");
            }
            userExperience.IndexOfMedicine = codifiedMedicine;
            DatabaseConnector.GetDatabaseConnector().CreateAndInsertData(userExperience);
        }
    }
}
