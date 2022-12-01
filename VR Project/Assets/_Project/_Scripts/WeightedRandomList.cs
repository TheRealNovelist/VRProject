using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class WeightedRandomList<T>
{
    [System.Serializable]
    public struct Pair 
    {
        [HorizontalGroup("Value"), HideLabel]
        public T item;
        [HorizontalGroup("Value", 50f), HideLabel]
        public float weight;

        public Pair(T item, float weight)
        {
            this.item = item;
            this.weight = weight;
        }
    }

    [SerializeField, LabelText("Weighted List")] private List<Pair> list = new List<Pair>();

    public int Count => list.Count;

    public void Add(T item, float weight)
    {
        list.Add(new Pair(item, weight));
    }
    
    public T GetRandom()
    {
        float total = 0;
        foreach (var pair in list)
        {
            total += pair.weight;
        }

        float value = Random.value * total;
        float sum = 0;

        foreach (var pair in list)
        {
            sum += pair.weight;
            if (sum >= value)
            {
                return pair.item;
            }
        }

        return default;
    }

    /*public T GetRandom(List<T> excludes)
    {
        if (excludes.Count == list.Count)
            return default;
        
        List<Pair> pairs = ListWithExclude(excludes);
        
        float total = 0;
        foreach (var pair in pairs)
        {
            total += pair.weight;
        }

        float value = Random.value * total;
        float sum = 0;

        foreach (var pair in pairs)
        {
            sum += pair.weight;
            if (sum >= value)
            {
                return pair.item;
            }
        }

        return default;
    }

    public List<Pair> ListWithExclude(List<T> excludes)
    {
        Dictionary<T, float> dict = new Dictionary<T, float>();
        List<Pair> sortedPair = new List<Pair>();
        
        foreach (Pair pair in list)
        {
            dict.Add(pair.item, pair.weight);
        }

        foreach (var exclude in excludes)
        {
            if (dict.ContainsKey(exclude))
            {
                dict.Remove(exclude);
            }
        }

        foreach (KeyValuePair<T, float> pair in dict)
        {
            sortedPair.Add(new (pair.Key, pair.Value));
        }
        
        return sortedPair;
    }*/
}
