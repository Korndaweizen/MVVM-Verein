using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Mitgliederverwaltung.Views.Data
{
    /// <summary>
    ///     Interaction logic for ShowDataGridAll.xaml
    /// </summary>
    public partial class Complete : UserControl
    {
        public Complete()
        {
            InitializeComponent();
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (IsTypeOrNullableOfType(e.PropertyType, typeof(DateTime)))
            {
                var column = e.Column as DataGridTextColumn;
                var binding = column.Binding as Binding;
                binding.StringFormat = "dd.MM.yyyy";

                var col = new DataGridTemplateColumn();
                col.Header = e.Column.Header;
                var datePickerFactoryElem = new FrameworkElementFactory(typeof(DatePicker));
                var dateBind = new Binding(e.PropertyName);
                dateBind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                dateBind.Mode = BindingMode.TwoWay;
                datePickerFactoryElem.SetValue(DatePicker.SelectedDateProperty, dateBind);
                datePickerFactoryElem.SetValue(DatePicker.DisplayDateProperty, dateBind);
                var cellTemplate = new DataTemplate();
                cellTemplate.VisualTree = datePickerFactoryElem;
                col.CellTemplate = cellTemplate;
                col.CanUserSort = true;
                col.SortMemberPath = e.Column.SortMemberPath;
                e.Column = col; //Set the new generated column
            }
            if (IsTypeOrNullableOfType(e.PropertyType, typeof(int)))
            {
                e.Column.CanUserSort = true;
            }
            if (IsTypeOrNullableOfType(e.PropertyType, typeof(double)))
            {
                e.Column.CanUserSort = true;
            }
        }

        private bool IsTypeOrNullableOfType(Type propertyType, Type desiredType)
        {
            return propertyType == desiredType || Nullable.GetUnderlyingType(propertyType) == desiredType;
        }
    }
}