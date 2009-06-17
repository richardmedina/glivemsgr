
using System;
using Gtk;
using Gdk;
using System.Net.Protocols;
using System.Net.Protocols.Msnp;


using RickiLib.Types;

namespace GLiveMsgr.Gui
{
	
	
	public class ContactList : Gtk.TreeView
	{
		private TreeStore store;

		private Gdk.Pixbuf [] pixbufState;
		
		private Gtk.CellRenderer cell1;
		private Gtk.CellRenderer cell2;
		
		private Gtk.TreeViewColumn col1;
		private Gtk.TreeViewColumn col2;
		
		private MsnpAccount account;
		
		private TreeIterCollection groupIters;
		private TreeIterCollection contactIters;
		
		private ContactListMenu contactListMenu;
		
		private static readonly string group_nogroup_name = "No Group";
		private static readonly string group_empty_message = 
			"Drag and drop your contacts here";
		
		private ContactCardPopup cardPopup;
		
		public ContactList (MsnpAccount account)
		{
			groupIters = new TreeIterCollection ();
			contactIters = new TreeIterCollection ();
			
			contactListMenu = new ContactListMenu ();
			contactListMenu.ItemOpenConv.Activated += contactListMenu_OpenConv_activated;
			
			this.account = account;
			this.account.Buddies.Added += account_Buddies_Added;
			this.account.Groups.Added += account_Groups_Added;
			this.account.Terminated += account_Terminated;
			
			store = new TreeStore (
				typeof (Gdk.Pixbuf), // pixbuf
				typeof (string), // text
				typeof (ContactListItem)); // element
			base.Model = store;
			HeadersVisible = false;
			
			store.DefaultSortFunc = OnTreeIterCompare;
			store.SetSortFunc (2, OnTreeIterCompare);
			store.SetSortColumnId (2, SortType.Ascending);
			
			cell1 = new CellRendererContactIcon ();
			cell2 = new CellRendererContact ();
			
			col1 = base.AppendColumn ("Icon",
				cell1,
				"pixbuf",
				0);
			
			col2 = base.AppendColumn ("Contacts", 
				cell2,
				"text",
				1);
			
			col1.SetCellDataFunc (cell1, 
				(TreeCellDataFunc) col1_cellDataFunc);
			col2.SetCellDataFunc (cell2, 
				(TreeCellDataFunc) col2_cellDataFunc);
			
			pixbufState = new Gdk.Pixbuf [] {
				Gdk.Pixbuf.LoadFromResource ("online.png"), // Online
				Gdk.Pixbuf.LoadFromResource ("brb.png"), // Busy
				Gdk.Pixbuf.LoadFromResource ("brb.png"), // Brb
				Gdk.Pixbuf.LoadFromResource ("brb.png"), // Away
				Gdk.Pixbuf.LoadFromResource ("brb.png"), // Phone
				Gdk.Pixbuf.LoadFromResource ("brb.png"), // Lunch
				Gdk.Pixbuf.LoadFromResource ("offline.png") // Offline
			};
			
		//	PopulateList ();
						
			/*			
			for (int i =0; i < 3; i ++) {
				MsnpGroup group = new MsnpGroup ("Group " + (i+1), i);
				GroupAdd (group);
				MsnpContact contact = new MsnpContact ("ricki@dana-ide.org",
				":) Ricki :p (h) dsadsa :@ que paso? :)");
				contact.State = MsnpContactState.Offline;
				contact.Groups.Add (group);
				ContactAdd (contact);
			}
			
			MsnpGroup empty_group = new MsnpGroup ("Empty Group", 3);
			GroupAdd (empty_group);
			*/
			// White
			
			cardPopup = new ContactCardPopup ();
			Theme.Modified += delegate { theme_refresh (); };
			theme_refresh ();
		}
		
		public ContactListItem GetItem (Gtk.TreeIter iter)
		{
			return (ContactListItem) store.GetValue (iter, 2);
		}
		
		public bool GetSelectedItem (out ContactListItem item)
		{
			item = null;
			
			Gtk.TreeIter iter;
			
			if (Selection.GetSelected (out iter)) {
				item = GetItem (iter);
				return true;
			}
			
			return false;
		}
		
		public bool GetOverItem (out ContactListItem item)
		{
			int x, y;
			
			item = null;
			
			GetPointer (out x, out y);
			
			Gtk.TreePath path;
			
			if (base.GetPathAtPos (x, y, out path)) {
				Gtk.TreeIter iter;
				if (store.GetIterFromString (out iter, path.ToString ())) {
					item = GetItem (iter);
					return true;
				}
			}
			
			return false;
		}
				
		public void PopulateList ()
		{
			store.Clear ();
			
			foreach (MsnpGroup group in this.account.Groups) {
				Gtk.TreeIter iter = store.AppendValues (
					null, 
					group.Name, 
					new ContactListItem (group));
				
				int i = 0;
				
					
				foreach (MsnpContact c in this.account.Buddies) {
					//if (c.State == MsnpContactState.Offline)
					//	continue;
					
					if (contactIsInGroup (c, group)) {
						store.AppendValues (
							iter, 
							pixbufState [(int) c.State],
							c.Alias,
							new ContactListItem (c));
						Debug.WriteLine ("\tContact {0} ({1})", 
							c.Alias,
							Utils.ContactStateToString (c.State));
							i ++;
					}
				}
				if (i == 0) {
					store.AppendValues (iter, null, 
						group_empty_message,
						new ContactListItem (group_empty_message));
				}
			}
		}


		public void ContactAdd (MsnpContact contact)
		{
			contact.StateChanged += contact_StateChanged;
			contact.AliasChanged += contact_AliasChanged;
						
			bool added = false;
			
			foreach (MsnpGroup group in contact.Groups) {
				Gtk.TreeIter iter;
				if (GetIterFromGroup (group, out iter)) {
					if (store.IterNChildren (iter) == 1) {
						Gtk.TreeIter iter_child;
						
						if (store.IterChildren (out iter_child, iter)) {
							ContactListItem item = 
								(ContactListItem) store.GetValue (iter_child, 2);
							
							if (item.Type == ContactListItemType.Message)
								store.Remove (ref iter_child);
						}
					}
					
					iter = store.AppendValues (iter, 
						pixbufState [(int) contact.State], 
						contact.Alias,
						new ContactListItem (contact));
					
					contactIters.Add (iter);
					added = true;
				}
			}
		}
		
		public bool GetIterFromGroup (MsnpGroup group, out Gtk.TreeIter iter)
		{
			iter = TreeIter.Zero;
			
			foreach (Gtk.TreeIter i in groupIters) {
				ContactListItem item = 
					(ContactListItem) store.GetValue (i, 2);
				
				if (item.Obj.Equals (group)) {
					iter = i;
					return true;
				}
			}
			
			return false;
		}
		
		public TreeIterCollection GetItersFromContact (MsnpContact contact)
		{
			TreeIterCollection iters = new TreeIterCollection ();
			
			foreach (Gtk.TreeIter iter in contactIters) {
				ContactListItem item =
					(ContactListItem) store.GetValue (iter, 2);
				
				if (item.Obj.Equals (contact)) {
					iters.Add (iter);
				}
			}
			
			return iters;
		}
		
		public bool GroupExists (MsnpGroup group)
		{
			Gtk.TreeIter iter;
			return GetIterFromGroup (group, out iter);
		}
		
		public void GroupAdd (MsnpGroup group)
		{
			if (!GroupExists (group)) {
				Gtk.TreeIter iter = store.AppendValues ( 
					null, 
					group.Name, 
					new ContactListItem (group)); 
				store.AppendValues (iter, 
					null,
					group_empty_message,
					new ContactListItem (group_empty_message));
			
				groupIters.Add (iter);
			}
	
		}
		
		protected override bool OnLeaveNotifyEvent (EventCrossing evnt)
		{
			cardPopup.Hide ();
			return base.OnLeaveNotifyEvent (evnt);
		}

		
		protected override bool OnMotionNotifyEvent (EventMotion args)
		{
			Gtk.TreePath path;
			Gtk.TreeIter iter;
			
			if (GetPathAtPos ((int) args.X, (int)args.Y, out path)) {
				if (store.GetIterFromString (out iter, path.ToString ())) {
					ContactListItem item = 
						(ContactListItem) store.GetValue (iter, 2);
					
					if (item.Type == ContactListItemType.Buddy) {
						cardPopup.Contact = 
							(MsnpContact) item.Obj;
						
						cardPopup.Flash (
							(int)args.XRoot, 
							(int) args.YRoot);
					} else 
						cardPopup.Hide ();
				}
			}
		
			return base.OnMotionNotifyEvent (args);
		}


		protected override bool OnButtonPressEvent (Gdk.EventButton args)
		{
			Gtk.TreeIter iter;
			
			if (args.Button == 1) {
				if (args.Type == Gdk.EventType.TwoButtonPress) {
					ContactListItem item;
					if (GetSelectedItem (out item)) {
						if (item.Type == ContactListItemType.Buddy) {
							account.StartConversation ((MsnpContact) item.Obj);
						}
					}
				}
			} else if (args.Button == 3) {
				contactListMenu.Popup ();
			}
			
			return base.OnButtonPressEvent (args);
		}
						
		protected virtual int OnTreeIterCompare (Gtk.TreeModel model,
			Gtk.TreeIter iter1,
			Gtk.TreeIter iter2)
		{
		
		
			ContactListItem item1 = 
				(ContactListItem) model.GetValue (iter1, 2);
			
			ContactListItem item2 =
				(ContactListItem) model.GetValue (iter2, 2);
			
			if (item1 == null && item2 == null)
				return 0;
			
			if (item1 == null)
				return -1;
			
			if (item2 == null)
				return 1;
				
			
			if (item1.Type == ContactListItemType.Group &&
				item2.Type == ContactListItemType.Group) {
				
				MsnpGroup g1 = (MsnpGroup) item1.Obj;
				MsnpGroup g2 = (MsnpGroup) item2.Obj;
				
				if (g1.Id == -1)
					return 1;
				
				if(g2.Id == -1)
					return 0;
				
				return string.Compare (g1.Name, g2.Name);
			}
			
			if (item1.Type == ContactListItemType.Message)
				return -1;
			
			if (item2.Type == ContactListItemType.Message)
				return 1;
			
			
			// both are Contacts
			
			MsnpContact c1 = (MsnpContact) item1.Obj;
			MsnpContact c2 = (MsnpContact) item2.Obj;
			
			if ((int) c1.State < (int) c2.State)
				return -1;
			else if ((int) c1.State > (int) c2.State)
				return 1;
			
			return 0;
				
		}
				
		protected override void OnShown ()
		{
			base.OnShown ();
			//GLib.Timeout.Add (500, findIconMouseOver);
		}
		
		private bool contactIsInGroup (MsnpContact contact, 
			MsnpGroup group)
		{
			Debug.WriteLine ("Contact {0} belong to {1} groups",
				contact.Username, contact.Groups.Count);
			foreach (MsnpGroup g in contact.Groups)
				if (g.Id == group.Id)
					return true;
			
			return false;
		}

		private void contactListMenu_OpenConv_activated (object sender,
			EventArgs args)
		{
			ContactListItem item;
			
			if (GetSelectedItem (out item)) {
				if (item.Type == ContactListItemType.Buddy) {
					MsnpContact c = (MsnpContact) item.Obj;
					account.StartConversation (c);
				}
			}
		}
		
		private void account_Terminated (object sender, EventArgs args)
		{
			groupIters.Clear ();
			iterNoGroup = TreeIter.Zero;
			store.Clear ();
		}

		private void col1_cellDataFunc (
			TreeViewColumn column, 
			CellRenderer renderer, 
			TreeModel model, 
			TreeIter iter)
		{	
			if (store.GetValue (iter, 0) == null)
				return;
		}		
		
		private void col2_cellDataFunc (
			TreeViewColumn column, 
			CellRenderer renderer, 
			TreeModel model, 
			TreeIter iter)
		{
			/*ContactListItem item = 
				(ContactListItem) store.GetValue (iter, 2);
			
			if (item.Type == ContactListItemType.Buddy) {
				MsnpContact contact = (MsnpContact) item.Obj;								
				//store.SetValue (iter, 1, "");
				
				//store.SetValue (iter, 0, );
			}*/
		}
		
		private void account_Groups_Added (object sender,
			RickiLib.Types.WatchedCollectionEventArgs<MsnpGroup> args)
		{
			GroupAdd (args.Instance);
		}
		
		private void account_Buddies_Added (object sender, 
			RickiLib.Types.WatchedCollectionEventArgs<Buddy> args)
		{
			ContactAdd ((MsnpContact) args.Instance);
		}
		
		private void contact_StateChanged (object sender, EventArgs args)
		{
			MsnpContact contact = (MsnpContact) sender;
			TreeIterCollection iters = GetItersFromContact (contact);
			
			foreach (TreeIter iter in iters) {
				store.SetValue (iter, 0, pixbufState [(int) contact.State]);
			}
		}
		
		private void contact_AliasChanged (object sender, EventArgs args)
		{
			MsnpContact contact = (MsnpContact) sender;
			
			TreeIterCollection iters = GetItersFromContact (contact);
			
			foreach (Gtk.TreeIter iter in iters) {
				store.SetValue (iter, 1, contact.Alias);
			}
		}
		
		public bool ContactExists (MsnpContact contact)
		{
			foreach (Gtk.TreeIter iter  in contactIters) {
				ContactListItem item = 
					(ContactListItem) store.GetValue (iter, 2);
				
				if (item.Obj.Equals (contact))
					return true;
			}
			return false;
		}
		
		private Gtk.TreeIter iterNoGroup = TreeIter.Zero;
		
		private void addToNoGroup (MsnpContact contact)
		{
			if (!store.IterIsValid (iterNoGroup)) {
				MsnpGroup g = new MsnpGroup (group_nogroup_name, -1);
				GroupAdd (g);
				//iterNoGroup = store.AppendValues (
				//	null,
				//	g.Name,
				//	new ContactListItem (g));
			}
			
			Gtk.TreeIter iter = store.AppendValues (iterNoGroup,
				pixbufState [(int) contact.State],
				contact.Alias,
				new ContactListItem (contact));
			
			contactIters.Add (iter);
		}
		
		private void theme_refresh ()
		{
			ModifyBase (StateType.Normal,
				Theme.GdkColorFromCairo (Theme.BaseColor));
			ModifyBase (StateType.Selected,
				Theme.GdkColorFromCairo (Theme.SelectedBgColor));
			ModifyBase (StateType.Active,
				Theme.GdkColorFromCairo (Theme.SelectedBgColor));
		}

		public new Gtk.TreeStore Model {
			get {
				return store;
			}
		}	
/*		
		public MsnpGroupCollection Groups {
			get { return groups; }
			set { groups = value; }
		}
		
		public BuddyCollection Buddies {
			get { return buddies; }
			set { 
				buddies = value;
			}
		}
*/
	}
}
