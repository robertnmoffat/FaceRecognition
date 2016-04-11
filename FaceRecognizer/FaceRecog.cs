using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognizer
{
    class FaceRecog
    {
        public const double SAME_FACE_THRESH = 7.0;
        public const double FACE_THRESH = 16000;
        private const int REGULAR = 0, DIFFERENCE = 1, EIGEN = 2;
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


        public FaceRecog() {
            mainBmp = new Bitmap(Image.FromFile("./plane.bmp"));

            //Would be picturebox of compared image.
            //pb_main.Image = mainBmp;

            double[,] img = ImageTool.GetGreyScale(mainBmp);
            ImageTool.SetImage(mainBmp, img);

            int libCount = LoadLibrary("./imgLib", mainBmp.Width, mainBmp.Height, FACES_PER_PERSON);
            avg = ImageTool.GetAvg(lib);
            difLib = ImageTool.GetDifferenceArray(lib, avg);

            //Would be scrollbar
            //sb_lib.Maximum = libCount;

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

            //Would be guess of face
            //lb_person.Text = "Person: " + p;

            recon = ImageTool.reconstruct(weights, eigFaces, avg);
            ImageTool.normalize(recon, 255);

            //would be displaying the image.
            //pb_lib.Image = libBmp;

            ImageTool.SetImage(libBmp, lib[p]);

            //Would be setting scrollbar to correct position
            //sb_lib.Value = p;
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

        public string loadFace(Bitmap mainBmp)
        {
            String output;
            //display image
            //pb_main.Image = mainBmp;

            double[,] img = ImageTool.GetGreyScale(mainBmp);
            ImageTool.SetImage(mainBmp, img);
            double[] weights = ImageTool.getWeights(eigFaces, img, avg);
            comp = ImageTool.compareWeigths(libWeights, weights);
            int p = ImageTool.smallestVal(comp);

            if (comp[p] > SAME_FACE_THRESH)
            {
                //lb_person.Text = "Person: Unknown";
                output = "Person: Unknown";
            }
            else
            {
                //lb_person.Text = "Person: " + p;
                output = "Person: " + p;
            }
            recon = ImageTool.reconstruct(weights, eigFaces, avg);

            //pb_lib.Image = libBmp;
            ImageTool.SetImage(libBmp, lib[p]);
            //sb_lib.Value = p;
            //lb_distance.Text = "Distance : " + comp[p];
            faceSpace = ImageTool.difference(img, recon);
            //lb_faceSpace.Text = "Face Space : " + faceSpace;
            if (faceSpace > FACE_THRESH)
            {
                //lb_faceSpace.Text += "\nNot a face";
                output = "Not a face";
            }

            return output;
        }
    }
}
