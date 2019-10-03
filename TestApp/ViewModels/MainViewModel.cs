using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.ViewModels
{
    public class MainViewModel : ReactiveObject, IScreen
    {
        public bool NavigantionPanelVisible => Router.NavigationStack.Count > 2;

        public RoutingState Router { get; }

        public MainViewModel()
        {
            Router = new RoutingState();
            Locator.CurrentMutable.Register<IScreen>(() => this);

            
        }
    }
}
