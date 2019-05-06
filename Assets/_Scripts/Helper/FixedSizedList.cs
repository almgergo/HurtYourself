using System.Collections.Generic;

public class FixedSizedList<T> {
    public FixedSizedList (int length) {
        this.length = length;
    }

    private LinkedList<T> list = new LinkedList<T> ();
    private int length;

    public void Push (T element) {
        if (list.Count == length) {
            list.RemoveFirst ();
        }
        list.AddLast (element);
    }

    public T First () {
        return list.First.Value;
    }

    public T Last () {
        return list.Last.Value;
    }
}