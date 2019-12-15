using Checkers.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Checkers
{
    public partial class App : Application
    {
        public static Game newGame;
        public const string DBFILENAME = "dbmodelsapp.db";
        public static NavigationPage navigationPage;

        public App()
        {
            InitializeComponent();

            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (var db = new ApplicationContext(dbPath))
            {
                db.Database.EnsureCreated();
            }

            newGame = new Game();
            navigationPage = new NavigationPage(newGame);
            CreateNavigationPage();

            MainPage = navigationPage;
        }

        public static void CreateNavigationPage()
        {
            navigationPage.BarBackgroundColor = Color.FromRgb(51, 25, 6);
            navigationPage.BarTextColor = Color.WhiteSmoke;
            var save = new ToolbarItem
            {
                Icon = "Save.png"
            };

            var restart = new ToolbarItem
            {
                Icon = "Restart.png"
            };

            var download = new ToolbarItem
            {
                Icon = "download.png"
            };

            var firstMenuItem = new ToolbarItem
            {
                Order = ToolbarItemOrder.Secondary,
                Text = "Rules"
            };
            var secondMenuItem = new ToolbarItem
            {
                Order = ToolbarItemOrder.Secondary,
                Text = "About"
            };


            if (navigationPage.ToolbarItems.Count == 0)
            {
                App.navigationPage.ToolbarItems.Add(save);
                App.navigationPage.ToolbarItems.Add(restart);
                App.navigationPage.ToolbarItems.Add(download);
                App.navigationPage.ToolbarItems.Add(firstMenuItem);
                App.navigationPage.ToolbarItems.Add(secondMenuItem);
            }

            navigationPage.ToolbarItems[4].Clicked += (object sender, EventArgs e) =>
            {
                Views.AboutAndRules aboutAndRules = new Views.AboutAndRules("About");
                navigationPage.Navigation.PushAsync(aboutAndRules);
                navigationPage.ToolbarItems.Clear();
                navigationPage.Popped += (object sender2, NavigationEventArgs e2) =>
                {
                    if (navigationPage.RootPage.ToolbarItems.Count == 0)
                    {
                        navigationPage.RootPage.ToolbarItems.Add(save);
                        navigationPage.RootPage.ToolbarItems.Add(restart);
                        navigationPage.RootPage.ToolbarItems.Add(download);
                        navigationPage.RootPage.ToolbarItems.Add(firstMenuItem);
                        navigationPage.RootPage.ToolbarItems.Add(secondMenuItem);
                    }
                };
            };

            navigationPage.ToolbarItems[3].Clicked += (object sender, EventArgs e) =>
            {
                Views.AboutAndRules aboutAndRules = new Views.AboutAndRules("Rules");
                navigationPage.Navigation.PushAsync(aboutAndRules);
                navigationPage.ToolbarItems.Clear();
                navigationPage.Popped += (object sender2, NavigationEventArgs e2) =>
                {
                    if (navigationPage.RootPage.ToolbarItems.Count == 0)
                    {
                        navigationPage.RootPage.ToolbarItems.Add(save);
                        navigationPage.RootPage.ToolbarItems.Add(restart);
                        navigationPage.RootPage.ToolbarItems.Add(download);
                        navigationPage.RootPage.ToolbarItems.Add(firstMenuItem);
                        navigationPage.RootPage.ToolbarItems.Add(secondMenuItem);
                    }
                };
            };

            App.navigationPage.ToolbarItems[2].Clicked += (object sender, EventArgs e) =>
            {
                Download download2 = new Download();
                navigationPage.Navigation.PushAsync(download2);
                navigationPage.ToolbarItems.Clear();
                navigationPage.Popped += (object sender2, NavigationEventArgs e2) => 
                {
                    if(navigationPage.RootPage.ToolbarItems.Count == 0)
                    {
                        navigationPage.RootPage.ToolbarItems.Add(save);
                        navigationPage.RootPage.ToolbarItems.Add(restart);
                        navigationPage.RootPage.ToolbarItems.Add(download);
                        navigationPage.RootPage.ToolbarItems.Add(firstMenuItem);
                        navigationPage.RootPage.ToolbarItems.Add(secondMenuItem);
                    }
                };
            };

            App.navigationPage.ToolbarItems[1].Clicked += async (object sender, EventArgs e) =>
            {
                bool answer = await Current.MainPage.DisplayAlert("Restart the game", "Would you like to restart this game?", "Yes", "No");
                if (answer)
                {
                    newGame = new Game();
                    await navigationPage.Navigation.PushAsync(newGame);
                    navigationPage.ToolbarItems.Clear();
                    navigationPage.Navigation.RemovePage(navigationPage.RootPage);
                    if (navigationPage.RootPage.ToolbarItems.Count == 0)
                    {
                        navigationPage.RootPage.ToolbarItems.Add(save);
                        navigationPage.RootPage.ToolbarItems.Add(restart);
                        navigationPage.RootPage.ToolbarItems.Add(download);
                        navigationPage.RootPage.ToolbarItems.Add(firstMenuItem);
                        navigationPage.RootPage.ToolbarItems.Add(secondMenuItem);
                    }
                }
            };

            App.navigationPage.ToolbarItems[0].Clicked += async (object sender, EventArgs e) =>
            {
                bool answer = await Current.MainPage.DisplayAlert("Save the game", "Would you like to save this game?", "Yes", "No");
                if (answer)
                {
                    newGame.SaveGame();
                }
            };
        }

        protected override void OnStart()  { }

        protected override void OnSleep()  { }

        protected override void OnResume() { }
    }
}