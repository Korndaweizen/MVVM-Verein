/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:BaseMVVM"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Microsoft.Practices.Unity;

namespace Mitgliederverwaltung.ViewModel
{
    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private static Bootstrapper _bootStrapper;

        /// <summary>
        ///     Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            if (_bootStrapper == null)
                _bootStrapper = new Bootstrapper();
        }

        public GridViewModel GridView => _bootStrapper.Container.Resolve<GridViewModel>();
        public AddUserViewModel AddUserView => _bootStrapper.Container.Resolve<AddUserViewModel>();

        public ExportVm ExportView => _bootStrapper.Container.Resolve<ExportVm>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}