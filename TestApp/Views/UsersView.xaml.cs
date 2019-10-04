using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestApp.ViewModels;

namespace TestApp.Views
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class UsersView : ReactiveUserControl<UsersListViewModel>
    {
        public UsersView()
        {
            InitializeComponent();

            var a = ViewModel;

            this.WhenActivated(disposables =>
            {
                this.BindCommand(ViewModel, x => x.CreateUserCommand, x => x.CreateUserButton).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.GetUsersCommand, x => x.RefreshButton).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.Users, x => x.UsersList.ItemsSource).DisposeWith(disposables);
                ViewModel.GetUsersCommand.Execute();
            });
        }
    }
}
