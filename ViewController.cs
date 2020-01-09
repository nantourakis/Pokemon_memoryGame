using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using MemoryGame.Classes;
using MemoryGame.Interfaces;

namespace MemoryGame
{
    public partial class ViewController : UIViewController
    {
        //declare game variables
        float gameViewWidth;
        int gridCellSize = 4;
        int gameBoardSize = 0;
        int gameTimerCounter = 60;

        // declare our variables for handling image comparisons
        bool isComparing = false;
        bool selAllowed = true;
        int indexOfFirstTile;
        int indexOfSecondTile;
        int totalFound = 0;
        int currentScore = 0;

        // Declare game Timer, Game Tile Backgrounds
        NSTimer gameTimer;
        UIImageView firstTileImage;
        UIImageView secondTileImage;

        // Declare our Variables for our game Images and Tile Indexes arrays
        List<UIImageView> gameTileImagesArray = new List<UIImageView>();

        public ViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        // layout our game board and load game tiles
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            // make sure game view is laid out
            gameBoardView.LayoutIfNeeded();
            gameViewWidth = (float)gameBoardView.Frame.Size.Width;

            GameTile tile = new GameTile();
            tile.LoadGameBoardTiles();

            // call our instance method to start a new game
            resetGameVariables();
            startNewGame();

        }

        // handle touch events
        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            // Get the touch that was activated within the view
            UITouch myTouch = (UITouch)touches.AnyObject;
            if (gameTileImagesArray.Contains(myTouch.View) && selAllowed)
            {
                // get the image at the postion within the view
                UIImageView gameBoardTile = (UIImageView)myTouch.View;

                // Get the index of the image within out array
                int tileIndex = gameTileImagesArray.IndexOf(gameBoardTile);
                int imageIndex = (tileIndex < gameBoardSize ? tileIndex :
                    tileIndex - gameBoardSize);

                // call our instance method to handle flipping of the chosen tiles
                flipGameBoardTiles(gameBoardTile, tileIndex, imageIndex);
            }
        }

        void flipGameBoardTiles(UIImageView gameBoardTile, int tileIndex, int imageIndex)
        {
            UIView.Transition(gameBoardTile, .5,
                                UIViewAnimationOptions.TransitionFlipFromRight,
            () => // animation block, animation beginds
            {
                // get the image from our array that was chosen
                gameBoardTile.Image = GameTile.GameBoardObjects[imageIndex].Image;
            }, () => // animation complete
            {
                // check to see if we are performaing a comparison
                if (isComparing)
                {
                    //get the index of the second tile that was selected
                    indexOfSecondTile = tileIndex;
                    secondTileImage = gameBoardTile;
                    doGameTilesMatch();

                    if (totalFound == gameBoardSize)
                    {
                        gameTimer.Invalidate();
                        startNewGame();
                    }
                    isComparing = false;
                }
                else
                {
                    indexOfFirstTile = tileIndex;
                    firstTileImage = gameBoardTile;
                    isComparing = true;
                }
            }); 
        }

        // instance method to check if our game tiles match
        void doGameTilesMatch()
        {
            int chosenImageIndex = Math.Abs(indexOfFirstTile - indexOfSecondTile);

            if (chosenImageIndex == gameBoardSize)
            {
                // remove the matched tiles from the views superview
                firstTileImage.RemoveFromSuperview();
                secondTileImage.RemoveFromSuperview();

                // if we have a matched title, increment our total count found
                totalFound++;
                currentScore += 10;
                scoreLabel.Text = "Score: " + currentScore.ToString();
            }
            else
            {
                // if we match an incorrect tile
                firstTileImage.Image = UIImage.FromFile("front_tile.png");
                secondTileImage.Image = UIImage.FromFile("front_tile.png");
            }
        }

        // instance method to end the current game and start a new game
        void startNewGame()
        {
            // Remove reminants of our ImageViews from our GameBoard
            foreach (UIView any in gameBoardView.Subviews)
            {
                any.RemoveFromSuperview();
            }

            // clear out our game tile images array
            gameTileImagesArray.Clear();
            gridCellSize = 4;
            gameBoardSize = (gridCellSize * gridCellSize) / 2;
            selAllowed = true;
            isComparing = false;

            // Instantiate a new instance of our GameBoard class
            var gameBoard = new GameBoard(gridCellSize, gameViewWidth);

            //Pass in values for each of the properties
            gameBoard.GameBoardView = gameBoardView;
            gameBoard.TilesImageArray = gameTileImagesArray;
            gameBoard.CreateGameBoard();
            gameBoard.ShuffleBoardTiles();

            setupGameTimer();
        }

        // instance method to reset our game timer, score and time
        void resetGameVariables()
        {
            gameTimerCounter = 60;
            scoreLabel.Text = "Score: 0";
            TimeLabel.Text = "Time: 0";
        }

        // set up our game timer
        void setupGameTimer()
        {
            gameTimer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(1.0),
                delegate
                {
                    startTimerCountDown();
                });
        }

        // method that starts our game timer countdown
        void startTimerCountDown()
        {
            //decrement our seconds counter and update label
            gameTimerCounter--;
            String timeString = "Time: " + gameTimerCounter + " sec's";
            TimeLabel.Text = timeString;

            // check to see if our countdown timer has reached zero
            if (gameTimerCounter == 0)
            {
                //stop our timer control from going negative
                gameTimer.Invalidate();

                // set up our UIAlert View Controller and action methods
                UIApplication.SharedApplication.InvokeOnMainThread(new Action(() =>
                {
                    var alert = UIAlertController.Create("Times up!", "Your Time is up! You get a score",
                        UIAlertControllerStyle.Alert);

                    // set up button event handlers
                    alert.AddAction(UIAlertAction.Create("Play Again?",
                        UIAlertActionStyle.Default, a =>
                        {
                            resetGameVariables();
                            startNewGame();
                        }));
                    alert.AddAction(UIAlertAction.Create("Cancel",
                        UIAlertActionStyle.Default,
                        null));
                    // display ui alert to current view
                    this.ShowViewController(alert, this);
                }));
            }
        }

        // method that will reset the current game in progress
        partial void ResetGame_Clicked(UIButton sender)
        {
            // set up ui alert view controller and action methods
            UIApplication.SharedApplication.InvokeOnMainThread(new Action(() =>
            {
                var alert = UIAlertController.Create("Reset Game", "are you sure you want to start again?",
                    UIAlertControllerStyle.Alert);

                //set up button event handlers
                alert.AddAction(UIAlertAction.Create("ok",
                    UIAlertActionStyle.Default, a =>
                    {
                        gameTimer.Invalidate();
                        resetGameVariables();
                        startNewGame();
                    }));
                alert.AddAction(UIAlertAction.Create("Cancel",
                    UIAlertActionStyle.Default,
                    null));

                // display the ui view to the current view
                this.ShowViewController(alert, sender);
            }));
        }

        public override void DidReceiveMemoryWarning ()
        {
            base.DidReceiveMemoryWarning ();
            // Release any cached data, images, etc that aren't in use.
        }

    }
}