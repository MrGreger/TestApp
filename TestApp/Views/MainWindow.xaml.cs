using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using ReactiveUI;
using Splat;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestApp.ViewModels;

namespace TestApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// C:\Users\Greg I Am\Downloads\testproject-ad7ed-firebase-adminsdk-9rmjs-2d59dd753d.json
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault()
            });

            ViewModel = new MainViewModel();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.NavigantionPanelVisible, x => x.NavigationPanel.Visibility, x => x ? Visibility.Visible : Visibility.Collapsed).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.Router, x => x.RoutedViewHost.Router).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.PopStack, x => x.BackButton).DisposeWith(disposables);
            });


        }
    }
}
