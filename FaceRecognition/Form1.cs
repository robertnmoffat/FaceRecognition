using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceRecognition
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loadImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap firstImage = new Bitmap(10,10);
            Bitmap secondImage = new Bitmap(10, 10);

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.InitialDirectory = @"N:\My Documents\My Pictures";
            openFileDialog1.Filter = "JPEG Compressed Image (*.jpg|*.jpg" + "|GIF Image(*.gif|*.gif" + "|Bitmap Image(*.bmp|*.bmp";
            openFileDialog1.Multiselect = true;
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                firstImage = new Bitmap(openFileDialog1.FileName);                
            }
            firstImage.SetResolution(256,256);
            pictureBox1.Image = firstImage;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.InitialDirectory = @"N:\My Documents\My Pictures";
            openFileDialog1.Filter = "JPEG Compressed Image (*.jpg|*.jpg" + "|GIF Image(*.gif|*.gif" + "|Bitmap Image(*.bmp|*.bmp";
            openFileDialog1.Multiselect = true;
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                secondImage = new Bitmap(openFileDialog1.FileName);
            }
            secondImage.SetResolution(256,256);
            pictureBox2.Image = secondImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;            
        }

        /*button to generate motion vectors*/
        private void button1_Click(object sender, EventArgs e)
        {
            VideoCompression vidcom = new VideoCompression();

            Bitmap firstImage = ImageFunctions.downsizeBitmap((Bitmap)pictureBox1.Image, 1);
            Bitmap secondImage = ImageFunctions.downsizeBitmap((Bitmap)pictureBox2.Image, 1);

            Block[,] firstBlocks = ImageFunctions.turnImageToBlockArray(firstImage);
            //Block[,] secondBlocks = ImageFunctions.turnImageToBlockArray((Bitmap)pictureBox2.Image);
            
            int range = 15;

            double[,] dImage = ImageFunctions.bitmapToDoubleArray(secondImage);

            List<MotionBlob> blobs = new List<MotionBlob>();
            //List<Point> points = new List<Point>();
            Point[,] points = new Point[firstBlocks.GetLength(0),firstBlocks.GetLength(1)];

            for (int y=0; y<firstBlocks.GetLength(1); y++) {
                for (int x=0; x<firstBlocks.GetLength(0); x++) {                   

                    int pixelX = x * 8;
                    int pixelY = y * 8;
                                        
                    //points.Add(vidcom.getVector(dImage, firstBlocks[x, y], pixelX, pixelY, range));
                    points[x, y] = vidcom.getVector(dImage, firstBlocks[x, y], pixelX, pixelY, range);

                    //Debug.Write("("+point.X+","+point.Y+"),");
                }
                //Debug.WriteLine("");
            }


            for (int y = 0; y < firstBlocks.GetLength(1); y++)
            {
                for (int x = 0; x < firstBlocks.GetLength(0); x++)
                {
                    int pixelX = x * 8;
                    int pixelY = y * 8;

                    bool blobFound = false;
                    int distanceThreshold = 10;
                    foreach (MotionBlob currentBlob in blobs)
                    {
                        if (currentBlob.vector.Equals(points[x,y]))
                        {
                            //Check if close enough to join this blob
                            if (!currentBlob.withinThreshold(new Point(pixelX, pixelY))) break;

                            if (pixelX < currentBlob.topLeft.X) currentBlob.topLeft.X = pixelX;
                            if (pixelY < currentBlob.topLeft.Y) currentBlob.topLeft.Y = pixelY;

                            if (pixelX > currentBlob.topRight.X) currentBlob.topRight.X = pixelX;
                            if (pixelY < currentBlob.topRight.Y) currentBlob.topRight.Y = pixelY;

                            if (pixelX < currentBlob.bottomLeft.X) currentBlob.bottomLeft.X = pixelX;
                            if (pixelY > currentBlob.bottomLeft.Y) currentBlob.bottomLeft.Y = pixelY;

                            if (pixelX > currentBlob.bottomRight.X) currentBlob.bottomRight.X = pixelX;
                            if (pixelY > currentBlob.bottomRight.Y) currentBlob.bottomRight.Y = pixelY;

                            currentBlob.count++;

                            blobFound = true;
                        }
                    }

                    if (blobFound == false)
                    {
                        MotionBlob newBlob = new MotionBlob();
                        newBlob.vector.X = points[x,y].X;
                        newBlob.vector.Y = points[x, y].Y;

                        newBlob.topLeft.X = pixelX;
                        newBlob.topLeft.Y = pixelY;
                        newBlob.topRight.X = pixelX + 8;
                        newBlob.topRight.Y = pixelY;
                        newBlob.bottomLeft.X = pixelX;
                        newBlob.bottomLeft.Y = pixelY + 8;
                        newBlob.bottomRight.X = pixelX + 8;
                        newBlob.bottomRight.Y = pixelY + 8;

                        blobs.Add(newBlob);
                    }
                }
            }


            Bitmap postImage = (Bitmap)pictureBox1.Image.Clone();
            
            using (Graphics g = Graphics.FromImage(postImage))
            {
                //g.Clear(Color.White);
                
                Pen pen = new Pen(Color.Yellow);
                foreach (MotionBlob currentBlob in blobs)
                {
                    if (currentBlob.count > 1)
                    {
                        g.DrawLine(pen, currentBlob.topLeft, currentBlob.topRight);
                        g.DrawLine(pen, currentBlob.topRight, currentBlob.bottomRight);
                        g.DrawLine(pen, currentBlob.bottomRight, currentBlob.bottomLeft);
                        g.DrawLine(pen, currentBlob.bottomLeft, currentBlob.topLeft);
                        
                    }

                }
            }

            postImage = vidcom.movementDifference(firstImage,secondImage);

            pictureBox3.Image = postImage;

        }
        
        
    }
}
