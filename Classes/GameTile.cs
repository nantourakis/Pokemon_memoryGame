using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemoryGame.Classes;
using MemoryGame.Interfaces;
using Foundation;
using UIKit;

namespace MemoryGame.Classes
{
    class GameTile : UIImageView, IGameTile
    {
        // Constant variable for total number of Game Images
        const int MAX_GAME_IMAGES = 9;

        // ImageTile Class Constructor
        public GameTile()
        {
        }

        // Overload our ImageTile Class Constructor
        public GameTile(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        // Instance method to load our game tiles
        public void LoadGameBoardTiles()
        {
            // TODO: Load Game Board Tiles Implementation
            for (int imageNo = 1; imageNo <= MAX_GAME_IMAGES; imageNo++)
            {
                var boardImage = new UIImageView(new UIImage("img_" + imageNo + ".png"));
                GameBoardObjects.Add(boardImage);
            }
        }

        // Instance method to create a circle from an ImageView
        public UIImageView CreateCircle(float tileWidth, float tileHeight)
        {
            // Instantiate an instance of our ImageCircle Class
            var tileImage = new ImageCircle(tileWidth, tileHeight);
            tileImage.ImageName = this.Image;
            tileImage.ImageCenter = this.Center;
            return tileImage.CreateImageCircle();
        }

        // Define the properties that will be used by our class
        public int Row { private set; get; }
        public int Col { private set; get; }

        // Property to hold our Memory Game Board Images
        public static List<UIImageView> GameBoardObjects { get; } = new List<UIImageView>();
    }
}