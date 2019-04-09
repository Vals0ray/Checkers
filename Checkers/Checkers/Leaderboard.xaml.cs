using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace Checkers
{
    public partial class Leaderboard : ContentPage
    {
        public Leaderboard()
        {
            InitializeComponent();

            Grid grid = new Grid
            {
                RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
            },
                ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            },
                ColumnSpacing = 0,
                RowSpacing = 0,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            grid.Children.Add(new Frame()
            {
                BackgroundColor = Color.Yellow,
                Content = new Image()
                { Source = ImageSource.FromResource("Checkers.img.Crown.jpg") }
            }, 0, 0);

            Content = grid;
        }
    }
}
