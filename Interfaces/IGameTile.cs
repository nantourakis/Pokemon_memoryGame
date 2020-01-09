using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace MemoryGame.Interfaces
{
    public interface IGameTile
    {

        // Instance method to load our GameBoard Tiles into an array
        void LoadGameBoardTiles();

        // Instance method to create a circle from an ImageView
        UIImageView CreateCircle(float titleWidth, float titleHeight);
    }
}