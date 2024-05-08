using System;
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
                // Check if value is comparable
                if (IsComparable<TValue>())
                {
                    // Value type is comparable, so proceed with sorting
                    dictionary = dictionary.OrderBy(i => i.Value).ToDictionary(i => i.Key, i => i.Value);
                }
                break;
            case SortType.Random:
                var random = new System.Random();
                dictionary = dictionary.OrderBy(i => random.Next(dictionary.Count)).ToDictionary(i => i.Key, i => i.Value);
                break;
        }

        if (SortOrder == SortOrder.Descending)
        {
            dictionary = dictionary.Reverse().ToDictionary(i => i.Key, i => i.Value);
        }

        return dictionary;
    }

    private static bool IsComparable<TValue>() => typeof(IComparable<TValue>).IsAssignableFrom(typeof(TValue)) || typeof(TValue).IsEnum;
}