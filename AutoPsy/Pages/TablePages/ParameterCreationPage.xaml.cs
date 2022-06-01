using AutoPsy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParameterCreationPage : ContentPage        // форма для создания нового параметра
    {
        private readonly FullVersionTablePage parent;        // родительский элемент для финальной передачи параметров
        private delegate void AddMethod(string parameter);     // делегат определяет, какая именно функция будет использована для создания параметра (3 типа)

        private AddMethod addMethod;
        public ParameterCreationPage(FullVersionTablePage parent)       // в конструкторе класса передаем ссылку на родительский элемент
        {
            InitializeComponent();
            this.parent = parent;

            this.TypePicker.Items.Add(Constants.RECOMENDATIONS_TAG_RUS);     // заполняем элемент для выбора категории базовыми значениями
            this.TypePicker.Items.Add(Constants.CONDITIONS_TAG_RUS);
            this.TypePicker.Items.Add(Constants.TRIGGERS_TAG_RUS);
        }

        private void TypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var values = new List<string>();       // создаем список для помещения туда наблюдаемых величин
            if (this.TypePicker.SelectedIndex == Const.Constants.RECOMMENDATION_PARAMETER)       // если выбраны рекомендации...
            {
                this.addMethod = this.parent.AddRecomendationParameter;       // кладем в делегат метод дял создания рекомендаций
                values = App.TableGraph.GetRecomendations().Select(x => x.Name).ToList();       // получаем список всех рекомендаций из графа
            }
            else if (this.TypePicker.SelectedIndex == Const.Constants.CONDITION_PARAMETER)       // если выбраны состояния...
            {
                this.addMethod = this.parent.AddConditionParameter;       // кладем в делегат метод для создания состояния
                values = App.TableGraph.GetConditions().Select(x => x.Name).ToList();       // получаем список всех состояний из графа
            }
            else if (this.TypePicker.SelectedIndex == Const.Constants.TRIGGER_PARAMETER)     // если выбраны триггеры...
            {
                this.addMethod = this.parent.AddTriggerParameter;     // кладем в делегат метод для создания триггеров
                values = App.TableGraph.GetTriggers().Select(x => x.Name).ToList();     // получаем список триггеров из граффа
            }

            this.EntityPicker.ItemsSource = values;      // помещаем список элементов в компонент для их последующего выбора
            this.EntityPicker.IsEnabled = true;      // делаем элемент доступным
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)       // обработчик события при нажатии на кнопку сохранения результатов
        {
            if (CheckCorrectness())     // если данные заполнены корректно...
            {
                var selectedItemIdValue = App.TableGraph.GetIdStringByName(this.EntityPicker.SelectedItem.ToString());       // получаем ID выбранно элемента
                this.addMethod(selectedItemIdValue);       // отрабатываем метод, помещенный в делегат
                await this.Navigation.PopModalAsync();       // покидаем данную страницу
            }
            else
            {
                await DisplayAlert(Alerts.AlertMessage, "Сначала нужно выбрать параметр", AuxiliaryResources.ButtonOK);     // иначе выводим сообщение об ошибке
                return;
            }
        }

        private bool CheckCorrectness()     // метод проверки корректности заполнения данных
        {
            if (this.EntityPicker.SelectedIndex != -1) return true; else return false;
        }
    }
}