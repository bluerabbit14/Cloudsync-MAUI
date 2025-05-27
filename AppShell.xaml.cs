using CloudSync.Pages;
namespace CloudSync
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
            Routing.RegisterRoute(nameof(NotesPage), typeof(NotesPage));
            Routing.RegisterRoute(nameof(NoteDetailPage), typeof(NoteDetailPage));
            Routing.RegisterRoute(nameof(SignaturePage), typeof(SignaturePage));
            Routing.RegisterRoute(nameof(WeatherPage), typeof(WeatherPage));
            Routing.RegisterRoute(nameof(ConvertPage), typeof(ConvertPage));
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        }
    }
}
