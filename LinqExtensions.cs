using System;
using System.Linq;

public static class LinqExtensions
{
    private static Lazy<Random> _defaultRng = new Lazy<Random>(() => new Random());

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable, Random rng = null)
    {
        if (!enumerable.Any())
            yield break;

        var list = enumerable.ToList();
        rng = rng ?? _defaultRng.Value;

        for (int i = list.Count - 1; i >= 0; i--)
        {
            int next = rng.Next(i + 1);
            yield return list[next];
            list[next] = list[i];
        }
    }
}
