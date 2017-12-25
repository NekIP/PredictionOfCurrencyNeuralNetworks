using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork.Helper
{
    public class Vector
    {
        public double[] Values { get; private set; }

        public int Length => Values.Length;

        public Vector(double[] values)
        {
            Values = values;
        }

        public Vector(int length)
        {
            Values = new double[length];
        }

        public double this[int index]
        {
            get
            {
                return Values[index];
            }
            set
            {
                Values[index] = value;
            }
        }

        public static Vector operator+(Vector item1, Vector item2)
        {
            if (item1.Length != item2.Length)
            {
                throw new ArithmeticException("Length of vectors must be equals");
            }
            var result = new Vector(item1.Length);
            for (var i = 0; i < item1.Length; i++)
            {
                result[i] = item1[i] + item2[i]; 
            }
            return result;
        }

        public static Vector operator -(Vector item1, Vector item2)
        {
            return item1 + (-item2);
        }

        public static Vector operator -(Vector item)
        {
            var result = new Vector(item.Length);
            for (var i = 0; i < item.Length; i++)
            {
                result[i] = -item[i];
            }
            return result;
        }
    }
}
