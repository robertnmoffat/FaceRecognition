using ILNumerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognizer
{
    class EigenObject
    {
        public complex[,] vectors;
        public complex[] values;
        
        public EigenObject(complex[,] vec, complex[] val)
        {
            vectors = vec;
            values = val;
        }
        public EigenObject(ILArray<complex> vec, ILArray<complex> val, int width)
        {
            vectors = ILArray2Array(vec, width);
            values = ILArray2Values(val, width);
        }

        public complex[,] ILArray2Array(ILArray<complex> il, int width)
        {
            complex[,] result = new complex[width, width];
            complex[] ila = il.ToArray();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    result[i, j] = ila[i * width + j];
                }
            }
            return result;
        }

        public complex[] ILArray2Values(ILArray<complex> il, int width)
        {
            complex[] result = new complex[width];
            complex[] ila = il.ToArray();
            for (int i = 0; i < width; i++)
            {
                result[i] = ila[i * width + i];
            }
            return result;
        }
    }
}
