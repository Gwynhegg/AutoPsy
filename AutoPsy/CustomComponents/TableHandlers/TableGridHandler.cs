using AutoPsy.Database.Entities;
using AutoPsy.Pages.TablePages;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace AutoPsy.CustomComponents
{
    public abstract class TableGridHandler      // главный метод-обработчик взаимодействий с таблицы. Дочерние компоненты специфицируют сущностью, с которыми производится работа
    {
        private readonly FullVersionTablePage parentPage;        // ссылка на основную форму
        public Grid mainGrid { get; private set; }      // таблица, содержащая ячейки-сущности и параметры
        protected TableEntityHandler entityHandler;     // обработчик взаимодействия с ячейкой-сущностью (также является абстрактным, взаимодействия происходит через интерфейсы)
        protected DateTime start, end;      // даты начала и конца выборки значений

        public TableGridHandler(FullVersionTablePage parentPage, DateTime start, DateTime end)      // в конструкторе задаются опорные значения дат и ссылка на родительскую форму 
        {
            this.parentPage = parentPage;
            this.start = start;
            this.end = end;
        }
        protected void AddParameter(ITableEntity entity)        // метод добавления параметра по ячейке-образцу (используется в качестве шаблона - тип, имя, важность)
        {
            this.mainGrid.RowDefinitions.Add(new RowDefinition() { Height = 50 });       // добавляем новую строку в таблицу
            var parameterName = new TableHandlers.EntityLabel(entity, this);        // создаем кастомный элемент для параметра
            this.mainGrid.Children.Add(parameterName, 0, this.mainGrid.RowDefinitions.Count - 1);     // помещаем его в макет таблицы
        }

        public void UpdateDateTimes(DateTime start, DateTime end)       // метод для актуализации дат и обновления таблицы
        {
            this.start = start;
            this.end = end;
            UpdateDataGrids();
        }

        // Метод для вызова добавления параметра (переопределяется в классах-наследниках)
        public virtual void AddParameter(string parameter) => this.entityHandler.AddParameter(parameter);

        public abstract void DeleteParameter(string parameter);     // метод удаления параметра. Специфичный, поэтому полностью переопределяется в наследниках

        public void UpdateDataGrids()       // метод для обновления таблицы
        {
            FillTableInformation();     // заполняем информацию
            this.parentPage.SynchronizeContentPage(this.parentPage.InitialStep);      // передаем таблицу на родительскую форму для отображения
        }

        public abstract void UpdateParameter(string parameter);      // метод обновления параметра. Полностью переопределяется классами-наследниками

        public abstract void UpdateValue(ITableEntity entity);      // Метод обновления значения ячейки. Полностью переопределяется классами-наследниками

        public abstract TableEntityHandler GetEntityHandler();      // Геттер для обработчика сущностей-ячеек. Поскольку нас интересуют конкретные и специфичные обработчики, их мы получаем в наследниках

        public void FillTableInformation()      // метод заполнения информации таблицы
        {
            List<string> filterResults = this.entityHandler.GetFilterResults(this.start, this.end);     // через обработчик сущностей получаем набор ячеек, попадающих в указанный период
            GetDateTimeResults(filterResults);      // передаем набор ячеек для отображения на календарной сетке
        }

        protected void GetDateTimeResults(List<string> filterResults)       // Метод отображения результатов выборки по датам
        {
            ClearGrid(this.start, this.end);      // очищаем таблицу и создаем календарную сетку

            foreach (var filter in filterResults)       // для каждого результата из фильтра...
            {
                List<ITableEntity> entities = this.entityHandler.GetEntities(filter);       // получаем набор ячеек-значений по фильтру-параметру

                ITableEntity entityPattern = entities.First();       // создаем шаблон для клонирования
                AddParameter(entityPattern);        // добавляем соответствующий параметр

                var iterator = 1;       // инициализируем итератор
                for (DateTime i = this.start.Date; i <= this.end.Date; i = i.AddDays(1))      // проходим по каждой дате из интервала start к end
                {
                    ITableEntity entity = entities.FirstOrDefault(x => x.Time == i);     // пытаемся получить элемент, удовлетворяющий условию совпадения дат
                    if (entity == null)     // если такой не найдет, создаем по шаблону "фантомный элемент"
                        this.mainGrid.Children.Add(new TableHandlers.EntityButton(entityPattern.Clone(i), this), iterator++, this.mainGrid.RowDefinitions.Count - 1);
                    else
                        // иначе создаем реальный элемент
                        this.mainGrid.Children.Add(new TableHandlers.EntityButton(entity, this), iterator++, this.mainGrid.RowDefinitions.Count - 1);
                }
            }
        }

        private void ClearGrid(DateTime start, DateTime end)        // метод для очистки и пересобрания таблицы
        {
            this.mainGrid = new Grid() { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand, ColumnSpacing = 2, RowSpacing = 2 };      // инициализируем новую таблицу
            this.mainGrid.RowDefinitions.Add(new RowDefinition() { Height = 50 });       // создаем базовые строки и столбцы
            this.mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = 200 });

            var indexator = 1;

            for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))       // проходим по каждой дате из интервала
            {
                this.mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = 50 });      // создаем соответствующий столбец
                var day = i.Day.ToString().Length < 2 ? string.Concat("0", i.Day) : i.Day.ToString();       // получаем строку для отображения дня
                var month = i.Month.ToString().Length < 2 ? string.Concat("0", i.Month) : i.Month.ToString();       // получаем строку для отображения месяца
                this.mainGrid.Children.Add(new Label() { Text = string.Concat(day, ".", month), VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand }, indexator++, 0);       // соединяем строки и помещаем в новый столбец
            }
        }
    }
}
