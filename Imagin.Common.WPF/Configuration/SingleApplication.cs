using Imagin.Common.Analytics;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Imagin.Common.Configuration
{
    public delegate void UnhandledExceptionDelegate(object sender, EventArgs<Error> e);

    public abstract class SingleApplication : Application, IApp, ISingleApplication
    {
        public event UnhandledExceptionDelegate ExceptionUnhandled;

        public event EventHandler<EventArgs<IEnumerable<string>>> Reopened;

        //............................................................................................................

        public abstract DataProperties Data { get; }

        //............................................................................................................

        public abstract string Name { get; }

        public readonly string Publisher = nameof(Imagin);

        //............................................................................................................

        IMainViewModel mainViewModel;
        public abstract Func<IMainViewModel> MainViewModel { get; }

        //............................................................................................................

        public SingleApplication() : base()
        {
            Get.New(GetType(), this);
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandled;
            DispatcherUnhandledException += OnDispatcherUnhandled;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskUnhandled;
        }

        //............................................................................................................

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var options = Configuration.Data.Load(Data);
            Get.New(options.GetType(), options);
            options.Changed(() => options.Language);
            options.Changed(() => options.Theme);
            options.OnApplicationStart();

            mainViewModel = MainViewModel();
            options.OnApplicationReady();
        }

        //............................................................................................................

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Get.Where<Data>().OnApplicationExit();
            Get.Where<Data>().Save();
        }

        //............................................................................................................

        protected virtual void OnCurrentDomainUnhandled(object sender, UnhandledExceptionEventArgs e)
        {
            OnExceptionUnhandled(e.ExceptionObject as Exception);
        }

        protected virtual void OnDispatcherUnhandled(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
#if DEBUG  
            e.Handled = false;
#else
            e.Handled = true;
#endif     
            OnExceptionUnhandled(e.Exception);
        }

        protected virtual void OnUnobservedTaskUnhandled(object sender, UnobservedTaskExceptionEventArgs e)
        {
#if DEBUG  

#else
            e.SetObserved();
#endif     
            OnExceptionUnhandled(e.Exception);
        }

        //............................................................................................................

        protected virtual void OnExceptionUnhandled(Exception e)
        {
            ExceptionUnhandled?.Invoke(this, new EventArgs<Error>(new Error(e)));
        }

        //............................................................................................................

        public virtual bool OnReopened(IList<string> arguments)
        {
            if (MainWindow.CanMaximize())
            {
                MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                MainWindow.WindowState = WindowState.Normal;
                MainWindow.Center();
            }

            MainWindow.Activate();

            if (arguments?.Count > 0)
                arguments.RemoveAt(0);

            Reopened?.Invoke(this, new EventArgs<IEnumerable<string>>(arguments));
            return true;
        }
    }
}