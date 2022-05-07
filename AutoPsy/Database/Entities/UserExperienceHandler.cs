using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AutoPsy.Resources;

namespace AutoPsy.Database.Entities
{
    public class UserExperienceHandler
    {
        private UserExperience userExperience;
        private ObservableCollection<Database.Entities.Medicine> listOfMedicine;
        public byte stateMode { get; set; } = 0;

        public UserExperienceHandler(int userId)
        {
            userExperience = new UserExperience();
            userExperience.UserId = userId;
            userExperience.Diagnosis = String.Empty;
            listOfMedicine = new ObservableCollection<Medicine>();
        }

        public void CopyUserExperience(UserExperience userExperience)
        {
            this.userExperience = (UserExperience)userExperience.Clone();
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
            userExperience.Appointment = appointmentDate.Date;
        }

        public void AddDiagnosis(string diagnosis)
        {
            userExperience.Diagnosis = diagnosis;
        }

        public void AddMedicine()
        {
            if (listOfMedicine.Count == 0) {
                listOfMedicine.Add(new Medicine() { NameOfMedicine = UserExperienceDefault.NameOfMedicine,
                    Dosage = Double.Parse(UserExperienceDefault.Dosage) });
                return;
            }

            var item = listOfMedicine.Last();

            if (!item.NameOfMedicine.Equals(UserExperienceDefault.NameOfMedicine) && item.NameOfMedicine != String.Empty)
                listOfMedicine.Add(new Medicine() { NameOfMedicine = UserExperienceDefault.NameOfMedicine, 
                    Dosage = Double.Parse(UserExperienceDefault.Dosage) });         
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
        public DateTime GetAppointmentDate()
        {
            return userExperience.Appointment;
        }

        public string GetClinic()
        {
            if (userExperience.NameOfClinic != null)
                return userExperience.NameOfClinic;
            else 
                return AuxiliaryResources.NotMentioned;
        }

        public string GetDoctor()
        {
            if (userExperience.TreatingDoctor != null)
                return userExperience.TreatingDoctor;
            else
                return AuxiliaryResources.NotMentioned;
        }

        public string GetDiagnosis()
        {
            return userExperience.Diagnosis;
        }

        public int GetScore()
        {
            return userExperience.Score;
        }

        public bool CheckCorrectness()
        {
            if (userExperience.Appointment != null && userExperience.Diagnosis != String.Empty && userExperience.Diagnosis != UserExperienceDefault.Diagnosis) return true; else return false;
        }

        public void CreateUserExperienceInfo()
        {            
            CodifyListOfMedicine();

            if (stateMode == 0)
                App.Connector.CreateAndInsertData<UserExperience>(userExperience);
            else
                App.Connector.UpdateData<UserExperience>(userExperience);
        }

        private void CodifyListOfMedicine()
        {
            string codifiedMedicine = String.Empty;
            foreach (Medicine medicine in listOfMedicine)
            {
                string temp = String.Join("/", medicine.NameOfMedicine, medicine.Dosage);
                codifiedMedicine += String.Concat(temp, '\\');
            }
            userExperience.IndexOfMedicine = codifiedMedicine;
        }

        public void RecreateListOfMedicine(Database.Entities.UserExperience userExperience)
        {
            if (userExperience.IndexOfMedicine == null) return;

            string[] medicineRequest = userExperience.IndexOfMedicine.Split('\\');

            foreach (string subRequest in medicineRequest)
            {
                string[] tempString = subRequest.Split('/');
                if (tempString.Length == 2) listOfMedicine.Add(new Medicine() { NameOfMedicine = tempString[0], Dosage = Double.Parse(tempString[1]) });
            }
        }
    }
}
