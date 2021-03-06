﻿using System;
using System.Collections.Generic;

namespace Qart.Testing
{
    [Serializable]
    public class AssertException : Exception
    {
        private static string[] NoCategories = new string[0];

        public static string CategoriesKey = "Categories";

        public AssertException(string message)
            : this(message, (IReadOnlyCollection<string>)null)
        { }

        public AssertException(string message, IReadOnlyCollection<string> categories)
            : base(message)
        {
            if (categories != null && categories.Count > 0)
                Data.Add(CategoriesKey, categories);
        }


        public AssertException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public bool TryGetCategories(out IReadOnlyCollection<string> categories)
        {
            if (Data.Contains(CategoriesKey))
            {
                categories = (IReadOnlyCollection<string>)Data[CategoriesKey];
                return true;
            }
            categories = default;
            return false;
        }
    }
}
