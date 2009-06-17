// CategoryChanged.cs
// 
// Copyright (C) 2009 Ricardo Medina López
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;

namespace GLiveMsgr.Gui
{
	public delegate void PageActionHandler (object sender,
		PageActionArgs args);
	
	public class PageActionArgs : System.EventArgs
	{
		private IConfigPage _page;
		
		public PageActionArgs (IConfigPage page)
		{
			_page = page;
		}
		
		public IConfigPage Page {
			get { return _page; }
		}
	}
}
