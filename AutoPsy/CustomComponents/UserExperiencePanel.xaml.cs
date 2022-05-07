using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoPsy.Database.Entities;
using AutoPsy.Resources;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.CustomComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserExperiencePanel : StackLayout, IСustomComponent
    {
        public UserExperienceHandler experienceHandler { get; private set; }
        public UserExperiencePanel(bool enabled)
        {
            InitializeComponent();
            this.Children.All(x => x.IsEnabled = enabled);

            AppointmentDate.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            AppointmentDate.MaximumDate = DateTime.Now;

            var currentUser = App.Connector.currentConnectedUser;
            experienceHandler = new UserExperienceHandler(currentUser) { stateMode = 0};
            experienceHandler.AddAppointment(DateTime.Now);
        }

        public UserExperiencePanel(bool enabled, UserExperience userExperience)       //---------------TODO: ПРОБЛЕМА С ОТОБРАЖЕНИЕМ МЕДИКАМЕНТОВ
        {
            InitializeComponent();
            this.Children.All(x => x.IsEnabled = enabled);

            var currentUser = App.Connector.currentConnectedUser;
            experienceHandler = new UserExperienceHandler(currentUser) { stateMode = 1};

            SynchronizeData(userExperience);
        }

        private void SynchronizeData(UserExperience userExperience)
        {
            experienceHandler.CopyUserExperience(userExperience);

            AppointmentDate.Date = experienceHandler.GetAppointmentDate();
            ClinicEntry.Text = experienceHandler.GetClinic();
            DoctorEntry.Text = experienceHandler.GetDoctor();
            DiagnosisEntry.Text = experienceHandler.GetDiagnosis();
            ScoreSlider.Value = experienceHandler.GetScore();
            experienceHandler.RecreateListOfMedicine(userExperience);
            ListOfMedicine.ItemsSource = experienceHandler.GetMedicine();
            
        }

        public void TrySave()
        {
            if (experienceHandler.CheckCorrectness())
                experienceHandler.CreateUserExperienceInfo();
            else throw new Exception();
        }

        private void AddMedicine_Clicked(object sender, EventArgs e)
        {
            experienceHandler.AddMedicine();
            ListOfMedicine.ItemsSource = experienceHandler.GetMedicine();
        }

        private void ClinicEntry_Focused(object sender, FocusEventArgs e)
        {
            if (ClinicEntry.Text == UserExperienceDefault.Clinic) ClinicEntry.Text = String.Empty;
        }

        private void DoctorEntry_Focused(object sender, FocusEventArgs e)
        {
            if (DoctorEntry.Text == UserExperienceDefault.Doctor) DoctorEntry.Text = String.Empty;
        }

        private void DiagnosisEntry_Focused(object sender, FocusEventArgs e)
        {
            if (DiagnosisEntry.Text == UserExperienceDefault.Diagnosis) DiagnosisEntry.Text = String.Empty;
        }

        private void ClinicEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (ClinicEntry.Text != String.Empty && ClinicEntry.Text != UserExperienceDefault.Clinic)
                experienceHandler.AddNameOfClinic(ClinicEntry.Text);
            else
                ClinicEntry.Text = UserExperienceDefault.Clinic;
        }

        private void DoctorEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (DoctorEntry.Text != String.Empty && DoctorEntry.Text != UserExperienceDefault.Doctor)
                experienceHandler.AddTreatingDoctor(DoctorEntry.Text);
            else
                DoctorEntry.Text = UserExperienceDefault.Doctor;
        }

        private void DiagnosisEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (DiagnosisEntry.Text != String.Empty && DiagnosisEntry.Text != UserExperienceDefault.Diagnosis)
                experienceHandler.AddDiagnosis(DiagnosisEntry.Text);
            else
                DiagnosisEntry.Text = UserExperienceDefault.Diagnosis;
        }

        private void ScoreSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            experienceHandler.AddScore((int)ScoreSlider.Value);
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry.Text == UserExperienceDefault.NameOfMedicine || entry.Text == UserExperienceDefault.Dosage) entry.Text = String.Empty;
        }

        private void NameOfMedicine_Unfocused(object sender, FocusEventArgs e)
        {
            var entryMedicine = sender as Entry;
            if (entryMedicine.Equals(String.Empty)) entryMedicine.Text = UserExperienceDefault.NameOfMedicine;
        }

        private async void Dosage_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entryDosage = sender as Entry;
            try
            {
                if (entryDosage.Text != String.Empty) experienceHandler.SetCurrentMedicineDosage(entryDosage.Text);
            }
            catch
            {
                entryDosage.Text = UserExperienceDefault.Dosage;
            }
        }

        private void NameOfMedicine_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entryMedicine = sender as Entry;
            if (entryMedicine.Text != String.Empty && entryMedicine.Text != UserExperienceDefault.NameOfMedicine) experienceHandler.SetCurrentMedicineName(entryMedicine.Text);
        }

        private void AppointmentDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            experienceHandler.AddAppointment(AppointmentDate.Date);
        }
    }
}