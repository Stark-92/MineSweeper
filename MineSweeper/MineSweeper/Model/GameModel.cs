using System;
using Xamarin.Forms;

namespace MineSweeper.Model
{
    internal class GameModel
    {
        //Variable used to calculate mines left
        private int _minesNumber;

        //Variable used to count taped cells
        private int _tapedCells;


        //2D array used to represent the game grid with mines
        private int [][] _gameGrid;

        //Variable used to represent Rows number
        private const int RowsCount = 7;

        //Variable used to Columns Rows number
        private const int ColumnsCount=5;

        //2D array of labels used to access each cell inside main grid
        private Label[][] _gUiGameGrid;

        public GameModel(Grid gameGrid)
        {
            //Mines number is optional !
            _minesNumber = 5;
            InitGameGrid();
            ConvertFromGridtoCellsMatrix(gameGrid);
            _tapedCells = _minesNumber;
        }

        //Method used to convert from Grid object to label [] [] array
        private void ConvertFromGridtoCellsMatrix(Grid gameGrid)
        {
            _gUiGameGrid = new Label[GetRowsCount()][];
            for (var i = 0; i < GetRowsCount(); i++)
            {
                _gUiGameGrid[i] = new Label[GetColumnsCount()];
                for (var j = 0; j < GetColumnsCount(); j++)
                {
                    _gUiGameGrid[i][j] = (Label)gameGrid.Children[i* GetColumnsCount() + j];
                }

            }

        }

        //Method used to return rows number
        public  int GetRowsCount()
        {
            return RowsCount;
        }

        //Method used to return columns number
        public int GetColumnsCount()
        {
            return ColumnsCount;
        }

        //Method used to generating the grid with mines
        public void InitGameGrid()
        {
            _gameGrid = new int[RowsCount][];

            for (var i = 0; i < RowsCount; i++)
            {
                _gameGrid[i]= new int[ColumnsCount];
                for (var j = 0; j < ColumnsCount; j++)
                {
                    _gameGrid[i][j] = 0;
                }
            }

            int mineX=0, mineY=0;
            for (var k = 0; k < _minesNumber; k++)
            {
                GenerateRandomPosition(ref mineX,ref mineY);

                for (var i = -1; i < 2; i++)
                {
                    for (var j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            _gameGrid[mineX][mineY] = -1;
                        }
                        else if(((mineX + i)<RowsCount && (mineX + i) >=0) && ((mineY + j) < ColumnsCount && (mineY + j) >=0) && (_gameGrid[mineX + i][mineY + j]>=0))
                        {
                            _gameGrid[mineX+i][mineY+j]++;
                        }
                    }
                }
            }
        }

        //Method used to generate random position for mines
        private void GenerateRandomPosition(ref int mineX,ref int mineY)
        {
            do
            {
                Random random = new Random();
                mineX = random.Next(0, 7);
                mineY = random.Next(0, 5);
            } while (_gameGrid[mineX][ mineY] < 0);
        }

        //Method used to return game grid 2D array 
        public int[][] GetGameGridModel()
        {
            return _gameGrid;
        }

        //Method used to return 2D array of cell {label}
        public Label[][] GetGuiGameGrid()
        {
            return _gUiGameGrid;
        }

        //Method called by SingleTapViewModel in GameViewModel class
        //In this method I check if the cell is mine or a number or empty
        public int SingleTapModel(object sender)
        {
            var status=0;
            var cell = (Label) sender;
            if (!CheckCellStatus(cell)) return status;
            int x = -1, y = -1;
            FindPosition(ref x, ref y, cell);
            status = StartDiscovering(x, y);
            return status;

        }

        //Method used to discover the cell itself and its neighbors
        private int  StartDiscovering(int x, int y)
        {
            if (_gameGrid[x][y] < 0)//User revealed on a mine
            {
                _gUiGameGrid[x][y].BackgroundColor = Color.Red;
                GameEnd();
                return -1;
            }
            else if (_gameGrid[x][y] > 0) //User revealed on a number
            {
                _gUiGameGrid[x][y].BackgroundColor = Color.Silver;
                _gUiGameGrid[x][y].Text = _gameGrid[x][y].ToString();
                _tapedCells++;
            }
            else//User revealed on a blank cell
            {
                ChechNeighbors(x,y);
            }

            if (_tapedCells == GetColumnsCount() * GetRowsCount())
            {
                GameEnd();
                return 1;
            }           
            return 0;
        }

        //Method checks a blank cell neighbors
        private void ChechNeighbors(int x, int y)
        {
            if ((x>=0 && (x < GetRowsCount())) && (y >= 0 && (y < GetColumnsCount())))
            {
                //Don't check taped or flaged cell
                if (_gUiGameGrid[x][y].BackgroundColor ==Color.Gray)
                {

                    if (_gameGrid[x][y] > 0) //Cell with a number
                    {
                        _gUiGameGrid[x][y].BackgroundColor = Color.Silver;
                        _gUiGameGrid[x][y].Text = _gameGrid[x][y].ToString();
                        _tapedCells++;
                    }
                    else if (_gameGrid[x][y] == 0) //Blank cell                    
                    {
                        _gUiGameGrid[x][y].BackgroundColor = Color.Silver;
                        _gUiGameGrid[x][y].Text = "";
                        _tapedCells++;


                        ChechNeighbors(x - 1, y - 1);
                        ChechNeighbors(x - 1, y);
                        ChechNeighbors(x - 1, y + 1);

                        ChechNeighbors(x, y - 1);
                        ChechNeighbors(x, y + 1);

                        ChechNeighbors(x + 1, y - 1);
                        ChechNeighbors(x + 1, y);
                        ChechNeighbors(x + 1, y + 1);
                    }
                }
            }
        }

        //Method used to disable the game grid from being clickable  
        private void GameEnd()
        {
            for (var i = 0; i < GetRowsCount(); i++)
            {
                for (var j = 0; j < GetColumnsCount(); j++)
                {
                    //This code is not working and that's why i wrote if (GameStatusLabel.Text == "") _gameViewModel.DoubleTapViewModel(sender);
                    _gUiGameGrid[i][j].IsEnabled = false;
                }

            }
        }

        //Method used to check if the cell is already taped or not
        private bool CheckCellStatus(Label cell)
        {
            return cell.BackgroundColor == Color.Gray;
        }

        //Method used to find X,Y of the cell in the grid
        private void FindPosition(ref int x, ref int y, Label cell)
        {
            for (var i = 0; i < GetRowsCount(); i++)
            {
                for (var j = 0; j < GetColumnsCount(); j++)
                {
                    if (!_gUiGameGrid[i][j].Equals(cell)) continue;
                    x = i;
                    y = j;
                }
            }
        }

        //Method called by DoubleTapViewModel in GameViewModel class
        //This method changes the cell color if it is possible
        public Color DoubleTapModel(Color labelBackgroundColor)
        {
            if (labelBackgroundColor == Color.Gray && _minesNumber > 0)
            {
                _minesNumber--;
                return Color.Yellow;
            }
            else if(labelBackgroundColor==Color.Yellow)
            {
                _minesNumber++;
                return Color.Gray;
            }

            return Color.Silver;
        }

        //Method returns number of mines left
        public string GetMinesNumberModel()
        {
            return _minesNumber + " Mines Left!";
        }
    }
       
}
