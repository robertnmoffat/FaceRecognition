using AForge.Video;
using AForge.Video.DirectShow;
using FaceRecognizer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceRecognition
{
    public partial class Form1 : Form
    {
        FaceRecog faceRecog = new FaceRecog();

        int faceMissed = 15;

        VideoCaptureDevice vidCap;
        FilterInfoCollection videoDevices;
        Bitmap previousFrame;
        Bitmap currentFrame;
        int frameOffset = 0;

        Object currentFrameLock = new object();
        Object previousFrameLock = new object();

        Rectangle rect = new Rectangle(0,0,0,0);
        Bitmap differenceImage;
        Graphics g;
        Pen p = new Pen(Color.Red);
        private TextBox textBox1;
        VideoCompression vidcom = new VideoCompression();

        public Form1()
        {
            InitializeComponent();
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            Debug.WriteLine(videoDevices.Count);
            foreach (FilterInfo device in videoDevices) {
                Debug.WriteLine(device.Name);
            }
            vidCap = new VideoCaptureDevice(videoDevices[0].MonikerString);
            vidCap.NewFrame += new NewFrameEventHandler(video_newFrame);
            
            string highestSolution = "0;0" ;
            for (int i = 0; i < vidCap.VideoCapabilities.Length; i++) {
                highestSolution = vidCap.VideoCapabilities[i].FrameSize.Width.ToString() + ";" + i.ToString();
            }
            
            vidCap.SnapshotResolution = vidCap.VideoCapabilities[Convert.ToInt32(highestSolution.Split(';')[1])];
            vidCap.Start();            
        }
        

        private void video_newFrame(object sender, NewFrameEventArgs eventArgs) {
            frameOffset++;
            if (frameOffset > 4)
            {
                setPreviousFrame(getCurrentFrame());
                //previousFrame = currentFrame;
                setCurrentFrame((Bitmap)eventArgs.Frame.Clone());

                Bitmap cur = getCurrentFrame();
                Bitmap prev = getPreviousFrame();

                //currentFrame = (Bitmap)eventArgs.Frame.Clone();
                if (prev == null) setPreviousFrame(cur);
                //pictureBox3.Image = currentFrame;
                //pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                //Debug.WriteLine(currentFrame.Width + "," + currentFrame.Height);             


                g = Graphics.FromImage(cur);
                g.DrawRectangle(p, rect);
                //g.DrawRectangle(p,10,10,10,frameOffset);
                pictureBox3.Image = cur;
                pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;


                //frameOffset = 0;

            } 
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
            for (int i=0; i<faceRecog.candidates.Length; i++) {
                faceRecog.candidates[i].Save("./imgLib/face" + (faceRecog.imageCount+i) + ".jpg", ImageFormat.Jpeg);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread loop = new Thread(videoLoop);
            loop.Start();
        }

        public void videoLoop() {
            while (true)
            {
                if (frameOffset > 5)
                {
                    Bitmap cur = getCurrentFrame();
                    Bitmap prev = getPreviousFrame();

                    differenceImage = VideoCompression.movementDifference(cur, prev);
                    rect = ImageFunctions.findBlobs(differenceImage,rect);
                    

                    if (rect.Width!=0&&rect.Height!=0) {
                        Bitmap resized = new Bitmap(ImageFunctions.getRectOfBitmap(cur, rect), new Size(256,256));
                        string face = faceRecog.loadFace(resized);
                        if (face.Contains("Person")&&faceMissed>0)
                            faceMissed--;
                        else if(faceMissed<20)
                            faceMissed++;
                        Debug.WriteLine(face);
                    }

                    if (faceRecog.currentFace != null)
                    {
                        pictureBox1.Image = faceRecog.currentFace;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox2.Image = faceRecog.candidateFace;
                        pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                    }

                    if (faceMissed >= 15)
                        p.Color = Color.Red;
                    else
                        p.Color = Color.Green;

                    //frameOffset = 0;
                }
            }
        }

        public Bitmap getCurrentFrame() {
            lock (currentFrameLock) {
                if (currentFrame == null)
                    return null;
                return (Bitmap)currentFrame.Clone();
            }
        }

        public Bitmap getPreviousFrame() {
            lock (previousFrameLock) {
                if (previousFrame == null)
                    return null; 
                return (Bitmap)previousFrame.Clone();
            }
        }

        public void setPreviousFrame(Bitmap image) {
            lock (previousFrameLock) {
                previousFrame = image;
            }
        }

        public void setCurrentFrame(Bitmap image)
        {
            lock (currentFrameLock)
            {
                currentFrame = image;
            }
        }

    }
}
