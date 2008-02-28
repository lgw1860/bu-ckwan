using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace DynamicString
{
	#region class DString
	/// <summary>
	///  DString represents text; that is, a series of Unicode characters, 
	///  unlike String can be mutated.
	///  Additional methods were added to enhance string manipulation.
	///  Code was adapted from DString(C++) Version 4.97 Copyright (C) 2003 James P. Devlin
	///  DString Version 2.03 Copyright (C) 2007 Lou Stein and James P. Devlin 
	/// </summary>
	[Serializable]
	public sealed class DString : ICloneable,IComparable,IEnumerable 
	{
		#region DString Members
		#region Constructors
		#region DString()
		/// <summary>
		/// Default DString ctor, takes no arguments and assigns DString internal value to null.
		/// <remark>Supported by the .NET Compact Framework</remark>
		/// </summary>
		public DString()
		{
			_strBuffer = new StringBuilder();
		}
		#endregion
		#region DString(object obj)
		/// <summary>
		/// DString ctor, takes an object and convert the objects value
		///  to string and assigns it into DString.
		/// </summary>
		/// <param name="obj">
    /// <a href="http://msdn.microsoft.com/library/default.asp?url=
    /// /library/en-us/cpref/html/frlrfSystemObjectClassTopic.asp">System.Object
    /// </a>
    /// </param>
		/// <remarks>
    /// This constructor will only assign object's ToString() value to DString
    /// </remarks>
		public DString(object obj)
		{
			if (obj != null)
				_strBuffer = new StringBuilder(obj.ToString());
			else 
				_strBuffer = new StringBuilder();
		}
		#endregion
		#region DString(Stream stream)
		/// <summary>
		/// DString ctor, takes a Text File Stream and reads the contents
		/// into DString.
		/// </summary>
		/// <param name="stream">
    /// <a href="http://msdn.microsoft.com/library/default.asp?url=
    /// /library/en-us/cpref/html/frlrfsystemiostreamclasstopic.asp">System.IO.Stream
    /// </a>
    /// </param>
		/// <remarks>Designed to read "only" Text Files</remarks>
		public DString(Stream stream)
		{
			if (stream != null)
			{
				StreamReader sr = new StreamReader(stream);
				_strBuffer = new StringBuilder(sr.ReadToEnd());
			}
			else 
				_strBuffer = new StringBuilder();
		}
		#endregion
		#region DString(string,FileMode)
		/// <summary>
		/// DString ctor, takes a string with the file path and the file mode 
		/// and reads in the file contents into DString.
		/// </summary>
		/// <param name="d">System.String ([DirectoryPath]/File to be read]</param>
		/// <param name="m">
    /// <a href=
    /// "http://msdn.microsoft.com/library/default.asp?url=
    /// /library/en-us/cpref/html/frlrfSystemIOFileModeClassTopic.asp">
    /// System.IO.FileMode</a>
    /// </param>
		/// <remarks>Designed to read only Text Files</remarks>
		public DString(string d,FileMode m)
		{
			FileStream stream = null;
			try
			{
				stream = new FileStream(d,FileMode.Open,FileAccess.Read,FileShare.Read);
				StreamReader sr = new StreamReader(stream);
				_strBuffer = new StringBuilder(sr.ReadToEnd());
			}
			catch (Exception e) 
			{
				Console.WriteLine("The process failed: {0}", e.ToString());
			}
			finally
			{
			  if (stream != null)
  				stream.Close();
			}
		}
		#endregion
		#region DString(DateTime,string)
		/// <summary>
		/// DString ctor, takes a DateTime and the date formatting  
		/// and initializes DString to that value.
		/// </summary>
		/// <param name="dt">
    /// <a href=
    /// "http://
    /// msdn.microsoft.com/library/en-us/cpref/html/frlrfSystemDateTimeClassTopic.asp">
    /// System.DateTime</a>
    /// </param>
		/// <param name="format">
    /// System.String 
    /// <a href="http://msdn.microsoft.com/library/default.asp?url=
    /// /library/en-us/cpref/html/frlrfsystemglobalizationdatetimeformatinfoclasstopic.asp">
    /// Formatting based on DateTimeFormatInfo</a>
    /// </param>
		/// <remarks>Uses the default culture</remarks>
		public DString(DateTime dt, string format)
		{
			_strBuffer = new StringBuilder(dt.ToString(format,DateTimeFormatInfo.InvariantInfo));
		}
		#endregion
		#region DString(DateTime,string,CultureInfo)
		/// <summary>
		/// DString ctor, takes a DateTime,date formatting  and culture  
		/// and initializes DString to that value. This is used if Globalization is required.
		/// </summary>
		/// <param name="dt">
    /// <a href="http://msdn.microsoft.com/library/en-us/cpref/html/
    /// frlrfSystemDateTimeClassTopic.asp">
    /// System.DateTime
    /// </a>
    /// </param>
		/// <param name="format">
    /// System.String <a href=
    /// "http://msdn.microsoft.com/library/default.asp?url=
    /// /library/en-us/cpref/html/frlrfsystemglobalizationdatetimeformatinfoclasstopic.asp">
    /// Formatting based on DateTimeFormatInfo
    /// </a>
    /// </param>
		/// <param name="info">
    /// System.Globalization.CultureInfo. 
    /// <a href=
    /// "http://msdn.microsoft.com/library/en-us/cpref/html/
    /// frlrfsystemglobalizationcultureinfoclasstopic.asp">
    /// Formatting based on a specific culture
    /// </a>
    /// </param>
		public DString(DateTime dt, string format, CultureInfo info)
		{
			_strBuffer = new StringBuilder(dt.ToString(format,info));
		}
		#endregion
		#region DString(int,char)
		/// <summary>
		/// DString ctor, takes an integer and char and intializes 
		/// DString to a given number of repeated character.
		/// </summary>
		/// <param name="count">Number of times the character will be repeated</param>
		/// <param name="ch">The character to be repeated</param>
		public DString(int count,char ch)
		{
			if ( count > 0)
				_strBuffer = new StringBuilder(new string(ch,count));
			else 
				_strBuffer = new StringBuilder(@"");
		}
		#endregion
		#region DString(ICollection)
		/// <summary>
		/// DString ctor, takes a ICollection object (Array,ArrayList,Queue,Stack etc) 
		/// and initializes DString to that value. 
		/// </summary>
		/// <param name="elements">
    /// <a href=
    /// "http://msdn.microsoft.com/library/en-us/cpref/html/
    /// frlrfSystemCollectionsICollectionClassTopic.asp">
    /// System.Collections.ICollection
    /// </a>
    /// </param>
		public DString(ICollection elements)
		{
			IEnumerator iter = elements.GetEnumerator();
			_strBuffer = new StringBuilder();
			while(iter.MoveNext())
				_strBuffer.AppendFormat("{0} ",iter.Current.ToString());
			Remove(Length-1,1);
		}
		#endregion
		#region DString(ICollection, string)
		/// <summary>
		/// DString ctor, takes a ICollection object (Array,ArrayList,Queue,Stack etc) 
    /// and a string separator and initializes DString to that value. 
		/// </summary>
		/// <param name="elements">
    /// <a href=
    /// "http://msdn.microsoft.com/library/en-us/cpref/html/
    /// frlrfSystemCollectionsICollectionClassTopic.asp">
    /// System.Collections.ICollection
    /// </a>
    /// </param>
		/// <param name="separator">Separators between collection items</param>
		public DString(ICollection elements, string separator)
		{
			IEnumerator iter = elements.GetEnumerator();
			_strBuffer = new StringBuilder();
			while(iter.MoveNext())
				_strBuffer.AppendFormat("{0}{1}",iter.Current.ToString(),separator);
			Remove(Length-separator.Length,separator.Length);
		}
		#endregion
		#region DString(IDictionary)
		/// <summary>
		/// DString ctor, takes a IDictionary object
		/// and initializes DString to all key-value combinations
		/// </summary>
		/// <param name="table">
    /// <a href=
    /// "http://msdn.microsoft.com/library/en-us/cpref/html/
    /// frlrfSystemCollectionsIDictionaryClassTopic.asp">
    /// System.Collections.IDictionary
    /// </a>
    /// </param>
		public DString(System.Collections.IDictionary table)
		{
			IDictionaryEnumerator enumerator = table.GetEnumerator();
			_strBuffer = new StringBuilder();
			while(enumerator.MoveNext())
			{
				_strBuffer.AppendFormat("{0}:{1} ",enumerator.Key,enumerator.Value);
			}
			Remove(Length-1,1);
		}
		#endregion	
    #region DString(IDictionary,string)
    /// <summary>
    /// DString ctor, takes a IDictionary object 
    /// and initializes DString to its keys separated by sep. 
    /// </summary>
    /// <param name="table">
    /// <a href=
    /// "http://msdn.microsoft.com/library/en-us/cpref/html/
    /// frlrfSystemCollectionsIDictionaryClassTopic.asp">
    /// System.Collections.IDictionary
    /// </a>
    /// </param>
    /// <param name="sep">String to separate Keys</param>
    public DString(IDictionary table,string sep)
    {
      IDictionaryEnumerator enumerator = table.GetEnumerator();
      _strBuffer = new StringBuilder();
      while(enumerator.MoveNext())
      {
        _strBuffer.AppendFormat("{0}{1}",enumerator.Key,sep);
      }
      Remove(Length - sep.Length,sep.Length);
    }
    #endregion	
		#region DString(IDictionary,string,string)
		/// <summary>
		/// DString ctor, takes an IDictionary object
		/// and initializes DString to that value. 
		/// </summary>
		/// <param name="table">
    /// <a href=
    /// "http://msdn.microsoft.com/library/en-us/cpref/html/
    /// frlrfSystemCollectionsIDictionaryClassTopic.asp">
    /// System.Collections.IDictionary
    /// </a>
    /// </param>
		/// <param name="sep1">String to separate Key and Value</param>
		/// <param name="sep2">String to separate Item from next Item</param>
		public DString(IDictionary table,string sep1, string sep2)
		{
			IDictionaryEnumerator enumerator = table.GetEnumerator();
			_strBuffer = new StringBuilder();
			while(enumerator.MoveNext())
			{
				_strBuffer.AppendFormat("{0}{1}{2}{3}",
					enumerator.Key,sep1, enumerator.Value, sep2);
			}
			Remove(Length - sep2.Length,sep2.Length);
		}
		#endregion	
		#endregion Constructors
		#region Operators
		/// <summary>
		/// Concatenates objects to DString.
		/// </summary>
		/// <param name="d">Original string</param>
		/// <param name="o">Primnitive types(byte, short, float etc.) 
		/// plus object types, char and sting values</param>
		/// <returns>DString with the appended object valuss</returns>
		public static DString operator +( DString d , object o) 
		{ 
      DString d2 = (DString) d.Clone();
			if (o != null)
				d2._strBuffer.Append(o);
			return d2;
		}	
		/// <summary>
		/// Concatenates an array of objects to DString.
		/// </summary>
		/// <param name="d">Original string</param>
		/// <param name="array">An array of Primnitive types(byte, short, float etc.) 
		/// plus object types, char and sting values; 
		/// that is delimited with spaces between items</param>
		/// <returns>Original string appended with an array of object values</returns>
		public static DString operator +( DString d, Array array) 
		{ 
      DString d2 = d;
			if (array != null)
			{
        d2 = (DString) d.Clone();
        foreach(object o in array)
					d2._strBuffer.AppendFormat("{0} ",o.ToString());
        d2.TrimEnd(' ');
			}
			return d2;
		}	

		/// <summary>
		/// Concatenates an array of objects to DString.
		/// </summary>
		/// <param name="d">Original string</param>
		/// <param name="collection">A collection of objects 
		/// that is delimited with spaces between items</param>
		/// <returns>Original string appended with a collection of object values</returns>
		public static DString operator +( DString d, ICollection collection) 
		{ 
      DString d2 = d;
      if (collection != null)
			{
        d2 = (DString) d.Clone();
        foreach(object o in collection)
					d2._strBuffer.AppendFormat("{0} ",o);
        d2.TrimEnd(' ');
      }
      return d2;
		}	
		#endregion
		#region public static implicit operator string (DString d)
		/// <summary>
		/// Implicitly converts DString to string.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>string</returns>
		public static implicit operator string (DString d)
		{
			return d.ToString();
		}
		#endregion
		#region public static implicit operator DString (string s)
		/// <summary>
		/// Implicitly converts string to DString.
		/// </summary>
		/// <param name="s">string</param>
		/// <returns>DString</returns>
		public static implicit operator DString (string s)
		{
			return new DString(s);
		}
		#endregion
		#region public static explicit operator bool(DString)
		/// <summary>
		/// Explicitly converts a DString containing only a boolean value to a boolean.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>bool</returns>
		public static explicit operator bool(DString d)
		{
			bool b = false;
			try
			{
				b = Convert.ToBoolean(d);
			}
			catch (System.SystemException ex)
			{
				string error = string.Format(" ***In operator bool, value \'{0}\'" +
					" is not valid.***\n ***Message: {1}***",d,ex.Message);
				System.Diagnostics.Trace.WriteLine(error);
			}
			return b;
		}
		#endregion
		#region public static explicit operator int(DString)
		/// <summary>
		/// Explicitly converts a DString containing only a integer value to a integer.
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static explicit operator int(DString d)
		{
			int i = 0;
			try
			{
				i = Convert.ToInt32(d);
			}
			catch (System.SystemException ex)
			{
				string error = string.Format(" ***In operator int, value \'{0}\'" +
					" is not valid.***\n ***Message: {1}***",d,ex.Message);
				System.Diagnostics.Trace.WriteLine(error);
			}
			return i;
		}
		#endregion
		#region public static explicit operator byte(DString d)
		/// <summary>
		/// Explicitly converts a DString containing only a byte value to a byte.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>byte</returns>
		public static explicit operator byte(DString d)
		{
			byte bit = 0;
			try
			{
				bit = Convert.ToByte(d);
			}
			catch (System.SystemException ex)
			{
				string error = string.Format(" ***In operator byte, value \'{0}\'" +
					" is not valid.***\n ***Message: {1}***",d,ex.Message);
				System.Diagnostics.Trace.WriteLine(error);
			}
			return bit;
		}
		#endregion
		#region public static explicit operator char(DString d)
		/// <summary>
		/// Explicitly converts a DString containing only a character value to a character.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>char</returns>
		public static explicit operator char(DString d)
		{
			char ch = '\0';
			try
			{
				ch = Convert.ToChar(d);
			}
			catch (System.SystemException ex)
			{
				string error = string.Format(" ***In operator char, value \'{0}\'" +
					" is not valid.***\n ***Message: {1}***",d,ex.Message);
				System.Diagnostics.Trace.WriteLine(error);
			}
			return ch;
		}
		#endregion
		#region public static explicit operator DateTime(DString d)
		/// <summary>
		/// Explicitly converts a DString containing only a DateTime value to a DateTime.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>DateTime</returns>
		public static explicit operator DateTime(DString d)
		{
			DateTime dt = new DateTime(0);
			try
			{
				dt = Convert.ToDateTime(d);
			}
			catch (System.SystemException ex)
			{
				string error = string.Format(" ***In operator DateTime, value \'{0}\'" +
					" is not valid.***\n ***Message: {1}***",d,ex.Message);
				System.Diagnostics.Trace.WriteLine(error);
			}
			return dt;
		}
		#endregion
		#region public static explicit operator uint(DString d)
		/// <summary>
		/// Explicitly converts a DString containing only an unsigned integer value to uint.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>uint</returns>
		public static explicit operator uint(DString d)
		{
			uint ui = 0;
			try
			{
				ui = Convert.ToUInt32(d);
			}
			catch (System.SystemException ex)
			{
				string error = string.Format(" ***In operator uint, value \'{0}\'" +
					" is not valid.***\n ***Message: {1}***",d,ex.Message);
				System.Diagnostics.Trace.WriteLine(error);
			}
			return ui;
		}
		#endregion
		#region public static explicit operator ushort(DString d)
		/// <summary>
		/// Explicitly converts a DString containing an unsigned short value to unsigned short.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>ushort</returns>
		public static explicit operator ushort(DString d)
		{
			ushort us = 0;
			try
			{
				us = Convert.ToUInt16(d);
			}
			catch (System.SystemException ex)
			{
				string error = string.Format(" ***In operator ushort, value \'{0}\'" +
					" is not valid.***\n ***Message: {1}***",d,ex.Message);
				System.Diagnostics.Trace.WriteLine(error);
			}
			return us;
		}
		#endregion
		#region public static explicit operator ulong(DString d)
		/// <summary>
		/// Explicitly converts a DString containing only unsigned long value to unsigned long.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>ulong</returns>
		public static explicit operator ulong(DString d)
		{
			ulong ul = 0;
			try
			{
				ul = Convert.ToUInt64(d);
			}
			catch (System.SystemException ex)
			{
				string error = string.Format(" ***In operator ulong, value \'{0}\'" +
					" is not valid.***\n ***Message: {1}***",d,ex.Message);
				System.Diagnostics.Trace.WriteLine(error);
			}
			return ul;
		}
		#endregion
		#region public static explicit operator short(DString d)
		/// <summary>
		/// Explicitly converts a DString containing only a short value to a short.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>short</returns>
		public static explicit operator short(DString d)
		{
			short sh = 0;
			try
			{
				sh = Convert.ToInt16(d);
			}
			catch (System.SystemException ex)
			{
				string error = string.Format(" ***In operator short, value \'{0}\'" +
					" is not valid.***\n ***Message: {1}***",d,ex.Message);
				System.Diagnostics.Trace.WriteLine(error);
			}
			return sh;
		}
		#endregion
		#region public static explicit operator long(DString d)
		/// <summary>
		/// Explicitly converts a DString containing only a long value to a long.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>long</returns>
		public static explicit operator long(DString d)
		{
			long l = 0;
			try
			{
				l = Convert.ToInt64(d);
			}
			catch (System.SystemException ex)
			{
				string error = string.Format(" ***In operator long, value \'{0}\'" +
					" is not valid.***\n ***Message: {1}***",d,ex.Message);
				System.Diagnostics.Trace.WriteLine(error);
			}
			return l;
		}
		#endregion
		#region public static explicit operator float(DString d)
		/// <summary>
		/// Explicitly converts a DString containing only a float value to a float.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>float</returns>
		public static explicit operator float(DString d)
		{
			float f = 0f;
			try
			{
				f = Convert.ToSingle(d);
			}
			catch (System.SystemException ex)
			{
				string error = string.Format(" ***In operator float, value \'{0}\'" +
					" is not valid.***\n ***Message: {1}***",d,ex.Message);
				System.Diagnostics.Trace.WriteLine(error);
			}
			return f;
		}
		#endregion
		#region public static explicit operator decimal(DString d)
		/// <summary>
		/// Explicitly converts a DString containing only a decimal value to a decimal.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>decimal</returns>
		public static explicit operator decimal(DString d)
		{
			decimal dec = 0M;
			try
			{
				dec = Convert.ToDecimal(d);
			}
			catch (System.SystemException ex)
			{
				string error = string.Format(" ***In operator decimal, value \'{0}\'" +
					" is not valid.***\n ***Message: {1}***",d,ex.Message);
				System.Diagnostics.Trace.WriteLine(error);
			}
			return dec;
		}
		#endregion
		#region public static explicit operator sbyte(DString d)
		/// <summary>
		/// Explicitly converts a DString containing only a sbyte value to a sbyte.
		/// </summary>
		/// <param name="d">Dstring</param>
		/// <returns>sbyte</returns>
		public static explicit operator sbyte(DString d)
		{
			sbyte sb = 0;
			try
			{
				sb = Convert.ToSByte(d);
			}
			catch (System.SystemException ex)
			{
				string error = string.Format(" ***In operator sbyte, value \'{0}\'" +
					" is not valid.***\n ***Message: {1}***",d,ex.Message);
				System.Diagnostics.Trace.WriteLine(error);
			}
			return sb;
		}
		#endregion
		#region public static explicit operator double(DString)
		/// <summary>
		/// Explicitly converts a DString containing only a double value to a double.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>double</returns>
		public static explicit operator double(DString d)
		{
			double dbl = 0.0;
			try
			{
				dbl = Convert.ToDouble(d);
			}
			catch (System.SystemException ex)
			{
				string error = string.Format(" ***In operator double, value \'{0}\'" +
					" is not valid.***\n ***Message: {1}***",d,ex.Message);
				System.Diagnostics.Trace.WriteLine(error);
			}
			return dbl;
		}
    #endregion
    #region equality operators
    /// <summary>
    /// compares == by value
    /// </summary>
    /// <param name="d1">DString</param>
    /// <param name="d2">DString</param>
    /// <returns>bool</returns>
    public static bool operator ==(DString d1,DString d2)
    {
      return d1.Equals(d2);
    }
    /// <summary>
    /// compares == by value
    /// </summary>
    /// <param name="d1">DString itself</param>
    /// <param name="s2">string</param>
    /// <returns></returns>
    public static bool operator ==(DString d1,string s2)
    {
      return d1.Equals(s2);
    }
    /// <summary>
    /// compares == by value
    /// </summary>
    /// <param name="s1">string</param>
    /// <param name="d2">DString itself</param>
    /// <returns>bool</returns>
    public static bool operator ==(string s1,DString d2)
    {
      return d2.Equals(s1);
    }
    /// <summary>
    /// compares != by value
    /// </summary>
    /// <param name="d1">DString</param>
    /// <param name="d2">DString</param>
    /// <returns>bool</returns>
    public static bool operator !=(DString d1,DString d2)
    {
      return !d1.Equals(d2);
    }
    /// <summary>
    /// compares != by value
    /// </summary>
    /// <param name="d1">DString</param>
    /// <param name="s2">string</param>
    /// <returns>bool</returns>
    public static bool operator !=(DString d1,string s2)
    {
      return !d1.Equals(s2);
    }
    /// <summary>
    /// compares != by value
    /// </summary>
    /// <param name="s1">string</param>
    /// <param name="d2">DString</param>
    /// <returns>bool</returns>
    public static bool operator !=(string s1,DString d2)
    {
      return !d2.Equals(s1);
    }
    #endregion Equality Operators
		#region Base Methods
    /// <summary>
    /// returns a value hash, taken from StringBuilder
    /// </summary>
    public override int GetHashCode()
    {
      return Value.GetHashCode();
    }
		/// <summary>
		/// Compares the value of two objects for equality.
		/// A relay to CompareTo method that returns a bool instead of an integer.
		/// </summary>
		/// <param name="obj">object</param>
		/// <returns>Equal Values, true or Not Equal, false</returns>
		public override bool Equals(object obj)
		{
			return (Value == obj.ToString());	
		}
		/// <summary>
		/// Compares the value of two objects for equality.
		/// A relay to CompareTo method that returns a bool instead of an integer.
		/// </summary>
		/// <param name="obj1">object</param>
		/// <param name="obj2">object</param>
		/// <returns>Equal Values, true or Not Equal, false</returns>
		public new static bool Equals(object obj1, object obj2)
		{
			return (obj1.ToString() == obj2.ToString());	
		}

		#region string ToString()
		/// <summary>
		/// Gets the string value.
		/// </summary>
		/// <returns>string</returns>
		public override string ToString()
		{
			return Value;
		}
		#endregion
		#region GetType()
		/// <summary>
		/// Override base method to return a type of DString.
		/// </summary>
		/// <returns>DString type</returns>
		public new Type GetType()
		{
			return Type.GetType("DynamicString.DString"); 
		}
		#endregion
		#region int GetHashCode()
		/// <summary>
		/// Determines the HashCode.
		/// </summary>
		/// <returns>HashCode value as an integer</returns>
		#endregion
		#endregion Base Methods
		#region ICloneable Methods
		/// <summary>
		/// A method to create a new instance of the object.
		/// Another way to extract out the DString value.
		/// </summary>
		/// <returns>A new instance of the object</returns>
		public object Clone()
		{
      DString d = new DString(this);
			return d;
		}
		#endregion
		#region IComparable Methods
		/// <summary>
		/// Compares the value of two objects for equality.
		/// </summary>
		/// <param name="obj">An Object to compare to</param>
		/// <returns>Value 1 if greater, 0 if equal and -1 if less</returns>
		public int CompareTo(object obj)
		{
			return Value.CompareTo(obj.ToString());
		}
		#endregion
		#region IEnumerable Methods
		/// <summary>
		/// Gets the IEnumerator.
		/// </summary>
		/// <returns>IEnumerator</returns>
		public IEnumerator GetEnumerator()
		{
			return Value.GetEnumerator();
		} 
		/// <summary>
		/// The DString to add to the string. 
		/// </summary>
		/// <param name="value"></param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add( Object value )  
		{
			int pos = Value.Length+1;
			Value += value.ToString();
			return pos;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public void Remove( Object value)
		{
			ElimFirst(value.ToString());
		}
		#endregion
		#region ToValue Methods 
		#region public static bool ToBoolean(DString)
		/// <summary>
		/// Relay to operator bool
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>bool</returns>
		public static bool ToBoolean(DString d)
		{
			return (bool)d;
		}
		#endregion
		#region public static int ToInt32(DString)
		/// <summary>
		/// Relay to operator int
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>int</returns>
		public static int ToInt32(DString d)
		{
			return (Int32)d;
		}
		#endregion
		#region public static byte ToByte(DString d)
		/// <summary>
		/// Relay to operator byte.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>byte</returns>
		public static byte ToByte(DString d)
		{
			return (Byte)d;
		}
		#endregion
		#region public static char ToChar(DString d)
		/// <summary>
		/// Relay to operator char
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>char</returns>
		public static char ToChar(DString d)
		{
			return (Char)d;
		}
		#endregion
		#region public static DateTime ToDateTime(DString d)
		/// <summary>
		/// Relay to operator DateTime.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>DateTime</returns>
		public static DateTime ToDateTime(DString d)
		{
			return (DateTime)d;
		}
		#endregion
		#region public static uint ToUInt32(DString d)
		/// <summary>
		/// Relay to operator uint.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>uint</returns>
		public static uint ToUInt32(DString d)
		{
			return (UInt32)d;
		}
		#endregion
		#region public static ushort ToUInt16(DString d)
		/// <summary>
		/// Relay to operator ushort
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>ushort</returns>
		public static ushort ToUInt16(DString d)
		{
			return (UInt16)d;
		}
		#endregion
		#region public static ulong ToUInt64(DString d)
		/// <summary>
		/// Relay to operator ulong
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>ulong</returns>
		public static ulong ToUInt64(DString d)
		{
			return (UInt64)d;
		}
		#endregion
		#region public static short ToInt16(DString d)
		/// <summary>
		/// Relay to operator short
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>short</returns>
		public static short ToInt16(DString d)
		{
			return (Int16)d;
		}
		#endregion
		#region public static long ToInt64(DString d)
		/// <summary>
		/// Relay to operator long
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>long</returns>
		public static long ToInt64(DString d)
		{
			return (Int64)d;
		}
		#endregion
		#region public static float ToSingle(DString d)
		/// <summary>
		/// Relay to operator float
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>float</returns>
		public static float ToSingle(DString d)
		{
			return (Single)d;
		}
		#endregion
		#region public static decimal ToDecimal(DString d)
		/// <summary>
		/// Relay to operator decimal.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>decimal</returns>
		public static decimal ToDecimal(DString d)
		{
			return (Decimal)d;
		}
		#endregion
		#region public static sbyte ToSByte(DString d)
		/// <summary>
		/// Relay to sbyte.
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>sbyte</returns>
		public static sbyte ToSByte(DString d)
		{
			return (SByte)d;
		}
		#endregion
		#region public static double ToDouble(DString)
		/// <summary>
		/// Relay to operator double
		/// </summary>
		/// <param name="d">DString</param>
		/// <returns>double</returns>
		public static double ToDouble(DString d)
		{
			return (Double)d;
		}
		#endregion
		#endregion ToValue Methods
		#region String Relay Methods
		#region Compare
		/// <summary>
		///  Compares substrings of two specified String objects. 
		/// </summary>
		/// <param name="strA">The first String. </param>
		/// <param name="strB">The second String. </param>
		/// <returns>An integer indicating the lexical relationship between the two comparands.
		/// </returns>
		public static int Compare(string strA,string strB)
		{
			return string.Compare(strA,strB);
		}
		/// <summary>
		/// Compares substrings of two specified String objects, ignoring or honoring case.
		/// </summary>
		/// <param name="strA">The first String. </param>
		/// <param name="strB">The second String. </param>
		/// <param name="ignoreCase">
    /// A Boolean indicating a case-sensitive or insensitive comparison 
    /// (true indicates a case-insensitive comparison). 
    /// </param>
		/// <returns>An integer indicating the lexical relationship between the two comparands.
		/// </returns>
		public static int Compare(string strA,string strB,bool ignoreCase)
		{
			return string.Compare(strA,strB,ignoreCase);
		}
		/// <summary>
		///  Compares substrings of two specified String objects, ignoring or honoring case,
		///  and using culture-specific information to influence the comparison.
		/// </summary>
		/// <param name="strA">The first String. </param>
		/// <param name="ignoreCase">
    /// A Boolean indicating a case-sensitive or insensitive comparison.
    ///  ( true indicates a case-insensitive comparison.) 
    /// </param>
    /// <param name="strB">The second String. </param>
		/// <param name="culture">
    /// A CultureInfo object that supplies culture-specific comparison information. 
    /// </param>
		/// <returns>An integer indicating the lexical relationship between the two comparands.
		/// </returns>
		public static int Compare(string strA,string strB,bool ignoreCase,CultureInfo culture)
		{
			return string.Compare(strA,strB,ignoreCase,culture);
		}
		/// <summary>
		///  Compares substrings of two specified String objects, ignoring or honoring case. 
		/// </summary>
		/// <param name="strA">The first String. </param>
		/// <param name="indexA">The first Index. </param>
		/// <param name="strB">The second String. </param>
		/// <param name="indexB">The second Index. </param>
		/// <param name="length">
    /// The maximum number of characters in the substrings to compare.
    /// </param>
		/// <returns>An integer indicating the lexical relationship between the two comparands.
		/// </returns>
		public static int Compare(string strA,int indexA,string strB,int indexB,int length)
		{
			return string.Compare(strA,indexA,strB,indexB,length);
		}
		/// <summary>
		///  Compares substrings of two specified String objects, ignoring or honoring case.
		/// </summary>
		/// <param name="strA">The first String. </param>
		/// <param name="indexA">The first Index. </param>
		/// <param name="strB">The second String. </param>
		/// <param name="indexB">The second Index. </param>
		/// <param name="length">
    /// The maximum number of characters in the substrings to compare.
    /// </param>
		/// <param name="ignoreCase">
    /// A Boolean indicating a case-sensitive or insensitive comparison
    ///  (true indicates a case-insensitive comparison). 
    /// </param>
		/// <returns>An integer indicating the lexical relationship between the two comparands.
		/// </returns>
		public static int Compare(string strA,int indexA,string strB,
                              int indexB,int length,bool ignoreCase)
		{
			return string.Compare(strA,indexA,strB,indexB,length,ignoreCase);
		}
		/// <summary>
		///  Compares substrings of two specified String objects, ignoring or honoring case, 
		///  and using culture-specific information to influence the comparison.
		/// </summary>
		/// <param name="strA">The first String. </param>
		/// <param name="indexA">The first Index. </param>
		/// <param name="strB">The second String. </param>
		/// <param name="indexB">The second Index. </param>
		/// <param name="length">
    /// The maximum number of characters in the substrings to compare.
    /// </param>
		/// <param name="ignoreCase">
    /// A Boolean indicating a case-sensitive or insensitive comparison
    ///  (true indicates a case-insensitive comparison) .
    /// </param>
		/// <param name="culture">
    /// A CultureInfo object that supplies culture-specific comparison information.
    /// </param>
		/// <returns>An integer indicating the lexical relationship between the two comparands.
		/// </returns>
		public static int Compare(string strA,int indexA,string strB,int indexB,
                              int length,bool ignoreCase,CultureInfo culture)
		{
			return string.Compare(strA,indexA,strB,indexB,length,ignoreCase,culture);
		}
		#endregion Compare
		#region CompareOrdinal
		/// <summary>
		///  Compares two specified String objects by evaluating the numeric values of the 
		///  corresponding Char objects in each substring. Parameters specify the length and 
		///  starting positions of the substrings.
		/// </summary>
		/// <param name="strA">The first string.</param>
		/// <param name="indexA">The first index</param>
		/// <param name="strB">The second string</param>
		/// <param name="indexB">The second index</param>
		/// <param name="length">
    /// The maximum number of characters in the substrings to compare. 
    /// </param>
		/// <returns>
    /// A 32-bit signed integer indicating the lexical relation between the two comparands.
    /// </returns>
		/// <remarks>Value Type Condition
		/// Less than zero The substring in strA is less than the substring in strB. 
		/// Zero The substrings are equal, or length is zero.
		/// Greater than zero The substring in strA is greater than the substring in strB. 
		///	</remarks>
		public static int CompareOrdinal(string strA,int indexA,
                                     string strB,int indexB,int length)
		{
			return string.CompareOrdinal(strA,indexA,strB,indexB,length);
		}
		/// <summary>
		///  Compares two specified String objects by evaluating the numeric values of the 
		///  corresponding Char objects in each substring. Parameters specify the length and 
		///  starting positions of the substrings.
		/// </summary>
		/// <param name="strA">The first string.</param>
		/// <returns>A 32-bit signed integer indicating the lexical relationship between the two 
		/// <param name="strB">The second string.</param>
		/// comparands.</returns>
		/// <remarks>Value Type Condition
		/// Less than zero The substring in strA is less than the substring in strB. 
		/// Zero The substrings are equal, or length is zero.
		/// Greater than zero The substring in strA is greater than the substring in strB. 
		///	</remarks>
		/// <remarks>Replaces less_i(DString), and eq_i(DString)</remarks>
		public static int CompareOrdinal(string strA,string strB)
		{
			return string.CompareOrdinal(strA,strB);
		}
		#endregion CompareOrdinal
		#region Concat
		/// <summary>
		///  Concatenates the elements of a specified String array.
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public static DString Concat(params string[]values)
		{
			return string.Concat(values);
		}
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public static DString Concat(params object[] args)
		{
			return string.Concat(args);
		}
		#endregion Concat
		#region Copy
		/// <summary>
		///   See String documentation.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static DString Copy(string str)
		{
			return string.Copy(str);
		}
		#endregion
		#region CopyTo
		/// <summary>
		///   See String documentation.
		/// </summary>
		/// <param name="sourceIndex"></param>
		/// <param name="destination"></param>
		/// <param name="destinationIndex"></param>
		/// <param name="count"></param>
		public void CopyTo(int sourceIndex,ref char[] destination,int destinationIndex,int count)
		{
			Value.CopyTo(sourceIndex,destination,destinationIndex,count);
		}
		#endregion
		#region EndsWith
		/// <summary>
		///   See String documentation.
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		/// <remarks></remarks>
		public bool EndsWith(string val)
		{
			return Value.EndsWith(val);
		}
		#endregion
		#region Format
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static DString Format(IFormatProvider provider,string format,params object[] args)
		{
			return string.Format(provider,format,args);
		}
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static DString Format(string format,params object[] args)
		{
			return string.Format(format,args);
		}
		#endregion		
		#region IndexOf
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="val"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public int IndexOf(string val,int startIndex,int count)
		{
			return Value.IndexOf(val,startIndex,count);
		}
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="val"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		public int IndexOf(string val,int startIndex)
		{
			return Value.IndexOf(val,startIndex);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public int IndexOf(string val)
		{
			return Value.IndexOf(val);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public int IndexOf(char val,int startIndex,int count)
		{
			return Value.IndexOf(val,startIndex,count);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		public int IndexOf(char val,int startIndex)
		{
			return Value.IndexOf(val,startIndex);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public int IndexOf(char val)
		{
			return Value.IndexOf(val);
		}
		#endregion
		#region IndexOfAny
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="anyOf"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public int IndexOfAny(string anyOf,int startIndex,int count)
		{
			return Value.IndexOfAny(anyOf.ToCharArray(),startIndex,count);
		}
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="anyOf"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		public int IndexOfAny(string anyOf,int startIndex)
		{
			return Value.IndexOfAny(anyOf.ToCharArray(),startIndex);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="anyOf"></param>
		/// <returns></returns>
		public int IndexOfAny(string anyOf)
		{
			return Value.IndexOfAny(anyOf.ToCharArray());
		}
		#endregion
		#region Join
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="separator"></param>
		/// <param name="val"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static DString Join(string separator, string[] val,int startIndex,int count)
		{
			return string.Join(separator,val,startIndex,count);
		}
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="separator"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public static DString Join(string separator, string[] val)
		{
			return string.Join(separator,val);
		}
		#endregion
		#region LastIndexOf
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public int LastIndexOf(char val)
		{
			return Value.LastIndexOf(val);
		}
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="val"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		public int LastIndexOf(char val,int startIndex)
		{
			return Value.LastIndexOf(val,startIndex);
		}
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="val"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public int LastIndexOf(char val,int startIndex,int count)
		{
			return Value.LastIndexOf(val,startIndex,count);
		}
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public int LastIndexOf(string val)
		{
			return Value.LastIndexOf(val);
		}
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="val"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		public int LastIndexOf(string val,int startIndex)
		{
			return Value.LastIndexOf(val,startIndex);
		}
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="val"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public int LastIndexOf(string val,int startIndex,int count)
		{
			return Value.LastIndexOf(val,startIndex,count);
		}
		#endregion
		#region LastIndexOfAny 
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="anyOf"></param>
		/// <returns></returns>
		public int LastIndexOfAny(string anyOf)
		{
			return Value.LastIndexOfAny(anyOf.ToCharArray());
		}
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="anyOf"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		public int LastIndexOfAny(string anyOf,int startIndex)
		{
			return Value.LastIndexOfAny(anyOf.ToCharArray(),startIndex);
		}
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="anyOf"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public int LastIndexOfAny(string anyOf,int startIndex,int count)
		{
			return Value.LastIndexOfAny(anyOf.ToCharArray(),startIndex,count);
		}
		#endregion
		#region PadLeft 
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="totalWidth"></param>
		/// <returns></returns>
		/// <remarks>Replaces pad_left()</remarks>
		public DString PadLeft(int totalWidth)
		{
			Value = Value.PadLeft(totalWidth);
      return this;
		}
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="totalWidth"></param>
		/// <param name="paddingChar"></param>
		/// <returns></returns>
		/// <remarks>Replaces pad_left(const char*,int)</remarks>
		public DString PadLeft(int totalWidth,char paddingChar)
		{
			Value = Value.PadLeft(totalWidth,paddingChar);
      return this;
		}
    /// <summary>
    ///  Guarantees exact length.  See String documentation.
    /// </summary>
    /// <param name="totalWidth"></param>
    /// <param name="paddingChar"></param>
    /// <returns></returns>
    /// <remarks>like PadLeft but shortens too</remarks>
    public DString PadLeftChop(int totalWidth,char paddingChar)
    {
      Value = Value.PadLeft(totalWidth,paddingChar);
      if (Value.Length > totalWidth)
      {
        if(Value[0] == paddingChar)
          Value = Value.Substring(0,totalWidth);
        else
          Value = Value.Substring(Value.Length - totalWidth,totalWidth);
      }
      return this;
    }
		#endregion
		#region PadRight
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="totalWidth"></param>
		/// <returns></returns>
		/// <remarks>Replaces pad_right(int)</remarks>
		public DString PadRight(int totalWidth)
		{
			Value = Value.PadRight(totalWidth); 
      return this;
		}
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="totalWidth"></param>
		/// <param name="paddingChar"></param>
		/// <returns></returns>
		/// <remarks>Replaces pad_right(int,char)</remarks>
		public DString PadRight(int totalWidth,char paddingChar)
		{
			Value = Value.PadRight(totalWidth,paddingChar);
      return this;
		}
    /// <summary>
    ///  Truncate or expand.  Also see String documentation.
    /// </summary>
    /// <param name="totalWidth"></param>
    /// <param name="paddingChar"></param>
    /// <returns></returns>
    /// <remarks>expand rightward or truncate</remarks>
    public DString PadRightChop(int totalWidth,char paddingChar)
    {
      Value = Value.PadRight(totalWidth,paddingChar);
      if (Value.Length > totalWidth)
        Value = Value.Substring(0,totalWidth);
      return this;
    }
		#endregion
		#region StartsWith 
		/// <summary>
		///  See String documentation.
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		/// <remarks>Replaces firstchar(char)</remarks>
		public bool StartsWith(string val)
		{
			return Value.StartsWith(val);
		}
		#endregion
		#region Substring 
		/// <summary>
		/// See String documentation.
		/// </summary>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		/// <remarks>Replaces substr(int,int)</remarks>
		public DString Substring(int startIndex)
		{
			if (startIndex < 0 )
			{
				System.Diagnostics.Trace.WriteLine("In Substring(int);\"Out of bounds error\"");
				return DString._EMPTY;
			}
			return Value.Substring(startIndex);
		}
		/// <summary>
		/// See String documentation.
		/// </summary>
		/// <param name="startIndex"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public DString Substring(int startIndex,int length)
		{
			if (startIndex < 0 )
			{
				System.Diagnostics.Trace.WriteLine("In Substring(int,int);\"Out of bounds error\"");
				return DString._EMPTY;
			}
      if (length < 0)
      {
        length = Length - startIndex;
      }
      if (length <= 0)
        return DString._EMPTY;
			return Value.Substring(startIndex,length);
		}
		#endregion
		#region ToCharArray
		/// <summary>
		/// See String documentation.
		/// </summary>
		/// <returns></returns>
		public char[] ToCharArray()
		{
			return Value.ToCharArray();
		}
		/// <summary>
		/// See String documentation.
		/// </summary>
		/// <param name="startIndex"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public char[] ToCharArray(int startIndex,int length)
		{
			return Value.ToCharArray(startIndex,length);
		}
		#endregion
		#region ToLower
		/// <summary>
		/// See String documentation.
		/// </summary>
		/// <returns></returns>
		public DString ToLower()
		{
			Value = Value.ToLower();
			return this;
		}
		/// <summary>
		/// See String documentation.
		/// </summary>
		/// <param name="culture"></param>
		/// <returns></returns>
		public DString ToLower(CultureInfo culture)
		{
			Value = Value.ToLower(culture);
			return this;
		}
		#endregion
		#region ToUpper
		/// <summary>
		/// See String documentation.
		/// </summary>
		/// <returns></returns>
		public DString ToUpper()
		{
			Value = Value.ToUpper();
			return this;
		}
		/// <summary>
		/// See String documentation.
		/// </summary>
		/// <param name="culture"></param>
		/// <returns></returns>
		public DString ToUpper(CultureInfo culture)
		{
			Value = Value.ToUpper(culture);
			return this;
		}
		#endregion		
		#region Trim
		/// <summary>
		/// See String documentation.
		/// </summary>
		/// <returns>Combines TrimStart and TrimEnd</returns>
		public DString Trim() 
		{ 
			Value = Value.Trim(); 
			return this;
		} 
		/// <summary>
		/// See String documentation.
		/// </summary>
		/// <param name="trimChars"></param>
		/// <returns></returns>
		public DString Trim(params char[] trimChars) 
		{ 
			Value = Value.Trim(trimChars); 
			return this;
		} 
		#endregion
		#region TrimEnd
		/// <summary>
		/// See String documentation.
		/// </summary>
		/// <param name="trimChars"></param>
		/// <returns></returns>
		/// <remarks>Replaces trim_r(char)</remarks>
		public DString TrimEnd(params char[] trimChars)
		{
			Value = Value.TrimEnd(trimChars);
			return this;
		}
		#endregion
		#region TrimStart
		/// <summary>
		/// See String documentation.
		/// </summary>
		/// <param name="trimChars"></param>
		/// <returns></returns>
		/// <remarks>Replace trim_r(char)</remarks>
		public DString TrimStart(params char[] trimChars)
		{
			Value = Value.TrimStart(trimChars);
			return this;
		}
		#endregion
		#endregion String Relay Methods
		#region StringBuilder Relay Methods
		#region Append
		/// <summary>
		///  See StringBuider documentation.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public DString Append(object obj)
		{
			_strBuffer.Append(obj);
			return this;
		}
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public DString Append(string obj,int startIndex, int count)
		{
			_strBuffer.Append(obj,startIndex,count);
			return this;
		}
		/// <summary>
		///  See StringBuider documentation.
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public DString Append(char[] val)
		{
			_strBuffer.Append(val);
			return this;
		}
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="val"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public DString Append(char[] val,int startIndex,int count)
		{
			_strBuffer.Append(val,startIndex,count);
			return this;
		}
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="val"></param>
		/// <param name="repeatCount"></param>
		/// <returns></returns>
		public DString Append(char val,int repeatCount)
		{
			_strBuffer.Append(val,repeatCount);
			return this;
		}
		#endregion Append
		#region AppendFormat
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public DString AppendFormat(IFormatProvider provider,string format,params object[] args)
		{
			_strBuffer.AppendFormat(provider,format,args);
			return this;
		}
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public DString AppendFormat(string format,params object[] args)
		{
			_strBuffer.AppendFormat(format,args);
			return this;
		}	
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="arg0"></param>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		/// <returns></returns>
		public DString AppendFormat(string format,object arg0,object arg1,object arg2)
		{
			_strBuffer.AppendFormat(format,arg0,arg1,arg2);
			return this;
		}
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="arg0"></param>
		/// <param name="arg1"></param>
		/// <returns></returns>
		public DString AppendFormat(string format,object arg0,object arg1)
		{
			_strBuffer.AppendFormat(format,arg0,arg1);
			return this;
		}
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="arg0"></param>
		/// <returns></returns>
		public DString AppendFormat(string format,object arg0)
		{
			_strBuffer.AppendFormat(format,arg0);
			return this;
		}
		#endregion AppendFormat
		#region EnsureCapacity
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="capacity"></param>
		/// <returns></returns>
		public int EnsureCapacity(int capacity)
		{
			return _strBuffer.EnsureCapacity(capacity);
		}
		#endregion EnsureCapacity
		#region Insert
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public DString Insert(int index,object val)
		{
			_strBuffer.Insert(index,val);
			return this;
		}	
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="val"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public DString Insert(int index,string val,int count)
		{
			_strBuffer.Insert(index,val,count);
			return this;
		}
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="val"></param>
		/// <param name="startIndex"></param>
		/// <param name="charCount"></param>
		/// <returns></returns>
		public DString Insert(int index,char[] val,int startIndex,int charCount)
		{
			_strBuffer.Insert(index,val,startIndex,charCount);
			return this;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="val"></param>
		/// <param name="startIndex"></param>
		/// <param name="charCount"></param>
		/// <returns></returns>
		public DString Insert(int index,string val,int startIndex,int charCount)
		{
			_strBuffer.Insert(index,val.ToCharArray(),startIndex,charCount);
			return this;
		}
		#endregion Insert
		#region Remove
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="startIndex"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public DString Remove(int startIndex,int length)
		{
      if (length < 0)
        length = Length - startIndex;
      try
      {
        _strBuffer.Remove(startIndex,length);
      }
      catch
      {
      }
			return this;
		}
		#endregion
		#region Replace
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="oldChar"></param>
		/// <param name="newChar"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public DString Replace(char oldChar,char newChar,int startIndex,int count)
		{
			_strBuffer.Replace(oldChar,newChar,startIndex,count);
			return this;
		}
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="oldChar"></param>
		/// <param name="newChar"></param>
		/// <returns></returns>
		public DString Replace(char oldChar,char newChar)
		{
			_strBuffer.Replace(oldChar,newChar);
			return this;
		}
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		/// <param name="startIndex"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public DString Replace(string oldValue,string newValue,int startIndex,int count)
		{
			_strBuffer.Replace(oldValue,newValue,startIndex,count);
			return this;
		}
		/// <summary>
		/// See StringBuider documentation.
		/// </summary>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		/// <returns></returns>
		public DString Replace(string oldValue,string newValue)
		{
			_strBuffer.Replace(oldValue,newValue);
			return this;
		}
		#endregion Replace
		#endregion StringBuilder Relay Methods
		#region Static DString Methods
		#region Alphas
		/// <summary>
		/// Initializes DString to alphabet characters.
		/// </summary>
		/// <returns>DString</returns>
		public static DString Alphas()
		{
			return new DString(@"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
		}
		#endregion
		#region DecimalPrefixes
		/// <summary>
		/// Initializes DString to decimal prefixes.
		/// </summary>
		/// <returns></returns>
		public static DString DecimalPrefixes()
		{
			return new DString(@"+-.");
		}
		#endregion
		#region Numbers
		/// <summary>
		/// Initialize DString to numeric characters.
		/// </summary>
		/// <returns></returns>
		public static DString Numbers()
		{
			return new DString(@"0123456789");
		}
		#endregion
		#region Numericals
		/// <summary>
		/// Initialize DString to significant numerical words.
		/// </summary>
		/// <returns>DString</returns>
		public static DString Numericals()
		{
			return new DString(@"0 1 2 3 4 5 6 7 8 9 10 " +	
				@"11 12 13 14 15 16 17 18 " +
				@"19 20 30 40 50 60 70 80 90 " +
				@"100 1000 1000000 1000000000 " +
				@"1000000000000 " +
				@"1000000000000000 " +
				@"1000000000000000000"
				);
		}
		#endregion
		#region EnglishNumericals
		/// <summary>
		/// Initialize DString to significant numerical words.
		/// </summary>
		/// <returns>DString</returns>
		public static DString EnglishNumericals()
		{
			return new DString(@"zero,one,two,three,four,five," +
				@"six,seven,eight,nine,ten,eleven," +
				@"twelve,thirteen,fourteen,fifteen," +
				@"sixteen,seventeen,eighteen,nineteen," +
				@"twenty,thirty,forty,fifty,sixty,seventy," +
				@"eighty,ninety,hundred,thousand,million," +
				@"billion,trillion,quadrillion,quintillion"
				);
		}
		#endregion EnglishNumericals
		#region EnglishTextByHundreds
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		
		public static DStringCollection EnglishTextByHundreds()
		{
			return (new DString(@"hundred,thousand,million," +
				@"billion,trillion,quadrillion,quintillion"
				).Split(","));
		}
		#endregion EnglishTextByHundreds
		#region MapNumbersToEnglishText
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static Hashtable MapNumbersToEnglishText()
		{
			DString dNumericals = DString.Numericals();     // 0 1 2 3 ...
			DString dWrittenNumericals = DString.EnglishNumericals(); // zero one two three ...
			// Map zero to 0, one to 1, two to 2 ...
			return dNumericals.MapTo(dWrittenNumericals.Split(",")," ");
		}
		#endregion
		#region MapEnglishTextToNumbers
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static Hashtable MapEnglishTextToNumbers()
		{
			DString dNumericals = DString.Numericals();     // 0 1 2 3 ...
			DString dWrittenNumericals = DString.EnglishNumericals(); // zero one two three ...
			// Map zero to 0, one to 1, two to 2 ...
			return dWrittenNumericals.MapTo(dNumericals.Split(),",");
		}
    #endregion
    #region GetLastNumberAndStub
    /// <summary>
    /// static version of GetLastNumberAndStub
    /// returns pair of string stub, long number
    /// </summary>
    public static Pair GetLastNumberAndStub(string str)
    {
      DString d = new DString(str);
      return d.GetLastNumberAndStub();
    }
    #endregion
    #region GetTripartite
    /// <summary>
		/// static version of GetTripartite
		/// returns stub, number, and final number of a string
		/// </summary>
		public static Triplet GetTripartite(string str)
		{
		  DString d = new DString(str);
		  return d.GetTripartite();
		}
		#endregion
		#endregion Static DString Methods
		#region DString Methods
		#region ChopLeft
		/// <summary>
		/// ChopLeft() chops one character off the beginning .
		/// </summary>
		/// <returns>Chopped substring</returns>
		public DString ChopLeft()
		{
			if ( 0 == Length )
				return DString._EMPTY;
			else
			{
				string s = Substring(0,1);
				Remove(0,1);
				return s;
			}
		}
		/// <summary>
		/// ChopLeft(int n) chops n number of character(s) off the beginning. 
		/// </summary>
		/// <param name="n">int</param>
		/// <returns>Chopped substring</returns>
		/// <remarks>Replaces chop_left(int)</remarks>
		public DString ChopLeft(int n)
		{
			if ( 1 > n)
			{
				return DString._EMPTY;
			}
			else if ( n > Length - 1 )
			{
				string d = Value;
				Clear();
				return d;
			}
			else
			{
				string s = Substring(0,n);
				Remove(0,n);
				return s;
			}
		}
		#endregion
		#region ChopRight
		/// <summary>
		/// ChopRight() chops one character off the end. 
		/// </summary>
		/// <returns>DString with one less character at the beginning</returns>
		public DString ChopRight()
		{
			if ( 0 == Length )
				return DString._EMPTY;
			else
			{
				string d = Substring(Length-1,1);
				Remove(Length-1,1);
				return d;
			}
		}
		/// <summary>
		/// ChopRight(int n) chops n number of character(s) off the end.
		/// </summary>
		/// <param name="n">int</param>
		/// <returns>DString with "n" less character(s) at the end</returns>
		public DString ChopRight(int n)
		{
			if ( 1 > n)
			{
				return DString._EMPTY;
			}
			else if ( n > Length - 1)
			{
				string d = Value;
				Clear();
				return d;
			}
			else
			{
				string d = Substring(Length-n,n);
				Remove(Length-n,n);
				return d;
			}
		}
		#endregion
		#region Clear
		/// <summary>
		/// Set DString to empty
		/// </summary>
		/// <returns>
    /// True; Release of buffer was successful. False; Failed to release buffer
    /// </returns>
		public bool Clear() 
		{ 
			Remove(0,Length);
			return IsEmpty();
		}
		#endregion
		#region Contains
		/// <summary>
		/// Is the character found?
		/// </summary>
		/// <param name="val">char</param>
		/// <returns>bool; True - character is found, False, if not found</returns>
		public bool Contains(char val) 
		{ 
			return NOT_FOUND != IndexOf(val);
		}
		/// <summary>
		/// Is the string found?
		/// </summary>
		/// <param name="val">string</param>
		/// <returns>bool; True - string is found, False, if not found</returns>
		/// <remarks>Replaces isin()</remarks>
		public bool Contains(string val) 
		{ 
			return NOT_FOUND != IndexOf(val);
		}
		#endregion Contains
		#region ContainsAny
		/// <summary>
		/// Does the string contain any of these characters?
		/// </summary>
		/// <param name="val">string; characters to find</param>
		/// <returns>
    /// bool; True - contains one of the characters; False - none of the characters are found
    /// </returns>
		public bool ContainsAny(string val)
		{ 
			if (NOT_FOUND != IndexOfAny(val))
				return true;
			return false;
		}
		#endregion
		#region CountWords
		/// <summary>
		/// Split the string by the default separator (space), 
    /// and return the number of words that result.
		/// </summary>
		/// <returns>int; The number of splits</returns>
		public int CountWords()
		{
			DStringCollection v = Split();
			if (1 == v.Count) // If one item and it happens to be empty.
			{
				if (v[0].IsBlank())
					v.Clear();
			}
			return v.Count;
		}
		/// <summary>
		/// Split the string by the separator, and return the number of words that result.
		/// </summary>
		/// <param name="separ">string; Separator</param>
		/// <returns>int; The number of splits</returns>
		public int CountWords(string separ)
		{
			DStringCollection v = Split(separ);
			if (1 == v.Count) // If one item and it happens to be empty.
			{
				if (v[0].IsBlank())
					v.Clear();
			}
			return v.Count;
		}
		#endregion
		#region Cutout
		/// <summary>
		/// Remove all characters between the beginning of left position and right position
		/// depending on the exclude value and returns the result.
		/// </summary>
		/// <param name="leftpos">int; Start position</param>
		/// <param name="rightpos">int; End position</param>
		/// <param name="excl"> 
		///  case 1: Do not cut out left
		///  case 2: Do not cut out right
		///  case 3: Do not cut out left or right
		///  </param>
		/// <returns>The substring cutout from the original string</returns>
		public DString Cutout(int leftpos, int rightpos, int excl)
		{
			if (leftpos < 0)
				leftpos = 0;
			if ( rightpos < 0 )  // negative rightpos indicates width
				rightpos = leftpos - rightpos - 1;
			switch ( excl )
			{
				case 1: 
					leftpos++; 
					break;  // do not cut out left
				case 2: 
					rightpos--; 
					break; // do not cut out right
				case 3: 
					leftpos++; 
					rightpos--; 
					break; // do not cut out either
			}
			DString ret = Substring( leftpos, rightpos - leftpos + 1);
			Remove(leftpos, rightpos - leftpos + 1);
			return ret;
		}
		/// <summary>
		/// Remove all characters between beginning of left postion and end of right substring
		/// depending on the exclude value and returns the result.
		/// </summary>
		/// <param name="leftpos">int; Start position</param>
		/// <param name="right">string; bound string at right side</param>
		/// <param name="excl"> 
		///  case 1: Do not cut out left
		///  case 2: Do not cut out right
		///  case 3: Do not cut out left or right
		///  </param>
		/// <returns>The substring cutout from the original string</returns>
		public DString Cutout(int leftpos, string right, int excl)
		{
			int pos2 = -1;			
			DString ret = new DString();
			if ((pos2 = IndexOf( right, leftpos+1 ))!= NOT_FOUND)
			{
				pos2 += right.Length - 1;
				switch ( excl )
				{
					case 1: 
						leftpos++; 
						break;
					case 2: 
						pos2 -= right.Length; 
						break;
					case 3: 
						leftpos++; 
						pos2 -= right.Length; 
						break;
				}
				ret = CutoutExcl0(leftpos, pos2);
			}
			return ret;
		}
		/// <summary>
		/// Remove all characters between the beginning of left string 
    /// and the right index positiondepending on the exclude value and returns the result.
		/// </summary>
		/// <param name="left">string; Start substring</param>
		/// <param name="rightpos">int; right index position</param>
		/// <param name="excl"> 
		///  case 1: Do not cut out left
		///  case 2: Do not cut out right
		///  case 3: Do not cut out left or right
		///  </param>
		/// <returns>The substring cutout from the original string</returns>
		public DString Cutout(string left, int rightpos, int excl)
		{
			DString ret = new DString();
			int pos  = 0;
			if ( rightpos == -1 )
				rightpos = Length - 1;
			if ((pos = IndexOf( left ))!= NOT_FOUND)
			{
				switch ( excl )
				{
					case 1: 
						pos += left.Length; 
						break;            // do not cut out left
					case 2: 
						if (rightpos > 0) 
							--rightpos; 
						break;// do not cut out right
					case 3: 
						pos += left.Length; 
						--rightpos; 
						break;// do not cut out left or right
				}
				ret = CutoutExcl0(pos, rightpos);
			}
			return ret;
		}
		#endregion Cutout
		#region Cutout Relays
		/// <summary>
		/// Relay to Cutout(int,int,0); Exclude nothing from cutout; cutout maximum
		/// <seealso cref="DString.Cutout"/>
		/// </summary>
		/// <param name="leftpos">int; Left index position</param>
		/// <param name="rightpos">int; Right index position</param>
		/// <returns>DString; Return Cutout substring.</returns>
		public DString CutoutExcl0(int leftpos, int rightpos)
		{  
			return Cutout(leftpos, rightpos, 0);
		}
		/// <summary>
		/// Relay to Cutout(string,string,0); Exclude nothing from cutout; cutout maximum
		/// <seealso cref="DString.Cutout"/>
		/// </summary>
		/// <param name="left">string; Beginning index position of left</param>
		/// <param name="right">string; Beginning index position of right</param>
		/// <returns>DString; Return Cutout substring.</returns>
		public DString CutoutExcl0(string left, string right)
		{  
			return Cutout(left, right, 0);
		}
		/// <summary>
		/// Relay to Cutout(int,int,1); Exclude left from cutout
		/// <seealso cref="DString.Cutout"/>
		/// </summary>
		/// <param name="left">string; Beginning index position of left</param>
		/// <param name="right">string; Beginning index position of right</param>
		/// <returns>DString; Return Cutout substring.</returns>
		public DString CutoutExcl1(string left, string right)
		{  
			return Cutout(left, right, 1);
		}
		/// <summary>
		/// Relay to Cutout(int,int,2); Exclude right from cutout.
		/// <seealso cref="DString.Cutout"/>
		/// </summary>
		/// <param name="left">string; Beginning index position of left</param>
		/// <param name="right">string; Beginning index position of right</param>
		/// <returns>DString; Return Cutout substring.</returns>
		public DString CutoutExcl2(string left, string right)
		{  
			return Cutout(left, right, 2);
		}
		/// <summary>
		/// Relay to Cutout(int,int,3); Exclude both from cutout.
		/// <seealso cref="DString.Cutout"/>
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public DString CutoutExcl3(string left, string right)
		{  
			return Cutout(left, right, 3);
		}
		#endregion
		#region Decimalize
		/// <summary>
		/// Transforms the original value into a decimalized value with 
		/// "x" number of decimal places and returns the result.
		/// </summary>
		/// <param name="places">int; Number of decimal places</param>
		/// <returns>Modified DString</returns>
		public DString Decimalize( int places )
		{
			const int maxplaces = 28;
			places = System.Math.Abs(places);
			if (places > maxplaces )
				places = maxplaces;
			string format = "F" + places.ToString();
      double dbl = Convert.ToDouble(Value);
      Value = dbl.ToString(format);
			TrimStart('0');
			return this;
		}
		#endregion
		#region DecimalPlaces 
		/// <summary>
		/// Counts the number of decimal places, and returns the value.
		/// </summary>
		/// <returns>int; Number of decimal places</returns>
		public int DecimalPlaces() 
		{
			int pos = LastIndexOf('.');
			if( pos == NOT_FOUND || LastChar() == '.')
				return 0;
			DString result = Substring(pos + 1);
			return result.Length;
		}
		#endregion
		#region ElimFirst
		/// <summary>
		/// Remove first control-character from string
		/// </summary>
		/// <param name="control">string; control item, to be found in the string</param>
		/// <returns>
    /// bool; 
    /// True - control string found and character removed; 
    /// False - control string not found
    /// </returns>
		public bool ElimFirst(string control)
		{
			int pos = IndexOf(control);
			if ( pos != NOT_FOUND )
				Remove( pos, 1 );
			return pos != NOT_FOUND;
		}
		#endregion
		#region ElimLast 
		/// <summary>
		/// Remove first character of the last search item found in the string
		/// </summary>
		/// <param name="control">string; control item, to be found in the string</param>
		/// <returns>
    /// bool; 
    /// True - control string found and character removed;
    /// False - control string not found
    /// </returns>
		public bool ElimLast(string control)
		{
			int pos = LastIndexOf(control);
			if ( pos != NOT_FOUND )
				Remove( pos, 1 );
			return pos != NOT_FOUND;
		}
		#endregion
		#region EveryNthChar
		/// <summary>
		/// // Extract every nth char and return the string result
		/// </summary>
		/// <param name="n">int; The interval in which the characters are to be extracted</param>
		/// <param name="startPos">int; The beginning of the position.</param>
		/// <param name="endPos">int; The end of the extraction.</param>
		/// <returns>DString; Extracted characters</returns>
		public DString EveryNthChar(int n, int startPos, int endPos )
		{
			if ( n <= 0 )				// not valid, return nothing
				return new DString();
			if (startPos < 0)			// Make sure startPos is at least 0.
				startPos = 0;
			if (endPos > Length )		// Make sure endPos is no greater than length of string
				endPos = Length;
			DString d = new DString();

			while ( startPos < endPos )
			{
				d += this[startPos];
				startPos +=n;
			}
			return d;
		}
		#endregion
		#region Excise
		/// <summary>
		/// Excise the first occurence of the target from the string 
		/// and return boolean value, if operation succeed or failed.
		/// </summary>
		/// <param name="target">string; Item to be excised from the string</param>
		/// <returns>bool
		///  True - Target was removed from the string.
		///  False - Target was not removed from the string.
		///  </returns>
		public bool Excise(string target)
		{
			bool ret = false;
			int pos = IndexOf(target);
			if ( pos != NOT_FOUND )
			{
				Remove(pos, target.Length);
				ret = true;
			}
			return ret;
		}
		#endregion
		#region FirstChar
		/// <summary>
		/// Gets the first char from the string
		/// </summary>
		/// <returns>char; first character</returns>
		public char FirstChar()
		{
			return Length > 0?Value[0]:' ';
		}
		#endregion
		#region FirstWord
		/// <summary>
		/// Gets the first word from the string
		/// </summary>
		/// <returns>DString; First word from the string</returns>
		public DString FirstWord()
		{
			return FirstWord(" ");
		}
		/// <summary>
		/// Gets the first word from the string
		/// </summary>
		/// <param name="separ">string; delimitor to split string to words</param>
		/// <returns>DString; First word from the string</returns>
		public DString FirstWord( string separ )
		{
			int pos = -1;
			if (Length > 0)
			{
				pos = IndexOf(separ);
				if ( pos != NOT_FOUND)
				{
					return Substring(0,pos);
				}
				else
				{
					return Value;
				}
			}
			return new DString();
		}
		#endregion
		#region ForceRightSpace 
		/// <summary>
		/// Nonempty string does not end in space, than append a space on end of string
		/// </summary>
		/// <returns></returns>
		public bool ForceRightSpace()
		{
			if ( !IsEmpty() && LastChar() != ' ' )
			{
				_strBuffer.Append(' ');
				return true;
			}
			return false;
		}
		#endregion
    #region FormatNumber
    /// <summary>
    /// Insert formatting commas in number (U.S. culture)
    /// </summary>
    /// <returns>Modified DString</returns>
    public DString FormatNumber()
    {
      int pos = this.IndexOf('.');
      bool bzero = false;
      string str = pos > -1 ? this.Cutout(pos,0,0).ToString() : "";  // cut decimal
      bool neg = FirstChar() == '-';
      if (neg)
        ChopLeft();
      while (FirstChar() == '0')
      {
        ChopLeft();
        bzero = true;
      }
      if (Length == 0 && bzero)
        Value = "0"; 
      InsertEveryNFromRight(",", 3);
      this.Append(str);
      if (neg && Value != "0" && Value != "0.")
        Prepend("-");
      return this;
    }
    #endregion FormatNumber
		#region GetLastNumber
		/// <summary>
		/// Get last positive int from string; do not remove it, but return it
		/// </summary>
		/// <returns>long; The last number from string; else -1</returns>
		public long GetLastNumber() 
		{
      List<string> lstr = GetNumNonNumPieces();
      DString d = new DString();
      if (lstr.Count > 0)
      {
        for (int i = lstr.Count - 1; i >= 0; --i)
        {
          d = lstr[i];
          if (d.IsNumber())
            return Convert.ToInt64(d.ToString());
        }
      }
      return -1;
		}

		/// <summary>
		/// Get both the terminal number and the stub before it
		/// return pair of string stub and long number
		/// </summary>
    public Pair GetLastNumberAndStub()
    {
      List<string> lstr = GetNumNonNumPieces();
      DString d = new DString();
      string stub = "";
      string last = "";
      if (lstr.Count > 0)
      {
        foreach (string str in lstr)
        {
          d = str;
          if (last != string.Empty)
          {
            stub += last;
            last = string.Empty;
          }
          if (!d.IsNumber())
            stub += str;
          else
            last = str;
        }
      }
      if (last == string.Empty)
        last = "-1";
      return new Pair(stub, Convert.ToInt64(last));
    }
    #endregion
    #region GetMap
    /// <summary>
    /// Creates a map from the string. Matching adjacent words. 
    /// An unmatch word at the end the string will be tossed. 
    /// Returns the map result.
    /// </summary>
    /// <returns>Hashtable; Mapped words</returns>
    public Hashtable GetMap()
		{					
			int iv = 0;
			Hashtable m = new Hashtable();
			DStringCollection v = Split();
			while ( iv + 1 < v.Count )
			{
				m.Add(v[iv],v[iv+1]);
				iv += 2;
			}
			return m;
		}
		/// <summary>
		/// Creates a map from the string. Matching adjacent words. 
		/// An unmatch word at the end the string will be tossed. 
		/// Returns the map result.
		/// </summary>
		/// <param name="separ">string; Used to tokenize the string</param>
		/// <returns>Hashtable; Mapped words</returns>
		public Hashtable GetMap( string separ )
		{					
			int iv = 0;
			DStringCollection v = Split(separ,true);
      Hashtable m = new Hashtable(v.Count/2);
      while ( iv + 1 < v.Count )
			{
				m.Add(v[iv],v[iv+1]);
				iv += 2;
			}
			return m;
		}
    /// <summary>
    /// Creates a StringDictionary from the string. Matching adjacent words. 
    /// An unmatch worded at the end the string will be tossed. 
    /// Returns the StringDictionary result.
    /// </summary>
    /// <param name="separ">string; Used to tokenize the string</param>
    /// <returns>Hashtable; Mapped words</returns>
    public StringDictionary GetStringDictionary(string separ)
    {
      int iv = 0;
			DStringCollection v = Split(separ,true);
      StringDictionary sd = new StringDictionary();
      while ( iv + 1 < v.Count )
			{
				sd.Add(v[iv],v[iv+1]);
				iv += 2;
			}
			return sd;
    }          
    #endregion
		#region GetNumber 
		/// <summary>
		/// Get first int from string; do not remove it, but return it
		/// </summary>
		/// <returns>long; The first number from string; else return -1</returns>
		public long GetNumber()
		{
      List<string> lstr = GetNumNonNumPieces();
      DString d = new DString();
      foreach (string str in lstr)
      {
        d = str;
        if (d.IsNumber())
          return Convert.ToInt64(d.ToString());
      }
      return -1;
		}
		#endregion
		#region GetNumberString 
		/// <summary>
		/// Get first number in string form from string
		/// do not remove it, but return it;
		/// if no number, return ""
		/// </summary>
		/// <returns>DString; Number or 0;</returns>
		public DString GetNumberString()
		{
      List<string> lstr = GetNumNonNumPieces();
      DString d = new DString();
      foreach (string str in lstr)
      {
        d = str;
        if (d.IsNumber())
          return d;
      }
      return d;
		}
		#endregion
    #region GetNumNonNumPieces()
    /// <summary>
    /// Get numeric (decimal digits only) and non-numeric pieces in string array
    /// </summary>
    public List<string> GetNumNonNumPieces()
    {
      MatchCollection mc = Regex.Matches(Value,"(\\d+|[^\\d]+)");
      List<string> lstr = new List<string>(mc.Count);
      foreach (Match m in mc)
      {
        lstr.Add(m.Value);
      }
      return lstr;
    }
    #endregion
    #region GetStrings
    /// <summary>
    ///  Tokenizes a string into a list of elements using space as deliminator
    /// </summary>
    /// <returns>string list; A list of words.</returns>
    public List<string> GetStrings()
    {
      return GetStrings(" ",false);
    }
    /// <summary>
    /// Tokenizes a string into a list of elements.
    /// </summary>
    /// <param name="separator">string; Where to break the string into elements.</param>
    /// <returns>string list</returns>
    /// <remarks>
    /// this version processes the separator as a string of separate letters
    /// </remarks>
    public List<string> GetStrings(string separator)
    {
      DString temp = this.Clone() as DString;
      if (separator.Contains(" "))
        temp.TrimAll(' ');
      return temp.GetStrings(separator, true);
    }
    /// <summary>
    /// Tokenizes a string into a list of elements.
    /// </summary>
    /// <param name="separator">string; Where to break the string into elements.</param>
    /// <param name="bseparate">bool; Does the string indicate separate characters?</param>
    /// <returns>string list; Contains a list of elements(words)</returns>
    public List<string> GetStrings(string separator, bool bseparate)
    {
      DString sep = DString.CreateEscapeChars(separator);
      DString temp = this.Clone() as DString;
      if (bseparate || separator == " ")
      {
        if (separator.Contains(" "))
        {
          temp.TrimAll(' ');
        }
        sep.GuaranteeFirstLast('[', ']');
      }
      Regex rx = new Regex(sep);
      List<string> ls = new List<string>(rx.Split(temp.Value));
      return ls;
    }

    #endregion GetStrings
    #region GetTripartite
    /// <summary>
    /// Parse string for first non number, second to last number, and last number
    /// stub will contain everything but last number unless the separator between the two numbers is -
    /// </summary>
    /// <returns>Triplet: 1st string stub 2nd long penultimate number 3rd long ultimate number</returns>
    public Triplet GetTripartite()
    {
      List<string> lstr = GetNumNonNumPieces();
      DString d = new DString();
      long last = -1;
      long middle = -1;
      int index = lstr.Count;
      int lindex = lstr.Count;
      string sep = "";
      for (int i = lstr.Count - 1; i >= 0; --i)  // start w/ last work back
      {
        d = lstr[i];
        if (d.IsNumber())    // if not get first last nonnum
        {
          long l = Convert.ToInt64(d.ToString());
          if (last == -1)    // no last yet
          {
            last = l;
            index = i;
            lindex = i;
          }
          else if (middle == -1) // middle number
          {
            middle = l;
            index = i;
            break;
          }
        }
        else
        {
          if (last != -1 && string.IsNullOrEmpty(sep))
            sep = d;
        }
      }
      string stub = "";
      if (sep != "-")
        index = lindex;
      for (int j = 0; j < index; ++j)
        stub += lstr[j];
      if (middle == -1)
        middle = last;
      return new Triplet(stub, middle, last);
    }
    #endregion
    #region Guarantees
    /// <summary>
    /// Guarantee the first character, is that character.
    /// </summary>
    /// <param name="c">char;</param>
    /// <returns>DString; Modified string</returns>
    public DString GuaranteeFirst(char c)
		{
			return FirstChar() != c?Insert(0, c):this;
		}
		/// <summary>
		/// Guarantee the last character, is that character.
		/// </summary>
		/// <param name="c">char; </param>
		/// <returns>DString; Modified string</returns>
		public DString GuaranteeLast(char c)
		{
			return LastChar()!=c?Append(c):this;
		}
		/// <summary>
		/// Guarantee the first and last characters, are that character.
		/// </summary>
		/// <param name="first">char; first character</param>
		/// <param name="last">char; last character</param>
		/// <returns>DString; Modified string</returns>
		public DString GuaranteeFirstLast(char first,char last)
		{
			GuaranteeFirst(first);
			return GuaranteeLast(last);
		}
    /// <summary>
    /// make sure start of string is not char c
    /// </summary>
    /// <param name="target"></param>
    /// <param name="c"></param>
    /// <returns>If change made, modified string return
    ///          No change made, returns original string
    /// </returns>
    public DString GuaranteeNotFirst(char c)
    {
      string expr = Regex.Escape(c.ToString());
      expr = @"^" + expr + "*";
      string str = Regex.Replace(this.ToString(),expr,"");
      Value = str;
      return this;
    }
    /// <summary>
    /// make sure end of string is not char c
    /// </summary>
    /// <param name="target"></param>
    /// <param name="c"></param>
    /// <returns>If change made, modified string return
    ///          No change made, returns original string
    /// </returns>
    public DString GuaranteeNotLast(char c)
    {
      string expr = Regex.Escape(c.ToString())+ "*$";
      string str = Regex.Replace(this.ToString(),expr,"");
      Value = str;
      return this;
    }
    #endregion 
    #region IndexOfNotAny 
		/// <summary>
		/// Finds the first position of any character not of the control string.
		/// </summary>
		/// <param name="control">
		/// string; Any character that is not of this string, 
		/// return it's position
		/// </param>
		/// <returns>int; Position of the first character not of the control string</returns>
		public int IndexOfNotAny(string control) //basic_string::find_first_not_of
		{
			bool found = false;
			for(int pos = 0; pos < Length; ++pos)
			{
				foreach(char c in control)
				{
					if (0 == Value[pos].CompareTo(c)) // It is one of the control characters
					{
						found = true;    // flag it.
					}
				}
				if (found)
					found = false;
				else 
					return pos;
			}
			return NOT_FOUND;
		}
		/// <summary>
		/// Finds the first position of any character not of the control string.
		/// </summary>
		/// <param name="control">
		/// string; Any character that is not of this string, return it's position
		/// </param>
		/// <param name="startPos">int; start position for the search</param>
		/// <returns>int; Position of the first character not of the control string</returns>
		public int IndexOfNotAny(string control, int startPos) //basic_string::find_first_not_of
		{
			bool found = false;
			for(int pos = startPos; pos < Length; ++pos)
			{
				foreach(char c in control)
				{
					if (0 == Value[pos].CompareTo(c)) // It is one of the control characters
					{
						found = true;    // flag it.
					}
				}
				if (found)
					found = false;
				else 
					return pos;
			}
			return NOT_FOUND;
		}
		#endregion
    #region InsertEveryNFromRight
    /// <summary>
    /// Inserts a substring every n characters from the right
    /// 999888777 (with ",") becomes 999,888,777
    /// performs the last insert only if substring to left ends with a number
    /// </summary>
    /// <param name="control">
    /// string to insert
    /// </param>
    /// <param name="n">
    /// number of characters before insertion (from the right)
    /// </param>
    /// <returns>Modified String </returns>>
    public DString InsertEveryNFromRight(string control, int n)
    {
      DString dum = this.Clone() as DString;
      DString dnew = new DString();
      do
      {
        dnew.Prepend(dum.ChopRight(n));
        if (!dum.IsEmpty())
        {
          dnew.Prepend(control);
        }
      }
      while (!dum.IsEmpty());
      _strBuffer = dnew._strBuffer;
      return this;
    }
    #endregion InsertEveryNFromRight
		#region IsAlpha
		/// <summary>
		/// Test for Alphabet characters
		/// </summary>
		/// <returns>bool; True, if all alphabet character; 
		/// False, if non-alphabet characters found</returns>
		public bool IsAlpha()
		{
			int pos=IndexOfNotAny(DString.Alphas());
			return pos==NOT_FOUND;
		}
		#endregion
		#region IsBlank 
		/// <summary>
		///  Test for finding only white characters ("\t\n\r ")
		/// </summary>
		/// <returns>bool; True, found white characters only; 
		/// False, if not found something other than white characters.</returns>
		public bool IsBlank()
		{
			int pos=IndexOfNotAny("\t\n\r ");
			return pos==NOT_FOUND;
		}
		#endregion
		#region IsBound
		/// <summary>
		/// Test for string to be bounded by a set of characters
		/// </summary>
		/// <param name="bounds">string</param>
		/// <returns>bool; True, string is bounded by character, otherwise false</returns>
		public bool IsBound( string bounds ) 
		{
			DString d = bounds;
			bool ret = FirstChar() == d.FirstChar();
			ret = ret && LastChar() == d.LastChar();
			return ret;
		}
		#endregion
		#region IsEmpty
		/// <summary>
		/// Tests for Empty DString.
		/// </summary>
		/// <returns>bool; True, for empty; False, not empty</returns>
		public bool IsEmpty()
		{
			return this.Equals(DString._EMPTY);
		}
		#endregion
		#region IsDecimal 
		/// <summary>
		/// Tests for floating point number
		/// </summary>
		/// <returns>bool; True, valid floating point number; False, not valid.</returns>
		public bool IsDecimal()
		{
			int pos = IndexOfNotAny( Numbers() + "+-.Ee" );
			return pos==NOT_FOUND;
		}
		#endregion
		#region IsNumber 
		/// <summary>
		/// Tests for valid number.
		/// </summary>
		/// <returns>bool; True, valid integer; 
		/// False, not valid integer</returns>
		public bool IsNumber() 
		{
			int pos = IndexOfNotAny( Numbers() );
			return pos==NOT_FOUND && Length > 0;
		}
		#endregion
		#region LastChar
		/// <summary>
		/// Gets the last character of the string.
		/// </summary>
		/// <returns>char; Last character or nothing.</returns>
		public char LastChar()
		{
			return ( Length < 1 )? ' ': Value[Length-1];
		}
    /// <summary>
    /// returns the last character left offset by index
    /// index may be positive or negative
    /// because negative index is not intuitively right
    /// index of 0 emits same behavior as LastChar()
    /// index of 1 or -1 gets second to last character
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public char LastChar(int index)
    {
      index = Math.Abs(index);
      if (index >= Length)
        throw new IndexOutOfRangeException("DString Get this[" + index + "]");
      else
        return _strBuffer[Length-index-1];
    }
		#endregion
		#region LastIndexOfNotAny  
		/// <summary>
		/// 
		/// </summary>
		/// <param name="control"></param>
		/// <returns></returns>
		public int LastIndexOfNotAny(string control) // basic_string::find_last_not_of
		{
			bool found = false;
			for(int pos = Length-1; pos >= 0; --pos)
			{
				foreach(char c in control)
				{
					if (0 == Value[pos].CompareTo(c)) // It is one of the control characters
					{
						found = true;    // flag it.
					}
				}
				if (found)
					found = false;
				else 
					return pos;
			}	  
			return NOT_FOUND;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="control"></param>
		/// <param name="startPos"></param>
		/// <returns></returns>
		public int LastIndexOfNotAny(string control, int startPos) 
		{
			bool found = false;
			for(int pos = startPos; pos >= 0; --pos)
			{
				foreach(char c in control)
				{
					if (0 == Value[pos].CompareTo(c)) // It is one of the control characters
					{
						found = true;    // flag it.
					}
				}
				if (found)
					found = false;
				else 
					return pos;
			}	  
			return NOT_FOUND;
		}
		#endregion
		#region LastWord 
		/// <summary>
		/// Gets the first word from the string
		/// </summary>
		/// <returns>DString; First word from the string</returns>
		public DString LastWord()
		{
			int pos = -1;
			if ((pos = LastIndexOf(' ')) != NOT_FOUND) 
				return Substring(pos+1);
			return DString._EMPTY;
		}
		/// <summary>
		/// Gets the first word from the string
		/// </summary>
		/// <param name="separ">string; delimitor to split string to words</param>
		/// <returns>DString; First word from the string</returns>
		public DString LastWord( string separ )
		{
			int pos = -1;
			if ((pos = LastIndexOf(separ)) != NOT_FOUND) 
				return Substring(pos + separ.Length);
			return DString._EMPTY;
		}
		#endregion
		#region Left
		/// <summary>
		/// String to the left of the index position
		/// </summary>
		/// <param name="n">int; Index position</param>
		/// <returns>DString; Substring left of that position</returns>
		/// <remarks>Unlike ChopLeft, the original string remains unchanged</remarks>
		public DString Left(int n) 
		{ 
			// Make sure 'n' doesn't go past string's length
			return ( n > Length)? this : Substring(0, n);
		}
		#endregion
		#region MapTo
		/// <summary>
		/// Map to two collections together to create a hashtable
		/// </summary>
		/// <param name="collection">ICollection; Elements to map</param>
		/// <param name="separator">string; Tokenizer to split the DString into elements</param>
		/// <returns>Hashtable; Map of DString to ICollection</returns>
		public Hashtable MapTo(ICollection collection, string separator)
		{
			Hashtable map = new Hashtable();
			DStringCollection words = Split(separator);
			IEnumerator iterOther = collection.GetEnumerator();
			IEnumerator iterSelf = words.GetEnumerator();
			while( iterSelf.MoveNext() && iterOther.MoveNext())
			{
				try
				{
					map.Add(iterSelf.Current.ToString(), iterOther.Current.ToString());
				}
				catch (ArgumentException exception)
				{
					System.Console.WriteLine("{0}",exception.Message);
				}
			}
			return map;
		}
		#endregion
		#region PadDependingDecimal
		/// <summary>
		/// If a decimal point is found, pad right, else pad left to the specified string size.
		/// </summary>
		/// <param name="sz">int; The length of the modified string</param>
		/// <returns>DString; The modified result.</returns>
		public DString PadDependingDecimal(int sz)
		{
			if ( Contains('.') )
				Value = PadRight(sz, '0');
			else 
				Value = PadLeft(sz, '0' );
			return this;
		}
		#endregion
		#region Prepend
		/// <summary>
		/// Insert string at the beginning
		/// </summary>
		/// <param name="val">string; </param>
		/// <returns>DString; Modified string</returns>
		public DString Prepend(string val) 
		{
			Insert(0,val); 
			return this;
		
		}
		#endregion
		#region RemoveNotAll 
		/// <summary>
		/// If find after word starts, delete just right part of word
		/// If find at start, delete word and following space.
		/// Checks if the modified string has only whitespace (i.e. \t,\n,\r and space,
		/// then this method will return -1 and return to the original value.
		/// </summary>
		/// <param name="target">string;</param>
		/// <returns>int; Start position of the removal.</returns>
		public int RemoveNotAll(string target)
		{
			if ( IsEmpty() )
				return NOT_FOUND;
			string hold  = Value;
			int pos = RemoveRightToVarious( target );
			if ( IsBlank() )
			{
				Value = hold;
				return NOT_FOUND;
			}
			else return pos;
		}
		#endregion
		#region RemoveRight 
		/// <summary>
		/// Remove from target in string to next space after target
		/// wherever it may be string will end in space
		/// </summary>
		/// <param name="target"></param>
		/// <returns>Start position of the change.</returns>
		/// <remarks>The C++ version would append a space at the end under all
		/// circumstances, the C# version mirror this aritifact. 
		/// </remarks>
		public int RemoveRight(string target)
		{
			ForceRightSpace();
			int pos = IndexOf(target);
			if ( pos != NOT_FOUND )
			{
				int pos2 = IndexOfAny(" ",pos + target.Length - 1);
				Remove( pos, pos2-pos );
			}
			return pos;
		}
		#endregion
		#region RemoveRightToSpace 
		/// <summary>
		/// Remove string and whatever is to the right of it until space or end of string
		/// make sure string ends in space [separate issue]
		/// </summary>
		/// <param name="target">string; Used to find position to start</param>
		/// <returns>int; Start position of the removal</returns>
		/// <remarks>The C++ version would append a space at the end under all
		/// circumstances, the C# version mirror this aritifact. 
		/// </remarks>
		public int RemoveRightToSpace(string target)
		{
			int pos = RemoveRight(target);
			if ( pos != NOT_FOUND && this[pos] == ' ' )
				Remove(pos, 1);
			ForceRightSpace();
			return pos;
		}
		#endregion
		#region RemoveRightToVarious 
		/// <summary>
		/// if find after word starts, delete just right part of word
		/// if find at start, delete word and following space
		/// </summary>
		/// <param name="target">string; Target</param>
		/// <returns>Start position of the removal</returns>
		/// <remarks>The C++ version would append a space at the end under all
		/// circumstances, the C# version mirror this aritifact. 
		/// </remarks>
		public int RemoveRightToVarious( string target)
		{
			int pos = IndexOf( target );
			if (0 == pos || this[pos-1] ==' ')
				RemoveRightToSpace(target); 
			else 
				RemoveRight(target);
			return pos;
		}
		#endregion
		#region RemoveSimple 
		/// <summary>
		/// Find exact target string, remove it (first one only)
		/// </summary>
		/// <param name="target">string;</param>
		/// <param name="force">bool; Force space at end.</param>
		/// <returns>int; Start position of the removal</returns>
		public int RemoveSimple(string target, bool force)
		{
			// 
			int pos = IndexOf(target);
			if ( pos != NOT_FOUND )
				Remove( pos, target.Length );
			if (force)
				ForceRightSpace();
			return pos;
		}
		#endregion
		#region ReplaceOne 
		/// <summary>
		/// If target in string is followed by space get rid of space
		/// else carefully replace only the target's exact characters
		/// </summary>
		/// <param name="target">String to be matched.</param>
		/// <param name="neww">String to replace the matched string.</param>
		/// <returns>True, change had been made; false, no change had been made.</returns>
		public bool ReplaceOne(string target, string neww)
		{
			int pos = IndexOf(target);
			if ( pos != NOT_FOUND )
			{
				if ( this[pos + target.Length] == ' ' )
					pos = RemoveRightToSpace(target);
				else
					pos = RemoveSimple(target,true);
				if ( pos != NOT_FOUND )
					Insert(pos, neww);
			}
			ForceRightSpace();
			return pos != NOT_FOUND;
		}
		#endregion
		#region ReplaceRestOfWord 
		/// <summary>
		/// Replace string and whatever is to the right of it until space or end of string
		/// and insert new substring. Make sure string ends in space [separate issue]
		/// </summary>
		/// <param name="target">String to match</param>
		/// <param name="neww">String to replace</param>
		/// <returns>True, change had been made; false, no change had been made.</returns>
		public bool ReplaceRestOfWord(string target, string neww)
		{
			int pos = RemoveRightToSpace(target);
			if ( pos != NOT_FOUND )
				Insert(pos, neww);
			ForceRightSpace();
			return pos != NOT_FOUND;
		}
		#endregion
		#region ReplaceSimple 
		/// <summary>
		/// Space means nothing.
		/// Carefully replace only the target's exact characters with neww
		/// Do not force space at end of the string.
		/// </summary>
		/// <param name="target">string</param>
		/// <param name="neww">string</param>
		/// <returns>DString; Modified string.</returns>
		public bool ReplaceSimple(string target, string neww)
		{
			int pos = RemoveSimple(target, false);
			if ( pos != NOT_FOUND )
			{
				Insert(pos, neww);
			}
			return pos != NOT_FOUND;
		}
		#endregion
		#region Reverse
		/// <summary>
		/// Returns a Reverse of the string's contents, non-mutable.
		/// </summary>
		/// <returns>A reversed string</returns>
		public DString Reverse()
		{
			DString d = new DString();
			foreach (Char c in Value)
				d.Insert(0,c);
			return d;
		}
		#endregion Reverse
		#region Right
		/// <summary>
		/// Gets all the characters right of the position passed.
		/// </summary>
		/// <param name="n">int; Index Position in string</param>
		/// <returns>DString; Everything right of n</returns>
		/// <remarks>Replaces right(int)</remarks>
		public DString Right(int n) 
		{
			return n <= Length ? Substring(Length-n) : new DString();
		}
		#endregion
    #region Split
    /// <summary>
    ///  Tokenizes a string into a list of elements using spaces as the deliminators
    /// </summary>
    /// <returns>DStringCollection; A list of words.</returns>
    public DStringCollection Split()
    {
      string temp = Value;
      DStringCollection dc = new DStringCollection();
      dc.AddRange(temp.Split());
      return dc;
    }
    /// <summary>
    /// Tokenizes a string into a list of elements.
    /// </summary>
    /// <param name="separator">string; Where to break the string into elements.</param>
    /// <returns>DStringCollection; Contains a list of elements(words)</returns>
    /// <remarks>
    /// this version processes the separator as a list of separate letters
    /// </remarks>
    public DStringCollection Split(string separator)
    {
      DString temp = this.Clone() as DString;
      if (separator.Contains(" "))
        temp.TrimAll(' ');
      return temp.Split(separator, true);
    }
    /// <summary>
    /// Tokenizes a string into a list of elements.
    /// </summary>
    /// <param name="separator">string; Where to break the string into elements.</param>
    /// <param name="bseparate">bool; Does the string indicate separate characters?</param>
    /// <returns>DStringCollection; Contains a list of elements(words)</returns>
    public DStringCollection Split(string separator, bool bseparate)
    {
      DStringCollection dc = new DStringCollection();
      DString sep = DString.CreateEscapeChars(separator);
      DString temp = this.Clone() as DString;
      if (bseparate)
      {
        if (separator.Contains(" "))
        {
          temp.TrimAll(' ');
        }
        sep.GuaranteeFirstLast('[', ']');
      }
      Regex rx = new Regex(sep);
      dc.AddRange(rx.Split(temp.Value));
      return dc;
    }

    #endregion Split
		#region SplitForced
		/// <summary>
		///  Tokenizes a string into a list of elements using spaces as the deliminators.
		///  Guarantees that no element is empty.
		/// </summary>
		/// <returns>DStringCollection; A list of words.</returns>
		public DStringCollection SplitForced()
		{
			DStringCollection dc = Split();
			for(int i = 0; i < dc.Count; ++i)
				if (dc[i].IsEmpty())
					dc.RemoveAt(i);
			return dc;
		}
		/// <summary>
		/// Tokenizes a string into a list of elements.
		/// Guarantees that no element is empty.
		/// </summary>
		/// <param name="separator">string; Where to break the string into elements.</param>
		/// <returns>DStringCollection; Contains a list of elements(words)</returns>
		/// <remarks>
		/// </remarks>
		public DStringCollection SplitForced(string separator)
		{
			DStringCollection dc = Split(separator);
			for(int i = 0; i < dc.Count; ++i)
				if (dc[i].IsEmpty())
					dc.RemoveAt(i);
			return dc;
		}
		#endregion SplitForced
		#region TrimAll
		/// <summary>
		/// Trims start and end of string completely
		/// Trims extra contiguous characters to one char between start and end of string. 
		/// </summary>
		/// <param name="c">The character to be trimmed</param>
		/// <returns>Modified string.</returns>
		public DString TrimAll(char c)
		{
      TrimMiddle(c);
      Trim(c);
			return this;
		}
		#endregion
		#region TrimStartToMiddle
    /// <summary>
    /// Trims extra contiguous characters to one char between start and end of string. 
    /// </summary>
    /// <param name="c">The character to be trimmed</param>
    /// <returns>Modified string.</returns>
    public DString TrimStartToMiddle(char c)
    {
      TrimStart(c);
      TrimMiddle(c);
      return this;
    }
    #endregion
		#endregion DString Methods
		#region Static RegEx Implementations
		#region static DString CreateEscapeChars(DString target)
		private static DString CreateEscapeChars(DString target)
		{
			if(target.Length > 0)
			{
				string original = target;
				target = Regex.Escape(original);
			}
			return target;
		}
		#endregion
		#region static DString Cutout(ref string target, string left, string right, int excl)
		/// <summary>
		/// Remove all characters between the beginning of left position and right position
		/// depending on the exclude value and returns the result.
		/// </summary>
		/// <param name="target">String to be modified</param>
		/// <param name="left">Left boundaryt</param>
		/// <param name="right">Right boundary</param>
		/// <param name="excl"> 
		///  case 1: Do not cut out left
		///  case 2: Do not cut out right
		///  case 3: Do not cut out left or right
		///  </param>
		/// <returns>The substring cutout from the original string</returns>
		public static DString Cutout(ref string target, string left, string right, int excl)
		{
			string pattern = left + ".*" + right;
			Regex r = new Regex(pattern);
			Match m = r.Match(target);
			
			if (m.Length == 0)
				return new DString();
			int n = 0, pos = 0;
			pos = m.Index;
			n   = m.Length;
			switch ( excl )
			{
				case 1:  // exclude left from cutout
					pos += left.Length;
					n   -= left.Length;
					break;
				case 2:  // exclude right from cutout
					n   -= right.Length;
					break;
				case 3:  // exclude both from cutout
					pos += left.Length;
					n   -= (left.Length + right.Length);
					break;
				default: // cut everything
					break;
			}
			DString result = target.Substring(pos, n);
			target.Remove( pos, n );
			return result;
		}
		#endregion public static DString Cutout(ref string target, string left, string right, int excl)
		#region static bool ElimAll(ref string target, string control)
		/// <summary>
		/// Eliminate all the characters in the control string found in the target string.
		/// </summary>
		/// <param name="target">String to be modified</param>
		/// <param name="control">String containing characters to be removed</param>
		/// <returns>True, string modified; false, string not modified.</returns>
		public static bool ElimAll(ref DString target, string control)
		{
			if (control.Length == 0)
				return target.Clear();
			DString expr = new DString("[");
			foreach(char c in control)
				expr += CreateEscapeChars(c.ToString()) + "|";
			expr += "]";
			Regex rx = new Regex(expr);
			// Save original string
			string result = target;
			target = rx.Replace(target,"");
			// If different, changes have been made; 
			//return true; else return false.
			return (target != result);
		}
		#endregion public static bool ElimAll(ref string target, string control)
		#region static DString GetInitials(DString target,INIT_TYPE t)
		/// <summary>
		/// Returns a specialized string formating of either initials 
		/// or a concatenation of initials and words.
		/// </summary>
		/// <param name="target">String target.</param>
		/// <param name="t">INIT_TYPE (see below)</param>
		/// <returns>a new string composed of each word's initial letters</returns>
		/// <remarks>
		/// INITIALS: new string composed of each word's initial letters
		/// --The Early Bird Catches Worms --> TEBCW
		/// CONCAT1: 1st word, then initials:
		/// --The Early Bird Catches Worms --> The_EBCW
		/// CONCAT2: 1st 2 wds, then initials:
		///   --The Early Bird Catches Worms --> The_EarlyBCW
		/// </remarks>
		public static DString GetInitials(DString target,INIT_TYPE t)
		{
			__COUNTER__   = 0;
			__INIT_TYPE__ = t;
			string expr = "\\w+";
			Regex rx = new Regex(expr);
			MatchEvaluator me = new MatchEvaluator(DString.GetInitialsEvaluator);
			target.ToLower();
			DString result = rx.Replace(target,me);
			result.ElimAll(" ");
			__COUNTER__   = 0;
			__INIT_TYPE__ = INIT_TYPE.INITIALS;
			return result;
		}
		private static string GetInitialsEvaluator(Match m)
		{
			DString result = new DString(m.Value);
			if ( __INIT_TYPE__ == INIT_TYPE.INITIALS )
				result = char.ToUpper(m.Value[0]).ToString();
			else
			{
				switch (__COUNTER__ )
				{
					case 0:
						result[0] = char.ToUpper(m.Value[0]);
						result +=  "_";
						break;
					case 1:
						if (__INIT_TYPE__ == INIT_TYPE.CONCAT1)
							result = char.ToUpper(m.Value[0]).ToString();
						else
							result[0] = char.ToUpper(m.Value[0]);
						break;
					default:
						result = char.ToUpper(m.Value[0]).ToString();
						break;
				}
			}
			__COUNTER__++;
			return result;
		}
		private static int __COUNTER__   = 0;
		private static INIT_TYPE __INIT_TYPE__ = 0;
		#endregion
		#region static DString GetStylizedName(DString target)
		/// <summary>
		/// This will accept the name BUSH GEORGE W. and return Bush, G.W.
		/// It should return McBush, G.W. from MCBUSH George W.
		/// </summary>
		/// <returns>DString; Stylized Name</returns>
		public static DString GetStylizedName(DString target) 
		{
			string expr = @"(ibn |ben |della |v[a|o]n |m[a]?c[k]?)?[a-z]+[ .]?";
			if (target.IsEmpty())
				return target;
			Regex rx = new Regex(expr);
			MatchEvaluator me = new MatchEvaluator(DString.GetStylizedNameEvaluator);
			target.ToLower();
			__COUNTER__ = 0;
			DString result = rx.Replace(target,me);
			__COUNTER__ = 0;
			return result;
		}
		private static string GetStylizedNameEvaluator(Match m)
		{
			DString result = new DString();
			//Console.WriteLine(m.Value);
			if(__COUNTER__ == 0)
			{
				result = m.Value;
				// ibn |ben |della |v[a|o]n |m[a]?c
				if ( m.Groups[1].Captures.Count > 0)
				{ 
					string temp = m.Groups[1].Captures[0].Value;
					string c    = temp[temp.Length-1].ToString();
					if (c.CompareTo(" ")== 0 || c.CompareTo("c") == 0)
					{
						result[0] = char.ToUpper(result[0]);
						result[temp.Length] = char.ToUpper(result[temp.Length]);
					}
					else // mack
					{
						result[0] = char.ToUpper(result[0]);
					}
					//Console.WriteLine("--" + c.Value);
				}
				else
				{
					result[0] = char.ToUpper(result[0]);
				}
				result.TrimEnd(' ');
				result += ", ";
			}
			else if (m.Value.Length > 0)
			{
				if (string.Compare(m.Value,0,"jr",0,2) == 0 && m.Value.Length < 4)
					result = "";
				else
				{
					result = m.Value[0].ToString() + ".";
					result.ToUpper();
				}
			}
			__COUNTER__++;
			return result;
		}
		#endregion
		#region static DString InitCase(DString target)
		/// <summary>
		/// Make the first character of every word upper case.
		/// </summary>
		/// <param name="target">String to be converted</param>
		/// <returns>Modified string</returns>
		public static DString InitCase(DString target)
		{
			target.ToLower();
			string expr = "[^\\s\\p{P}]+";
			Regex rx = new Regex(expr);
			MatchEvaluator me = new MatchEvaluator(DString.InitCaseEvaluator);
			return rx.Replace(target,me);
		}
		/// <summary>
		/// MatchEvaluator Delegate to make initial character uppercase.
		/// </summary>
		/// <param name="m">Modified string</param>
		/// <returns></returns>
		private static string InitCaseEvaluator(Match m)
		{
			DString result = m.Value;
			result[0] = char.ToUpper(result[0]);
			return result;
		}
		#endregion
		#region static bool KeepAll(ref DString target, string control)
		/// <summary>
		/// Keeps all the characters listed in the control string.
		/// </summary>
		/// <param name="target">String to be modified.</param>
		/// <param name="control">Characters to be kept.</param>
		/// <returns>
    /// True,string had been modified; false, string had not beeen modified.
    /// </returns>
		public static bool KeepAll(ref DString target, string control)
		{
			if (1 > control.Length )
				return target.Clear();
			DString expr = new DString("[^");
			foreach(char c in control)
				expr += CreateEscapeChars(c.ToString()) + "|";
			expr += "]";
			Regex rx = new Regex(expr);
			// Save original string
			string result = target;
			target = rx.Replace(target,"");
			// If different, changes have been made; 
			//return true; else return false.
			return ( target != result);
		}
		#endregion static bool KeepAll(ref string target, string control)
		#region static DString NumberToText(ulong target)
		/// <summary>
		/// Translates a number to its english equivalent.
		/// </summary>
		/// <param name="target">A number to be translated to english.</param>
		/// <returns>The english equivalent of that number.</returns>
		public static DString NumberToText(ulong target)
		{
			// Capture digits by 3’s from right to left.
			string expr = "(\\d{0,3})+";
			Regex rx = new Regex(expr,RegexOptions.RightToLeft);
			MatchEvaluator me = new MatchEvaluator(DString.NumberToTextEvaluator);
			DString result = rx.Replace(Convert.ToString(target),me);
			return result;
		}
		private static string NumberToTextEvaluator(Match m)
		{
			// resulting conversion string
			DString result = new DString();
			// Capture index
			int index = 0;

			DStringCollection Units = DString.EnglishTextByHundreds();
			Hashtable map = DString.MapNumbersToEnglishText();
			
			//Iterate through the captured strings
			foreach(Capture c in m.Groups[1].Captures)
			{
				// A temporary cache for conversion string
				DString d = new DString();
				// integer value for capture string
				int value = 0;
				// Nothing capture, return empty result
				// Else convert capture string to int
				if (c.Length == 0)
					return result;
				else
					value = Convert.ToInt32(c.Value);
				// Check if capture string is 100 or greater
				if ( value > 99)
				{
					d = map[Convert.ToString(value/100)] + " hundred ";
					value %= 100;
					if (value > 0)
						d += "and ";
				}
				// Check if remaining value is 1 or greater
				if ( value > 0)
				{
					//When no hundreds value exists, but higher order values do, prepend "and"
					if(d.Length == 0 && m.Groups[1].Captures.Count - 1 > 1 && index == 0)
						d+= "and ";

					// then, is 20 or greater
					if (value > 19)
					{
						// Translate the tens digit into text (ex. 25 to twenty)
						// and save the remainer
						d += map[Convert.ToString((value/10)*10)];
						value %= 10;
						if (value > 0)
							d += "-";
					}
					// if remainder is 1 or greater, translate ones value to text
					if (value > 0)
					{
						d += map[Convert.ToString(value)];
					}
				}
					// value is equal to zero
				else 
				{
					// if only one capture group, and result and temp strings are zero
					// then you can safely conclude that this is zero
					if (m.Groups[1].Captures.Count == 2  && result.Length == 0 && d.Length == 0)
					{
						d = (string)map["0"];
					}
				}
				// Determine if a higher order denomination needs to be added
				// (ex. thousand, million, billion etc.
				if (index > 0)
				{
					//Only add order unit label if non-zero value
					if(Convert.ToInt32(c.Value) > 0)
					{
						//Prepend space to higher order unit only if it does not begin with "hundred "
						//This prevents an extra space from being displayed for numbers such as 100036
						if(d.EndsWith("hundred ") == false)
							d += " ";
						d+= Units[index];
						if (result.Length > 0)
							d += " ";
					}
				}
				// Insert capture string to beginning of previous result.
				result.Insert(0,d);
				index++;
			}
			return result;
		}

		#endregion public static NumberToText(ulong target)
		#region static bool ReplaceAll(ref string target, string pattern, string newString)
		/// <summary>
		/// Replace all the matching string patterns with a new string. 
		/// </summary>
		/// <param name="target">String to be modified.</param>
		/// <param name="pattern">String pattern to be matched</param>
		/// <param name="newString">String to replace the pattern</param>
		/// <returns>
    /// True, string pattern found and replaced; false, string pattern not found.
    /// </returns>
		public static bool ReplaceAll(ref DString target, string pattern, string newString)
		{
			DString patternEsc = CreateEscapeChars(pattern);
			Regex rx = new Regex(patternEsc);
			DString temp = target;
			target = rx.Replace(target,newString);
			return ((bool)(target.CompareTo(temp) != 0));
		}
		#endregion
		#region static DString SaveAll( DString target, string ctl )
		/// <summary>
		/// Relay to KeepAll, which returns a DString instead of boolean
		/// </summary>
		/// <param name="target">string; Characters to keep.</param>
		/// <param name="ctl">string; Characters to keep.</param>
		/// <returns>DString; Modified string</returns>
		public static DString SaveAll( DString target, string ctl )
		{ 
			DString.KeepAll(ref target,ctl);
			return target; 
		} 
		#endregion static DString SaveAll( DString target, string ctl )
		#region static DString TrimMiddle( ref DString target, char c )
		/// <summary>
		///  Trim extra internal doubles 
		/// </summary>
		/// <param name="target">Reference String to be modified</param>
		/// <param name="c">Extra character(s) to be removed from middle</param>
		/// <returns>Modified string</returns>
		public static DString TrimMiddle( ref DString target, char c )
		{
			// c+ *** Always one and find one or more adjacent characters
			string expr = c.ToString() + "+";
			Regex rx = new Regex(expr);
			// Don't trim start and end.
			DString temp = target.Substring(1,target.Length-2);
			target = target[0].ToString() + rx.Replace(temp,c.ToString()) 
        + target[target.Length-1].ToString();	
			return target;
		}
		#endregion
		#region static ulong TextToNumber(DString word)
		/// <summary>
		/// Converts Numerical description to it's numerical value.
		/// </summary>
		/// <param name="word">
    /// DString, numerical statement from zero to eighteen quintrillion
    /// </param>
		/// <returns>ulong; Numerical translation to an Unsigned Long</returns>
		public static ulong TextToNumber(DString word)
		{
			ulong result = 0;
			ulong finalResult = 0;
			Regex rx = new Regex(@"[ -]");
			string[] list = rx.Split(word);
			Hashtable m_WordList = DString.MapEnglishTextToNumbers();
			foreach(string s in list)
			{
				DString dVal = new DString(m_WordList[s]);
				ulong val = 0;
				if (!dVal.IsEmpty())
					val = (ulong)dVal;
				if (  val < 1000)
				{
					if (val < 100)
						result += val;
					else
						result *= val;
				}
				else 
				{
					result *= val;
					finalResult +=result;
					result = 0;
				}
			}
			finalResult +=result;
			return finalResult;
		}
		#endregion 
		#endregion RegEx Implementations
		#region Non-Static RegEx Implementations
		#region Count
		/// <summary>
		/// The number of times the target occurs in the string
		/// </summary>
		/// <param name="target">string; The target of the count</param>
		/// <returns>int; The number of substrings found</returns>
		public int Count(string target)
		{
			Regex r = new Regex(target);
			MatchCollection mc = r.Matches(Value);
			return mc.Count;;
		}
		#endregion
		#region DString Cutout(string left, string right, int excl)
		/// <summary>
		/// Remove all characters between the beginning of left position and right position
		/// depending on the exclude value and returns the result.
		/// </summary>
		/// <param name="left">Left boundaryt</param>
		/// <param name="right">Right boundary</param>
		/// <param name="excl"> 
		///  case 1: Do not cut out left
		///  case 2: Do not cut out right
		///  case 3: Do not cut out left or right
		///  </param>
		/// <returns>The substring cutout from the original string</returns>
		public DString Cutout(string left, string right, int excl)
		{
			string pattern = left + ".*" + right;
			Regex r = new Regex(pattern,RegexOptions.Compiled);
			Match m = r.Match(Value);
			
			if (m.Length == 0)
				return new DString();
			int n = 0, pos = 0;
			pos = m.Index;
			n   = m.Length;
			switch ( excl )
			{
				case 1:  // exclude left from cutout
					pos += left.Length;
					n   -= left.Length;
					break;
				case 2:  // exclude right from cutout
					n   -= right.Length;
					break;
				case 3:  // exclude both from cutout
					pos += left.Length;
					n   -= (left.Length + right.Length);
					break;
				default: // cut everything
					break;
			}
			DString result = Substring(pos, n);
			Remove( pos, n );
			return result;
		}
		#endregion public DString Cutout(string left, string right, int excl)
		#region bool ElimAll(string control)
		/// <summary>
		/// Eliminate all the characters in the control string found in the DString.
		/// </summary>
		/// <param name="control">String containing characters to be removed</param>
		/// <returns>True, string modified; false, string not modified.</returns>
		public bool ElimAll(string control)
		{
			if (control.Length == 0)
				return false;
			DString expr = new DString("[");
			foreach(char c in control)
				expr += CreateEscapeChars(c.ToString()) + "|";
			expr += "]";
			Regex rx = new Regex(expr, RegexOptions.Compiled);
			// Save original string
			string result = Value;
			Value = rx.Replace(Value,"");
			// If different, changes have been made; 
			//return true; else return false.
			return (Value != result);
		}
		#endregion
		#region DString GetInitials(INIT_TYPE t)
		/// <summary>
		/// Returns a specialized string formating of either initials 
		/// or a concatenation of initials and words.
		/// </summary>
		/// <param name="t">INIT_TYPE (see below)</param>
		/// <returns>a new string composed of each word's initial letters</returns>
		/// <remarks>
		/// INITIALS: new string composed of each word's initial letters
		/// --The Early Bird Catches Worms --> TEBCW
		/// CONCAT1: 1st word, then initials:
		/// --The Early Bird Catches Worms --> The_EBCW
		/// CONCAT2: 1st 2 wds, then initials:
		///   --The Early Bird Catches Worms --> The_EarlyBCW
		/// </remarks>
		public DString GetInitials(INIT_TYPE t)
		{
			__COUNTER__   = 0;
			__INIT_TYPE__ = t;
			string expr = "\\w+";
			Regex rx = new Regex(expr,RegexOptions.Compiled);
			MatchEvaluator me = new MatchEvaluator(DString.GetInitialsEvaluator);
			string target = Value;
			DString result = rx.Replace(target.ToLower(),me);
			result.ElimAll(" ");
			__COUNTER__   = 0;
			__INIT_TYPE__ = INIT_TYPE.INITIALS;
			return result;
		}
		#endregion
		#region DString GetStylizedName()
		/// <summary>
		/// This will accept the name BUSH GEORGE W. and return Bush, G.W.
		/// It should return McBush, G.W. from MCBUSH George W.
		/// </summary>
		/// <returns>DString; Stylized Name</returns>
		public DString GetStylizedName() 
		{
			string expr = @"(ibn |ben |della |v[a|o]n |m[a]?c[k]?)?[a-z]+[ .]?";
			if (IsEmpty())
				return this;
			Regex rx = new Regex(expr,RegexOptions.Compiled);
			MatchEvaluator me = new MatchEvaluator(DString.GetStylizedNameEvaluator);
			__COUNTER__ = 0;
			string target = Value;
			DString result = rx.Replace(target.ToLower(),me);
			__COUNTER__ = 0;
			return result;
		}
		#endregion
		#region DString InitCase()
		/// <summary>
		/// Make the first character of every word upper case.
		/// </summary>
		/// <returns>Modified string</returns>
		public DString InitCase()
		{
			string target = Value;
			string expr = "[^\\s\\p{P}]+";
			Regex rx = new Regex(expr,RegexOptions.Compiled);
			MatchEvaluator me = new MatchEvaluator(DString.InitCaseEvaluator);
			return rx.Replace(target.ToLower(),me);
		}
		#endregion
		#region bool KeepAll(string control)
		/// <summary>
		/// Keep all the characters in the control string.
		/// </summary>
		/// <param name="control">Characters to be kept.</param>
		/// <returns>True, string modified; false, string not modified.</returns>
		public bool KeepAll(string control)
		{
			if (1 > control.Length )
				return Clear();

			DString expr = new DString("[^");
			foreach(char c in control)
				expr += c.ToString() + "|";
            expr += "]";
			Regex rx = new Regex(expr,RegexOptions.Compiled);
			// Save original string
			string result = Value;
			Value = rx.Replace(Value,"");
			// If different, changes have been made; 
			//return true; else return false.
			
			return ( Value != result);
		}
		#endregion bool KeepAll(string control)
		#region DString NumberToText()
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DString NumberToText()
		{
			if (this.IsNumber())
			{
				// Capture digits by 3’s from right to left.
				string expr = "(\\d{0,3})+";
				Regex rx = new Regex(expr,RegexOptions.RightToLeft | RegexOptions.Compiled);
				MatchEvaluator me = new MatchEvaluator(DString.NumberToTextEvaluator);
				DString result = rx.Replace(Value,me);
				return result;
			}
			else return DString._EMPTY;
		}
		#endregion DString NumberToText()
		#region bool ReplaceAll(string pattern, string newString)
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="newString"></param>
		/// <returns></returns>
		public bool ReplaceAll(string pattern, string newString)
		{
			DString patternEsc = CreateEscapeChars(pattern);
			Regex rx = new Regex(patternEsc, RegexOptions.Compiled);
			DString temp = Value;
			Value = rx.Replace(Value,newString);
			return ((bool)(CompareTo(temp) != 0));
		}
		#endregion
		#region DString SaveAll( string ctl )
		/// <summary>
		/// Relay to KeepAll, which returns a DString instead of boolean
		/// </summary>
		/// <param name="ctl">string; Characters to keep.</param>
		/// <returns>DString; Modified string</returns>
		public DString SaveAll( string ctl )
		{ 
			if (KeepAll(ctl))
				return this;
			else
				return DString._EMPTY;
		} 
		#endregion DString SaveAll( DString target, string ctl )
		#region DString TrimMiddle( char c )
		/// <summary>
		///  Trim extra internal doubles 
		/// </summary>
		/// <param name="c">Extra character(s) to be removed from middle</param>
		/// <returns>Modified string</returns>
		public DString TrimMiddle( char c )
		{
			// c++ *** Always one and find one or more adjacent characters
      if (this.Length <= 2)
        return this;
			string expr = c.ToString() + "+";
      char fc = this.FirstChar();
      char lc = this.LastChar();
      this.ChopLeft();
      this.ChopRight();
      Value = fc.ToString()
        + Regex.Replace(Value, expr, c.ToString())
        + lc.ToString();
			return Value;
		}
		#endregion
		#region ulong TextToNumber()
		/// <summary>
		/// Converts Numerical description to it's numerical value.
		/// </summary>
		/// <returns>ulong; Numerical translation to an Unsigned Long</returns>
		public ulong TextToNumber()
		{
			ulong result = 0;
			ulong finalResult = 0;
			Regex rx = new Regex(@"[ -]",RegexOptions.Compiled);
			string[] list = rx.Split(Value);
			Hashtable m_WordList = DString.MapEnglishTextToNumbers();
			foreach(string s in list)
			{
				DString dVal = new DString(m_WordList[s]);
				ulong val = 0;
				if (!dVal.IsEmpty())
					val = (ulong)dVal;
				if (  val < 1000)
				{
					if (val < 100)
						result += val;
					else
						result *= val;
				}
				else 
				{
					result *= val;
					finalResult +=result;
					result = 0;
				}
			}
			finalResult +=result;
			return finalResult;
		}
		#endregion 
		#endregion Non-Static RegEx Implementations
		#region Property
		/// <summary>
		/// DString's internal string value.
		/// </summary>
		public string Value
		{
			get
			{
				return _strBuffer.ToString();
			}
			set
			{
				_strBuffer.Length = 0;
        _strBuffer.Append(value);
			}
		} 
		/// <summary>
		/// DString's zero based index, to an individual character in the string.
		/// </summary>
		public char this[int index]
		{
			get
			{
				if (index < 0 || index > Length -1 || 0 == Length)
				{
					throw new IndexOutOfRangeException("DString Get this[" + index + "]");
				}
				return _strBuffer[index];
			}
			set
			{
				if (index < 0  || index > Length -1 || 0 == Length) //Out of bounds error, do nothing
				{
					throw new IndexOutOfRangeException("DString Set this[" + index.ToString() + "]");
				}
				_strBuffer[index] = value;

			}
		}
		/// <summary>
		/// The number of characters in the string.
		/// </summary>
		public int Length
		{
			get
			{
				return _strBuffer.Length;
			}
			set
			{
				_strBuffer.Length = value;
			}
		}
		/// <summary>
		/// The internal memory usualized by DString.
		/// </summary>
		public int Capacity
		{
			get
			{
				return _strBuffer.Capacity;
			}
			set
			{
				_strBuffer.Capacity = value;
			}
		}
		/// <summary>
		/// The maximum memory available.
		/// </summary>
		public int MaxCapacity
		{
			get
			{
				return _strBuffer.MaxCapacity;
			}
		}
		#endregion
		#region DATA
		/// <summary>
		/// Internal variable for string allocation.
		/// </summary>
		private StringBuilder _strBuffer = null;
		/// <summary>
		/// Empty string
		/// </summary>
		private static readonly string _EMPTY = "";
		/// <summary>
		/// Not found flag
		/// </summary>
		public static int NOT_FOUND = -1;		
		/// <summary>
		/// Initial types 
		/// </summary>
		public enum INIT_TYPE
		{
			/// <summary>
			/// Only initials
			/// </summary>
			INITIALS=0,
			/// <summary>
			/// A word, then initials.
			/// </summary>
			CONCAT1=1, 
			/// <summary>
			/// Two words, then initials.
			/// </summary>
			CONCAT2=2
		}

		#endregion DATA
		#endregion DString Members
	}
	#endregion class DString
}
