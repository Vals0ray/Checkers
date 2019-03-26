using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace Checkers
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        private async void Game_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Game());
        }

        private async void Settings_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }

        private void Exit_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}
