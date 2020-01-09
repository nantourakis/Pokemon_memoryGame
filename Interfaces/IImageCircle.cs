using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace MemoryGame.Interfaces
{
    public interface IImageCircle
    {
        // Instance method to convert an imageView into a Circle
        UIImageView CreateImageCircle();
    }
}