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

            pictureBox2.Image = secondImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;            
        }

        /*button to generate motion vectors*/
        private void button1_Click(object sender, EventArgs e)
        {
            Block[,] firstBlocks = ImageFunctions.turnImageToBlockArray((Bitmap)pictureBox1.Image);
            //Block[,] secondBlocks = ImageFunctions.turnImageToBlockArray((Bitmap)pictureBox2.Image);

            VideoCompression vidcom = new VideoCompression();
            int range = 15;

            double[,] dImage = ImageFunctions.bitmapToDoubleArray((Bitmap)pictureBox2.Image);

            for (int y=0; y<firstBlocks.GetLength(1); y++) {
                for (int x=0; x<firstBlocks.GetLength(0); x++) {
                    Point point = vidcom.getVector(dImage, firstBlocks[x,y], x*8, y*8, range);
                    Debug.Write("("+point.X+","+point.Y+"),");
                }
                Debug.WriteLine("");
            }
            
        }
        
        
    }
}
