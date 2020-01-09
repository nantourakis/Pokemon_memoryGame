using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace MemoryGame.Interfaces
{
    public interface IGameBoard
    {
        //Instance method to create our Game Board
        void CreateGameBoard();

        //Instance method to shuffle tiles within the Game Board
        void ShuffleBoardTiles();
    }
}