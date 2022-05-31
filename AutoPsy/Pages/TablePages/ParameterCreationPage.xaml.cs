using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoPsy.Resources;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParameterCreationPage : ContentPage        // форма для создания нового параметра
    {
        private FullVersionTablePage parent;        // родительский элемент для финальной передачи параметров
        private delegate void AddMethod(string parameter);     // делегат определяет, какая именно функция будет использована для создания параметра (3 типа)
        AddMethod addMethod;
        public ParameterCreationPage(FullVersionTablePage parent)       // в конструкторе класса передаем ссылку на родительский элемент
        {
            InitializeComponent();
            this.parent = parent;

            TypePicker.Items.Add(Constants.RECOMENDATIONS_TAG_RUS);     // заполняем элемент для выбора категории базовыми значениями
            TypePicker.Items.Add(Constants.CONDITIONS_TAG_RUS);
            TypePicker.Items.Add(Constants.TRIGGERS_TAG_RUS);
        }

        private void TypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> values = new List<string>();       // создаем список для помещения туда наблюдаемых величин
            if (TypePicker.SelectedIndex == Const.Constants.RECOMMENDATION_PARAMETER)       // если выбраны рекомендации...
            {
                addMethod = parent.AddRecomendationParameter;       // кладем в делегат метод дял создания рекомендаций
                values = App.TableGraph.GetRecomendations().Select(x => x.Name).ToList();       // получаем список всех рекомендаций из графа
            }
            else if (TypePicker.SelectedIndex == Const.Constants.CONDITION_PARAMETER)       // если выбраны состояния...
            {
                addMethod = parent.AddConditionParameter;       // кладем в делегат метод для создания состояния
                values = App.TableGraph.GetConditions().Select(x => x.Name).ToList();       // получаем список всех состояний из графа
            }
            else if (TypePicker.SelectedIndex == Const.Constants.TRIGGER_PARAMETER)     // если выбраны триггеры...
            {
                addMethod = parent.AddTriggerParameter;     // кладем в делегат метод для создания триггеров
                values = App.TableGraph.GetTriggers().Select(x => x.Name).ToList();     // получаем список триггеров из граффа
            }

            EntityPicker.ItemsSource = values;      // помещаем список элементов в компонент для их последующего выбора
            EntityPicker.IsEnabled = true;      // делаем элемент доступным
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)       // обработчик события при нажатии на кнопку сохранения результатов
        {
            if (CheckCorrectness())     // если данные заполнены корректно...
            {
                var selectedItemIdValue = App.TableGraph.GetIdStringByName(EntityPicker.SelectedItem.ToString());       // получаем ID выбранно элемента
                addMethod(selectedItemIdValue);       // отрабатываем метод, помещенный в делегат
                await Navigation.PopModalAsync();       // покидаем данную страницу
            }
            else
            {
                await DisplayAlert(Alerts.AlertMessage, "Сначала нужно выбрать параметр", AuxiliaryResources.ButtonOK);     // иначе выводим сообщение об ошибке
                return;
            }
        }

        private bool CheckCorrectness()     // метод проверки корректности заполнения данных
        {
            if (EntityPicker.SelectedIndex != -1) return true; else return false;
        }
    }
}