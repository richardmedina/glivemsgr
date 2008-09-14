// ObservedCollection.cs
//
// Copyright (c) 2008 Ricardo Medina <ricki@dana-ide.org>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//

using System;
using System.Collections.Generic;
using NonGenEnum = System.Collections.IEnumerable;

namespace System.Net.Protocols.Msnp
{
	
	public class ObservedCollection <T> : ICollection <T>
	{
		private List<T> _list;
		
		private event ObservedCollectionAction<T> _added;
		private event ObservedCollectionAction<T> _removed;
		
		public ObservedCollection ()
		{
			_list = new List<T> ();
			_added = onAdded;
			_removed = onRemoved;
		}
		
		public virtual void Add (T instance)
		{
			_list.Add (instance);
			OnAdded (instance);
		}
		
		public virtual bool Remove (T instance)
		{
			bool ret = _list.Remove (instance);
			OnRemoved (instance);
			return ret;
		}
		
		public virtual void Clear ()
		{
			_list.Clear ();
		}
		
		public virtual bool Contains (T instance)
		{
			return _list.Contains (instance);
		}
		
		public virtual void CopyTo (T [] array, int array_index)
		{
			_list.CopyTo (array, array_index);
		}
		
		protected virtual void OnAdded (T instance)
		{
			_added (this,
				new ObservedCollectionActionArgs<T> (instance));
		}
		
		protected virtual void OnRemoved (T instance)
		{
			_removed (this,
				new ObservedCollectionActionArgs<T> (instance));
		}
		
		private void onAdded (object sender, 
			ObservedCollectionActionArgs<T> args)
		{
		}
		
		private void onRemoved (object sender,
			ObservedCollectionActionArgs<T> args)
		{
		}
		
		
		public IEnumerator<T> GetEnumerator ()
		{
			return (IEnumerator<T>) _list.GetEnumerator ();
			//foreach (T t in _list)
			//	yield return t;
		}
		
		System.Collections.IEnumerator NonGenEnum.GetEnumerator ()
		{
			return ((NonGenEnum)_list).GetEnumerator ();
		}
		
		public int Count {
			get { return _list.Count; }
		}
		
		public bool IsReadOnly {
			get { return false; } 
		}
		
		public event ObservedCollectionAction<T> Added {
			add { _added += value; }
			remove { _added -= value; }
		}
		
		public event ObservedCollectionAction<T> Removed {
			add { _removed += value; }
			remove { _removed -= value; } 
		}
	}
}
