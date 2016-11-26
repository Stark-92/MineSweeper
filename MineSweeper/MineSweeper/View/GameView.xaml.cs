using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace MineSweeper.View
{
    public partial class GameView : ContentPage
    {

        public GameView()
        {
            InitializeComponent();

        }

        private void SingleTap(object sender, EventArgs e)
        {
            var cell = (Label) sender;
            cell.Text = "ONE!!!!";
        }

        private void DoubleTap(object sender, EventArgs e)
        {
            var cell = (Label)sender;
            cell.Text = "TWO!!!!";
        }
        
    }
        

    
}
