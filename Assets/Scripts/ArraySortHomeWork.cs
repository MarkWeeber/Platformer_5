using UnityEngine;
using Random = System.Random;

public class ArraySortHomeWork : MonoBehaviour
{

    // this class is intended for array sorting homework

    private void Start()
    {
        // making 200 random integers [-100, 100]
        int [] array = getRandomIntegers(200, -100, 100);
        // printing array
        printArray(array);
        // measuring time
        float time1 = 0f, time2 = 0f;
        time1 = System.DateTime.Now.Ticks;
        // sorting array
        sortArray(ref array);
        // printing time spent
        time2 = System.DateTime.Now.Ticks;
        Debug.Log("TIME SPENT IN 100 NANOSECONDS = " + (time2 - time1).ToString());
        // printing the results
        printArray(array);
    }

    private int[] getRandomIntegers(int length, int minRange, int maxRange)
    {
        int[] ans = new int[length];
        Random randomizer = new Random();
        for (int i = 0; i < length; i++)
        {
            ans[i] = randomizer.Next(minRange, maxRange + 1);
        }
        return ans;
    }

    private void printArray(int[] array)
    {
        foreach (int item in array)
        {
            Debug.Log(item);
        }
    }

    private void sortArray(ref int[] array)
    {
        int length = array.Length;
        int minValue;
        int swapIndex = -1;
        for (int i = 0; i < length; i++)
        {
            // starting new min value
            minValue = array[i];
            for (int j = i + 1; j < length; j++)
            {
                // storing minimum value
                if(array[j] < minValue)
                {
                    minValue = array[j];
                    swapIndex = j;
                }
            }
            // swapping
            array[swapIndex] = array[i];
            array[i] = minValue;
        }
    }

}
