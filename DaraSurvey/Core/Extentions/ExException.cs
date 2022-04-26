using System;
using System.Collections.Generic;
using System.Linq;

namespace DaraSurvey.Core
{
    public static class ExException
    {
        public static string GetAllMessages(this Exception exception)
        {
            var messages = exception
                .FromHierarchy(nextItem => nextItem.InnerException)
                .Select(ex => ex.Message);

            return String.Join(Environment.NewLine, messages);
        }

        // ---------------------

        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        // ---------------------

        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem) where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }
    }
}
