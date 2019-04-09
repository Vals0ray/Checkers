﻿using System.Linq;
using Xamarin.Forms;
namespace Checkers
{
    public partial class Download : ContentPage
    {
        static string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
        DBModel selectedItem;
        ApplicationContext db = new ApplicationContext(dbPath);
        public Download()
        {
            InitializeComponent();
            friendsList.ItemsSource = db.DBModels.ToList();
        }
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = (DBModel)e.SelectedItem;          
        }
        private async void Load_Clicked(object sender, System.EventArgs e)
        {
            if(selectedItem != null)
            {
                await Navigation.PushAsync(new Game(selectedItem.Board, selectedItem.WhoMove));
            }
        }
        private void Delete_Clicked(object sender, System.EventArgs e)
        {
            if (selectedItem != null)
            {
                db.DBModels.Remove(selectedItem);
                db.SaveChanges();
                friendsList.ItemsSource = db.DBModels.ToList();
            }
        }
    }
}