using System;
using System.ComponentModel;
using System.Collections;

namespace DynamicString
{
	/// <summary>
	/// DStringCollection represents a container; that is, a collection or bag of DStringFixed elements.
	/// This is a strongly typed DStringFixed collection that has-a CollectionBase; 
	/// exposes all IList and ArrayList functionality to the user.
	/// DStringCollection Version 2.00 Copyright (C) 2004 Lou Stein
	/// </summary>
	public sealed class DStringCollection : CollectionBase 
	{
		#region Property
		#region this[index] or Item
		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		public DStringFixed this[ int index ]  
		{
			get  
			{
				if (index < 0 || index > Count -1 || 0 == Count) //Out of bounds error, do nothing
				{
					throw new ArgumentOutOfRangeException("DStringCollection this[" + index.ToString() + "]--On Get");
				}

				return (DStringFixed)List[index].ToString();
			}
			set  
			{
				if (index < 0  || index > Count -1 || 0 == Count) 
				{
					throw new ArgumentOutOfRangeException("DStringCollection this[" + index.ToString() + "]--On Set,\"Out of Bounds Error Occurred\"");
				}
				List[index] = value;
			}
		}
		#endregion
		#region Capacity
		/// <summary>
		/// Gets or sets the number of elements that the DStringCollection can contain.
		/// </summary>
		new public int Capacity
		{
			get
			{
				return InnerList.Capacity;
			}
			set
			{
				InnerList.Capacity = value;
			}
		}
		#endregion
		#region IsFixedSize
		/// <summary>
		/// Gets a value indicating whether the DStringCollection has a fixed size.
		/// Default is false
		/// </summary>
		public bool IsFixedSize
		{
			get
			{
				return InnerList.IsFixedSize;
			}
		}
		#endregion
		#region IsReadOnly
		/// <summary>
		/// Gets a value indicating whether the DStringCollection is read-only.
		/// Default is false;
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return InnerList.IsReadOnly;
			}
		}
		#endregion
		#region IsSynchronized
		/// <summary>
		/// Gets a value indicating whether access to the DStringCollection is synchronized (thread-safe).
		/// Default is false.
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return InnerList.IsSynchronized;
			}
		}
		#endregion
		#region SyncRoot
		/// <summary>
		/// An object that can be used to synchronize access to the DStringCollection.
		/// </summary>
		/// <example>
		/// DStringCollection myCollection = new DStringCollection();
		/// lock( myCollection.SyncRoot ) 
		/// {
		///    foreach ( DStringFixed item in myCollection ) 
		///    {
		///       Insert your code here.
		///		}
		/// }
		/// </example>
		public object SyncRoot
		{
			get
			{
				return InnerList.SyncRoot;
			}
		}
		#endregion
		#endregion Property
		#region Constructor
		#region DStringCollection()
		/// <summary>
		/// Default Constructor
		/// </summary>
		public DStringCollection(){}
		#endregion
		#region DStringCollection(ICollection)
		/// <summary>
		/// Initializes a new instance of the DStringCollection class that contains elements copied from the specified collection and that has the same initial capacity as the number of elements copied.
		/// </summary>
		/// <param name="c">System.Collections.ICollection</param>
		public DStringCollection(ICollection c)
		{
			AddRange(c);
		}
		#endregion
		#region DStringCollection(Int32)
		/// <summary>
		/// Initializes a new instance of the DStringCollection class that is empty and has the specified initial capacity.
		/// </summary>
		/// <param name="size"></param>
		public DStringCollection(Int32 size)
		{
			Capacity = size;
		}
		#endregion
		#endregion
		#region Reflection methods
		#region Adapter
		/// <summary>
		/// 
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static DStringCollection Adapter(ArrayList list)
		{
			return new DStringCollection(list);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static DStringCollection Adapter(IList list)
		{
			return new DStringCollection(list);
		}
		#endregion
		#region Add
		/// <summary>
		/// Adds a DStringFixed to the end of the DStringCollection. 
		/// </summary>
		/// <param name="value">DynamicStringFixed.DStringFixed</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add( DStringFixed value )  
		{
			return( List.Add( value ) );
		}
		#endregion Add
		#region AddRange
		/// <summary>
		/// Adds the elements of an ICollection to the end of the DStringCollection.
		/// </summary>
		/// <param name="c">System.Collections.ICollection</param>
		public void AddRange(ICollection c)
		{
			foreach(object o in c)
				List.Add((DStringFixed)Convert.ToString(o));
		}
		#endregion AddRange
		#region BinarySearch
		/// <summary>
		/// Uses a binary search algorithm to locate a specific element in the sorted DStringCollection or a portion of it.
		/// </summary>
		/// <param name="value">DynamicStringFixed.DStringFixed</param>
		/// <param name="compare">System.Collections.IComparer; Method to compare two items.</param>
		/// <returns>DStringFixed; Returns the found item.</returns>
		public int BinarySearch(DStringFixed value,IComparer compare)
		{
			return InnerList.BinarySearch(value,compare);
		}
		/// <summary>
		/// Uses a binary search algorithm to locate a specific element in the sorted DStringCollection or a portion of it.
		/// </summary>
		/// <param name="value">DynamicStringFixed.DStringFixed; Item to find.</param>
		/// <returns>DStringFixed; Return the found element</returns>
		public int BinarySearch(DStringFixed value)
		{
			return InnerList.BinarySearch(value.Value);
		}
		/// <summary>
		/// Uses a binary search algorithm to locate a specific element in the sorted DStringCollection or a portion of it.
		/// </summary>
		/// <param name="index">int; Start Index</param>
		/// <param name="count">int; Item Count</param>
		/// <param name="value">DynamicStringFixed.DStringFixed; Item to find</param>
		/// <param name="comparer">System.Collections.IComparer; Method to comparer two items</param>
		/// <returns>DStringFixed; Return the found Item</returns>
		public int BinarySearch(int index,int count,DStringFixed value,IComparer comparer)
		{
			return InnerList.BinarySearch(index,count,value,comparer);
		}
		#endregion BinarySearch
		#region Clone
		/// <summary>
		/// A Shallow Copy of DStringCollection
		/// </summary>
		/// <returns>DStringCollection</returns>
		public DStringCollection Clone()
		{
			return this;
		}
		#endregion Clone
		#region Contains
		/// <summary>
		/// Determines whether an element is in the DStringCollection.
		/// </summary>
		/// <param name="value">Element to search for.</param>
		/// <returns>If found return true, otherwise false</returns>
		public bool Contains( DStringFixed value )  
		{
			// If value is not of type DStringFixed, this will return false.
			return( InnerList.Contains( value ) );
		}
		#endregion Contains
		#region CopyTo
		/// <summary>
		/// Copies the DStringCollection or a portion of it to a one-dimensional array.
		/// </summary>
		/// <param name="index">int; The zero-based index in the source DStringCollection at which copying begins. </param>
		/// <param name="array">Array; The one-dimensional Array that is the destination of the elements copied from DStringCollection. The Array must have zero-based indexing.</param>
		/// <param name="arrayIndex">int; The zero-based index in array at which copying begins. </param>
		/// <param name="count">int; The number of elements to copy.</param>
		public void CopyTo(int index,Array array,int arrayIndex,int count)
		{
			InnerList.CopyTo(index,array,arrayIndex,count);
		}
		/// <summary>
		/// Copies the DStringCollection or a portion of it to a one-dimensional array.
		/// </summary>
		/// <param name="array">Array; The one-dimensional Array that is the destination of the elements copied from DStringCollection. The Array must have zero-based indexing.</param>
		/// <param name="index">int; The zero-based index in the source DStringCollection at which copying begins. </param>
		public void CopyTo(Array array,int index)
		{
			List.CopyTo(array,index);
		}
		/// <summary>
		/// Copies the DStringCollection or a portion of it to a one-dimensional array.
		/// </summary>
		/// <param name="array">Array; The one-dimensional Array that is the destination of the elements copied from DStringCollection. The Array must have zero-based indexing.</param>
		public void CopyTo(Array array)
		{
			InnerList.CopyTo(array);
		}
		#endregion CopyTo
		#region Equals
		/// <summary>
		/// A Shallow comparision between two DStringCollections.
		/// </summary>
		/// <param name="obj">DStringCollection to be compared.</param>
		/// <returns>True, same object; else false.</returns>
		public bool Equals(DStringCollection obj)
		{
			return InnerList.Equals(obj.InnerList);
		}
		#endregion
		#region FixedSize
		/// <summary>
		/// Returns an DStringCollection wrapper with a fixed size.
		/// </summary>
		/// <param name="list">ArrayList.></param>
		/// <returns>Fixed size ArrayList</returns>
		public static ArrayList FixedSize(ArrayList list)
		{
			return ArrayList.FixedSize(list);
		}
		/// <summary>
 		/// Returns an IList wrapper with a fixed size.
		/// </summary>
		/// <param name="list">IList</param>
		/// <returns>Fixed Size IList</returns>
		public static IList FixedSize(IList list)
		{
			return ArrayList.FixedSize(list);
		}

		#endregion
		#region GetRange
		/// <summary>
		/// Returns an DStringCollection which represents a subset of the elements in the source DStringCollection.
		/// </summary>
		/// <returns>DStringCollection containg that range of elements</returns>
		public DStringCollection GetRange(int index,int count)
		{
			DStringCollection dc = new DStringCollection(count);
			for(int i = index; i < index + count; ++i)
				dc.Add(this[i]);
			return dc;
		}
		#endregion GetRange
		#region GetType()
		/// <summary>
		/// Returns Type, DynamicStringFixed.DStringFixed.
		/// </summary>
		/// <returns>System.Object.Type</returns>
		public new Type GetType()
		{
			return Type.GetType("DynamicString.DStringCollection"); 
		}
		#endregion
		#region IndexOf
		/// <summary>
		/// Find the position of the element.
		/// </summary>
		/// <param name="value">DStringFixed; Item to find</param>
		/// <returns>int; Index location of the item.</returns>
		public int IndexOf( DStringFixed value )  
		{
			return InnerList.IndexOf(value );
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public int IndexOf(DStringFixed value,int startIndex,int count)
		{
			return InnerList.IndexOf(value,startIndex,count);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		public int IndexOf(DStringFixed value, int startIndex)
		{
			return InnerList.IndexOf(value,startIndex);
		}
		#endregion IndexOf
		#region Insert
		/// <summary>
		/// Insert an new element.
		/// </summary>
		/// <param name="index">int; The position the element should be inserted.</param>
		/// <param name="value">DStringFixed; The element to be inserted.</param>
		public void Insert( int index, DStringFixed value )  
		{
			List.Insert( index, value );
		}
		#endregion Insert
		#region InsertRange
		/// <summary>
		/// Inserts an element into the DStringCollection at the specified index.
		/// </summary>
		/// <param name="index">int; Position to insert items</param>
		/// <param name="c">System.Collections.ICollections; Items to be inserted</param>
		public void InsertRange(int index,ICollection c)
		{
			for(IEnumerator e = c.GetEnumerator();e.MoveNext();index++)
				List.Insert(index,(DStringFixed)Convert.ToString(e.Current));
		}
		#endregion InsertRange
		#region LastIndexOf
		/// <summary>
		///  Searches for the specified value and returns the zero-based index of the last occurrence
		///  within the section of the DStringCollection that contains the specified number of 
		///  elements and ends at the specified index.
		/// </summary>
		/// <param name="value">DStringFixed; The Object to locate in the ArrayList. The value can be a null reference (Nothing in Visual Basic). </param>
		/// <param name="startIndex">int; The zero-based starting index of the backward search. </param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <returns>The zero-based index of the last occurrence of value within the section of the DStringCollection that contains count number of elements and ends at startIndex, if found; otherwise, -1.</returns>
		public int LastIndexOf(DStringFixed value,int startIndex,int count)
		{
			return InnerList.LastIndexOf(value,startIndex,count);
		}
		/// <summary>Searches for the specified value and returns the zero-based index of
		///  the last occurrence within the section of the DStringCollection that 
		///  contains the specified number of elements and ends at the specified index.
		/// </summary>
		/// <param name="value">DStringFixed; The Object to locate in the ArrayList. The value can be a null reference (Nothing in Visual Basic). </param>
		/// <param name="startIndex">int; The zero-based starting index of the backward search. </param>
		/// <returns>The zero-based index of the last occurrence of value within the section of the DStringCollection that contains count number of elements and ends at startIndex, if found; otherwise, -1.</returns>
		public int LastIndexOf(DStringFixed value,int startIndex)
		{
			return InnerList.LastIndexOf(value,startIndex);
		}
		///  <summary>Searches for the specified value and returns the zero-based index 
		///  of the last occurrence within the section of the DStringCollection that 
		///  contains the specified number of elements and ends at the specified index.
		/// </summary>
		/// <param name="value">DStringFixed; The Object to locate in the ArrayList. The value can be a null reference (Nothing in Visual Basic). </param>
		/// <returns>The zero-based index of the last occurrence of value within the section of the DStringCollection that contains count number of elements and ends at startIndex, if found; otherwise, -1.</returns>
		public int LastIndexOf(DStringFixed value)
		{
			return InnerList.LastIndexOf(value);
		}
		#endregion LastIndexOf
		#region ReadOnly
		/// <summary>
		/// Returns a read-only ArrayList wrapper. 
		/// </summary>
		/// <param name="list">The ArrayList to wrap.</param>
		/// <returns>A read-only ArrayList wrapper around list.</returns>
		public static ArrayList ReadOnly(ArrayList list)
		{
			return ArrayList.ReadOnly(list);
		}
		#endregion
		#region Remove
		/// <summary>
		/// Removes the first occurrence of a specific object from the DStringCollection.
		/// </summary>
		/// <param name="value">The Object to remove from the DStringCollection. The value can be a null reference (Nothing in Visual Basic). </param>
		/// <exception cref="NotSupportedException" >
		/// The ArrayList is read-only. 
		/// -or- 
		/// The ArrayList has a fixed size.
		/// </exception>
		public void Remove( DStringFixed value )  
		{
			InnerList.Remove( value );
		}
		#endregion Remove
		#region RemoveRange
		/// <summary>
		/// Removes a range of elements from the DStringCollection.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range of elements to remove. </param>
		/// <param name="count">The number of elements to remove. </param>
		public void RemoveRange(int index,int count)
		{
			InnerList.RemoveRange(index,count);
		}
		#endregion RemoveRange
		#region Repeat
		/// <summary>
		/// Returns a DStringCollection whose elements are copies of the specified value.
		/// </summary>
		/// <param name="value">The DStringFixed to copy multiple times in the new DStringCollection. The value can be a null reference (Nothing in Visual Basic). </param>
		/// <param name="count">The number of times value should be copied. </param>
		/// <returns>A DStringCollection with count number of elements, all of which are copies of value.</returns>
		public static DStringCollection Repeat(DStringFixed value, int count)
		{
            return new DStringCollection(ArrayList.Repeat(value,count));
		}
		#endregion
		#region Reverse
		/// <summary>
		/// Reverses the order of the elements in the specified range.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range to reverse. </param>
		/// <param name="count">The number of elements in the range to reverse. </param>
		public void Reverse(int index,int count)
		{
			InnerList.Reverse(index,count);
		}
		/// <summary>
		/// Reverses the order of the elements in the entire DStringCollection.
		/// </summary>
		public void Reverse()
		{
			InnerList.Reverse();
		}
		#endregion Reverse
		#region SetRange
		/// <summary>
		/// Copies the elements of a collection over a range of elements in the DStringCollection.
		/// </summary>
		/// <param name="index">The zero-based DStringCollection index at which to start copying the elements of c. </param>
		/// <param name="c">The ICollection whose elements to copy to the DStringCollection.
		///  The collection itself cannot be a null reference (Nothing in Visual Basic), 
		///  but it can contain elements that are a null reference (Nothing). 
		/// </param>
		public void SetRange(int index,ICollection c)
		{
			InnerList.SetRange(index,c);
		}
		#endregion SetRange
		#region Sort
		/// <summary>
		/// Sorts the elements in a section of DStringCollection using the specified comparer.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range to sort. </param>
		/// <param name="count">The length of the range to sort. </param>
		/// <param name="comparer">The IComparer implementation to use when comparing elements. 
		/// -or- A null reference (Nothing in Visual Basic) to use the IComparable implementation 
		/// of each element.
		/// </param>
		public void Sort(int index,int count,IComparer comparer)
		{
			InnerList.Sort(index,count,comparer);
		}
		/// <summary>
		/// Sorts the elements in the entire DStringCollection using the specified comparer.
		/// </summary>
		/// <param name="comparer">The IComparer implementation to use when comparing elements. 
		/// -or- A null reference (Nothing in Visual Basic) to use the IComparable implementation 
		/// of each element.
		/// </param>
		public void Sort(IComparer comparer)
		{
			InnerList.Sort(comparer);
		}
		/// <summary>
		/// Sorts the elements in the entire DStringCollection using the IComparable implementation 
		/// of each element.
		/// </summary>
		public void Sort()
		{
			InnerList.Sort();
		}
		#endregion Sort
		#region Synchronized
		// ***TBD*** DStringCollection implementation
		/// <summary>
		/// Returns an ArrayList wrapper that is synchronized (thread-safe).
		/// </summary>
		/// <param name="list">The ArrayList to synchronize. </param>
		/// <returns>An ArrayList wrapper that is synchronized (thread-safe).</returns>
		public static ArrayList Synchronized(ArrayList list)
		{
			return ArrayList.Synchronized(list);
		}
		#endregion
		#region ToArray
		/// <summary>
		/// Copies the elements of the DStringCollection to a new array of DStringFixed.
		/// </summary>
		/// <returns>An array of the specified type containing copies of the elements of the DStringCollection.</returns>
		public DStringFixed[] ToArray()
		{
			return (DStringFixed[])InnerList.ToArray(typeof (DStringFixed));
		}
		/// <summary>
		/// Copies the elements of the DStringCollection to a new array of the specified type.
		/// </summary>
		/// <param name="type">The Type of array to create and copy elements to. </param>
		/// <returns>An array of the specified type containing copies of the elements of the DStringCollection.</returns>
		public Array ToArray(Type type)
		{
			return InnerList.ToArray(type);
		}
		#endregion
		#region TrimToSize
		/// <summary>
		/// Sets the capacity to the actual number of elements in the DStringCollection.
		/// </summary>
		public void TrimToSize()
		{
			InnerList.TrimToSize();
		}
		#endregion TrimToSize
		#endregion Reflection methods
		#region GetDStringEnumerator
		/// <summary>
		/// Get the inner enumerator to iterate through every character.
		/// </summary>
		/// <param name="iter">IEnumerator;the enumerator for DStringCollection</param>
		/// <returns>CharEnumerator; string object's enumerator</returns>
		public IEnumerator GetDStringEnumerator(IEnumerator iter)
		{
			return Convert.ToString(iter.Current).GetEnumerator();
		}
		#endregion GetDStringEnumerator
	}
}
