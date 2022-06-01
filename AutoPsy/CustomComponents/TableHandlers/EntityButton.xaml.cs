using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.CustomComponents.TableHandlers
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntityButton : StackLayout     // класс-обработчик функций взаимодействия с ячейкой таблицы состояний
    {
        private readonly TableGridHandler parentGridHandler;     // ссылка на обработчик взаимодействий с таблицами
        private readonly Database.Entities.ITableEntity entity;      // изменяемая сущность-ячейка
        public EntityButton(Database.Entities.ITableEntity entity, TableGridHandler parentGridHandler)      // конструктор для инициализации ключевых ссылок
        {
            InitializeComponent();
            this.entity = entity;       // передаем ссылку на сущность-ячейку
            this.parentGridHandler = parentGridHandler;     // передаем ссылку на обработчик действий
            this.ValueLabel.Text = entity.Value == 0 ? string.Empty : entity.Value.ToString();
            SetBackgroundColor();
        }

        private void SetBackgroundColor()
        {
            if (this.entity is Database.Entities.TableTrigger)
            {
                this.Background = AuxServices.ColorPicker.CriticalBrushScheme[this.entity.Value];
                this.ValueLabel.BackgroundColor = AuxServices.ColorPicker.CriticalScheme[this.entity.Value];
            }
            else
            {
                this.Background = AuxServices.ColorPicker.ColorBrushScheme[this.entity.Value];
                this.ValueLabel.BackgroundColor = AuxServices.ColorPicker.ColorScheme[this.entity.Value];
            }
        }

        // при нажатии на кнопку-представление создаем форму для изменения ячейки и передаем туда все ссылки
        private async void ValueLabel_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new Pages.TablePages.ValueSetterPage(this.entity, this.parentGridHandler));
    }
}