using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineSweeper.Model;
using MineSweeper.View;
using MineSweeper.ViewModel;
using Xamarin.Forms;

namespace MineSweeperTest
{
    [TestClass]
    public class MineSweeperTest
    {
        [TestMethod]
        //Test GetGameGridViewModel Method
        public void GameViewModelGetGameGridViewModel()
        {
            //Arrange
            var gameViewModel = new GameViewModel(CreateGrid());

            //Act
            int[][] gameGrid = gameViewModel.GetGameGridViewModel();

            //Assert
            Assert.IsNotNull(gameGrid);

        }

        [TestMethod]
        //Test SingleTapViewModel and UpdateGameGridViewModel Methods
        public void GameViewModelSingleTapViewModelAndUpdateGameGridViewModel()
        {
            //Arrange
            var grid1 = CreateGrid();
            var grid2 = CreateGrid();
            CopyGrid(grid1, grid2);
            var gameViewModel = new GameViewModel(grid1);

            //Act
            gameViewModel.SingleTapViewModel(grid1.Children[0]);

            //Assert
            //Label color will change
            Assert.AreNotEqual(grid1.Children[0].BackgroundColor, grid2.Children[0].BackgroundColor);

        }

        [TestMethod]
        //Test DoubleTapViewModel Method
        public void GameViewModelDoubleTapViewModel()
        {
            //Arrange
            var grid1 = CreateGrid();
            var grid2 = CreateGrid();
            CopyGrid(grid1, grid2);
            var gameViewModel = new GameViewModel(grid1);

            //Act
            gameViewModel.DoubleTapViewModel(grid1.Children[0]);

            //Assert
            //Label color will change
            Assert.AreNotEqual(grid1.Children[0].BackgroundColor, grid2.Children[0].BackgroundColor);

        }

        [TestMethod]
        //Test MinesNumber Method
        public void GameViewModelMinesNumberViewModel()
        {
            //Arrange
            var grid = CreateGrid();
            var gameViewModel = new GameViewModel(grid);

            //Act
            //Mines number will decrease -1 from 5
            gameViewModel.DoubleTapViewModel(grid.Children[0]);

            //Assert
            Assert.AreNotEqual(gameViewModel.MinesNumber, "5 Mines Left!" );

        }

        [TestMethod]
        //Test ConvertFromGridtoCellsMatrix Method
        public void GameModelConvertFromGridtoCellsMatrixModel()
        {
            //Arrange
            var grid = CreateGrid();

            //Act
            //ConvertFromGridtoCellsMatrix() is called inside the constructor
            var gameModel = new GameModel(grid);

            //Assert
            Assert.AreEqual(gameModel.GetGuiGameGrid()[0][0], grid.Children[0]);

        }

        [TestMethod]
        //Test ConvertFromGridtoCellsMatrix Method
        public void GameModelInitGameGridModel()
        {
            //Arrange
            var grid = CreateGrid();

            //Act
            //ConvertFromGridtoCellsMatrix() is called inside the constructor
            var gameModel = new GameModel(grid);

            //Assert
            Assert.IsNotNull(gameModel.GetGameGridModel());

        }

        //This method is for creating an object from Grid to testing GameViewModel and GameModel classes
        private Grid CreateGrid()
        {
            var grid = new Grid();
            for (var i = 0; i < GameModel.GetRowsCount(); i++)
            {
                for (var j = 0; j < GameModel.GetColumnsCount(); j++)
                {
                    grid.Children.Add(new Label { BackgroundColor = Color.Gray }, i, j);
                }
            }

            return grid;
        }

        //This method is for copying a grid to another to testing GameViewModel and GameModel classes
        private void CopyGrid(Grid oldGrid, Grid newGrid)
        {
            var grid = new Grid();
            for (var i = 0; i < oldGrid.Children.Count; i++)
            {
                newGrid.Children[i].BackgroundColor = oldGrid.Children[i].BackgroundColor;
            }

        }
    }
}
