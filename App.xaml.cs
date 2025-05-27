using CloudSync.Pages;
namespace CloudSync
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage =new NavigationPage(new AppShell());
            //MainPage=new DashboardPage();
            //MainPage = new FlyoutDemoPage();
            //MainPage = new CorouselPage();
        }
    }
}
