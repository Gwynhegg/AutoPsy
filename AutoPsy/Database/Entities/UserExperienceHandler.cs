using AutoPsy.Resources;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoPsy.Database.Entities
{
    public class UserExperienceHandler
    {
        private UserExperience userExperience;
        private readonly ObservableCollection<Medicine> listOfMedicine;
        public byte stateMode { get; set; } = 0;

        public UserExperienceHandler(int userId)
        {
            this.userExperience = new UserExperience
            {
                UserId = userId,
                Diagnosis = string.Empty
            };
            this.listOfMedicine = new ObservableCollection<Medicine>();
        }

        public void CopyUserExperience(UserExperience userExperience) => this.userExperience = (UserExperience)userExperience.Clone();


        public void AddNameOfClinic(string nameOfClinic) => this.userExperience.NameOfClinic = nameOfClinic;

        public void AddTreatingDoctor(string doctor) => this.userExperience.TreatingDoctor = doctor;

        public void AddAppointment(DateTime appointmentDate) => this.userExperience.Appointment = appointmentDate.Date;

        public void AddDiagnosis(string diagnosis) => this.userExperience.Diagnosis = diagnosis;

        public void AddMedicine()
        {
            if (this.listOfMedicine.Count == 0)
            {
                this.listOfMedicine.Add(new Medicine()
                {
                    NameOfMedicine = UserExperienceDefault.NameOfMedicine,
                    Dosage = double.Parse(UserExperienceDefault.Dosage)
                });
                return;
            }

            Medicine item = this.listOfMedicine.Last();

            if (!item.NameOfMedicine.Equals(UserExperienceDefault.NameOfMedicine) && item.NameOfMedicine != string.Empty)
            {
                this.listOfMedicine.Add(new Medicine()
                {
                    NameOfMedicine = UserExperienceDefault.NameOfMedicine,
                    Dosage = double.Parse(UserExperienceDefault.Dosage)
                });
            }
        }

        public void SetCurrentMedicineName(string name) => this.listOfMedicine.Last().NameOfMedicine = name;

        public void SetCurrentMedicineDosage(string dosage)
        {
            try
            {
                this.listOfMedicine.Last().Dosage = double.Parse(dosage);
            }
            catch
            {
                throw new Exception();
            }
        }

        public ObservableCollection<Medicine> GetMedicine() => this.listOfMedicine;

        public void AddScore(int score) => this.userExperience.Score = score;

        public UserExperience GetUserExperience() => this.userExperience;
        public DateTime GetAppointmentDate() => this.userExperience.Appointment;

        public string GetClinic()
        {
            if (this.userExperience.NameOfClinic != null)
                return this.userExperience.NameOfClinic;
            else
                return AuxiliaryResources.NotMentioned;
        }

        public string GetDoctor()
        {
            if (this.userExperience.TreatingDoctor != null)
                return this.userExperience.TreatingDoctor;
            else
                return AuxiliaryResources.NotMentioned;
        }

        public string GetDiagnosis() => this.userExperience.Diagnosis;

        public int GetScore() => this.userExperience.Score;

        public bool CheckCorrectness()
        {
            if (this.userExperience.Appointment != null && this.userExperience.Diagnosis != string.Empty && this.userExperience.Diagnosis != UserExperienceDefault.Diagnosis) return true; else return false;
        }

        public void CreateUserExperienceInfo()
        {
            CodifyListOfMedicine();

            if (this.stateMode == 0)
                App.Connector.CreateAndInsertData<UserExperience>(this.userExperience);
            else
                App.Connector.UpdateData<UserExperience>(this.userExperience);
        }

        private void CodifyListOfMedicine()
        {
            var codifiedMedicine = string.Empty;
            foreach (Medicine medicine in this.listOfMedicine)
            {
                var temp = string.Join(", ", medicine.NameOfMedicine, medicine.Dosage);
                codifiedMedicine += string.Concat(temp, '\n');
            }
            this.userExperience.IndexOfMedicine = codifiedMedicine;
        }

        public void RecreateListOfMedicine(Database.Entities.UserExperience userExperience)
        {
            if (userExperience.IndexOfMedicine == null) return;

            var medicineRequest = userExperience.IndexOfMedicine.Split('\n');

            foreach (var subRequest in medicineRequest)
            {
                var tempString = subRequest.Split(',');
                if (tempString.Length == 2) this.listOfMedicine.Add(new Medicine() { NameOfMedicine = tempString[0], Dosage = double.Parse(tempString[1].Trim()) });
            }
        }
    }
}
