using System;
using System.Diagnostics;
using MineSweeper.ViewModel;

namespace MineSweeper.View
{
    public partial class GameView
    {
        //Object from GameViewModel class
        private readonly GameViewModel _gameViewModel;

        public GameView()
        {
            InitializeComponent();
            _gameViewModel = new GameViewModel(GameGrid);
            BindingContext = _gameViewModel;

            //Set the game discription to Discription label
            Discription.Text = "Welcome to Mine Sweeper game" + Environment.NewLine+
                "Tap once to reveal a cell and twice on a cell to plant or remove a flag marking suspicious mine.";
        }

        //Method called when the user taps once on any label
        private void SingleTapView(object sender, EventArgs e)
        {

            if(GameStatusLabel.Text =="")                    //I wrote this line to stop user's taps after win or lose.
                _gameViewModel.SingleTapViewModel(sender);   // The reason for it because Label.IsEnabled=false
                                                             // is not working due a bug in Xamarin as I read at Xamarin blog
                                                             // Or maybe because of my machine :\
        }

        //Method called when the user taps twice on any label
        private void DoubleTapView(object sender, EventArgs e)
        {

            //This code shows the game grid with mines.
            int[][] mygrid = _gameViewModel.GetGameGridViewModel();
            string line = "";

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    line += mygrid[i][j] + " ";
                }
                Debug.WriteLine(line);
                line = "";
            }

            if (GameStatusLabel.Text == "")
                _gameViewModel.DoubleTapViewModel(sender);

        }
        
    }
        

    
}
