using System.Collections;
using System.Collections.Generic;
using System;

namespace SystemShuffle {
    /*public class Array
    {
        public static void shuffle<T>(T[] data)
        {
            for(int i = 0; i < data.Length; i++)
            {
                Random ran = new Random();
                int randomValue = ran.Next(0, data.Length);
                T temp = data[i];
                data[i] = data[randomValue];
                data[randomValue] = temp;

            }
           

        }


    }*/

    public class List
    {
        public static void shuffle<T>(List<T> data)
        {
            /* int[] tt = { 0, 1, 2, 3 };
             int prev = -1;
             for (int i = 0; i < data.Count; i++)
             {
                 Random ran = new Random();
                 int randomValue = ran.Next(0, data.Count);
                 if (prev != randomValue)
                 {
                     prev = randomValue;
                     tt[i] = 
                 }
                 T temp = data[i];
                 data[i] = data[randomValue];
                 data[randomValue] = temp;

             }*/
            Random random = new Random();
            bool[] selected = new bool[4];
            int selectedCnt = 0;

            while (selectedCnt < data.Count)
            {
                int a = random.Next(0, data.Count); // 1에서 N-1까지

                while (selected[a] == true)
                {
                    a = (a + 1) % data.Count;
                }

                selected[a] = true;

                T temp = data[selectedCnt];
                data[selectedCnt] = data[a];
                data[a] = temp;

                selectedCnt++;
            }

        }
       
        

    }

}