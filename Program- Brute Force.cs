using System;

namespace KnapSackBrute
{
    class Program
    {
        public static int KnapSack(int capacity, int[] weight, int[] value, int itemsCount)
        {
            int[,] K = new int[itemsCount + 1, capacity + 1];

            for (int i = 0; i <= itemsCount; ++i)
            {
                for (int w = 0; w <= capacity; ++w)
                {
                    if (i == 0 || w == 0)
                        K[i, w] = 0;
                    else if (weight[i - 1] <= w)
                        K[i, w] = Math.Max(value[i - 1] + K[i - 1, w - weight[i - 1]], K[i - 1, w]);
                    else
                        K[i, w] = K[i - 1, w];
                }
            }

            return K[itemsCount, capacity];
        }
        static void Main(string[] args)
        {
           
            int[] value = { 10, 5, 15, 7, 6, 18, 3,8,2,1 };
            int[] weight = { 2, 3, 5, 7, 1, 4, 1,6,8,2,3 };
            int capacity = 20;
            int itemsCount = 7;

            int result = KnapSack(capacity, weight, value, itemsCount);
            Console.WriteLine(result);
        }


    }
}
