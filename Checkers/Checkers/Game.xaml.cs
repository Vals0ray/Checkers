using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Forms.Grid;

namespace Checkers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Game : ContentPage
	{
        Grid grid;
        double wigth = 0;
        double height = 0;
        IGridList<View> frames;
        Label beatGreenCount;
        Label beatRedCount;
        Color PlayerColor { get; set; }
        List<MoveInfo> moveInfos = new List<MoveInfo>();
        bool mustBeat = false;
        List<Frame> tapFrames = new List<Frame>();
        List<Frame> tapFrames2 = new List<Frame>();
        List<View> canMove = new List<View>();
        MoveInfo firstTapToBeat = new MoveInfo();
        string dbPath;
        int tapCount = 0;

        public Game ()
		{
			InitializeComponent ();

            PlayerColor = Color.Green;

            Title = "Green's turn";

            CreateBoard();

            CreateInterface();

            frames = grid.Children;
            wigth = grid.Width / 8;
            height = grid.Height / 8;

            CreateTabEvents();

            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
        }

        public Game(string str, string whoMove)
        {
            InitializeComponent();

            PlayerColor = whoMove == "Green's turn" ? Color.Green : Color.Red;

            Title = whoMove;

            CreateBoard(str);

            CreateInterface();

            frames = grid.Children;
            wigth = grid.Width / 8;
            height = grid.Height / 8;

            CreateTabEvents();

            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
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
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = 40 }
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
                    else if (!temp && (j == 0|| j == 1 || j == 2))
                    {
                        grid.Children.Add(new Frame
                        {
                            BackgroundColor = Color.Black,
                            CornerRadius = 0,
                            Padding = 0,
                            StyleId = $"{(j == 0 ? i.ToString() : (j.ToString() + i.ToString()))}",
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
                            StyleId = $"{(j == 0 ? i.ToString() : (j.ToString() + i.ToString()))}",
                            Content = new BoxView { BackgroundColor = Color.Green, CornerRadius = 100, Margin = 0}
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
                            StyleId = $"{(j == 0 ? i.ToString() : (j.ToString() + i.ToString()))}",
                            Content = null
                        }, i, j);

                        temp = true;
                    }

                }
                temp = temp ? false : true;
            } 
        }

        private void CreateBoard(string str)
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
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = 40 },
                new ColumnDefinition { Width = 40 }
            },
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 0,
                RowSpacing = 0
            };
            int iter = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (str[iter] == 'W')
                    {
                        grid.Children.Add(new Frame
                        {
                            BackgroundColor = Color.WhiteSmoke,
                            CornerRadius = 0,
                            Padding = 0,
                            Content = null
                        }, i, j);
                    }
                    else if (str[iter] == 'R')
                    {
                        grid.Children.Add(new Frame
                        {
                            BackgroundColor = Color.Black,
                            CornerRadius = 0,
                            Padding = 0,
                            StyleId = $"{(j == 0 ? i.ToString() : (j.ToString() + i.ToString()))}",
                            Content = new BoxView { BackgroundColor = Color.Red, CornerRadius = 100, Margin = 0 }

                        }, i, j);
                    }
                    else if (str[iter] == 'G')
                    {
                        grid.Children.Add(new Frame
                        {
                            BackgroundColor = Color.Black,
                            CornerRadius = 0,
                            Padding = 0,
                            StyleId = $"{(j == 0 ? i.ToString() : (j.ToString() + i.ToString()))}",
                            Content = new BoxView { BackgroundColor = Color.Green, CornerRadius = 100, Margin = 0 }
                        }, i, j);
                    }
                    else
                    {
                        grid.Children.Add(new Frame
                        {
                            BackgroundColor = Color.Black,
                            CornerRadius = 0,
                            Padding = 0,
                            StyleId = $"{(j == 0 ? i.ToString() : (j.ToString() + i.ToString()))}",
                            Content = null
                        }, i, j);
                    }
                    iter++;
                }
            }
        }

        private void CreateInterface()
        {
            StackLayout mainStack = new StackLayout()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 10
            };

            StackLayout borderStack = new StackLayout()
            {
                BackgroundColor = Color.FromRgb(28, 13, 3),
                Padding = 2,
                Spacing = 0
            };

            StackLayout borderStack2 = new StackLayout()
            {
                BackgroundColor = Color.SaddleBrown,
                Padding = 5,
                Spacing = 0
            };

            StackLayout borderStack3 = new StackLayout()
            {
                BackgroundColor = Color.Black,
                Padding = 2,
                Spacing = 0
            };

            StackLayout statsRedStack = new StackLayout() { Orientation = StackOrientation.Horizontal };
            StackLayout statsGreenStack = new StackLayout() { Orientation = StackOrientation.Horizontal };

            beatGreenCount = new Label()
            {
                Text = "0",
                FontSize = 24,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center
            };

            beatRedCount = new Label()
            {
                Text = "0",
                FontSize = 24,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center
            };

            BoxView firstPlayer = new BoxView() { BackgroundColor = Color.Green, CornerRadius = 100, Margin = 0, HorizontalOptions = LayoutOptions.Start };

            BoxView secondPlayer = new BoxView() { BackgroundColor = Color.Red, CornerRadius = 100, Margin = 0, HorizontalOptions = LayoutOptions.Start };

            statsRedStack.Children.Add(firstPlayer);
            statsRedStack.Children.Add(new Label() { Text = " x", FontSize = 24, TextColor = Color.White, VerticalOptions = LayoutOptions.Center });
            statsRedStack.Children.Add(beatGreenCount);
            mainStack.Children.Add(statsRedStack);

            borderStack.Children.Add(borderStack2);
            borderStack2.Children.Add(borderStack3);
            borderStack3.Children.Add(grid);
            mainStack.Children.Add(borderStack);

            statsGreenStack.Children.Add(secondPlayer);
            statsGreenStack.Children.Add(new Label() { Text = " x", FontSize = 24, TextColor = Color.White, VerticalOptions = LayoutOptions.Center });
            statsGreenStack.Children.Add(beatRedCount);
            mainStack.Children.Add(statsGreenStack);

            Content = mainStack;
        }

        private void SaveGame()
        {
            using (var db = new ApplicationContext(dbPath))
            {
                int count = db.DBModels.Count() + 1;
                string str = "Save: #" + count;
                string saveBoard = "";
                foreach(Frame f in frames)
                {
                    if (f.Content == null && f.BackgroundColor == Color.WhiteSmoke)
                    {
                        saveBoard += "W";
                    }
                    else if (f.Content == null && (f.BackgroundColor == Color.Black || 
                        (f.BackgroundColor == Color.YellowGreen && f.Content == null)))
                    {
                        saveBoard += "B";
                    }
                    else if(f.Content.BackgroundColor == Color.Red)
                    {
                        saveBoard += "R";
                    }
                    else if(f.Content.BackgroundColor == Color.Green)
                    {
                        saveBoard += "G";
                    }
                }
                db.DBModels.Add(new DBModel
                { Name = str, Date = DateTime.Now, Board = saveBoard, WhoMove = Title });
                db.SaveChanges();
            }
        }

        private void CreateTabEvents()
        {
            var save = new ToolbarItem
            {
                Icon = "Save.png"
            };

            App.navigationPage.ToolbarItems.Add(save);

            App.navigationPage.ToolbarItems[0].Clicked += async (object sender, EventArgs e) => 
            {
                bool answer = await DisplayAlert("Save the game", "Would you like to save this game?", "Yes", "No");
                if (answer)
                {
                    SaveGame();
                }
            };

            App.navigationPage.Popped += (object sender, NavigationEventArgs e) =>
            {
                App.navigationPage.ToolbarItems.Remove(save);
            };

            TapGestureRecognizer tap = new TapGestureRecognizer();

            for (int i = 0; i < frames.Count; i++)
            {
                frames[i].GestureRecognizers.Add(tap);
            }

            tap.Tapped += Tap_Tapped;
        }

        private void Tap_Tapped(object sender, EventArgs e)
        {
            Frame frame = sender as Frame;

            if (frame.BackgroundColor != Color.WhiteSmoke)
            {
                tapFrames.Add(sender as Frame);
                tapCount++;

                if (tapCount == 1 && !mustBeat)
                {
                    int pos = Convert.ToInt32(frame.StyleId);

                    if (frame.Content == null)
                    {
                        tapCount--;
                        tapFrames.RemoveRange(0, tapFrames.Count);
                        return;
                    }

                    if (PlayerColor == frame.Content.BackgroundColor)
                    {
                        pos = PlayerColor == Color.Green ? pos -= 11 : pos += 9;
                        if (pos != 59 && pos != 39 && pos != 19 && pos > 0 && pos < 77)
                        {
                            canMove.Add((from f in frames where f.StyleId == pos.ToString() select f).First());
                            if ((canMove.Last() as Frame).Content == null)
                            {
                                frames[frames.IndexOf(canMove.Last())].BackgroundColor = Color.GreenYellow;
                            }
                        }

                        pos = Convert.ToInt32(frame.StyleId);
                        pos = PlayerColor == Color.Green ? pos -= 9 : pos += 11;
                        if (pos != 58 && pos != 38 && pos != 18 && pos > 0 && pos < 77)
                        {
                            canMove.Add((from f in frames where f.StyleId == pos.ToString() select f).First());
                            if ((canMove.Last() as Frame).Content == null)
                            {
                                frames[frames.IndexOf(canMove.Last())].BackgroundColor = Color.GreenYellow;
                            }
                        }
                    }
                    else if(frame.Content.StyleId == "GreenQueen" && Title.Contains("Green") 
                         || frame.Content.StyleId == "RedQueen" && Title.Contains("Red"))
                    {
                        //1
                        bool progress = true;
                        pos = Convert.ToInt32(frame.StyleId) + 9;
                        while (progress)
                        {
                            if (pos != 59 && pos != 39 && pos != 19 && pos > 0 && pos < 77)
                            {
                                canMove.Add((from f in frames where f.StyleId == pos.ToString() select f).First());

                                if ((canMove.Last() as Frame).Content == null)
                                {
                                    frames[frames.IndexOf(canMove.Last())].BackgroundColor = Color.GreenYellow;
                                    pos += 9;
                                }
                                else
                                {
                                    progress = false;
                                }
                            }
                            else
                            {
                                progress = false;
                            }
                        }

                        //2
                        progress = true;
                        pos = Convert.ToInt32(frame.StyleId) + 11;
                        while (progress)
                        {
                            if (pos != 58 && pos != 38 && pos != 18 && pos > 0 && pos < 77 && pos > 0 && pos < 77)
                            {
                                canMove.Add((from f in frames where f.StyleId == pos.ToString() select f).First());

                                if ((canMove.Last() as Frame).Content == null)
                                {
                                    frames[frames.IndexOf(canMove.Last())].BackgroundColor = Color.GreenYellow;
                                    pos += 11;
                                }
                                else
                                {
                                    progress = false;
                                }
                            }
                            else
                            {
                                progress = false;
                            }
                        }

                        //3
                        progress = true;
                        pos = Convert.ToInt32(frame.StyleId) - 11;
                        while (progress)
                        {
                            if (pos != 59 && pos != 39 && pos != 19 && pos > 0 && pos < 77)
                            {
                                canMove.Add((from f in frames where f.StyleId == pos.ToString() select f).First());

                                if ((canMove.Last() as Frame).Content == null)
                                {
                                    frames[frames.IndexOf(canMove.Last())].BackgroundColor = Color.GreenYellow;
                                    pos -= 11;
                                }
                                else
                                {
                                    progress = false;
                                }
                            }
                            else
                            {
                                progress = false;
                            }
                        }

                        //4
                        progress = true;
                        pos = Convert.ToInt32(frame.StyleId) - 9;
                        while (progress)
                        {
                            if (pos != 58 && pos != 38 && pos != 18 && pos > 0 && pos < 77 && pos > 0 && pos < 77)
                            {
                                canMove.Add((from f in frames where f.StyleId == pos.ToString() select f).First());

                                if ((canMove.Last() as Frame).Content == null)
                                {
                                    frames[frames.IndexOf(canMove.Last())].BackgroundColor = Color.GreenYellow;
                                    pos -= 9;
                                }
                                else
                                {
                                    progress = false;
                                }
                            }
                            else
                            {
                                progress = false;
                            }
                        }
                    }
                    else
                    {
                        tapCount--;
                        tapFrames.RemoveRange(0, tapFrames.Count);
                        return;
                    }

                    frame.BackgroundColor = Color.Yellow;
                }

                if (tapCount == 2 && !mustBeat)
                {
                    foreach (var tf in tapFrames)
                    {
                        tf.BackgroundColor = Color.Black;
                    }

                    foreach (var cm in canMove)
                    {
                        cm.BackgroundColor = Color.Black;
                    }

                    if (canMove.Count == 1)
                    {
                        canMove.Add(null);
                    }

                    if (canMove.Count == 0)
                    {
                        canMove.Add(null);
                        canMove.Add(null);
                    }

                    if (tapFrames[0].Content != null && tapFrames[1].Content == null && canMove.Contains(tapFrames[1]))
                    {
                        if (tapFrames[0].Content.BackgroundColor == Color.Red)
                        {
                            tapFrames[1].Content = new BoxView { BackgroundColor = Color.Red, CornerRadius = 100, Margin = 0 };
                            PlayerColor = Color.Green;
                            Title = "Green's turn";
                        }
                        else if (tapFrames[0].Content.BackgroundColor == Color.Green)
                        {
                            tapFrames[1].Content = new BoxView { BackgroundColor = Color.Green, CornerRadius = 100, Margin = 0 };
                            PlayerColor = Color.Red;
                            Title = "Red's turn";
                        }
                        else if (tapFrames.First().Content.StyleId == "RedQueen")
                        {
                            tapFrames[1].Content = new Image()
                            { Source = ImageSource.FromResource("Checkers.img.RedQueen.png"), WidthRequest = wigth, HeightRequest = height, StyleId = "RedQueen" };
                            PlayerColor = Color.Green;
                            Title = "Green's turn";
                        }
                        else if (tapFrames.First().Content.StyleId == "GreenQueen")
                        {
                            tapFrames[1].Content = new Image()
                            { Source = ImageSource.FromResource("Checkers.img.GreenQueen.png"), WidthRequest = wigth, HeightRequest = height, StyleId = "GreenQueen" };
                            PlayerColor = Color.Red;
                            Title = "Red's turn";
                        }


                        tapFrames[0].Content = null;
                    }

                    CheackForBeating();
                    CheackForQueen();

                    tapFrames.RemoveRange(0, tapFrames.Count);
                    canMove.RemoveRange(0, canMove.Count);
                    tapCount = 0;
                }

                if (tapCount == 1 && mustBeat)
                {
                    if (frame.Content == null)
                    {
                        tapCount = 0;
                        return;
                    }
                    if (frame.Content.BackgroundColor == PlayerColor)
                    {
                        foreach (MoveInfo move in moveInfos)
                        {
                            if (move.whoBeat == frame)
                            {
                                move.whoBeat.BackgroundColor = Color.Yellow;

                                firstTapToBeat = move;
                            }
                            else
                            {
                                move.whoBeat.BackgroundColor = Color.Black;

                                foreach (var mCan in move.canBeat)
                                {
                                    if (!moveInfos.Any(m => m.canBeat.Contains(mCan) && m != move))
                                    {
                                        mCan.BackgroundColor = Color.Black;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        tapCount = 0;
                    }
                }

                if (tapCount == 2 && mustBeat)
                {
                    if (firstTapToBeat.canBeat.Contains(frame))
                    {
                        firstTapToBeat.whoBeat.Content = null;
                        firstTapToBeat.whomBeat[firstTapToBeat.canBeat.IndexOf(frame)].Content = null;
                        firstTapToBeat.whoBeat.BackgroundColor = Color.Black;
                        firstTapToBeat.canBeat.ForEach(m => m.BackgroundColor = Color.Black);

                        if (PlayerColor == Color.Green)
                        {
                            frame.Content = new BoxView { BackgroundColor = Color.Green, CornerRadius = 100, Margin = 0 };
                            beatRedCount.Text = (Convert.ToInt32(beatRedCount.Text) + 1).ToString();
                        }
                        else
                        {
                            frame.Content = new BoxView { BackgroundColor = Color.Red, CornerRadius = 100, Margin = 0 };
                            beatGreenCount.Text = (Convert.ToInt32(beatGreenCount.Text) + 1).ToString();
                        }

                        tapFrames.RemoveRange(0, tapFrames.Count);
                        canMove.RemoveRange(0, canMove.Count);
                        moveInfos.RemoveRange(0, moveInfos.Count);

                        CheackForBeating();

                        if (moveInfos.Count == 0)
                        {
                            if (PlayerColor == Color.Green)
                            {
                                PlayerColor = Color.Red;
                                Title = "Red's turn";
                            }
                            else
                            {
                                PlayerColor = Color.Green;
                                Title = "Green's turn";
                            }

                            mustBeat = false;
                            moveInfos.RemoveRange(0, moveInfos.Count);

                            CheackForBeating();
                        }

                        tapCount = 0;
                    }
                    else
                    {
                        tapCount = 1;
                    }
                }
            }
        }

        private void CheackForBeating()
        {
            MoveInfo move = new MoveInfo();
            IEnumerable<Frame> greensPos;
            IEnumerable<Frame> redsPos;
            int pos;

            CheackForQueen();

            if (PlayerColor == Color.Green)
            {
                greensPos = from f in frames
                            where (f as Frame).Content != null && (f as Frame).Content.BackgroundColor == Color.Green
                            select f as Frame;

                redsPos = from f in frames
                        where (f as Frame).Content != null && (f as Frame).Content.BackgroundColor == Color.Red
                        select f as Frame;
                //IEnumerable<Frame> greensPos = frames.Where(f => (f as Frame).Content != null && (f as Frame).Content.BackgroundColor == Color.Green) as IEnumerable<Frame>;
                //IEnumerable<Frame> redsPos = frames.Where(f => (f as Frame).Content != null && (f as Frame).Content.BackgroundColor == Color.Red) as IEnumerable<Frame>;
            }
            else
            {
                greensPos = from f in frames
                            where (f as Frame).Content != null && (f as Frame).Content.BackgroundColor == Color.Red
                            select f as Frame;

                redsPos = from f in frames
                        where (f as Frame).Content != null && (f as Frame).Content.BackgroundColor == Color.Green
                        select f as Frame;
            }

            foreach (Frame gP in greensPos)
            {
                foreach (Frame rP in redsPos)
                {
                    pos = Convert.ToInt32(rP.StyleId);

                    bool toBeatContition = pos > 10 && pos < 67 && pos != 30 && pos != 50 && pos != 27 && pos != 47;

                    if (Convert.ToInt32(gP.StyleId) - 11 == pos && toBeatContition)
                    {
                        Frame f = frames.Where(fr => Convert.ToInt32((fr as Frame).StyleId) == Convert.ToInt32(rP.StyleId) - 11).First() as Frame;
                        BeatsAdd(f, gP, rP, move);
                    }

                    if (Convert.ToInt32(gP.StyleId) + 9 == pos && toBeatContition)
                    {
                        Frame f = frames.Where(fr => Convert.ToInt32((fr as Frame).StyleId) == Convert.ToInt32(rP.StyleId) + 9).First() as Frame;
                        BeatsAdd(f, gP, rP, move);
                    }

                    if (Convert.ToInt32(gP.StyleId) - 9 == pos && toBeatContition)
                    {
                        Frame f = frames.Where(fr => Convert.ToInt32((fr as Frame).StyleId) == Convert.ToInt32(rP.StyleId) - 9).First() as Frame;
                        BeatsAdd(f, gP, rP, move);
                    }

                    if (Convert.ToInt32(gP.StyleId) + 11 == pos && toBeatContition)
                    {
                        Frame f = frames.Where(fr => Convert.ToInt32((fr as Frame).StyleId) == Convert.ToInt32(rP.StyleId) + 11).First() as Frame;
                        BeatsAdd(f, gP, rP, move);
                    }
                }
            }
        }

        private void BeatsAdd(Frame f, Frame gP, Frame rP, MoveInfo move)
        {
            if (f.Content == null)
            {
                if (moveInfos.Any(m => m.whoBeat == gP))
                {
                    moveInfos.Where(m => m.whoBeat == gP).First().whomBeat.Add(rP);
                    moveInfos.Where(m => m.whoBeat == gP).First().canBeat.Add(f);
                    f.BackgroundColor = Color.YellowGreen;
                }
                else
                {
                    move = new MoveInfo();
                    move.whoBeat = gP;
                    move.whomBeat.Add(rP);
                    move.canBeat.Add(f);
                    f.BackgroundColor = Color.YellowGreen;
                    gP.BackgroundColor = Color.YellowGreen;
                    moveInfos.Add(move);
                }
                mustBeat = true;
            }
        }

        private void CheackForQueen()
        {
            IEnumerable<Frame> greensPos = from f in frames
                        where (f as Frame).Content != null && (f as Frame).Content.BackgroundColor == Color.Green
                        select f as Frame;

            IEnumerable<Frame> redsPos = from f in frames
                      where (f as Frame).Content != null && (f as Frame).Content.BackgroundColor == Color.Red
                      select f as Frame;

            if (greensPos.Where(f => Convert.ToInt32(f.StyleId) > 0 && Convert.ToInt32(f.StyleId) < 8).Any())
            {
                greensPos.Where(f => Convert.ToInt32(f.StyleId) > 0 && Convert.ToInt32(f.StyleId) < 8).First().Content = new Image()
                { Source = ImageSource.FromResource("Checkers.img.GreenQueen.png"), WidthRequest = wigth, HeightRequest = height, StyleId = "GreenQueen" };
            }

            if (redsPos.Where(f => Convert.ToInt32(f.StyleId) > 69 && Convert.ToInt32(f.StyleId) < 77).Any())
            {
                redsPos.Where(f => Convert.ToInt32(f.StyleId) > 69 && Convert.ToInt32(f.StyleId) < 77).First().Content = new Image()
                { Source = ImageSource.FromResource("Checkers.img.RedQueen.png"), WidthRequest = wigth, HeightRequest = height, StyleId = "RedQueen" };
            }
        }
    }
}