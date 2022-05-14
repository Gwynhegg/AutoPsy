using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.CustomComponents.TableHandlers
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntityButton : StackLayout     // класс-обработчик функций взаимодействия с ячейкой таблицы состояний
    {
        private TableGridHandler parentGridHandler;     // ссылка на обработчик взаимодействий с таблицами
        private Database.Entities.ITableEntity entity;      // изменяемая сущность-ячейка
        public EntityButton(Database.Entities.ITableEntity entity, TableGridHandler parentGridHandler)      // конструктор для инициализации ключевых ссылок
        {
            InitializeComponent();
            this.entity = entity;       // передаем ссылку на сущность-ячейку
            this.parentGridHandler = parentGridHandler;     // передаем ссылку на обработчик действий
        }

        public string Value
        { set { this.ValueLabel.Text = value.ToString(); } }

        // при нажатии на кнопку-представление создаем форму для изменения ячейки и передаем туда все ссылки
        private async void ValueLabel_Clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new Pages.TablePages.ValueSetterPage(entity, parentGridHandler));
    }
}