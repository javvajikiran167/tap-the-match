using System.Collections.Generic;


static class ListExtensions
{
    private static readonly System.Random rnd = new System.Random();
    
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static List<T> GetRandom<T>(this IList<T> list, int count)
    {
        List<T> randomList = new List<T>();
        List<T> tempList = new List<T>(list);
        tempList.Shuffle();
        for (int i = 0; i < count; i++)
        {
            randomList.Add(tempList[i]);
        }
        return randomList;
    }

    public static T GetRandomElement<T>(this IList<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}