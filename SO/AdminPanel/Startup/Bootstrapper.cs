using AdminPanel.Pages.Main;
using Autofac;

namespace AdminPanel.Startup
{
    public class Bootstrapper 
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindow>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();

            return builder.Build();

            //builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            //builder.RegisterType<ReportFilesManager>()
            //    .As<IReportFilesManager>()
            //    .WithParameter(new TypedParameter(typeof(string), Consts.ReportsFolder));

            //builder.RegisterType<FileListViewModel>().As<IFileListViewModel>();
            //builder.RegisterType<FileDetailViewModel>().As<IFileDetailViewModel>();
        }
    }
}
