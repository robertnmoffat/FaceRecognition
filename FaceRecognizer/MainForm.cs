using ILNumerics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceRecognizer
{
    public partial class MainForm : Form
    {
        public const double SAME_FACE_THRESH = 7.0;
        public const double FACE_THRESH = 16000;
        private const int  REGULAR = 0, DIFFERENCE = 1, EIGEN = 2;
        private const int FACES_PER_PERSON = 3;
        double[][,] lib;
        double[][,] difLib;
        double[][,] eigFaces;
        double[,] avg;
        double[,] recon;
        double[][] libWeights;
        double[] comp;
        int display;
        Bitmap mainBmp;
        Bitmap libBmp;
        double faceSpace;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            mainBmp = new Bitmap(Image.FromFile("./plane.bmp"));
            pb_main.Image = mainBmp;
            double[,] img = ImageTool.GetGreyScale(mainBmp);
            ImageTool.SetImage(mainBmp, img);

            int libCount = LoadLibrary("./imgLib", mainBmp.Width, mainBmp.Height, FACES_PER_PERSON);
            avg = ImageTool.GetAvg(lib);
            difLib = ImageTool.GetDifferenceArray(lib, avg);
            sb_lib.Maximum = libCount;
            libBmp = new Bitmap(mainBmp.Width, mainBmp.Height);
            EigenObject eigVects = ImageTool.GetEigen(ImageTool.GetA(lib));
            ImageTool.normalize(eigVects.vectors);
            eigFaces = ImageTool.getEigenFaces(eigVects.vectors, difLib);

            libWeights = new double[lib.Length][];
            for (int i = 0; i < lib.Length; i++)
            {
                libWeights[i] = ImageTool.getWeights(eigFaces, lib[i], avg);
            }
            double[] weights = ImageTool.getWeights(eigFaces, img, avg);
            comp = ImageTool.compareWeigths(libWeights, weights);
            int p = ImageTool.smallestVal(comp);
            lb_person.Text = "Person: " + p;
            recon = ImageTool.reconstruct(weights, eigFaces, avg);
            ImageTool.normalize(recon, 255);

            pb_lib.Image = libBmp;
            ImageTool.SetImage(libBmp, lib[p]);
            sb_lib.Value = p;
        }

        private int LoadLibrary(string directory, int width, int height, int subSet)
        {
            string[] images = Directory.GetFiles(@directory, "*.jpg");
            if (subSet < 1) 
                subSet = 1;
            lib = new double[images.Length][,];
            int i = 0;
            foreach (string image in images)
            {
                lib[i++] = ImageTool.GetArray(new Bitmap(image));
            }
            if (subSet > 1)
                lib = ImageTool.avgSubsets(lib, subSet);
            return images.Length / subSet;
        }

        private void sb_lib_Scroll(object sender, ScrollEventArgs e)
        {
            switch (display)
            {
                
                case DIFFERENCE:
                    ImageTool.SetImage(libBmp, difLib[e.NewValue], true);
                    break;
                case EIGEN:
                    ImageTool.SetImage(libBmp, eigFaces[e.NewValue], true);
                    break;
                case REGULAR:
                default:
                    ImageTool.SetImage(libBmp, lib[e.NewValue]);
                    break;
            }
            pb_lib.Image = libBmp;
            lb_distance.Text = "Distance : " + comp[e.NewValue];
        }

        private void rb_reg_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                display = REGULAR;
            }
        }

        private void rb_dif_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                display = DIFFERENCE;
            }
        }

        private void rb_eig_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                display = EIGEN;
            }
        }

        private void bt_avg_Click(object sender, EventArgs e)
        {
            ImageTool.SetImage(libBmp, avg);
            pb_lib.Image = libBmp;
        }

        private void bt_recon_Click(object sender, EventArgs e)
        {
            ImageTool.SetImage(libBmp, recon, false);
            pb_lib.Image = libBmp;
        }

        private void bt_load_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog(); 
            ofd.Filter = "Jpeg Files (.jpg)|*.jpg|Bitmap Files (.bmp)|*.bmp|All Files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                mainBmp = new Bitmap(Image.FromFile(ofd.FileName));
                pb_main.Image = mainBmp;
                double[,] img = ImageTool.GetGreyScale(mainBmp);
                ImageTool.SetImage(mainBmp, img);
                double[] weights = ImageTool.getWeights(eigFaces, img, avg);
                comp = ImageTool.compareWeigths(libWeights, weights);
                int p = ImageTool.smallestVal(comp);

                if (comp[p] > SAME_FACE_THRESH)
                {
                    lb_person.Text = "Person: Unknown";
                }
                else
                {
                    lb_person.Text = "Person: " + p;
                }
                recon = ImageTool.reconstruct(weights, eigFaces, avg);

                pb_lib.Image = libBmp;
                ImageTool.SetImage(libBmp, lib[p]);
                sb_lib.Value = p;
                lb_distance.Text = "Distance : " + comp[p];
                faceSpace = ImageTool.difference(img, recon);
                lb_faceSpace.Text = "Face Space : " + faceSpace;
                if (faceSpace > FACE_THRESH)
                {
                    lb_faceSpace.Text += "\nNot a face";
                }
            }
        }
    }
}
