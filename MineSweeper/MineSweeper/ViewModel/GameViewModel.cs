using System.ComponentModel;
using System.Runtime.CompilerServices;
using MineSweeper.Model;
using Xamarin.Forms;

namespace MineSweeper.ViewModel
{
    internal class GameViewModel : INotifyPropertyChanged
    {
        //Object from GameModel class
        private readonly GameModel _gameModel;

        //String variable shows number of mines left
        private string _minesNumber;

        //2D array of labels used to access each cell inside main grid
        private Label[][] _gUiGameGrid;

        //variable used to referes to main GameGrid to update it
        private readonly Grid _gameGrid;

        //String variable used to inform the user if he won or lost
        private string _gameStatus;


        public GameViewModel(Grid gameGrid)
        {
            _gameGrid = gameGrid;
            _gameModel= new GameModel(gameGrid);
            MinesNumber = _gameModel.GetMinesNumberModel();
            GameStatus = "";

        }

       
        //TODO delete it after finish because it is useless
        public int[][] GetGameGridViewModel()
        {
            return _gameModel.GetGameGridModel();
        }

        //Method called by SingleTapView in GameView class
        public void SingleTapViewModel(object sender)
        {
            //status may has 3 values {-1,0,1} which mean {lose,continue playing, won}
            var status =_gameModel.SingleTapModel(sender);
            UpdateGameGrid();
            if (status == -1)
            {
                GameStatus = "YOU LOST!";
            }
            else if (status == 1)
            {
                GameStatus = "YOU WON!";
            }
        }

        //Method used to update main game grid
        private void UpdateGameGrid()
        {
            _gUiGameGrid = _gameModel.GetGuiGameGrid();
            for (var i = 0; i < _gameModel.GetRowsCount(); i++)
            {
                for (var j = 0; j < _gameModel.GetColumnsCount(); j++)
                {
                    _gameGrid.Children[i* _gameModel.GetColumnsCount() + j].BackgroundColor = _gUiGameGrid[i][j].BackgroundColor;
                    _gameGrid.Children[i * _gameModel.GetColumnsCount() + j].IsEnabled = _gUiGameGrid[i][j].IsEnabled;
                }

            }
        }


        //Method called by DoubleTapView in GameView class
        //Change the label color if it is possible and update mines number
        public void DoubleTapViewModel(object sender)
        {
            var label = (Label)sender;
            label.BackgroundColor = _gameModel.DoubleTapModel(label.BackgroundColor);                      
            MinesNumber = _gameModel.GetMinesNumberModel();            
        }

        //Binded variable 
        public string MinesNumber
        {
            get { return _minesNumber; }
            set
            {
                if (_minesNumber == value) return;
                _minesNumber = value;
                OnPropertyChanged();
            }
        }

        //Binded variable 
        public string GameStatus
        {
            get { return _gameStatus; }
            set
            {
                if (_gameStatus == value) return;
                _gameStatus = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
