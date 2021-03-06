﻿using Qart.Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Qart.Testing.Framework
{
    public static class OptionsExtensions
    {
        public static bool IsRebaseline(this IDictionary<string, string> options)
        {
            return options.GetOptionalValue("ct.rebase", false, bool.Parse);
        }

        public static string GetDirectory(this IDictionary<string, string> options)
        {
            return options.GetOptionalValue("ct.dir", null);
        }

        public static bool GetDeferExceptions(this IDictionary<string, string> options)
        {
            return options.GetOptionalValue("ct.deferExceptions", false, bool.Parse);
        }

        public static IEnumerable<string> GetIncludeTags(this IDictionary<string, string> options)
        {
            return GetCsv(options, "includeTags");
        }

        public static IEnumerable<string> GetExcludeTags(this IDictionary<string, string> options)
        {
            return GetCsv(options, "excludeTags");
        }

        public static int GetWorkersCount(this IDictionary<string, string> options)
        {
            return options.GetOptionalValue("workersCount", 1, int.Parse);
        }

        private static IEnumerable<string> GetCsv(IDictionary<string, string> options, string tagName)
        {
            return options.GetOptionalValue(tagName, Enumerable.Empty<string>(), t => t.ToLower().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
