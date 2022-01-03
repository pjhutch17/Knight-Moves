using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Microsoft.VisualBasic;
using System.IO;

namespace Knight_Moves
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Boolean[,] board; 
        Int32 krow=1, kcol = 7; // Initial knight position
        Int32 size = 40; // Square size

        System.Windows.Controls.Image knightImage;
        System.Windows.Media.Brush knBrush;

        Canvas knCanvas = new Canvas();

        public MainWindow()
        {
            InitializeComponent();
            loadImages();
            board = new Boolean[8, 8];
            
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            drawBoard();
            drawKnight(krow, kcol);
        }

        private void drawBoard()
        {
            Int32 row, col;
            // System.Windows.UIElement rect;
           System.Windows.Shapes.Rectangle rect;
            Boolean bw = true;

            Console.WriteLine("Drawing board");
            // Add rectandles to board
            for (row = 0; row < 8; row++)
            {
                for (col = 0; col < 8; col++)
                {
                    board[row, col] = true;
                    // Create a rectangle
                    rect = new System.Windows.Shapes.Rectangle();
                    rect.Width = size;
                    rect.Height = size;
                    if (board[row, col] == true)
                        rect.Fill = bw ? System.Windows.Media.Brushes.White : System.Windows.Media.Brushes.Black;
                    else
                        rect.Fill = System.Windows.Media.Brushes.DarkBlue; // Space visited by Knight
                    // Add rectangle and position it on Canvas.
                    chessBoard.Children.Add(rect);
                    Canvas.SetTop(rect,  row * size);
                    Canvas.SetLeft(rect, col * size);
                    bw = !bw; // Alternate colors
                }
                bw = (row % 2!= 0);
            }
        }

        // Load Knight image
        private void loadImages()
        {
            string currDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string parentDir = System.IO.Path.Combine(currDir, @"..\..");
            string[] files = Directory.GetFiles(parentDir, "Knight-40.png");
            // string knightImageFile =("D:\\My Documents\\Visual Studio 2017\\Projects\\Knight Moves\\Knight Moves\\Knight-40.png");
            Console.WriteLine("Path = " + files[0]);
            string knightImageFile = (files[0]);
            BitmapImage knBmp;

            Console.WriteLine("Loading bitmap");
            // Create image
            knightImage = new System.Windows.Controls.Image();
            knightImage.Width = size;

            // Set bitmap source
            knBmp = new BitmapImage();
            knBmp.BeginInit();
            knBmp.UriSource = new Uri(knightImageFile);
            knBmp.DecodePixelWidth = size;
            knBmp.EndInit();

            // Define image source
            Console.WriteLine("Defining image source");
            knightImage.Source = knBmp;

            

        }

        // Draw knight on canvas.
        private void drawKnight(Int32 r, Int32 c)
        {
            Int32 actualRow, actualCol; // Position on canvas


            actualRow = (size * (r-1)) - (r % size);
            actualCol = (size * (c-1)) - (c % size)+7;

            Console.WriteLine("Positioning image at (" + actualRow + "," + actualCol + ")");

            // Create a canvas for knight
            // knCanvas = new Canvas();
            if (knCanvas.Children.Contains(knightImage) == false)
                knCanvas.Children.Add(knightImage);

            Canvas.SetTop(knCanvas, actualRow);
            Canvas.SetLeft(knCanvas, actualCol);
            // Add knight to chess board
            if (chessBoard.Children.Contains(knCanvas))
            {
                Console.WriteLine("Found existing item. Removing it.");
                chessBoard.Children.Remove(knCanvas);
            }
            else
            {
                chessBoard.Children.Add(knCanvas);
            }
        }

        void Square_Clicked(object sender, EventArgs e)
        {
            double click_row, click_col;
            Int32 newrow, newcol;
            System.Windows.Point mpos;
            mpos = Mouse.GetPosition(chessBoard);
            click_col = mpos.X + size / 2 - 4;
            click_row = mpos.Y + size / 2 + 4;
            if (click_col < 0) click_col = 0;
            if (click_row < 0) click_row = 0;
            Console.WriteLine("Clicked at (" + click_col + "," + click_row + ")");
            newrow = Convert.ToInt32(click_row / size);
            newcol = Convert.ToInt32(click_col / size);
            // Check if valid move
            if (Math.Abs(newrow - krow) == 1 & Math.Abs(newcol - kcol) == 2 | Math.Abs(newrow - krow) == 2 & Math.Abs(newcol - kcol) == 1)
            {
                Console.WriteLine("Moving knight to (" + newrow + "," + newcol + ")");
                drawKnight(newrow, newcol);
                krow = newrow;
                kcol = newcol;
            }
            else
                Console.WriteLine("Invalid Move to (" + newrow + "," + newcol + ")");
        }
    }

}
