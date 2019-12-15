using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Checkers.Models;
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
        Color PlayerColor { get; set; }
        List<MoveInfo> moveInfos = new List<MoveInfo>();
        bool mustBeat = false;
        List<Frame> tapFrames = new List<Frame>();
        List<View> canMove = new List<View>();
        MoveInfo firstTapToBeat = new MoveInfo();
        int tapCount = 0;

        public Game()
		{
			InitializeComponent ();

            PlayerColor = Color.Green;
            Title.Text = "Green's turn";

            CreateBoard("WRWBWGWGRWRWBWGWWRWBWGWGRWRWBWGWWRWBWGWGRWRWBWGWWRWBWGWGRWRWBWGW");
            CreateInterface();

            frames = grid.Children;
            wigth = grid.Width / 8;
            height = grid.Height / 8;

            CreateTabEvents();
        }

        public Game(string str, string whoMove)
        {
            InitializeComponent();

            PlayerColor = whoMove == "Green's turn" ? Color.Green : Color.Red;
            Title.Text = whoMove;
            TitleBoxView.BackgroundColor = PlayerColor;

            CreateBoard(str);
            CreateInterface();

            frames = grid.Children;
            wigth = grid.Width / 8;
            height = grid.Height / 8;

            CheackForBeating();
            CheckingBeatOfTheQueen();
            CreateTabEvents();
        }

        private void CreateBoard(string str)
        {
            grid = new Grid
            {
                RowDefinitions =
            {
                new RowDefinition { Height = 40 },
                new RowDefinition { Height = 40 },
                new RowDefinition { Height = 40 },
                new RowDefinition { Height = 40 },
                new RowDefinition { Height = 40},
                new RowDefinition { Height = 40},
                new RowDefinition { Height = 40},
                new RowDefinition { Height = 40}
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
                            Content = new BoxView { BackgroundColor = Color.Red, CornerRadius = 100, Margin = 2 }

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
                            Content = new BoxView { BackgroundColor = Color.Green, CornerRadius = 100, Margin = 2 }
                        }, i, j);
                    }
                    else if (str[iter] == 'Z')
                    {
                        grid.Children.Add(new Frame
                        {
                            BackgroundColor = Color.Black,
                            CornerRadius = 0,
                            Padding = 0,
                            StyleId = $"{(j == 0 ? i.ToString() : (j.ToString() + i.ToString()))}",
                            Content = new Image()
                            { Source = ImageSource.FromResource("Checkers.img.GreenQueen.png"), WidthRequest = wigth, HeightRequest = height, StyleId = "GreenQueen", Margin = 2}
                        }, i, j);
                    }
                    else if (str[iter] == 'X')
                    {
                        grid.Children.Add(new Frame
                        {
                            BackgroundColor = Color.Black,
                            CornerRadius = 0,
                            Padding = 0,
                            StyleId = $"{(j == 0 ? i.ToString() : (j.ToString() + i.ToString()))}",
                            Content = new Image()
                            { Source = ImageSource.FromResource("Checkers.img.RedQueen.png"), WidthRequest = wigth, HeightRequest = height, StyleId = "RedQueen", Margin = 2 }
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
                BackgroundColor = Color.DarkGoldenrod,
                Padding = 2,
                Spacing = 0
            };

            borderStack.Children.Add(borderStack2);
            borderStack2.Children.Add(borderStack3);
            borderStack3.Children.Add(grid);
            mainStack.Children.Add(borderStack);

            Content = mainStack;
        }

        public void SaveGame()
        {
            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            using (var db = new ApplicationContext(dbPath))
            {
                int count = db.DBModels.Count() + 1;
                string str = "Save: #" + count;
                string saveBoard = "";

                foreach (Frame f in frames)
                {
                    if (f.Content == null && f.BackgroundColor == Color.WhiteSmoke)
                    {
                        saveBoard += "W";
                    }
                    else if (f.Content == null && (f.BackgroundColor == Color.Black ||
                        (f.BackgroundColor == Color.GreenYellow && f.Content == null)))
                    {
                        saveBoard += "B";
                    }
                    else if (f.Content.BackgroundColor == Color.Red)
                    {
                        saveBoard += "R";
                    }
                    else if (f.Content.BackgroundColor == Color.Green)
                    {
                        saveBoard += "G";
                    }
                    else if (f.Content.StyleId == "GreenQueen")
                    {
                        saveBoard += "Z"; // Z = GreenQueen
                    }
                    else if (f.Content.StyleId == "RedQueen")
                    {
                        saveBoard += "X"; // X = RedQueen
                    }
                }

                db.DBModels.Add(new DBModel
                { Name = str, Date = DateTime.Now, Board = saveBoard, WhoMove = Title.Text });
                db.SaveChanges();
            }
        }

        public void CreateTabEvents()
        {
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

            if (frame.BackgroundColor == Color.WhiteSmoke) return;

            tapFrames.Add(frame);
            tapCount++;

            if (tapCount == 1 && !mustBeat)
            {
                if (frame.Content == null)
                {
                    tapCount--;
                    tapFrames.RemoveRange(0, tapFrames.Count);
                    return;
                }

                int pos;

                if (PlayerColor == frame.Content.BackgroundColor)
                {
                    pos = Convert.ToInt32(frame.StyleId);
                    if (IsPosInTheBoard(PlayerColor == Color.Green ? pos -= 11 : pos += 9))
                    {
                        if ((frames.First(f => f.StyleId == pos.ToString()) as Frame).Content == null)
                        {
                            canMove.Add(frames.First(f => f.StyleId == pos.ToString()));
                            frames.First(f => f == canMove.First()).BackgroundColor = Color.GreenYellow;
                        }
                    }

                    pos = Convert.ToInt32(frame.StyleId);
                    if (IsPosInTheBoard(PlayerColor == Color.Green ? pos -= 9 : pos += 11))
                    {              
                        if ((frames.First(f => f.StyleId == pos.ToString()) as Frame).Content == null)
                        {
                            canMove.Add(frames.First(f => f.StyleId == pos.ToString()));
                            frames.First(f => f == canMove.Last()).BackgroundColor = Color.GreenYellow;
                        }
                    }
                }
                else if(frame.Content.StyleId == "GreenQueen" && Title.Text.Contains("Green") 
                || frame.Content.StyleId == "RedQueen" && Title.Text.Contains("Red"))
                {
                    pos = Convert.ToInt32(frame.StyleId);
                    while (CheckingForQueenMove(pos += 9)) ;

                    pos = Convert.ToInt32(frame.StyleId);
                    while (CheckingForQueenMove(pos += 11)) ;
                    
                    pos = Convert.ToInt32(frame.StyleId);
                    while (CheckingForQueenMove(pos -= 11));
                   
                    pos = Convert.ToInt32(frame.StyleId);
                    while (CheckingForQueenMove(pos -= 9)) ;    
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
                tapFrames.ForEach(t => t.BackgroundColor = Color.Black);
                canMove.ForEach(c => c.BackgroundColor = Color.Black);

                if (canMove.Contains(tapFrames[1]))
                {
                    if (tapFrames[0].Content.BackgroundColor == PlayerColor)
                    {
                        tapFrames[1].Content = new BoxView { BackgroundColor = PlayerColor, CornerRadius = 100, Margin = 2 };
                    }
                    else
                    {
                        string whatColorQueen = PlayerColor == Color.Green ? "GreenQueen" : "RedQueen";
                        tapFrames[1].Content = new Image()
                        {
                            Source = ImageSource.FromResource($"Checkers.img.{whatColorQueen}.png"),
                            WidthRequest = wigth, HeightRequest = height, StyleId = whatColorQueen, Margin = 2
                        };
                    }

                    tapFrames[0].Content = null;
                    CheackForQueen();
                    Title.Text = PlayerColor == Color.Green ? "Red's turn" : "Green's turn";
                    PlayerColor = PlayerColor == Color.Green ? Color.Red : Color.Green;
                    TitleBoxView.BackgroundColor = PlayerColor;
                }

                CheackForBeating();
                CheckingBeatOfTheQueen();
                tapFrames.RemoveRange(0, tapFrames.Count);
                canMove.RemoveRange(0, canMove.Count);
                tapCount = 0;
            }

            if (tapCount == 1 && mustBeat)
            {
                if (frame.Content == null)
                {
                    tapCount--;
                    tapFrames.RemoveRange(0, tapFrames.Count);
                    return;
                }
                if (moveInfos.Where(f => f.whoBeat == frame).Any()||
                    (frame.Content.StyleId == "GreenQueen" && PlayerColor == Color.Green) ||
                    (frame.Content.StyleId == "RedQueen" && PlayerColor == Color.Red))
                {
                    foreach (MoveInfo move in moveInfos)
                    {
                        if (move.whoBeat == frame)
                        {
                            firstTapToBeat = move;
                        }
                        else
                        {
                            move.whoBeat.BackgroundColor = Color.Black;
                            move.whomBeat.ForEach(w => w.BackgroundColor = Color.Black);

                            foreach (var mCan in move.canBeat)
                            {
                                if (!moveInfos.Any(m => m.canBeat.Contains(mCan) && m != move))
                                {
                                    mCan.BackgroundColor = Color.Black;
                                }
                            }
                        }
                    }

                    firstTapToBeat.whoBeat.BackgroundColor = Color.Yellow;
                    firstTapToBeat.whomBeat.ForEach(w => w.BackgroundColor = Color.YellowGreen);
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
                    if (firstTapToBeat.whoBeat.Content is Image)
                    {
                        firstTapToBeat.whomBeat.Where(w => w.ClassId.Contains(frame.ClassId)).First().Content = null;
                    }
                    else
                    {
                        firstTapToBeat.whomBeat[firstTapToBeat.canBeat.IndexOf(frame)].Content = null;
                    }

                    firstTapToBeat.whoBeat.BackgroundColor = Color.Black;
                    firstTapToBeat.canBeat.ForEach(m => m.BackgroundColor = Color.Black);
                    firstTapToBeat.whomBeat.ForEach(w => w.BackgroundColor = Color.Black);

                    if (PlayerColor == Color.Green)
                    {
                        if(firstTapToBeat.whoBeat.Content is Image)
                        {
                            frame.Content = new Image()
                            {
                                Source = ImageSource.FromResource("Checkers.img.GreenQueen.png"),
                                WidthRequest = wigth,
                                HeightRequest = height,
                                StyleId = "GreenQueen",
                                Margin = 2
                            };
                        }
                        else
                        {
                            frame.Content = new BoxView { BackgroundColor = Color.Green, CornerRadius = 100, Margin = 2 };
                        }
                    }
                    else
                    {
                        if (firstTapToBeat.whoBeat.Content is Image)
                        {
                            frame.Content = new Image()
                            {
                                Source = ImageSource.FromResource("Checkers.img.RedQueen.png"),
                                WidthRequest = wigth,
                                HeightRequest = height,
                                StyleId = "RedQueen",
                                Margin = 2
                            };
                        }
                        else
                        {
                            frame.Content = new BoxView { BackgroundColor = Color.Red, CornerRadius = 100, Margin = 2 };
                        }
                    }

                    firstTapToBeat.whoBeat.Content = null;
                    tapFrames.RemoveRange(0, tapFrames.Count);
                    canMove.RemoveRange(0, canMove.Count);
                    moveInfos.RemoveRange(0, moveInfos.Count);

                    CheackForQueen();
                    CheackForBeating();
                    CheckingBeatOfTheQueen();

                    if (moveInfos.Count != 0 & moveInfos.Where(f => f.whoBeat == frame).Any())
                    {
                        List<MoveInfo> remove = moveInfos.Where(f => f.whoBeat != frame).ToList();
                        foreach (var r in remove)
                        {
                            r.whoBeat.BackgroundColor = Color.Black;
                            r.canBeat.ForEach(c => c.BackgroundColor = Color.Black);
                            r.whomBeat.ForEach(w => w.BackgroundColor = Color.Black);
                            moveInfos.Remove(r);
                        }

                        remove.RemoveRange(0, remove.Count);
                            
                    }
                    else // if (moveInfos.Count == 0)
                    {
                        moveInfos.ForEach(m => m.whoBeat.BackgroundColor = Color.Black);
                        moveInfos.ForEach(m => m.canBeat.ForEach(c => c.BackgroundColor = Color.Black));
                        moveInfos.ForEach(m => m.whomBeat.ForEach(c => c.BackgroundColor = Color.Black));

                        if (PlayerColor == Color.Green)
                        {
                            PlayerColor = Color.Red;
                            Title.Text = "Red's turn";
                            TitleBoxView.BackgroundColor = PlayerColor;
                        }
                        else
                        {
                            PlayerColor = Color.Green;
                            Title.Text = "Green's turn";
                            TitleBoxView.BackgroundColor = PlayerColor;
                        }

                        mustBeat = false;
                        moveInfos.RemoveRange(0, moveInfos.Count);                          
                        CheackForBeating();
                        CheckingBeatOfTheQueen();
                    }

                    tapCount = 0;
                }
                else
                {
                    tapCount = 1;
                }
            }

            IsItTheEndOfTheGame();
        }

        private void CheackForBeating()
        {
            MoveInfo move = new MoveInfo();
            IEnumerable<Frame> greensPos;
            IEnumerable<Frame> redsPos;
            int pos;

            //string gPos = PlayerColor == Color.Green ? "GreenQueen" : "RedQueen";
            //string rPos = PlayerColor == Color.Green ? "RedQueen" : "RedQueen";

            if (PlayerColor == Color.Green)
            {
                greensPos = from f in frames
                            where (f as Frame).Content != null 
                            && ((f as Frame).Content.BackgroundColor == Color.Green || (f as Frame).Content.StyleId == "GreenQueen")
                            select f as Frame;

                redsPos = from f in frames
                        where (f as Frame).Content != null 
                        && ((f as Frame).Content.BackgroundColor == Color.Red || (f as Frame).Content.StyleId == "RedQueen")
                          select f as Frame;
            }
            else
            {
                greensPos = from f in frames
                            where (f as Frame).Content != null 
                            && ((f as Frame).Content.BackgroundColor == Color.Red || (f as Frame).Content.StyleId == "RedQueen")
                            select f as Frame;

                redsPos = from f in frames
                        where (f as Frame).Content != null 
                        && ((f as Frame).Content.BackgroundColor == Color.Green || (f as Frame).Content.StyleId == "GreenQueen")
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
                }
                else
                {
                    move = new MoveInfo();
                    move.whoBeat = gP;
                    move.whomBeat.Add(rP);
                    move.canBeat.Add(f);
                    gP.BackgroundColor = Color.GreenYellow;
                    moveInfos.Add(move);
                }
                f.BackgroundColor = Color.GreenYellow;
                rP.BackgroundColor = Color.YellowGreen;
                mustBeat = true;
            }
        }

        // Logic for the Queens
        private void CheackForQueen()
        {
            IEnumerable<Frame> checkersPos = from f in frames
                        where (f as Frame).Content != null && (f as Frame).Content.BackgroundColor == PlayerColor
                        select f as Frame;

            string whatQueenToCreate = PlayerColor == Color.Green ? "GreenQueen" : "RedQueen";
            bool isCheckerAQueen = checkersPos.Any(f => 
            Convert.ToInt32(f.StyleId) > (whatQueenToCreate == "GreenQueen" ? 0 : 69) && 
            Convert.ToInt32(f.StyleId) < (whatQueenToCreate == "GreenQueen" ? 8 : 78));

            if (isCheckerAQueen)
            {
                checkersPos.First(f => 
                Convert.ToInt32(f.StyleId) > (whatQueenToCreate == "GreenQueen" ? 0 : 69) && 
                Convert.ToInt32(f.StyleId) < (whatQueenToCreate == "GreenQueen" ? 8 : 78)).Content = 
                new Image()
                {
                    Source = ImageSource.FromResource($"Checkers.img.{whatQueenToCreate}.png"),
                    WidthRequest = wigth, HeightRequest = height, StyleId = whatQueenToCreate,
                    Margin = 2
                };
            }
        }

        private bool CheckingForQueenMove(int pos)
        {
            if (!IsPosInTheBoard(pos)) return false;

            View view = frames.First(f => f.StyleId == pos.ToString());

            if ((view as Frame).Content == null)
            {
                canMove.Add(view);
                view.BackgroundColor = Color.GreenYellow;
                return true;
            }

            return false;
        }

        private void CheckingBeatOfTheQueen()
        {
            int pos;
            string whoseQueensToFind = PlayerColor == Color.Red ? "RedQueen" : "GreenQueen";
            IEnumerable<Frame> queens =
            from f in frames
            where (f as Frame).Content != null 
            && (f as Frame).Content.StyleId == whoseQueensToFind
            select f as Frame;
                
            foreach (Frame queen in queens)
            {
                pos = Convert.ToInt32(queen.StyleId);
                while (ChekingDiagonalOfTheQueen(pos += 9, 9, queen)); // Search from the bottom left

                pos = Convert.ToInt32(queen.StyleId);
                while (ChekingDiagonalOfTheQueen(pos += 11, 11, queen)); // Search from the bottom right

                pos = Convert.ToInt32(queen.StyleId);
                while (ChekingDiagonalOfTheQueen(pos -= 11, -11, queen)); // Search from the top left

                pos = Convert.ToInt32(queen.StyleId);
                while (ChekingDiagonalOfTheQueen(pos -= 9, -9, queen)); // Search from the top right
            }    
        }

        private bool ChekingDiagonalOfTheQueen(int pos, int temp, Frame queen)
        {
            if (!IsPosInTheBoard(pos)) return false;

            Frame whomBeat = frames.First(f => f.StyleId == pos.ToString()) as Frame;

            if (whomBeat.Content == null) return true;
            else if (whomBeat.Content.StyleId != queen.Content.StyleId && whomBeat.Content.BackgroundColor != PlayerColor)
            {
                pos += temp;
                if (!IsPosInTheBoard(pos)) return false;
                Frame canBeat = frames.First(f => f.StyleId == pos.ToString()) as Frame;
                if (canBeat.Content == null)
                    BeatsQueenAdd(canBeat, queen, whomBeat, temp);
            }
            return false;
        }

        private void BeatsQueenAdd(Frame canBeat, Frame queen, Frame whomBeat, int temp)
        {
            MoveInfo move;
            whomBeat.ClassId += temp.ToString();
            canBeat.ClassId = temp.ToString();
            canBeat.BackgroundColor = Color.GreenYellow;
            queen.BackgroundColor = Color.GreenYellow;
            whomBeat.BackgroundColor = Color.YellowGreen;

            if (moveInfos.Any(m => m.whoBeat == queen))
            {
                moveInfos.Where(m => m.whoBeat == queen).First().whomBeat.Add(whomBeat);
                moveInfos.Where(m => m.whoBeat == queen).First().canBeat.Add(canBeat);
            }
            else
            {
                move = new MoveInfo();
                move.whoBeat = queen;
                move.whomBeat.Add(whomBeat);
                move.canBeat.Add(canBeat);
                moveInfos.Add(move);
            }

            int temp2 = temp;
            Frame frameCanBeat;
            while (true)
            {
                if (IsPosInTheBoard(Convert.ToInt32(canBeat.StyleId) + temp2))
                {
                    frameCanBeat = frames.Where(frame => 
                    Convert.ToInt32(frame.StyleId) == Convert.ToInt32(canBeat.StyleId) + temp2).First() as Frame;
                    if (frameCanBeat.Content == null)
                    {
                        frameCanBeat.BackgroundColor = Color.GreenYellow;
                        moveInfos.Where(m => m.whoBeat == queen).First().canBeat.Add(frameCanBeat);
                        frameCanBeat.ClassId = temp.ToString();
                        temp2 += temp;
                    }
                    else break;
                }
                else break;
            }

            mustBeat = true; 
        }
        // End Logic for the Queens

        private bool IsPosInTheBoard(int pos)
        {
            return pos != 18 && pos != 38 && pos != 58 &&
                   pos != 19 && pos != 39 && pos != 59 &&
                   pos < 77 && pos > 0;
        }

        private async void IsItTheEndOfTheGame()
        {
            // Is it a Green victory?
            if(!frames.Where(f => 
            (f as Frame).Content != null 
            && ((f as Frame).Content.BackgroundColor == Color.Red 
            || (f as Frame).Content.StyleId == "RedQueen")).Any())
            {
                bool answer = await DisplayAlert("The game is over", "\tThe green's won!!!\nWould you like to start new game?", "Yes", "No");
                await Navigation.PopAsync();
                if (answer)
                {
                    Game newGame = new Game();
                    await Navigation.PushAsync(newGame);
                    Navigation.RemovePage(this);
                }
            }

            // Is it a Red victory?
            else if (!frames.Where(f => 
            (f as Frame).Content != null 
            && ((f as Frame).Content.BackgroundColor == Color.Green
            || (f as Frame).Content.StyleId == "GreenQueen")).Any())
            {
                bool answer = await DisplayAlert("The game is over", "\tThe red's won!!!\nWould you like to start new game?", "Yes", "No");
                await Navigation.PopAsync();

                if (answer)
                {
                    Game newGame = new Game();
                    await Navigation.PushAsync(newGame);
                    Navigation.RemovePage(this);
                }
            }

            // Is this a draw?
            if (!moveInfos.Any())
            {
                IEnumerable<Frame> redsPos = from f in frames
                                             where (f as Frame).Content != null
                                             && ((f as Frame).Content.BackgroundColor == Color.Red || (f as Frame).Content.StyleId == "RedQueen")
                                             select f as Frame;
                IEnumerable<Frame> greenPos = from f in frames
                                              where (f as Frame).Content != null
                                              && ((f as Frame).Content.BackgroundColor == Color.Green || (f as Frame).Content.StyleId == "GreenQueen")
                                              select f as Frame;
                int pos;
                bool isCanMove = false;
                IEnumerable<Frame> forEach = PlayerColor == Color.Green ? greenPos : redsPos;
                foreach (var r in forEach)
                {
                    pos = Convert.ToInt32(r.StyleId);
                    if (IsPosInTheBoard(PlayerColor == Color.Green ? pos -= 11 : pos += 9))
                    {
                        if ((frames.First(f => f.StyleId == pos.ToString()) as Frame).Content == null)
                        {
                            isCanMove = true;
                            break;
                        }
                    }

                    pos = Convert.ToInt32(r.StyleId);
                    if (IsPosInTheBoard(PlayerColor == Color.Green ? pos -= 9 : pos += 11))
                    {
                        if ((frames.First(f => f.StyleId == pos.ToString()) as Frame).Content == null)
                        {
                            isCanMove = true;
                            break;
                        }
                    }
                }
                if (!isCanMove)
                {
                    bool answer = await DisplayAlert("The game is over", "\tIt is a Draw!!!\nWould you like to start new game?", "Yes", "No");
                    await Navigation.PopAsync();

                    if (answer)
                    {
                        Game newGame = new Game();
                        await Navigation.PushAsync(newGame);
                        Navigation.RemovePage(this);
                    }
                }
            }
        }
    }
}