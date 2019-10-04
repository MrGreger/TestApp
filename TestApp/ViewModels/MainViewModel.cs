using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Views;

namespace TestApp.ViewModels
{
    public class MainViewModel : ReactiveObject, IScreen
    {
        private bool _navigantionPanelVisible;

        public bool NavigantionPanelVisible
        {
            get => _navigantionPanelVisible;
            protected set => this.RaiseAndSetIfChanged(ref _navigantionPanelVisible, value);
        }

        public RoutingState Router { get; }

        public ReactiveCommand<Unit,Unit> PopStack { get; }

        public MainViewModel()
        {
            Router = new RoutingState();

            Locator.CurrentMutable.Register<IScreen>(() => this);
            Locator.CurrentMutable.Register(() => new UsersView(), typeof(IViewFor<UsersListViewModel>));
            Locator.CurrentMutable.Register(() => new UserCreate(), typeof(IViewFor<UserCreateViewModel>));

            Router.NavigationChanged.Subscribe(x => NavigantionPanelVisible = Router.NavigationStack.Count > 1);

            PopStack = ReactiveCommand.CreateFromObservable(() => Router.NavigateBack.Execute());

            Router.Navigate.Execute(new UsersListViewModel(this));
        }
    }
}
