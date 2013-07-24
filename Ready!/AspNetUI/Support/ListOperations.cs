using System.Collections.Generic;

namespace AspNetUI.Support
{
    public class ListOperations
    {
        public static bool ListsEquals<T>(List<T> list1, List<T> list2)
        {
            if (list1 == null && list2 == null) return true;
            if (list1 == null || list2 == null) return false;
            if (list1.Count != list2.Count) return false;
            int n = list1.Count;
            for (int i = 0; i < n; ++i)
            {
                if (!list1[i].Equals(list2[i])) return false;
            }

            return true;
        }
    }
}