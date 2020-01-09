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
    public class ImageCircle : IImageCircle
    {
        // Define the properties that will be used by our class
        public UIImage ImageName { get; set; }
        public CGPoint ImageCenter { get; set; }

        // Variables for image Width & Height
        float imageWidth;
        float imageHeight;

        // ImageCircle Class Constructor
        public ImageCircle(float imageWidth, float imageHeight)
        {
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
        }

        // Instance method to convert an imageview into a circle
        public UIImageView CreateImageCircle()
        {
            //Declare a new instance of our UIImageView class
            UIImageView theImageView = new UIImageView();

            // Define properties for our image view
            theImageView.Frame = new CGRect(0, 0, this.imageWidth, this.imageHeight);
            theImageView.Layer.CornerRadius = (float)(theImageView.Frame.Height / 2.0);
            theImageView.Layer.MasksToBounds = false;
            theImageView.ClipsToBounds = true;

            // Specify the image to be used, as well as specifying a centre coord
            theImageView.Center = ImageCenter;
            theImageView.Image = ImageName;

            // Return our generated ImageView
            return theImageView;
        }

    }
       
}