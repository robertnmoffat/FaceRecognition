using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition
{
    class ImageFunctions
    {

        public static Bitmap convertToGreyscale(Bitmap coloured) {
            Bitmap greyscale = new Bitmap(coloured.Width, coloured.Height);
            Color tempColor, tempGrey;

            for (int y=0; y<coloured.Height; y++) {
                for (int x=0; x<coloured.Width; x++) {                    
                    tempColor = coloured.GetPixel(x,y);
                    tempGrey = Color.FromArgb((tempColor.R + tempColor.B + tempColor.G) / 3, 
                        (tempColor.R + tempColor.B + tempColor.G) / 3, 
                        (tempColor.R + tempColor.B + tempColor.G) / 3);

                    greyscale.SetPixel(x, y, tempGrey);
                }
            }

            return greyscale;
        }

        public static Bitmap combineImages(Bitmap main, Bitmap addition, int amount) {
            if (amount == 0||amount==1) return addition;

            Bitmap output = new Bitmap(main.Width, main.Height);

            for (int y=0; y<main.Height; y++) {
                for (int x=0; x<main.Width; x++) {
                    int mainC = main.GetPixel(x, y).R;
                    int additionC = addition.GetPixel(x, y).R;
                    double weight = 1 / (double)amount;
                    int rgb = (int)(additionC * weight + mainC * (1 - weight));
                    output.SetPixel(x,y,Color.FromArgb(rgb,rgb,rgb));
                }
            }
            return output;
        }

        /// <summary>
        /// Generate an 8x8 block starting at the position passed in the image passed
        /// </summary>
        /// <param name="startX">horizontal starting point of block</param>
        /// <param name="startY">vertical starting point of block</param>
        /// <param name="image">image from which the block is being generated</param>
        /// <returns>block of pixels</returns>
        public static Block generateBlockFromPosition(int startX, int startY, Bitmap image) {
            Block block = new Block();
            double scale;

            for (int y=0; y<8; y++) {
                for (int x=0; x<8; x++){
                    if (x + startX >= image.Width || y + startY >= image.Height)
                        scale = 0;
                    else
                        scale = image.GetPixel(x+startX, y+startY).R;
                    block.set(x,y,scale);
                }
            }

            return block;
        }

        public static Color convertPixelToGreyscale(Color tempColor) {
            return Color.FromArgb((tempColor.R + tempColor.B + tempColor.G) / 3,
                        (tempColor.R + tempColor.B + tempColor.G) / 3,
                        (tempColor.R + tempColor.B + tempColor.G) / 3);
        }

        /// <summary>
        /// Turns a bitmap image into a 2d Block array
        /// </summary>
        /// <param name="image">image to be broken down</param>
        /// <returns>block array</returns>
        public static Block[,] turnImageToBlockArray(Bitmap image)
        {
            image = convertToGreyscale(image);//making sure that the image is greyscale before breaking into blocks

            double width = image.Width;
            double height = image.Height;
            int horizontalBlocks, verticalBlocks;

            //use ceiling to make sure block amount covers the full size of the image.
            //out of bounds block points will be filled with zeros
            horizontalBlocks = (int)Math.Ceiling(width / 8);
            verticalBlocks = (int)Math.Ceiling(height / 8);

            Block[,] blockArray = new Block[horizontalBlocks, verticalBlocks];

            for (int y=0; y<verticalBlocks*8; y+=8) {
                for (int x=0; x<horizontalBlocks*8; x+=8) {
                    blockArray[x/8,y/8] = generateBlockFromPosition(x,y,image);
                }
            }

            return blockArray;
        }

        public static Bitmap getRectOfBitmap(Bitmap image, Rectangle rect) {
            // Create the new bitmap and associated graphics object
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage(bmp);

            // Draw the specified section of the source bitmap to the new one
            g.DrawImage(image, 0, 0, rect, GraphicsUnit.Pixel);

            // Clean up
            g.Dispose();

            // Return the bitmap
            return bmp;
        }

        public static double[,] bitmapToDoubleArray(Bitmap image) {
            double[,] dImage = new double[image.Width,image.Height];

            image = convertToGreyscale(image);

            for (int y=0; y<image.Height; y++) {
                for (int x=0; x<image.Width; x++) {
                    dImage[x, y] = image.GetPixel(x,y).R;
                }
            }
            return dImage;
        }

        /// <summary>
        /// Downscale a bitmap
        /// </summary>
        /// <param name="input">bitmap to be downscaled</param>
        /// <param name="timesSmaller">how many times smaller you want it</param>
        /// <returns></returns>
        public static Bitmap downsizeBitmap(Bitmap input, int timesSmaller)
        {
            Bitmap output = new Bitmap(input.Width / timesSmaller + 1, input.Height / timesSmaller + 1);

            for (int y = 0; y < input.Height; y += timesSmaller)
            {
                for (int x = 0; x < input.Width; x += timesSmaller)
                {
                    output.SetPixel(x / timesSmaller, y / timesSmaller, input.GetPixel(x, y));
                }
            }

            return output;
        }

        public static Rectangle findBlobs(Bitmap image, Rectangle r) {
            int lowestX = 99999;
            int highestX = -1;
            int lowestY = 99999;
            int highestY = -1;          

            Color pixel;
            int distanceChangeThreshold = 20;

            for (int y=0; y<image.Height; y++) {
                for (int x=0; x<image.Width; x++) {
                    pixel = image.GetPixel(x,y);
                    if (pixel.R == 0) {
                        if (x < lowestX)
                            lowestX = x;
                        if (x > highestX)
                            highestX = x;
                        if (y < lowestY)
                            lowestY = y;
                        if (y > highestY)
                            highestY = y;
                    }
                }
            }

            if (highestY-lowestY > 1.1 * (highestX - lowestX)) {
                int space = highestY - lowestY;
                highestY = space / 2 + lowestY;
                space = highestY - lowestY;//do again with new y
                int diff = highestX - lowestX - space;
                lowestX += diff / 2;
                highestX -= diff / 2;
            }

            Rectangle rect;

            if (lowestX != 99999 && lowestY != 99999 && highestX != -1 && highestY != -1)
            {

                rect = new Rectangle(lowestX, lowestY, highestX - lowestX, highestY - lowestY);                
            }
            else
                rect = r;

            

            return rect;
        }


    }
}
