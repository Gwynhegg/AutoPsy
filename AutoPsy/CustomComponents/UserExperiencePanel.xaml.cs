using AutoPsy.Database.Entities;
using AutoPsy.Resources;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.CustomComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserExperiencePanel : Grid, IСustomComponent
    {
        public UserExperienceHandler experienceHandler { get; private set; }
        public UserExperiencePanel(bool enabled)
        {
            InitializeComponent();
            this.Children.All(x => x.IsEnabled = enabled);

            this.AppointmentDate.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            this.AppointmentDate.MaximumDate = DateTime.Now;

            var currentUser = App.Connector.currentConnectedUser;
            this.experienceHandler = new UserExperienceHandler(currentUser) { stateMode = 0 };
            this.experienceHandler.AddAppointment(DateTime.Now);
        }

        public UserExperiencePanel(bool enabled, UserExperience userExperience)       //---------------TODO: ПРОБЛЕМА С ОТОБРАЖЕНИЕМ МЕДИКАМЕНТОВ
        {
            InitializeComponent();
            this.Children.All(x => x.IsEnabled = enabled);

            var currentUser = App.Connector.currentConnectedUser;
            this.experienceHandler = new UserExperienceHandler(currentUser) { stateMode = 1 };

            SynchronizeData(userExperience);
        }

        private void SynchronizeData(UserExperience userExperience)
        {
            this.experienceHandler.CopyUserExperience(userExperience);

            this.AppointmentDate.Date = this.experienceHandler.GetAppointmentDate();
            this.ClinicEntry.Text = this.experienceHandler.GetClinic();
            this.DoctorEntry.Text = this.experienceHandler.GetDoctor();
            this.DiagnosisEntry.Text = this.experienceHandler.GetDiagnosis();
            this.ScoreSlider.Value = this.experienceHandler.GetScore();
            this.experienceHandler.RecreateListOfMedicine(userExperience);
            this.ListOfMedicine.ItemsSource = this.experienceHandler.GetMedicine();

        }

        public void TrySave()
        {
            if (this.experienceHandler.CheckCorrectness())
                this.experienceHandler.CreateUserExperienceInfo();
            else throw new Exception();
        }

        private void AddMedicine_Clicked(object sender, EventArgs e)
        {
            this.experienceHandler.AddMedicine();
            this.ListOfMedicine.ItemsSource = this.experienceHandler.GetMedicine();
        }

        private void ClinicEntry_Focused(object sender, FocusEventArgs e)
        {
            if (this.ClinicEntry.Text == UserExperienceDefault.Clinic) this.ClinicEntry.Text = string.Empty;
        }

        private void DoctorEntry_Focused(object sender, FocusEventArgs e)
        {
            if (this.DoctorEntry.Text == UserExperienceDefault.Doctor) this.DoctorEntry.Text = string.Empty;
        }

        private void DiagnosisEntry_Focused(object sender, FocusEventArgs e)
        {
            if (this.DiagnosisEntry.Text == UserExperienceDefault.Diagnosis) this.DiagnosisEntry.Text = string.Empty;
        }

        private void ClinicEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (this.ClinicEntry.Text != string.Empty && this.ClinicEntry.Text != UserExperienceDefault.Clinic)
                this.experienceHandler.AddNameOfClinic(this.ClinicEntry.Text);
            else
                this.ClinicEntry.Text = UserExperienceDefault.Clinic;
        }

        private void DoctorEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (this.DoctorEntry.Text != string.Empty && this.DoctorEntry.Text != UserExperienceDefault.Doctor)
                this.experienceHandler.AddTreatingDoctor(this.DoctorEntry.Text);
            else
                this.DoctorEntry.Text = UserExperienceDefault.Doctor;
        }

        private void DiagnosisEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (this.DiagnosisEntry.Text != string.Empty && this.DiagnosisEntry.Text != UserExperienceDefault.Diagnosis)
                this.experienceHandler.AddDiagnosis(this.DiagnosisEntry.Text);
            else
                this.DiagnosisEntry.Text = UserExperienceDefault.Diagnosis;
        }

        private void ScoreSlider_ValueChanged(object sender, ValueChangedEventArgs e) => this.experienceHandler.AddScore((int)this.ScoreSlider.Value);

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry.Text == UserExperienceDefault.NameOfMedicine || entry.Text == UserExperienceDefault.Dosage) entry.Text = string.Empty;
        }

        private void NameOfMedicine_Unfocused(object sender, FocusEventArgs e)
        {
            var entryMedicine = sender as Entry;
            if (entryMedicine.Equals(string.Empty)) entryMedicine.Text = UserExperienceDefault.NameOfMedicine;
        }

        private async void Dosage_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entryDosage = sender as Entry;
            try
            {
                if (entryDosage.Text != string.Empty) this.experienceHandler.SetCurrentMedicineDosage(entryDosage.Text);
            }
            catch
            {
                entryDosage.Text = UserExperienceDefault.Dosage;
            }
        }

        private void NameOfMedicine_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entryMedicine = sender as Entry;
            if (entryMedicine.Text != string.Empty && entryMedicine.Text != UserExperienceDefault.NameOfMedicine) this.experienceHandler.SetCurrentMedicineName(entryMedicine.Text);
        }

        private void AppointmentDate_DateSelected(object sender, DateChangedEventArgs e) => this.experienceHandler.AddAppointment(this.AppointmentDate.Date);
    }
}