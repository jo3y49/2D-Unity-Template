using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SortEnum {
    public enum SortType {
        Name,
        Value,
        Random,
    }
    public enum SortOrder {
        Ascending,
        Descending
    }

    public static IDictionary<TKey, TValue> Sort<TKey, TValue>(IDictionary<TKey, TValue> dictionary, SortType SortType, SortOrder SortOrder = SortOrder.Ascending)
    {
        switch (SortType)
        {
            case SortType.Name:
                dictionary = dictionary.OrderBy(i => i.Key).ToDictionary(i => i.Key, i => i.Value);
                break;
            case SortType.Value:
                dictionary = dictionary.OrderBy(i => i.Value).ToDictionary(i => i.Key, i => i.Value);
                break;
            case SortType.Random:
                int dictionaryCount = dictionary.Count;
                dictionary = dictionary.OrderBy(i => Random.Range(0, dictionaryCount)).ToDictionary(i => i.Key, i => i.Value);
                break;
        }

        if (SortOrder == SortOrder.Descending)
        {
            dictionary = dictionary.Reverse().ToDictionary(i => i.Key, i => i.Value);
        }

        return dictionary;
    }
}