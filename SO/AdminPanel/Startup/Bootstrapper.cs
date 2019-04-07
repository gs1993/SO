using AdminPanel.Pages.Main;
using AdminPanel.Pages.Users;
using Autofac;

namespace AdminPanel.Startup
{
    public class Bootstrapper 
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<UserNavigationViewModel>().As<IUserNavigationViewModel>();


            //builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            return builder.Build();
        }
    }
}
