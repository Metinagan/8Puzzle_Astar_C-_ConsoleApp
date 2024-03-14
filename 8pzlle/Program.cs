using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _8pzlle
{
    class Program
    {
        static List<int[,]> mainList = new List<int[,]>();
        static List<int[]> neighborsList = new List<int[]>();
        static List<int[,]> posList = new List<int[,]>();

        static int[,] startPos = new int[,] { { 0, 2, 3 }, { 1, 4, 5 }, { 7, 8, 6 } };
        static int[,] endPos = new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 0 } };

        static void Main(string[] args)
        {
            mainList.Add(startPos);
            AStar(mainList[0]);
        }

        static int Hamming(int[,] matrix)
        {
            int hammingValue = 0;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (matrix[x, y] != endPos[x, y])
                    {
                        hammingValue++;
                    }
                }
            }
            return hammingValue;
        }

        static int Mannathen(int[,] matrix)
        {
            int mannathenValue = 0;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (matrix[x, y] != 0)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            for (int l = 0; l < 3; l++)
                            {
                                if (matrix[x, y] == endPos[k, l])
                                {
                                    int value = Math.Abs(l - y) + Math.Abs(k - x);
                                    mannathenValue += value;
                                }
                            }
                        }
                    }
                }
            }
            return mannathenValue;
        }

        static int Heuristic(int[,] matrix)
        {
            return Hamming(matrix) + Mannathen(matrix);
        }

        static Tuple<int, int> FindBlanks(int[,] matrix)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (matrix[x, y] == 0)
                    {
                        return Tuple.Create(x, y);
                    }
                }
            }
            return Tuple.Create(-1, -1); // Bulunamazsa -1, -1 döndür
        }

        static void Neighbors(int[,] matrix)
        {
            neighborsList.Clear();
            var blankLocation = FindBlanks(matrix);
            int row = blankLocation.Item1;
            int col = blankLocation.Item2;

            if (row > 0)
            {
                neighborsList.Add(new int[] { row - 1, col });
            }
            if (row < 2)
            {
                neighborsList.Add(new int[] { row + 1, col });
            }
            if (col > 0)
            {
                neighborsList.Add(new int[] { row, col - 1 });
            }
            if (col < 2)
            {
                neighborsList.Add(new int[] { row, col + 1 });
            }
        }

        static void ChangeBlanks(int[,] matrix, int row, int col)
        {
            var blankLocation = FindBlanks(matrix);
            int zeroRow = blankLocation.Item1;
            int zeroCol = blankLocation.Item2;

            int newNumber = matrix[row, col];
            int[,] newNode = (int[,])matrix.Clone();
            newNode[zeroRow, zeroCol] = newNumber;
            newNode[row, col] = 0;

            if (!IsMatrixInList(mainList, newNode))
            {
                posList.Add(newNode);
            }
        }

        static void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("____________");
            Thread.Sleep(2000);
        }

        static void CreateNeighbors(int[,] matrix)
        {
            neighborsList.Clear();
            int[,] copyMatrix = (int[,])matrix.Clone();
            Neighbors(copyMatrix);

            foreach (var neighbor in neighborsList)
            {
                ChangeBlanks(copyMatrix, neighbor[0], neighbor[1]);
            }
        }

        static void AStar(int[,] matrix)
        {
            posList.Clear();
            neighborsList.Clear();
            List<int> heuristicList = new List<int>();

            if (MatrixEquals(matrix, endPos))
            {
                Console.WriteLine("bitti");
            }
            else
            {
                int[,] copyMatrix = (int[,])matrix.Clone();
                CreateNeighbors(copyMatrix);

                foreach (var newPos in posList)
                {
                    if (!IsMatrixInList(mainList, newPos))
                    {
                        heuristicList.Add(Heuristic(newPos));
                    }
                }

                int minHeuristic = heuristicList.Min();
                Console.WriteLine("Adim: " + mainList.Count);
                mainList.Add(posList[heuristicList.IndexOf(minHeuristic)]);
                Console.WriteLine("heuristic= " + minHeuristic);
                PrintMatrix(mainList[mainList.Count - 1]);
                AStar(mainList[mainList.Count - 1]);
            }
        }

        static bool IsMatrixInList(List<int[,]> matrixList, int[,] matrix)
        {
            foreach (var item in matrixList)
            {
                if (MatrixEquals(item, matrix))
                {
                    return true;
                }
            }
            return false;
        }

        static bool MatrixEquals(int[,] matrix1, int[,] matrix2)
        {
            if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
            {
                return false;
            }

            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    if (matrix1[i, j] != matrix2[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
