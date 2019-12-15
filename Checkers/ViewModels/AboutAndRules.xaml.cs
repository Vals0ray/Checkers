using Checkers.Models;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Checkers.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutAndRules : ContentPage
    {
        public string RulesText { get; set; }

        public string AboutText { get; set; }

        public AboutAndRules(string str = "")
        {
            InitializeComponent();
            LoadTextFromDB();
            ShowText(str);
        }

        private void LoadTextFromDB()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (var db = new ApplicationContext(dbPath))
            {
                IEnumerable<Models.TextModel> textModels = db.TextModels.ToList();
                RulesText = textModels.Last().RulesText;
                AboutText = textModels.Last().AboutText;
            }
        }

        private void ShowText(string str)
        {
            ScrollView scrollView = new ScrollView();

            Label label = new Label()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Color.White,
                FontSize = 20
            };

            if (str == "About")
            {
                label.HorizontalTextAlignment = TextAlignment.Center;
                label.Text = AboutText;
                Title = str;
            }
            else
            {
                label.Text = RulesText;
                Title = str;
            }

            scrollView.Content = label;
            Content = scrollView;
        }
    }
}