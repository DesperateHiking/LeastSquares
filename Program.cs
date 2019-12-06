using System;

namespace LeastSquares
{
    class Program
    {
        public static double[] GaussMethod(double[,] mat)
        {
            Print(mat);
            int rows = mat.GetLength(0);
            int columns = mat.GetLength(1);
            double tmp, delta = 0.00001;

            for (var columnIteration = 0; columnIteration < rows; columnIteration++)
            {
                for (var i = columnIteration; i < rows; i++)
                {
                    tmp = mat[i, columnIteration];
                    if (Math.Abs(tmp) > delta)
                    {
                        for (var j = 0; j < columns; j++)
                        {
                            mat[i, j] /= tmp;
                        }
                    }

                    if (i != columnIteration)
                    {
                        for (var j = 0; j < columns; j++)
                        {
                            mat[i, j] -= mat[columnIteration, j];
                        }
                    }
                }
                Print(mat);
            }

            var solution = new double[columns - 1];

            for (var i = 0; i < rows; i++)
            {
                solution[i] = mat[i, columns - 1];
            }

            for (var i = rows - 2; i >= 0; i--)
            {
                for (var j = i + 1; j < rows; j++)
                {
                    solution[i] -= solution[j] * mat[i, j];
                }

                if (Math.Abs(solution[i]) < delta)
                    solution[i] = 0;
            }

            return solution;
        }

        public static void Print(double[,] mat)
        {
            int rows = mat.GetLength(0);
            int columns = mat.GetLength(1);
            var tmp = new double[rows, columns];

            Console.WriteLine();
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    if ((mat[i, j] % 1).ToString().Length > 3)
                        tmp[i, j] = Math.Round(mat[i, j], 2);
                    if (j == columns - 1)
                        Console.Write("= {0}", tmp[i, j]);
                    else Console.Write("{0}*X{1} ", tmp[i, j], j + 1);
                }
                Console.Write("\n");
            }
            Console.WriteLine();
        }

        static void Quad(double[] xArr, double[] yArr)
        {
            var n = xArr.Length;
            var res = new double[2 * n - 1];

            for (var i = 0; i < n; i++)
            {
                for (var pow = 1; pow < n + 1; pow++)
                {
                    double tmp = 1;
                    for (var j = 0; j < pow; j++)
                    {
                        tmp *= xArr[i];
                    }
                    res[pow - 1] += tmp;
                }

                res[4] += yArr[i];
                res[5] += xArr[i] * yArr[i];
                res[6] += yArr[i] * (xArr[i] * xArr[i]);
            }

            var inputMatrix = new double[,] {
                { res[3], res[2], res[1], res[6] },
                { res[2], res[1], res[0], res[5] },
                { res[1], res[0], n, res[4] }};
            var solution = GaussMethod(inputMatrix);

            for (var i = 0; i< solution.Length; i++)
                solution[i] = Math.Round(solution[i], 3);

            Console.WriteLine("Полиномиальная аппроксимация");
            Console.WriteLine("y(x) = {0}*x^2 + {1}*x + {2}", solution[0], solution[1], solution[2]);
        }

        static void Linear(double[] xArr, double[] yArr)
        {
            var n = xArr.Length;
            var res = new double[n];

            for (var i = 0; i < n; i++)
            {
                res[0] += xArr[i];
                res[1] += yArr[i];
                res[2] += xArr[i] * yArr[i];
                res[3] += xArr[i] * xArr[i];
            }
            var inputMatrix = new double[,] {
                { res[3], res[0], res[2] },
                { res[0], n, res[1] } };
            var solution = GaussMethod(inputMatrix);

            Console.WriteLine("Линейная аппроксимация");
            Console.WriteLine("y(x) = {0}*x + {1}", solution[0], solution[1]);
        }

        static void Main(string[] args)
        {
            var xArr = new double[] { -2, 1, 2, 3 };
            var yArr = new double[] { -13, 5, 11, 17 };
            Linear(xArr, yArr);
            Quad(xArr, yArr);
            Console.ReadLine();
        }
    }
}
