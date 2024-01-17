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

using System.Windows.Threading; //input threading namespace

namespace PAC_Man_Game_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer(); 
        bool goLeft, goRight, goDown, goUp; 
        bool noLeft, noRight, noDown, noUp; 
        int speed = 8; 
        Rect pacmanHitBox; 
        int ghostSpeed = 10; 
        int ghostMoveStep = 170; 
        int currentGhostStep; 
        int score = 0; 
        public MainWindow()
        {
            InitializeComponent();
            GameSetUp(); // run the game set up function
        }
        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            // this is the key down event
            if (e.Key == Key.Left && noLeft == false)
            {
                
                goRight = goUp = goDown = false; 
                noRight = noUp = noDown = false; 
                goLeft = true; 
                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2); // rotate the pac man image to face left
            }
            if (e.Key == Key.Right && noRight == false)
            {
                
                noLeft = noUp = noDown = false; 
                goLeft = goUp = goDown = false; 
                goRight = true; 
                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2); // rotate the pac man image to face right
            }
            if (e.Key == Key.Up && noUp == false)
            {
                
                noRight = noDown = noLeft = false; 
                goRight = goDown = goLeft = false; 
                goUp = true; 
                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2); // rotate the pac man character to face up
            }
            if (e.Key == Key.Down && noDown == false)
            {
                
                noUp = noLeft = noRight = false;
                goUp = goLeft = goRight = false; 
                goDown = true; 
                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2); // rotate the pac man character to face down
            }
        }
        private void GameSetUp()
        {
            // this function will run when the program loads
            CanvasOne.Focus(); // set CanvasOne as the main focus for the program
            gameTimer.Tick += GameLoop; // link the game loop event to the time tick
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); // set time to tick every 20 milliseconds
            gameTimer.Start(); // start the time
            currentGhostStep = ghostMoveStep; // set current ghost step to the ghost move step
            ImageBrush pacmanImage = new ImageBrush(); //assigning the image brush to the rectangles
            pacmanImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pacman.jpg"));
            pacman.Fill = pacmanImage;
            ImageBrush redGhost = new ImageBrush();
            redGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/red.jpg"));
            redGuy.Fill = redGhost;
            ImageBrush orangeGhost = new ImageBrush();
            orangeGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/orange.jpg"));
            orangeGuy.Fill = orangeGhost;
            ImageBrush pinkGhost = new ImageBrush();
            pinkGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pink.jpg"));
            pinkGuy.Fill = pinkGhost;
        }
        private void GameLoop(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score ; // display score to the label 
            // start moving the character in the movement directions
            if (goRight)
            { 
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speed);
            }
            if (goLeft)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speed);
            }
            if (goUp)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speed);
            }
            if (goDown)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speed);
            }
            // restricting from Canvas borders
            if (goDown && Canvas.GetTop(pacman) + 80 > Application.Current.MainWindow.Height)
            {
                noDown = true;
                goDown = false;
            }
            if (goUp && Canvas.GetTop(pacman) < 1)
            {
                noUp = true;
                goUp = false;
            }
            if (goLeft && Canvas.GetLeft(pacman) - 10 < 1)
            {
                noLeft = true;
                goLeft = false;
            }
            if (goRight && Canvas.GetLeft(pacman) + 70 > Application.Current.MainWindow.Width)
            {
                noRight = true;
                goRight = false;
            }
            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height); // asssign the pac man hit box to the pac man rectangle
            // below is the main game loop that will scan through all of the rectangles available inside of the game
            foreach (var x in CanvasOne.Children.OfType<Rectangle>())
            {
                // loop through all of the rectangles inside of the game and identify them using the x variable
                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height); // create a new rect called hit box for all of the available rectangles inside of the game
                // find the walls, if any of the rectangles inside of the game has the tag wall inside of it
                if ((string)x.Tag == "wall")
                {
                    // check if we are colliding with the wall while moving left if true then stop the pac man movement
                    if (goLeft == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        noLeft = true;
                        goLeft = false;
                    }
                    // check if we are colliding with the wall while moving right if true then stop the pac man movement
                    if (goRight == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        noRight = true;
                        goRight = false;
                    }
                    // check if we are colliding with the wall while moving down if true then stop the pac man movement
                    if (goDown == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        noDown = true;
                        goDown = false;
                    }
                    // check if we are colliding with the wall while moving up if true then stop the pac man movement
                    if (goUp == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        noUp = true;
                        goUp = false;
                    }
                }
                // check if the any of the rectangles has a coin tag inside of them
                if ((string)x.Tag == "food")
                {
                    // if pac man collides with any of the coin and coin is still visible to the screen
                    if (pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                    {
                        // set the coin visiblity to hidden
                        x.Visibility = Visibility.Hidden;
                        // add 1 to the score
                        score++;
                    }
                }
                // if any rectangle has the tag ghost inside of it
                if ((string)x.Tag == "ghost")
                {
                    // check if pac man collides with the ghost 
                    if (pacmanHitBox.IntersectsWith(hitBox))
                    {
                        // if collision has happened, then end the game by calling the game over function and passing in the message
                        GameOver("Oh no! Ghosts got you, click to retry");
                    }
                    // if there is a rectangle called orange guy in the game
                    if (x.Name.ToString() == "orangeGuy")
                    {
                        // move that rectangle to towards the left of the screen
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);
                    }
                    else
                    {
                        // other ones can move towards the right of the screen
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + ghostSpeed);
                    }
                    // reduce one from the current ghost step integer
                    currentGhostStep--;
                    // if the current ghost step integer goes below 1 
                    if (currentGhostStep < 1)
                    {
                        // reset the current ghost step to the ghost move step value
                        currentGhostStep = ghostMoveStep;
                        // reverse the ghost speed integer
                        ghostSpeed = -ghostSpeed;
                    }
                }
            }
            
            if (score == 100)
            {
                txtScore.Content = "Score: " + score;
                // game over function 
                GameOver("Well done! you ate all of the food");
            }
        }
        private void GameOver(string message)
        {
            // inside the game over function we passing in a string to show the final message to the game
            gameTimer.Stop(); // stop the game timer
            MessageBox.Show(message, "The Pac Man Game"); // show a mesage box with the message that is passed in this function
            // when the player clicks ok on the message box
            // restart the application
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
