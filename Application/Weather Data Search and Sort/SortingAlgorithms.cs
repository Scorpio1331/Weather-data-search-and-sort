using System;
using System.Collections.Generic;
using System.Linq;

namespace Weather_Data_Search_and_Sort
{
    internal class SortingAlgorithms
    {
        /// <summary>
        /// Procedure to call function to sort array of int
        /// </summary>
        /// <param name="arrayToBeSorted">Integer array to be sorted</param>
        public static void Sort(int[] arrayToBeSorted)
        {
            Sort(arrayToBeSorted, 0, arrayToBeSorted.Length); //Call procedure to sort array
        }

        /// <summary>
        ///  Procedure to call function to sort array of float
        /// </summary>
        /// <param name="arrayToBeSorted">float array to be sorted</param>
        public static void Sort(float[] arrayToBeSorted)
        {
            Sort(arrayToBeSorted, 0, arrayToBeSorted.Length);
        }

        /// <summary>
        /// Function to sort integer data from a dictionary and return string array of keys in order
        /// </summary>
        /// <param name="dataDictionary">Dictionary to sort</param>
        /// <returns>Array of keys in order</returns>
        public static string[] SortDictionary(IDictionary<string, int> dataDictionary)
        {
            IList<string> keyArray = new List<string>();
            int[] dataArray = dataDictionary.Values.ToArray(); //Create new integer array from dictionaries values and then call function to sort array
            Sort(dataArray);
            foreach (int data in dataArray.Distinct()) //Loop through each distinct element in dataArray and find matches in dictionary
            {
                var matches = dataDictionary.Where(kvp => kvp.Value == data);
                foreach (KeyValuePair<string, int> match in matches)
                {
                    keyArray.Add(match.Key); //Loop through each match and add key in order
                }
            }

            return keyArray.ToArray();
        }

        /// <summary>
        /// Function to sort float data from a dictionary and return string array of keys in order
        /// </summary>
        /// <param name="dataDictionary">Dictionary to sort</param>
        /// <returns>Array of keys in order</returns>
        public static string[] SortDictionary(IDictionary<string, float> dataDictionary)
        {
            IList<string> keyArray = new List<string>();
            float[] dataArray = dataDictionary.Values.ToArray();//Create new float array from dictionaries values and then call function to sort array
            Sort(dataArray);
            foreach (float data in dataArray.Distinct()) //Loop through each distinct element in dataArray and find matches in dictionary
            {
                var matches = dataDictionary.Where(kvp => Math.Abs(kvp.Value - data) < 0.01f); //Find key values of data from dictionary where value is equal to data with 0.01 variation
                foreach (KeyValuePair<string, float> match in matches)
                {
                    keyArray.Add(match.Key); //Loop through each match and add key in order
                }
            }

            return keyArray.ToArray();
        }

        /// <summary>
        /// Function to return string array of keys for the min and max values of a dictionary
        /// </summary>
        /// <param name="dataDictionary">Dictionary to get min/max data keys from</param>
        /// <returns>Array of keys</returns>
        public static string[] GetMaxMin(IDictionary<string, int> dataDictionary)
        {
            IList<string> keyArray = new List<string>();
            var matches = dataDictionary.Where(kvp => kvp.Value == dataDictionary.Values.Min()); //Get all keyvaluepairs from dictionary where value is equal to min value
            foreach (KeyValuePair<string, int> match in matches)
            {
                keyArray.Add(match.Key); //Add key to list
            }
            matches = dataDictionary.Where(kvp => kvp.Value == dataDictionary.Values.Max());//Get all keyvaluepairs from dictionary where value is equal to max value
            foreach (KeyValuePair<string, int> match in matches)
            {
                keyArray.Add(match.Key);//Add key to list
            }
            keyArray.Add("EoD"); //Add EoD to signify end of data from this dictionary
            return keyArray.ToArray();
        }

        /// <summary>
        /// Function to return string array of keys for the min and max values of a dictionary
        /// </summary>
        /// <param name="dataDictionary">Dictionary to get min/max data keys from</param>
        /// <returns>Array of keys</returns>
        public static string[] GetMaxMin(IDictionary<string, float> dataDictionary)
        {
            IList<string> keyArray = new List<string>();
            var matches = dataDictionary.Where(kvp => Math.Abs(kvp.Value - dataDictionary.Values.Min()) < 0.01f); //Find key values of data from dictionary where value is equal to min value with 0.01 variation
            foreach (KeyValuePair<string, float> match in matches)
            {
                keyArray.Add(match.Key);
            }
            matches = dataDictionary.Where(kvp => Math.Abs(kvp.Value - dataDictionary.Values.Max()) < 0.01f);
            foreach (KeyValuePair<string, float> match in matches)
            {
                keyArray.Add(match.Key);
            }
            keyArray.Add("EoD");
            return keyArray.ToArray();
        }

        /// <summary>
        /// Function to return string array of keys for the median values of a dictionary
        /// </summary>
        /// <param name="dataDictionary">Dictionary to get median data keys from</param>
        /// <returns>Array of keys</returns>
        public static string[] GetMedian(IDictionary<string, int> dataDictionary)
        {
            IList<string> keyArray = new List<string>();
            int[] dataArray = dataDictionary.Values.ToArray(); //Sort data first
            Sort(dataArray);

            int data;
            data = dataArray[((dataArray.Length + 1) / 2)]; //Get median from list
            var matches = dataDictionary.Where(kvp => kvp.Value == data); //Find all keyvaluepairs with value equal to median
            foreach (KeyValuePair<string, int> match in matches)
            {
                keyArray.Add(match.Key); //Add each key
            }
            keyArray.Add("EoD"); //Add EoD to signify end of data from this dictionary
            return keyArray.ToArray();
        }

        /// <summary>
        /// Function to return string array of keys for the median values of a dictionary
        /// </summary>
        /// <param name="dataDictionary">Dictionary to get median data keys from</param>
        /// <returns>Array of keys</returns>
        public static string[] GetMedian(IDictionary<string, float> dataDictionary)
        {
            IList<string> keyArray = new List<string>();
            float[] dataArray = dataDictionary.Values.ToArray();
            Sort(dataArray);

            float data;
            data = dataArray[((dataArray.Length + 1) / 2)];
            var matches = dataDictionary.Where(kvp => Math.Abs(kvp.Value - data) < 0.01f); //Find key values of data from dictionary where value is equal to meidan value with 0.01 variation
            foreach (KeyValuePair<string, float> match in matches)
            {
                keyArray.Add(match.Key);
            }
            keyArray.Add("EoD");
            return keyArray.ToArray();
        }

        /// <summary>
        ///  Procedure to sort array of int
        /// </summary>
        /// <param name="arrayToBeSorted">Integer array to be sorted</param>
        /// <param name="fromIndex">Index to start sorting from</param>
        /// <param name="toIndex">Index to stop sorting at</param>
        public static void Sort(int[] arrayToBeSorted, int fromIndex, int toIndex)
        {
            DualPivotQuicksort(arrayToBeSorted, fromIndex, toIndex - 1, 3); //Call function to sort array
        }

        /// <summary>
        /// Procedure to sort array of float
        /// </summary>
        /// <param name="arrayToBeSorted">float array to be sorted</param>
        /// <param name="fromIndex">Index to start sorting from</param>
        /// <param name="toIndex">Index to stop sorting at</param>
        public static void Sort(float[] arrayToBeSorted, int fromIndex, int toIndex)
        {
            DualPivotQuicksort(arrayToBeSorted, fromIndex, toIndex - 1, 3);
        }

        #region "Quicksort Int"

        /// <summary>
        /// Procedure to swap two items in an array
        /// </summary>
        /// <param name="arrayToBeSorted">array containing items to be swapped</param>
        /// <param name="i">First item to be swapped</param>
        /// <param name="j">Second item to be swapped</param>
        private static void Swap(int[] arrayToBeSorted, int i, int j)
        {
            int temp = arrayToBeSorted[i];
            arrayToBeSorted[i] = arrayToBeSorted[j];
            arrayToBeSorted[j] = temp;
        }

        /// <summary>
        /// Function to sort an array using insertion sort
        /// </summary>
        /// <param name="arrayToBeSorted">array containing items to be swapped</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private static void InsertionSort(int[] arrayToBeSorted, int left, int right)
        {
            for (int i = left + 1; i <= right; i++)
            {
                for (int j = i; j > left && arrayToBeSorted[j] < arrayToBeSorted[j - 1]; j--)
                {
                    Swap(arrayToBeSorted, j, j - 1);
                }
            }
        }

        /// <summary>
        /// Procedure to sort an integer array using dual pivot quicksort algorithm, or if the array length is small use insertion sort
        /// </summary>
        /// <param name="arrayToBeSorted">array containing items to be swapped</param>
        /// <param name="left">Left pointer</param>
        /// <param name="right">Right pointer</param>
        /// <param name="div"></param>
        private static void DualPivotQuicksort(int[] arrayToBeSorted, int left, int right, int div)
        {
            int len = right - left;

            if (len < 27)
            { // insertion sort for tiny array
                InsertionSort(arrayToBeSorted, left, right);
                return;
            }
            int third = len / div;

            // "medians"
            int m1 = left + third;
            int m2 = right - third;

            if (m1 <= left)
            {
                m1 = left + 1;
            }
            if (m2 >= right)
            {
                m2 = right - 1;
            }
            if (arrayToBeSorted[m1] < arrayToBeSorted[m2])
            {
                Swap(arrayToBeSorted, m1, left);
                Swap(arrayToBeSorted, m2, right);
            }
            else
            {
                Swap(arrayToBeSorted, m1, right);
                Swap(arrayToBeSorted, m2, left);
            }
            // pivots
            int pivot1 = arrayToBeSorted[left];
            int pivot2 = arrayToBeSorted[right];

            // pointers
            int less = left + 1;
            int great = right - 1;

            // sorting
            for (int k = less; k <= great; k++)
            {
                if (arrayToBeSorted[k] < pivot1)
                {
                    Swap(arrayToBeSorted, k, less++);
                }
                else if (arrayToBeSorted[k] > pivot2)
                {
                    while (k < great && arrayToBeSorted[great] > pivot2)
                    {
                        great--;
                    }
                    Swap(arrayToBeSorted, k, great--);

                    if (arrayToBeSorted[k] < pivot1)
                    {
                        Swap(arrayToBeSorted, k, less++);
                    }
                }
            }
            // swaps
            int dist = great - less;

            if (dist < 13)
            {
                div++;
            }
            Swap(arrayToBeSorted, less - 1, left);
            Swap(arrayToBeSorted, great + 1, right);

            // subarrays
            DualPivotQuicksort(arrayToBeSorted, left, less - 2, div);
            DualPivotQuicksort(arrayToBeSorted, great + 2, right, div);

            // equal elements
            if (dist > len - 13 && pivot1 != pivot2)
            {
                for (int k = less; k <= great; k++)
                {
                    if (arrayToBeSorted[k] == pivot1)
                    {
                        Swap(arrayToBeSorted, k, less++);
                    }
                    else if (arrayToBeSorted[k] == pivot2)
                    {
                        Swap(arrayToBeSorted, k, great--);

                        if (arrayToBeSorted[k] == pivot1)
                        {
                            Swap(arrayToBeSorted, k, less++);
                        }
                    }
                }
            }
            // subarray
            if (pivot1 < pivot2)
            {
                DualPivotQuicksort(arrayToBeSorted, less, great, div);
            }
        }

        #endregion "Quicksort Int"

        #region "Quicksort Float"

        /// <summary>
        /// Procedure to swap two items in an array
        /// </summary>
        /// <param name="arrayToBeSorted">array containing items to be swapped</param>
        /// <param name="i">First item to be swapped</param>
        /// <param name="j">Second item to be swapped</param>
        private static void Swap(float[] arrayToBeSorted, int i, int j)
        {
            float temp = arrayToBeSorted[i];
            arrayToBeSorted[i] = arrayToBeSorted[j];
            arrayToBeSorted[j] = temp;
        }

        /// <summary>
        /// Function to sort an array using insertion sort
        /// </summary>
        /// <param name="arrayToBeSorted">array containing items to be swapped</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private static void InsertionSort(float[] arrayToBeSorted, int left, int right)
        {
            for (int i = left + 1; i <= right; i++)
            {
                for (int j = i; j > left && arrayToBeSorted[j] < arrayToBeSorted[j - 1]; j--)
                {
                    Swap(arrayToBeSorted, j, j - 1);
                }
            }
        }

        /// <summary>
        /// Procedure to sort a float array using dual pivot quicksort algorithm, or if the array length is small use insertion sort
        /// </summary>
        /// <param name="arrayToBeSorted">array containing items to be swapped</param>
        /// <param name="left">Left pointer</param>
        /// <param name="right">Right pointer</param>
        /// <param name="div"></param>
        private static void DualPivotQuicksort(float[] arrayToBeSorted, int left, int right, int div)
        {
            int len = right - left;

            if (len < 27)
            { // insertion sort for tiny array
                InsertionSort(arrayToBeSorted, left, right);
                return;
            }
            int third = len / div;

            // "medians"
            int m1 = left + third;
            int m2 = right - third;

            if (m1 <= left)
            {
                m1 = left + 1;
            }
            if (m2 >= right)
            {
                m2 = right - 1;
            }
            if (arrayToBeSorted[m1] < arrayToBeSorted[m2])
            {
                Swap(arrayToBeSorted, m1, left);
                Swap(arrayToBeSorted, m2, right);
            }
            else
            {
                Swap(arrayToBeSorted, m1, right);
                Swap(arrayToBeSorted, m2, left);
            }
            // pivots
            float pivot1 = arrayToBeSorted[left];
            float pivot2 = arrayToBeSorted[right];

            // pointers
            int less = left + 1;
            int great = right - 1;

            // sorting
            for (int k = less; k <= great; k++)
            {
                if (arrayToBeSorted[k] < pivot1)
                {
                    Swap(arrayToBeSorted, k, less++);
                }
                else if (arrayToBeSorted[k] > pivot2)
                {
                    while (k < great && arrayToBeSorted[great] > pivot2)
                    {
                        great--;
                    }
                    Swap(arrayToBeSorted, k, great--);

                    if (arrayToBeSorted[k] < pivot1)
                    {
                        Swap(arrayToBeSorted, k, less++);
                    }
                }
            }
            // swaps
            int dist = great - less;

            if (dist < 13)
            {
                div++;
            }
            Swap(arrayToBeSorted, less - 1, left);
            Swap(arrayToBeSorted, great + 1, right);

            // subarrays
            DualPivotQuicksort(arrayToBeSorted, left, less - 2, div);
            DualPivotQuicksort(arrayToBeSorted, great + 2, right, div);

            // equal elements
            if (dist > len - 13 && Math.Abs(pivot1 - pivot2) > 0.01f)
            {
                for (int k = less; k <= great; k++)
                {
                    if (Math.Abs(arrayToBeSorted[k] - pivot1) < 0.01f)
                    {
                        Swap(arrayToBeSorted, k, less++);
                    }
                    else if (Math.Abs(arrayToBeSorted[k] - pivot2) < 0.01f)
                    {
                        Swap(arrayToBeSorted, k, great--);

                        if (Math.Abs(arrayToBeSorted[k] - pivot1) < 0.01f)
                        {
                            Swap(arrayToBeSorted, k, less++);
                        }
                    }
                }
            }
            // subarray
            if (pivot1 < pivot2)
            {
                DualPivotQuicksort(arrayToBeSorted, less, great, div);
            }
        }

        #endregion "Quicksort Float"
    }
}