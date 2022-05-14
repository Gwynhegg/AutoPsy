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
    public partial class EntityLabel : StackLayout      // класс-обработчик функции взаимодействия с записью
    {
        private TableGridHandler parentGridHandler;     // ссылка на обработчик взаимодействий с таблицей
        private Database.Entities.ITableEntity entity;      // ссылка на сущность-ячейку (нужна номинально для определения имени параметра и типа)
        public EntityLabel(Database.Entities.ITableEntity entity, TableGridHandler parentGridHandler)       // конструктор объекта принимает ссылки на все эуказанные выше элементы
        {
            InitializeComponent();
            this.parentGridHandler = parentGridHandler;
            this.entity = entity;
            this.LabelEntity.Text = entity.Name;
        }

        // При нажатии на категорию создается форма редактирования категории (что повлияет на все записи), в которую передаются ссылки на обработчик и сущность
        private async void LabelEntity_Clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new Pages.TablePages.ParameterUpdatePage(entity, parentGridHandler));
    }
}