using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.CustomComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserExperiencePanel : StackLayout
    {
        private Database.Entities.UserExperienceHandler experienceHandler;
        public UserExperiencePanel(bool enabled)
        {
            InitializeComponent();
            this.Children.All(x => x.IsEnabled = enabled);

            var currentUser = Database.DatabaseConnector.GetDatabaseConnector().currentConnectedUser;
            experienceHandler = new Database.Entities.UserExperienceHandler(currentUser);
        }

        public void TrySave()
        {
            if (experienceHandler.CheckCorrectness()) experienceHandler.CreateUserExperienceInfo();
        }

        private void ClinicEntry_Focused(object sender, FocusEventArgs e)
        {
            ClinicEntry.Text = "";
        }

        private void DoctorEntry_Focused(object sender, FocusEventArgs e)
        {
            DoctorEntry.Text = "";
        }

        private void DiagnosisEntry_Focused(object sender, FocusEventArgs e)
        {
            DiagnosisEntry.Text = "";
        }

        private void ClinicEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (ClinicEntry.Text != "" && ClinicEntry.Text != AutoPsy.Resources.UserExperienceDefault.Clinic)
                experienceHandler.AddNameOfClinic(ClinicEntry.Text);
            else
                ClinicEntry.Text = AutoPsy.Resources.UserExperienceDefault.Clinic;
        }

        private void DoctorEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (DoctorEntry.Text != "" && DoctorEntry.Text != AutoPsy.Resources.UserExperienceDefault.Doctor)
                experienceHandler.AddTreatingDoctor(DoctorEntry.Text);
            else
                DoctorEntry.Text = AutoPsy.Resources.UserExperienceDefault.Doctor;
        }

        private void DiagnosisEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (DiagnosisEntry.Text != "" && DiagnosisEntry.Text != AutoPsy.Resources.UserExperienceDefault.Diagnosis)
                experienceHandler.AddDiagnosis(DiagnosisEntry.Text);
            else
                DiagnosisEntry.Text = AutoPsy.Resources.UserExperienceDefault.Diagnosis;
        }

        private void ScoreSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            experienceHandler.AddScore((int)ScoreSlider.Value);
        }
    }
}