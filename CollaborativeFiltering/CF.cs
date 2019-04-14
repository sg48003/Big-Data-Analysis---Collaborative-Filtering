using System;
using System.Linq;

namespace CollaborativeFiltering
{
    public class Similarity
    {
        public int Position { get; set; }
        public decimal Value { get; set; }
    }

    class CF
    {
        //imenovanje matrice ?
        public static decimal[,] usersItemsMatrix;
        public static decimal[,] usersItemsMatrix_T;

        public static decimal[] usersValueAverage;
        public static decimal[] itemsValueAverage;

        public static int itemCount;
        public static int userCount;

        public static void Main(string[] args)
        {

            #region Testing region

            //int queryCount;
            //List<string> query = new List<string>();
            //const Int32 BufferSize = 128;
            //using (var fileStream = File.OpenRead("S:\\Projekti\\CollaborativeFiltering\\CollaborativeFiltering\\bin\\Debug\\R2.in"))
            //using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            //{

            //    var line1 = streamReader.ReadLine()?.Split(' ');

            //    itemCount = Convert.ToInt32(line1?[0]);
            //    userCount = Convert.ToInt32(line1?[1]);

            //    userItemMatrix = new decimal[itemCount, userCount];
            //    userItemMatrix_T = new decimal[userCount, itemCount];

            //    //kasnije možda promijeniti ime?
            //    itemsRatingAverage = new decimal[itemCount];
            //    usersRatingAverage = new decimal[userCount];

            //    #region Loading the matrix

            //    for (var i = 0; i < itemCount; i++)
            //    {
            //        var line = streamReader.ReadLine()?.Split(' ');

            //        for (var j = 0; j < userCount; j++)
            //        {
            //            var value = line?[j];
            //            if (value == "X")
            //            {
            //                userItemMatrix[i, j] = 0;
            //                userItemMatrix_T[j, i] = 0;
            //            }
            //            else
            //            {
            //                userItemMatrix[i, j] = Convert.ToSingle(value);
            //                userItemMatrix_T[j, i] = Convert.ToSingle(value);
            //            }
            //        }

            //    }

            //    queryCount = Convert.ToInt32(streamReader.ReadLine());
            //    for (var i = 0; i < queryCount; i++)
            //    {
            //        query.Add(streamReader.ReadLine());
            //    }

            //    #endregion

            //}

            #endregion

            var line1 = Console.ReadLine().Split(' ');

            itemCount = Convert.ToInt32(line1[0]);
            userCount = Convert.ToInt32(line1[1]);

            usersItemsMatrix = new decimal[itemCount, userCount];
            usersItemsMatrix_T = new decimal[userCount, itemCount];

            itemsValueAverage = new decimal[itemCount];
            usersValueAverage = new decimal[userCount];

            #region Loading the matrix

            for (var i = 0; i < itemCount; i++)
            {
                var line = Console.ReadLine().Split(' ');

                for (var j = 0; j < userCount; j++)
                {
                    var value = line[j];
                    if (value == "X")
                    {
                        usersItemsMatrix[i, j] = 0;
                        usersItemsMatrix_T[j, i] = 0;
                    }
                    else
                    {
                        usersItemsMatrix[i, j] = Convert.ToDecimal(value);
                        usersItemsMatrix_T[j, i] = Convert.ToDecimal(value);
                    }
                }

            }

            #endregion

            #region Calculate Average For Each Row

            for (var i = 0; i < itemCount; i++)
            {
                decimal sum = 0;
                var count = 0;

                for (var j = 0; j < userCount; j++)
                {
                    var value = usersItemsMatrix[i, j];
                    if (value != 0)
                    {
                        sum += value;
                        count++;
                    }
                    else { }
                }

                itemsValueAverage[i] = sum / count;
            }

            for (var i = 0; i < userCount; i++)
            {
                decimal sum = 0;
                var count = 0;

                for (var j = 0; j < itemCount; j++)
                {
                    var value = usersItemsMatrix_T[i, j];
                    if (value != 0)
                    {
                        sum += value;
                        count++;
                    }
                    else { }
                }

                usersValueAverage[i] = sum / count;
            }

            #endregion

            #region Running the query

            var queryCount = Convert.ToInt32(Console.ReadLine());

            for (var i = 0; i < queryCount; i++)
            {
                var line = Console.ReadLine().Split(' ');
                var I = Convert.ToInt32(line[0]);
                var J = Convert.ToInt32(line[1]);
                var T = Convert.ToInt32(line[2]);
                var K = Convert.ToInt32(line[3]);

                if (T == 0)
                {
                    Algorithm(usersItemsMatrix, itemsValueAverage, I, J, K);
                }
                else
                {
                    Algorithm(usersItemsMatrix_T, usersValueAverage, J, I, K);
                }
            }

            #endregion

        }

        #region Testing region #2

        //private static void UserUserAlgorithm(int selectedUser, int selectedItem, int maxSimilarities, decimal[,] matrix, decimal[] average)
        //{
        //    var similarities = new SimilarityItem[userCount];
        //    var normalizedMatrix = NormalizeMatrix(userCount, itemCount, matrix, average);

        //    decimal brojnik;
        //    decimal result;
        //    for (var row = 0; row < userCount; row++)
        //    {
        //        brojnik = 0;
        //        double nazivnik1 = 0;
        //        double nazivnik2 = 0;

        //        if (row != selectedUser)
        //        {
        //            for (var column = 0; column < itemCount; column++)
        //            {
        //                brojnik += normalizedMatrix[row, column] * normalizedMatrix[selectedUser, column];

        //                nazivnik1 += Math.Pow((double)normalizedMatrix[row, column], 2);
        //                nazivnik2 += Math.Pow((double)normalizedMatrix[selectedUser, column], 2);
        //            }
        //            result = brojnik / (decimal)Math.Sqrt(nazivnik1 * nazivnik2);
        //        }
        //        else
        //        {
        //            result = 1;
        //        }

        //        similarities[row] = new SimilarityItem
        //        {
        //            Position = row,
        //            Value = result
        //        };
        //    }

        //    similarities = similarities.Where(x => x.Value > 0).OrderByDescending(x => x.Value).ToArray();

        //    var count = 0;
        //    brojnik = 0;
        //    decimal nazivnik = 0;
        //    for (var index = 1; index < similarities.Length; index++)
        //    {
        //        var t = similarities[index];
        //        if (count == maxSimilarities)
        //        {
        //            break;
        //        }

        //        var grade = (int)matrix[t.Position, selectedUser];
        //        if (grade > 0 && t.Position != selectedItem)
        //        {
        //            count++;
        //            brojnik += grade * t.Value;
        //            nazivnik += t.Value;
        //        }
        //        else
        //        {
        //        }

        //    }
        //    result = brojnik / nazivnik;
        //    Console.WriteLine(result.ToString("##.000"));

        //}

        #endregion

        private static void Algorithm(decimal[,] matrix, decimal[] average,int selectedItem, int selectedUser, int maxSimilarities)
        {
            selectedItem--;
            selectedUser--;

            var similarities = new Similarity[itemCount];
            var normalizedMatrix = NormalizeMatrix(matrix, average, itemCount, userCount);

            decimal brojnik;
            decimal result;
            for (var row = 0; row < itemCount; row++)
            {
                brojnik = 0;
                double nazivnik1 = 0;
                double nazivnik2 = 0;

                if (row == selectedItem)
                {
                    result = 1;
                }
                else
                {
                    for (var column = 0; column < userCount; column++)
                    {
                        brojnik += normalizedMatrix[row, column] * normalizedMatrix[selectedItem, column];

                        nazivnik1 += Math.Pow((double)normalizedMatrix[row, column], 2);
                        nazivnik2 += Math.Pow((double)normalizedMatrix[selectedItem, column], 2);
                    }
                    result = brojnik / (decimal)Math.Sqrt(nazivnik1 * nazivnik2);
                }

                similarities[row] = new Similarity
                {
                    Position = row,
                    Value = result
                };
            }

            similarities = similarities.Where(x => x.Value > 0).OrderByDescending(x => x.Value).ToArray();

            var count = 0;
            brojnik = 0;
            decimal nazivnik = 0;
            for (var index = 1; index < similarities.Length; index++)
            {
                var similarity = similarities[index];
                if (count == maxSimilarities)
                {
                    break;
                }

                var grade = (int) matrix[similarity.Position, selectedUser];
                if (grade > 0 && similarity.Position != selectedItem)
                {
                    count++;
                    brojnik += grade * similarity.Value;
                    nazivnik += similarity.Value;
                }
                else
                {
                }

            }

            result = brojnik / nazivnik;
            result = Math.Round(result, 3);
            Console.WriteLine(result.ToString("##.000"));

        }

        private static decimal[,] NormalizeMatrix(decimal[,] matrix, decimal[] average, int item, int user)
        {
            var result = (decimal[,])matrix.Clone();

            for (var i = 0; i < item; i++)
            {
                for (var j = 0; j < user; j++)
                {
                    var value = result[i, j];
                    if (value != 0)
                    {
                        result[i, j] = value - average[i];
                    }
                }
            }

            return result;
        }

    }
}
