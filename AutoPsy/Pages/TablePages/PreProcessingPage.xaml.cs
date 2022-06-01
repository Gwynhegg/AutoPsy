using AutoPsy.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreProcessingPage : ContentPage
    {
        private readonly Dictionary<string, List<ITableEntity>> entities = new Dictionary<string, List<ITableEntity>>();
        private readonly Dictionary<string, List<float>> entityValues = new Dictionary<string, List<float>>();
        private readonly List<TableEntityHandler> handlers = new List<TableEntityHandler>()
        {
            new RecomendationTableEntityHandler(),
            new CondtiionTableEntityHandler(),
            new TriggerTableEntityHandler()
        };
        public PreProcessingPage()
        {
            InitializeComponent();
            this.DateNavigationStart.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            this.DateNavigationEnd.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));

            this.DateNavigationStart.MaximumDate = DateTime.Now;
            this.DateNavigationEnd.MaximumDate = DateTime.Now.AddHours(1);

            this.DateNavigationStart.Date = DateTime.Now.Date;
            this.DateNavigationEnd.Date = DateTime.Now.Date;

            this.DateNavigationStart.Date = this.DateNavigationStart.Date.AddDays(-7);
        }

        private void SynchronizeEntities()
        {
            this.entityValues.Clear();
            this.entities.Clear();
            foreach (TableEntityHandler handler in this.handlers)
            {
                Dictionary<string, List<ITableEntity>> values = handler.GetValues(this.DateNavigationStart.Date, this.DateNavigationEnd.Date);
                if (values is null) continue;
                foreach (KeyValuePair<string, List<ITableEntity>> pair in values)
                    this.entities.Add(pair.Key, pair.Value);
            }
            CreateFloatValues();
        }

        private void CreateFloatValues()
        {
            var totalEmptyCount = 0;
            foreach (KeyValuePair<string, List<ITableEntity>> pair in this.entities)      // для каждого из параметров из списка...
            {
                this.entityValues[pair.Key] = new List<float>();     // инициализируем новый список значений

                for (DateTime i = this.DateNavigationStart.Date; i <= this.DateNavigationEnd.Date; i = i.AddDays(1))      // для каждой даты из интервала...
                {
                    ITableEntity entity = pair.Value.FirstOrDefault(x => DateTime.Compare(x.Time, i) == 0);      // пытаемся найти значение, совпадающее с датой
                    if (entity == null)
                    {
                        this.entityValues[pair.Key].Add(0);
                        totalEmptyCount++;
                    }
                    else
                    {
                        this.entityValues[pair.Key].Add(entity.Value);       // если оно не найдено, добавляем ноль, иначе добавляем его значение
                    }
                }
            }

            DisplayEmptyValues(totalEmptyCount);
        }

        private void DisplayEmptyValues(int emptyCount)
        {
            this.RequestMessage.IsVisible = true;
            if (emptyCount == 0)
            {
                this.NoNeedButton.IsVisible = true;
                this.PreprocessingButton.IsVisible = false;
                this.RequestMessage.Text = "Нет нужды в предобработке. Вперед!";
            }
            else
            {
                this.PreprocessingButton.IsVisible = true;
                this.NoNeedButton.IsVisible = false;
                this.RequestMessage.Text = string.Format("Пустых значений найдено: {0}. Требуется предобработка!", emptyCount);
            }
        }

        private void DateNavigationStart_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (this.DateNavigationStart.Date > this.DateNavigationEnd.Date) this.DateNavigationStart.Date = this.DateNavigationEnd.Date;
            SynchronizeEntities();
        }

        private void DateNavigationEnd_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (this.DateNavigationEnd.Date < this.DateNavigationStart.Date) this.DateNavigationEnd.Date = this.DateNavigationStart.Date;
            SynchronizeEntities();
        }

        private async void PreprocessingButton_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new PoolAnalysisPage(this.entities, this.entityValues, this.DateNavigationStart.Date, this.DateNavigationEnd.Date));

        private async void NoNeedButton_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new AnalysisSelectionPage(this.entityValues, this.DateNavigationStart.Date, this.DateNavigationEnd.Date));
    }
}