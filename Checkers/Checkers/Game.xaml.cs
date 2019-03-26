using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using static Xamarin.Forms.Grid;

namespace Checkers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Game : ContentPage
	{
        Grid grid;
        TapGestureRecognizer tap = new TapGestureRecognizer();
        List<string> board = new List<string>
        { "", "", "", "", "", "", "", "",
          "", "", "", "", "", "", "", "",
          "", "", "", "", "", "", "", "",
          "", "", "", "", "", "", "", "",
          "", "", "", "", "", "", "", "",
          "", "", "", "", "", "", "", "",
          "", "", "", "", "", "", "", "",
          "", "", "", "", "", "", "", "",};

        public Game ()
		{
			InitializeComponent ();

            CreateBoard();

            CreateTabEvents();
        }

        private void CreateBoard()
        {
            grid = new Grid
            {
                RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
            },
                ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            },
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 0,
                RowSpacing = 0
            };

            bool temp = true;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (temp)
                    {
                        grid.Children.Add(new Frame
                        {
                            BackgroundColor = Color.WhiteSmoke,
                            CornerRadius = 0,
                            Padding = 0,
                            Content = null
                        }, i, j);
                        temp = false;
                    }
                    else if (!temp && (j == 0 || j == 1 || j == 2))
                    {
                        grid.Children.Add(new Frame
                        {
                            BackgroundColor = Color.Black,
                            CornerRadius = 0,
                            Padding = 0,
                            StyleId = $"{j}{i}",
                            Content = new BoxView { BackgroundColor = Color.Red, CornerRadius = 100, Margin = 0 }
                        }, i, j);

                        temp = true;
                    }
                    else if (!temp && (j == 5 || j == 6 || j == 7))
                    {
                        grid.Children.Add(new Frame
                        {
                            BackgroundColor = Color.Black,
                            CornerRadius = 0,
                            Padding = 0,
                            StyleId = $"{j}{i}",
                            Content = new BoxView { BackgroundColor = Color.Green, CornerRadius = 100, Margin = 0 }
                        }, i, j);

                        temp = true;
                    }
                    else
                    {
                        grid.Children.Add(new Frame
                        {
                            BackgroundColor = Color.Black,
                            CornerRadius = 0,
                            Padding = 0,
                            StyleId = $"{j}{i}",
                            Content = null
                        }, i, j);

                        temp = true;
                    }

                }
                temp = temp ? false : true;
            }

            Content = grid;
        }

        private void CreateTabEvents()
        {
            List<Frame> tapFrames = new List<Frame>();
            List<View> canMove = new List<View>();
            IGridList<View> frames = grid.Children;
            int tapCount = 0;

            for (int i = 0; i < frames.Count; i++)
            {
                frames[i].GestureRecognizers.Add(tap);
            }

            tap.Tapped += (sender, e) =>
            {
                Frame frame = sender as Frame;

                if (frame.BackgroundColor != Color.WhiteSmoke)
                {                   
                    tapFrames.Add(sender as Frame);
                    tapCount++;

                    if (tapCount == 1)
                    {
                        if(frame.Content == null)
                        {
                            tapCount--;
                            return;
                        }

                        frame.BackgroundColor = Color.Yellow;

                        int pos2 = Convert.ToInt32(frame.StyleId) - 9;
                        int pos = Convert.ToInt32(frame.StyleId) - 11;
                        if (pos != 59 && pos != 39 && pos != 19 && pos > 0)
                        {
                            canMove.Add((from f in frames where f.StyleId == pos.ToString() select f).First());
                            if((canMove.Last() as Frame).Content == null)
                            {
                                frames[frames.IndexOf(canMove.Last())].BackgroundColor = Color.Yellow;
                            }
                        }

                        if (pos2 != 58 && pos2 != 38 && pos2 != 18 && pos > 0)
                        {
                            canMove.Add((from f in frames where f.StyleId == pos2.ToString() select f).First());
                            if ((canMove.Last() as Frame).Content == null)
                            {
                                frames[frames.IndexOf(canMove.Last())].BackgroundColor = Color.Yellow;
                            }
                        }
                    }

                    if (tapCount == 2)
                    {
                        foreach (var tf in tapFrames)
                        {
                            tf.BackgroundColor = Color.Black;
                        }

                        foreach (var cm in canMove)
                        {
                            cm.BackgroundColor = Color.Black;
                        }
                        

                        if (tapFrames[0].Content != null && tapFrames[1].Content == null && (tapFrames[1] == canMove[0] || tapFrames[1] == canMove[1]))
                        {
                            if (tapFrames[0].Content.BackgroundColor == Color.Red)
                            {
                                tapFrames[1].Content = new BoxView { BackgroundColor = Color.Red, CornerRadius = 100, Margin = 0 };
                            }
                            else
                            {
                                tapFrames[1].Content = new BoxView { BackgroundColor = Color.Green, CornerRadius = 100, Margin = 0 };
                            }

                            tapFrames[0].Content = null;
                        }

                        tapFrames.RemoveRange(0, tapFrames.Count);
                        canMove.RemoveRange(0, canMove.Count);
                        tapCount = 0;
                    }
                }
            };
        }
    }
}