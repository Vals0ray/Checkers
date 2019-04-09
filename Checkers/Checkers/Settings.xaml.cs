using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Checkers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
        public string NickName { get; set; }

        public List<Color> CheckerStyle = new List<Color>();

        public List<Color> BoardStyle = new List<Color>();

        public string Country { get; set; }

        public bool Music { get; set; }

        public Settings ()
		{
			InitializeComponent ();

            image.Source = ImageSource.FromResource("Checkers.img.NoneFlag.jpg");
        }

        private async void BackButton_Click(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ChooseCheckerLeftButt_Clicked(object sender, EventArgs e)
        {
            uint timeout = 1;

            for (int i = 0; i >= -50; i -= 3)
            {
                Task t1 = Task.Run(() => FirstChecker.TranslateTo(i, 0, timeout));
                Task t2 = Task.Run(() => SecondChecker.TranslateTo(i, 0, timeout));

                await Task.WhenAll(new[] { t1, t2 });
            }

            FirstChecker.TranslationX = 50;
            SecondChecker.TranslationX = 50;

            if (FirstChecker.BackgroundColor == Color.Green)
            {
                FirstChecker.BackgroundColor = Color.White;
                SecondChecker.BackgroundColor = Color.Black;
            }
            else if(FirstChecker.BackgroundColor == Color.White)
            {
                FirstChecker.BackgroundColor = Color.Yellow;
                SecondChecker.BackgroundColor = Color.Blue;
            }
            else if (FirstChecker.BackgroundColor == Color.Yellow)
            {
                FirstChecker.BackgroundColor = Color.Green;
                SecondChecker.BackgroundColor = Color.Red;
            }

            for (int i = 50; i >= 0; i -= 3)
            {
                Task t1 = Task.Run(() => FirstChecker.TranslateTo(i, 0, timeout));
                Task t2 = Task.Run(() => SecondChecker.TranslateTo(i, 0, timeout));

                await Task.WhenAll(new[] { t1, t2 });
            }
        }

        private async void ChooseCheckerRightButt_Clicked(object sender, EventArgs e)
        {
            uint timeout = 1;

            for (int i = 0; i <= 50; i += 3)
            {
                Task t1 = Task.Run(() => FirstChecker.TranslateTo(i, 0, timeout));
                Task t2 = Task.Run(() => SecondChecker.TranslateTo(i, 0, timeout));

                await Task.WhenAll(new[] { t1, t2 });
            }

            FirstChecker.TranslationX = -50;
            SecondChecker.TranslationX = -50;

            if (FirstChecker.BackgroundColor == Color.Green)
            {
                FirstChecker.BackgroundColor = Color.Yellow;
                SecondChecker.BackgroundColor = Color.Blue;
            }
            else if (FirstChecker.BackgroundColor == Color.Yellow)
            {
                FirstChecker.BackgroundColor = Color.White;
                SecondChecker.BackgroundColor = Color.Black;
            }
            else if (FirstChecker.BackgroundColor == Color.White)
            {
                FirstChecker.BackgroundColor = Color.Green;
                SecondChecker.BackgroundColor = Color.Red;
            }

            for (int i = -50; i <= 0; i += 3)
            {
                Task t1 = Task.Run(() => FirstChecker.TranslateTo(i, 0, timeout));
                Task t2 = Task.Run(() => SecondChecker.TranslateTo(i, 0, timeout));

                await Task.WhenAll(new[] { t1, t2 });
            }
        }

        private async void ChooseBoardLeftButt_Clicked(object sender, EventArgs e)
        {
            uint timeout = 1;

            for (int i = 0; i >= -25; i -= 3)
            {
                Task t1 = Task.Run(() => BoardFirst.TranslateTo(i, 0, timeout));
                Task t2 = Task.Run(() => BoardSecond.TranslateTo(i, 0, timeout));
                Task t3 = Task.Run(() => BoardFirst2.TranslateTo(i, 0, timeout));
                Task t4 = Task.Run(() => BoardSecond2.TranslateTo(i, 0, timeout));

                await Task.WhenAll(new[] { t1, t2, t3, t4 });
            }
            BoardFirst.TranslationX = 25;
            BoardSecond.TranslationX = 25;
            BoardFirst2.TranslationX = 25;
            BoardSecond2.TranslationX = 25;

            if (BoardFirst.BackgroundColor == Color.WhiteSmoke && BoardSecond.BackgroundColor == Color.Black)
            {
                BoardFirst.BackgroundColor = Color.Orange;
                BoardSecond.BackgroundColor = Color.SaddleBrown;
                BoardFirst2.BackgroundColor = Color.SaddleBrown;
                BoardSecond2.BackgroundColor = Color.Orange;
            }
            else if (BoardFirst.BackgroundColor == Color.Orange && BoardSecond.BackgroundColor == Color.SaddleBrown)
            {
                BoardFirst.BackgroundColor = Color.WhiteSmoke;
                BoardSecond.BackgroundColor = Color.Black;
                BoardFirst2.BackgroundColor = Color.Black;
                BoardSecond2.BackgroundColor = Color.WhiteSmoke;
            }

            for (int i = 25; i >= 0; i -= 3)
            {
                Task t1 = Task.Run(() => BoardFirst.TranslateTo(i, 0, timeout));
                Task t2 = Task.Run(() => BoardSecond.TranslateTo(i, 0, timeout));
                Task t3 = Task.Run(() => BoardFirst2.TranslateTo(i, 0, timeout));
                Task t4 = Task.Run(() => BoardSecond2.TranslateTo(i, 0, timeout));

                await Task.WhenAll(new[] { t1, t2, t3, t4 });
            }
        }

        private async void ChooseBoardRightButt_Clicked(object sender, EventArgs e)
        {
            uint timeout = 1;

            for (int i = 0; i <= 25; i += 3)
            {
                Task t1 = Task.Run(() => BoardFirst.TranslateTo(i, 0, timeout));
                Task t2 = Task.Run(() => BoardSecond.TranslateTo(i, 0, timeout));
                Task t3 = Task.Run(() => BoardFirst2.TranslateTo(i, 0, timeout));
                Task t4 = Task.Run(() => BoardSecond2.TranslateTo(i, 0, timeout));

                await Task.WhenAll(new[] { t1, t2, t3, t4 });
            }

            BoardFirst.TranslationX = -25;
            BoardSecond.TranslationX = -25;
            BoardFirst2.TranslationX = -25;
            BoardSecond2.TranslationX = -25;

            if (BoardFirst.BackgroundColor == Color.WhiteSmoke && BoardSecond.BackgroundColor == Color.Black)
            {
                BoardFirst.BackgroundColor = Color.Orange;
                BoardSecond.BackgroundColor = Color.SaddleBrown;
                BoardFirst2.BackgroundColor = Color.SaddleBrown;
                BoardSecond2.BackgroundColor = Color.Orange;
            }
            else if (BoardFirst.BackgroundColor == Color.Orange && BoardSecond.BackgroundColor == Color.SaddleBrown)
            {
                BoardFirst.BackgroundColor = Color.WhiteSmoke;
                BoardSecond.BackgroundColor = Color.Black;
                BoardFirst2.BackgroundColor = Color.Black;
                BoardSecond2.BackgroundColor = Color.WhiteSmoke;
            }

            for (int i = -25; i <= 0; i += 3)
            {
                Task t1 = Task.Run(() => BoardFirst.TranslateTo(i, 0, timeout));
                Task t2 = Task.Run(() => BoardSecond.TranslateTo(i, 0, timeout));
                Task t3 = Task.Run(() => BoardFirst2.TranslateTo(i, 0, timeout));
                Task t4 = Task.Run(() => BoardSecond2.TranslateTo(i, 0, timeout));

                await Task.WhenAll(new[] { t1, t2, t3, t4 });
            }
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(picker.Items[picker.SelectedIndex] == "Ukraine")
            {
                image.Source = ImageSource.FromResource("Checkers.img.UkraineFlag.jpg");
            }
            else if (picker.Items[picker.SelectedIndex] == "Belarus")
            {
                image.Source = ImageSource.FromResource("Checkers.img.BelarusFlag.jpg");
            }
            else if (picker.Items[picker.SelectedIndex] == "Moldova")
            {
                image.Source = ImageSource.FromResource("Checkers.img.MoldovaFlag.jpg");
            }
            else if (picker.Items[picker.SelectedIndex] == "Poland")
            {
                image.Source = ImageSource.FromResource("Checkers.img.PolandFlag.jpg");
            }
        }

        private async void SubmitBtm_Clicked(object sender, EventArgs e)
        {
            if(NName.Text == null)
            {
                uint timeout = 50;
                await NName.TranslateTo(-20, 0, timeout);
                await NName.TranslateTo(20, 0, timeout);
                await NName.TranslateTo(-15, 0, timeout);
                await NName.TranslateTo(15, 0, timeout);
                await NName.TranslateTo(-10, 0, timeout);
                await NName.TranslateTo(10, 0, timeout);
                await NName.TranslateTo(-5, 0, timeout);
                await NName.TranslateTo(5, 0, timeout);
                NName.TranslationX = 0;
                return;
            }
            else
            {
                NickName = NName.Text;
            }

            CheckerStyle.Add(FirstChecker.BackgroundColor);
            CheckerStyle.Add(SecondChecker.BackgroundColor);

            BoardStyle.Add(BoardFirst.BackgroundColor);
            BoardStyle.Add(BoardSecond.BackgroundColor);

            Country = picker.SelectedIndex == -1 ? "None" : picker.Items[picker.SelectedIndex];

            Music = @switch.IsEnabled;

            BackButton_Click(null, null);
        }
    }
}