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
    }
}
