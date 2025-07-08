using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace  MyBucketList.Filters;

public enum BucketFilters
{
    Unfiltered,
    Done, //true, false
    Title // Title Substring
}

public static class BucketListFilter
{
    public static List<BucketItem> ApplyFilters(List<BucketItem> list, Tuple<BucketFilters, object>[] filters)
    {
        foreach (var tuple in filters)
        {
            BucketFilters filter = tuple.Item1;
            object arg = tuple.Item2;

            switch (filter)
            {
                case BucketFilters.Done:
                    list = BucketListFilter.FilterByDoneProperty(list.ToList(), (bool)arg);
                    break;

                case BucketFilters.Title:
                    list = BucketListFilter.FilterByTitleSubstring(list.ToList(), (string)arg);
                    break;
            }
        }

        return list;
    }

    private static List<BucketItem> FilterByDoneProperty(List<BucketItem> list, bool done)
    {
        list.RemoveAll((item) => item.Done != done);
        return list;
    }

    private static List<BucketItem> FilterByTitleSubstring(List<BucketItem> list, string substring)
    {
        return list.FindAll((item) => item.Title.ToLower().Contains(substring.ToLower()));
    }
}