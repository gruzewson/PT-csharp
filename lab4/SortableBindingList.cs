using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

public class SortableBindingList<T> : BindingList<T>
{
    private bool isSorted;
    private PropertyDescriptor sortProperty;
    private ListSortDirection sortDirection;

    public SortableBindingList() : base() { }

    public SortableBindingList(IList<T> list) : base(list) { }

    protected override bool SupportsSortingCore => true;

    protected override bool IsSortedCore => isSorted;

    protected override PropertyDescriptor SortPropertyCore => sortProperty;

    protected override ListSortDirection SortDirectionCore => sortDirection;

    protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
    {
        if (prop.PropertyType.GetInterface("IComparable") == null && !prop.PropertyType.IsValueType)
        {
            throw new ArgumentException("Cannot sort by " + prop.Name + ". This" +
                " property does not implement IComparable");
        }

        List<T> list = Items as List<T>;

        if (list != null)
        {
            list.Sort(new Comparison<T>((lhs, rhs) =>
            {
                IComparable lhsValue = (IComparable)prop.GetValue(lhs);
                IComparable rhsValue = (IComparable)prop.GetValue(rhs);

                if (direction == ListSortDirection.Ascending)
                {
                    return lhsValue.CompareTo(rhsValue);
                }
                else
                {
                    return rhsValue.CompareTo(lhsValue);
                }
            }));

            isSorted = true;
            sortProperty = prop;
            sortDirection = direction;
        }
        else
        {
            isSorted = false;
        }

        OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
    }

    protected override void RemoveSortCore()
    {
        isSorted = false;
        sortProperty = null;
        sortDirection = ListSortDirection.Ascending;
    }

    protected override bool SupportsSearchingCore => true;


    protected override int FindCore(PropertyDescriptor prop, object key)
    {
        if (prop.PropertyType != typeof(string) && prop.PropertyType != typeof(int))
        {
            throw new ArgumentException("Cannot search by " + prop.Name + ". This" +
                " property is not of type string or int");
        }

        List<T> list = Items as List<T>;

        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (prop.GetValue(list[i]).Equals(key))
                {
                    return i;
                }
            }
        }

        return -1;
    }
    
    public void ApplySort(PropertyDescriptor prop, ListSortDirection direction)
    {
        ApplySortCore(prop, direction);
    }
    
    public int Find(PropertyDescriptor prop, object key)
    {
        return FindCore(prop, key);
    }
}
