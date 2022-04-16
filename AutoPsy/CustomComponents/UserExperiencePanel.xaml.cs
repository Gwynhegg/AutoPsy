﻿using System;
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
        public Database.Entities.UserExperienceHandler experienceHandler { get; private set; }
        public UserExperiencePanel(bool enabled)
        {
            InitializeComponent();
            this.Children.All(x => x.IsEnabled = enabled);

            var currentUser = Database.DatabaseConnector.GetDatabaseConnector().currentConnectedUser;
            experienceHandler = new Database.Entities.UserExperienceHandler(currentUser) { stateMode = 0};
        }

        public UserExperiencePanel(bool enabled, Database.Entities.UserExperience userExperience)
        {
            InitializeComponent();
            this.Children.All(x => x.IsEnabled = enabled);

            var currentUser = Database.DatabaseConnector.GetDatabaseConnector().currentConnectedUser;
            experienceHandler = new Database.Entities.UserExperienceHandler(currentUser) { stateMode = 1};
            SynchronizeData(userExperience);
        }

        private void SynchronizeData(Database.Entities.UserExperience userExperience)
        {
            experienceHandler.CopyUserExperience(userExperience);

            ClinicEntry.Text = userExperience.NameOfClinic;
            DoctorEntry.Text = userExperience.TreatingDoctor;
            DiagnosisEntry.Text = userExperience.Diagnosis;
            ScoreSlider.Value = userExperience.Score;
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
            if (ClinicEntry.Text == AutoPsy.Resources.UserExperienceDefault.Clinic) ClinicEntry.Text = "";
        }

        private void DoctorEntry_Focused(object sender, FocusEventArgs e)
        {
            if (DoctorEntry.Text == AutoPsy.Resources.UserExperienceDefault.Doctor) DoctorEntry.Text = "";
        }

        private void DiagnosisEntry_Focused(object sender, FocusEventArgs e)
        {
            if (DiagnosisEntry.Text == AutoPsy.Resources.UserExperienceDefault.Diagnosis) DiagnosisEntry.Text = "";
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

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            if (entry.Text == AutoPsy.Resources.UserExperienceDefault.NameOfMedicine || entry.Text == AutoPsy.Resources.UserExperienceDefault.Dosage) entry.Text = "";
        }

        private void NameOfMedicine_Unfocused(object sender, FocusEventArgs e)
        {
            var entryMedicine = sender as Entry;
            if (entryMedicine.Equals("")) entryMedicine.Text = AutoPsy.Resources.UserExperienceDefault.NameOfMedicine;
        }

        private async void Dosage_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entryDosage = sender as Entry;
            try
            {
                if (entryDosage.Text != "") experienceHandler.SetCurrentMedicineDosage(entryDosage.Text);
            }
            catch
            {
                entryDosage.Text = AutoPsy.Resources.UserExperienceDefault.Dosage;
            }
        }

        private void NameOfMedicine_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entryMedicine = sender as Entry;
            if (entryMedicine.Text != "" && entryMedicine.Text != AutoPsy.Resources.UserExperienceDefault.NameOfMedicine) experienceHandler.SetCurrentMedicineName(entryMedicine.Text);
        }
    }
}