using Xamarin.Forms;

namespace Checkers
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Subscribe();
            StartNewGame();
        }

        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(
                this,
                "Restart_Game", 
                async (sender) =>  { await Navigation.PushAsync(new Game()); }); 
        }

        private async void StartNewGame()
        {
            await Navigation.PushAsync(new Game());
        }
    }
}