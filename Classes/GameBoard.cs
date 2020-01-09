using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using MemoryGame.Interfaces;
using UIKit;

namespace MemoryGame.Classes
{
    public class GameBoard : IGameBoard
    {
        //Define the properties that will be used by our class
        public float BoardWidth { get; set; }
        public UIView GameBoardView { get; set;  }
        public List<UIImageView> TilesImageArray { get; set; }

        // Variables for Board Grid Size and Game View Width
        int boardGridSize;
        float gameViewWidth;

        GameTile[,] tiles = new GameTile[4, 4];
        List<CGPoint> GameTileCoords { get; } = new List<CGPoint>();

        // GameBoard Class Constructor
        public GameBoard(int boardGridSize, float gameViewWidth)
        {
            this.boardGridSize = boardGridSize;
            this.gameViewWidth = gameViewWidth;
        }

        // instance method to create our Game Board & Game Tiles
        public void CreateGameBoard()
        {
            // Specify our tile width and tile centre values
            float tileWidth = this.gameViewWidth / this.boardGridSize;
            float tileCenterX = tileWidth / 2;
            float tileCenterY = tileWidth / 2;

            //initialise our image counter position
            int imageIndex = 0;
            
            // Build our game board with images from our array
            for(int row = 0; row < this.boardGridSize; row++)
            {
                for(int column = 0; column < this.boardGridSize; column++)
                {
                    // create a new instance of our image tile
                    GameTile tile = new GameTile(row, column);
                    tile.Image = GameTile.GameBoardObjects[imageIndex].Image;
                    tile.Center = new CGPoint(tileCenterX, tileCenterY);

                    // convert the image tile into a circle
                    var theTileImageView = tile.CreateCircle(tileWidth - 5, tileWidth - 5);
                    theTileImageView.UserInteractionEnabled = true;

                    // Store our Tile Coordinates within our ArrayList object
                    GameTileCoords.Add(new CGPoint(tileCenterX, tileCenterY));

                    // Add the tile to our Tile Images
                    TilesImageArray.Add(theTileImageView);
                    GameBoardView.AddSubview(theTileImageView);

                    // Increment to the next tile position and image within array
                    tileCenterX = tileCenterX + tileWidth;
                    imageIndex++;

                    // if we have reached the end of our row, reset out index
                    if (imageIndex == this.boardGridSize)
                        imageIndex = 0;
                }
                tileCenterX = tileWidth / 2;
                tileCenterY = tileCenterY + tileWidth;
            }
        }

        // Instand method to randomly shuffle our game tiles
        public void ShuffleBoardTiles()
        {
            foreach (UIImageView any in TilesImageArray)
            {
                Random randGen = new Random();
                int randomIndex = randGen.Next(0, GameTileCoords.Count);
                CGPoint randomCentre = (CGPoint)GameTileCoords[randomIndex];
                any.Image = UIImage.FromFile("front_tile.png");
                any.Center = randomCentre;
                GameTileCoords.RemoveAt(randomIndex);
            }
        }
    }
}