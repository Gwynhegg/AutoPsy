using AutoPsy.Database.Entities;
using AutoPsy.Pages.TablePages;
using System;

namespace AutoPsy.CustomComponents.TableHandlers
{
    public class RecomendationsGridHandler : TableGridHandler       // Класс-обработчик взаимодействия с таблицей рекомендаций
    {
        // Конструктор использует родительскую логику, но переопределяет обработчик сущностей ячеек под свои нужды. Это необходимо для записи данных в отдельную таблицу
        public RecomendationsGridHandler(FullVersionTablePage parentPage, DateTime start, DateTime end) : base(parentPage, start, end) => this.entityHandler = new RecomendationTableEntityHandler();      // переопределения обработчика сущностей
        public override void AddParameter(string parameter)        // метод для добавления параметра в таблицу
        {
            base.AddParameter(parameter);       // вызов родительского метода

            // создание стартовой сущности для инициализации параметра и задание ее первоначальных значений
            var entity = new TableRecomendation() { IdValue = parameter, Name = App.TableGraph.GetNameByIdString(parameter), Time = this.end.Date, Value = 1, Type = "Recomendation" };

            this.entityHandler.AddEntity<TableRecomendation>(parameter, entity);     // передача сущности в обработчик для последующего добавления
        }
        public override TableEntityHandler GetEntityHandler()       // геттер для получения обработчика сущностей
=> this.entityHandler;

        public override void DeleteParameter(string parameter)      // метод для удаления определенного параметра
        {
            this.entityHandler.DeleteParameter(parameter);       // говорим обработчику сущностей, какой параметр следует удалить
            UpdateDataGrids();      // вызываем обновление таблицы
        }

        public override void UpdateParameter(string parameter)       // метод для обновления определенного параметра
        {
            this.entityHandler.UpdateParameter<TableRecomendation>(parameter);     // говорим обработчику сущностей, какой параметр следует обновить
            UpdateDataGrids();      // вызываем обновление таблицы
        }

        public override void UpdateValue(ITableEntity entity)       // метод для обновления определенного значения таблциы
        {
            this.entityHandler.UpdateEntityValue<TableRecomendation>(entity);        // передаем данные о сущности в обработчик, указав спецификацию с помощью обобщенных типов
            UpdateDataGrids();      // вызываем обновление таблицы
        }
    }

    public class ConditionsGridHandler : TableGridHandler       // Логика данного класса полностью повторяет предыдущий и нужна для спецификации значений в базе данных
    {
        public ConditionsGridHandler(FullVersionTablePage parentPage, DateTime start, DateTime end) : base(parentPage, start, end) => this.entityHandler = new CondtiionTableEntityHandler();
        public override void AddParameter(string parameter)
        {
            base.AddParameter(parameter);

            var entity = new TableCondition() { IdValue = parameter, Name = App.TableGraph.GetNameByIdString(parameter), Time = this.end.Date, Value = 1, Type = "Condition" };

            this.entityHandler.AddEntity<TableCondition>(parameter, entity);
        }
        public override TableEntityHandler GetEntityHandler() => this.entityHandler;

        public override void DeleteParameter(string parameter)
        {
            this.entityHandler.DeleteParameter(parameter);
            UpdateDataGrids();
        }
        public override void UpdateParameter(string parameter)
        {
            this.entityHandler.UpdateParameter<TableCondition>(parameter);
            UpdateDataGrids();
        }

        public override void UpdateValue(ITableEntity entity)
        {
            this.entityHandler.UpdateEntityValue<TableCondition>(entity);
            UpdateDataGrids();
        }


    }

    public class TriggersGridHandler : TableGridHandler     // логика данного класса полностью повторяет предыдущий и нужна для спецификации значений в базе данных
    {
        public TriggersGridHandler(FullVersionTablePage parentPage, DateTime start, DateTime end) : base(parentPage, start, end) => this.entityHandler = new TriggerTableEntityHandler();

        public override void AddParameter(string parameter)
        {
            base.AddParameter(parameter);

            var entity = new TableTrigger() { IdValue = parameter, Name = App.TableGraph.GetNameByIdString(parameter), Time = this.end.Date, Value = 0, Type = "Trigger" };

            this.entityHandler.AddEntity<TableTrigger>(parameter, entity);
        }

        public override TableEntityHandler GetEntityHandler() => this.entityHandler;

        public override void DeleteParameter(string parameter)
        {
            this.entityHandler.DeleteParameter(parameter);
            UpdateDataGrids();
        }
        public override void UpdateParameter(string parameter)
        {
            this.entityHandler.UpdateParameter<TableTrigger>(parameter);
            UpdateDataGrids();
        }

        public override void UpdateValue(ITableEntity entity)
        {
            this.entityHandler.UpdateEntityValue<TableTrigger>(entity);
            UpdateDataGrids();
        }
    }
}
