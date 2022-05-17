using AutoPsy.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreProcessingPage : ContentPage
    {
        private Dictionary<string, List<ITableEntity>> entities = new Dictionary<string, List<ITableEntity>>();
        private Dictionary<string, List<float>> entityValues = new Dictionary<string, List<float>>();
        private List<TableEntityHandler> handlers = new List<TableEntityHandler>()
        {
            new RecomendationTableEntityHandler(),
            new CondtiionTableEntityHandler(),
            new TriggerTableEntityHandler()
        };
        public PreProcessingPage()
        {
            InitializeComponent();
            DateNavigationStart.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            DateNavigationEnd.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));

            DateNavigationStart.MaximumDate = DateTime.Now;
            DateNavigationEnd.MaximumDate = DateTime.Now.AddHours(1);

            DateNavigationStart.Date = DateTime.Now.Date;
            DateNavigationEnd.Date = DateTime.Now.Date;

            DateNavigationStart.Date = DateNavigationStart.Date.AddDays(-7);
        }

        private void SynchronizeEntities()
        {
            entityValues.Clear();
            entities.Clear();
            foreach (var handler in handlers)
                foreach (var pair in handler.GetValues(DateNavigationStart.Date, DateNavigationEnd.Date))
                    entities.Add(pair.Key, pair.Value);

            CreateFloatValues();
        }

        private void CreateFloatValues()        
        {
            var totalEmptyCount = 0;
            foreach (var pair in entities)      // для каждого из параметров из списка...
            {
                entityValues[pair.Key] = new List<float>();     // инициализируем новый список значений

                for (DateTime i = DateNavigationStart.Date; i <= DateNavigationEnd.Date; i = i.AddDays(1))      // для каждой даты из интервала...
                {
                    var entity = pair.Value.FirstOrDefault(x => DateTime.Compare(x.Time, i) == 0);      // пытаемся найти значение, совпадающее с датой
                    if (entity == null) 
                    {
                        entityValues[pair.Key].Add(0);
                        totalEmptyCount++; 
                    }
                    else entityValues[pair.Key].Add(entity.Value);       // если оно не найдено, добавляем ноль, иначе добавляем его значение
                }
            }

            DisplayEmptyValues(totalEmptyCount);
        }

        private void DisplayEmptyValues(int emptyCount)
        {
            RequestMessage.IsVisible = true;
            if (emptyCount == 0)
            {
                NoNeedButton.IsVisible = true;
                PreprocessingButton.IsVisible = false;
                RequestMessage.Text = "Нет нужды в предобработке. Вперед!";
            }
            else
            {
                PreprocessingButton.IsVisible = true;
                NoNeedButton.IsVisible = false;
                RequestMessage.Text = String.Format("Пустых значений найдено: {0}. Требуется предобработка!", emptyCount);
            }
        }

        private void DateNavigationStart_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (DateNavigationStart.Date > DateNavigationEnd.Date) DateNavigationStart.Date = DateNavigationEnd.Date;
            SynchronizeEntities();
        }

        private void DateNavigationEnd_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (DateNavigationEnd.Date < DateNavigationStart.Date) DateNavigationEnd.Date = DateNavigationStart.Date;
            SynchronizeEntities();
        }

        private async void PreprocessingButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PoolAnalysisPage(entities, entityValues, DateNavigationStart.Date, DateNavigationEnd.Date));
        }

        private async void NoNeedButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AnalysisSelectionPage(entityValues, DateNavigationStart.Date, DateNavigationEnd.Date));
        }
    }
    }