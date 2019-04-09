using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Checkers
{
    public partial class App : Application
    {
        public const string DBFILENAME = "dbmodelsapp.db";
        public static NavigationPage navigationPage = new NavigationPage();

        public App()
        {
            InitializeComponent();

            //DB
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(DBFILENAME);
            using (var db = new ApplicationContext(dbPath))
            {
                db.Database.EnsureCreated();
            }
            //DB

            navigationPage  = new NavigationPage(new MainPage());
            navigationPage.BarBackgroundColor = Color.FromRgb(51, 25, 6);
            navigationPage.BarTextColor = Color.WhiteSmoke;

            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
