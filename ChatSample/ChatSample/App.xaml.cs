using Prism;
using Prism.Ioc;
using ChatSample.ViewModels;
using ChatSample.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ChatSample.Services;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ChatSample
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
        }

        protected override async void OnResume()
        {
            base.OnResume();

            try
            {
                await Container.Resolve<IMessageService>().StartAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        protected override async void OnSleep()
        {
            base.OnSleep();

            try
            {
                await Container.Resolve<IMessageService>().StopAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }
}
