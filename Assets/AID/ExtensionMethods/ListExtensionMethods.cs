using System.Collections.Generic;

public static class ListExtensionMethods
{
    public static T First<T>(this List<T> sequence)
    {
        return sequence[0];
    }

    public static T Last<T>(this List<T> sequence)
    {
        return sequence[sequence.Count-1];
    }

    public static T Front<T>(this List<T> sequence)
    {
        return sequence[0];
    }

    public static T Back<T>(this List<T> sequence)
    {
        return sequence[sequence.Count - 1];
    }

    public static T PopBack<T>(this List<T> sequence)
    {
		if(sequence.Empty()) return default(T);

		var retval = sequence.Last();
        sequence.RemoveAt(sequence.Count-1);
		return retval;
    }
    
    public static T PopFront<T>(this List<T> sequence)
	{
		if(sequence.Empty()) return default(T);

		var retval = sequence.First();
		sequence.RemoveAt(0);
		return retval;
    }

	public static bool Empty<T>(this List<T> seq)
	{
		return seq.Count == 0;
	}

    public static void Grow<T>(this List<T> seq, int size)
    {
        while (seq.Count < size)
            seq.Add(default(T));
    }
}

