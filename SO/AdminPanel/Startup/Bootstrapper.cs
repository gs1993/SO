using AdminPanel.Utils;
using AdminPanel.ViewModels;
using AdminPanel.Views;
using Autofac;
using Prism.Events;

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
            builder.RegisterType<UserDetailViewModel>().As<IUserDetailViewModel>();

            builder.RegisterType<MessageService>().As<IMessageService>();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            return builder.Build();
        }
    }
}
