//http://blogs.vbcity.com/hotdog/archive/2008/12/19/9225.aspx
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Subro.Controls
{
    /// <summary>
    /// Add this component in runtime or designtime and assign a datagridview to it to enable grouping on that grid.
    /// You can also add an 
    /// </summary>
    [DefaultEvent("DisplayGroup")]
    public class DataGridViewGrouper : Component, IGrouper
    {
        public DataGridViewGrouper()
        {
            source.DataSourceChanged += new EventHandler(source_DataSourceChanged);
            source.GroupingChanged += new EventHandler(source_GroupingChanged);
        }


        public DataGridViewGrouper(DataGridView Grid)
            : this()
        {
            this.DataGridView = Grid;
        }
        public DataGridViewGrouper(IContainer Container)
            : this()
        {
            Container.Add(this);
        }


        private DataGridView grid;
        [DefaultValue(null)]
        public DataGridView DataGridView
        {
            get { return grid; }
            set
            {
                if (grid == value) return;
                if (grid != null)
                {
                    grid.Sorted -= new EventHandler(grid_Sorted);
                    grid.RowPrePaint -= new DataGridViewRowPrePaintEventHandler(grid_RowPrePaint);
                    grid.RowPostPaint -= new DataGridViewRowPostPaintEventHandler(grid_RowPostPaint);
                    grid.CellBeginEdit -= new DataGridViewCellCancelEventHandler(grid_CellBeginEdit);
                    grid.CellDoubleClick -= new DataGridViewCellEventHandler(grid_CellDoubleClick);
                    grid.CellClick -= new DataGridViewCellEventHandler(grid_CellClick);
                    grid.MouseMove -= new MouseEventHandler(grid_MouseMove);
                    grid.SelectionChanged -= new EventHandler(grid_SelectionChanged);
                }
                RemoveGrouping();
                selectedrows.Clear();
                grid = value;
                if (grid != null)
                {
                    grid.Sorted += new EventHandler(grid_Sorted);
                    grid.RowPrePaint += new DataGridViewRowPrePaintEventHandler(grid_RowPrePaint);
                    grid.RowPostPaint += new DataGridViewRowPostPaintEventHandler(grid_RowPostPaint);
                    grid.CellBeginEdit += new DataGridViewCellCancelEventHandler(grid_CellBeginEdit);
                    grid.CellDoubleClick += new DataGridViewCellEventHandler(grid_CellDoubleClick);
                    grid.CellClick += new DataGridViewCellEventHandler(grid_CellClick);
                    grid.MouseMove += new MouseEventHandler(grid_MouseMove);
                    grid.SelectionChanged += new EventHandler(grid_SelectionChanged);
                }
            }
        }


        #region Select /  Collapse/Expand


        Point capturedcollapsebox = new Point(-1, -1);
        void grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < HeaderOffset && e.X >= HeaderOffset - collapseboxwidth)
            {
                DataGridView.HitTestInfo ht = grid.HitTest(e.X, e.Y);
                if (IsGroupRow(ht.RowIndex))
                {
                    var y = e.Y - ht.RowY;
                    if (y >= CollapseBox_Y_Offset && y <= CollapseBox_Y_Offset + collapseboxwidth)
                    {
                        checkcollapsedfocused(ht.ColumnIndex, ht.RowIndex);
                        return;
                    }
                }
            }
            checkcollapsedfocused(-1, -1);
        }
        void InvalidateCapturedBox()
        {
            if (capturedcollapsebox.Y == -1) return;
            grid.InvalidateCell(capturedcollapsebox.X, capturedcollapsebox.Y);
        }
        void checkcollapsedfocused(int col, int row)
        {
            if (capturedcollapsebox.X != col || capturedcollapsebox.Y != row)
            {
                InvalidateCapturedBox();
                capturedcollapsebox = new Point(col, row);
                InvalidateCapturedBox();
            }
        }


        void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (e.RowIndex == capturedcollapsebox.Y)
            {
                bool show = IsCollapsed(e.RowIndex);
                CollapseExpand(e.RowIndex, show);
                if (show)
                    grid.CurrentCell = grid[1, e.RowIndex + 1];
            }
        }
        /// <summary>
        /// selected rows are kept seperately in order to invalidate the entire row
        /// and not just one cell when the selection is changed
        /// </summary>
        List<int> selectedrows = new List<int>();
        void grid_SelectionChanged(object sender, EventArgs e)
        {
            invalidateselected();
            selectedrows.Clear();
            foreach (DataGridViewCell c in grid.SelectedCells)
                if (IsGroupRow(c.RowIndex))
                    selectedrows.Add(c.RowIndex);
            invalidateselected();
        }
        void invalidateselected()
        {
            if (grid.Rows.Count == 0 || selectedrows.Count == 0 || grid.SelectionMode == DataGridViewSelectionMode.FullRowSelect) return;
            foreach (int i in selectedrows)
                grid.InvalidateRow(i);
        }



        bool IsCollapsed(int index)
        {
            if (++index >= grid.Rows.Count) return false;
            return !grid.Rows[index].Visible;
        }
        void CollapseExpand(int index, bool show)
        {
            grid.SuspendLayout();
            foreach (DataGridViewRow row in GetRows(index))
                row.Visible = show;
            grid.ResumeLayout();
        }
        public void ExpandAll()
        {
            CollapseExpandAll(true);
        }
        public void CollapseAll()
        {
            CollapseExpandAll(false);
        }
        void CollapseExpandAll(bool show)
        {
            if (grid == null || !GridUsesGroupSource) return;
            grid.SuspendLayout();
            source.SuspendBinding();
            int cnt = source.Count;
            for (int i = 0; i < cnt; i++)
            {
                if (!IsGroupRow(i))
                    grid.Rows[i].Visible = show;
            }
            grid.ResumeLayout();
            source.ResumeBinding();
        }
        void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (IsGroupRow(e.RowIndex))
            {
                CollapseExpand(e.RowIndex, true);
                grid.SuspendLayout();
                grid.CurrentCell = grid[1, e.RowIndex + 1];
                grid.Rows[e.RowIndex].Selected = false;
                SelectGroup(e.RowIndex);
                grid.ResumeLayout();
            }
        }
        IEnumerable<DataGridViewRow> GetRows(int index)
        {
            while (!IsGroupRow(++index) && index < source.Count)
                yield return grid.Rows[index];
        }
        void SelectGroup(int offset)
        {
            foreach (DataGridViewRow row in GetRows(offset))
                row.Selected = true;
        }
        #endregion


        public IEnumerable<IGroupRow> GetGroups()
        {
            return source.GetGroups();
        }
        public bool IsGroupRow(int Index)
        {
            return source.IsGroupRow(Index);
        }
        void source_DataSourceChanged(object sender, EventArgs e)
        {
            if (PropertiesChanged != null)
                PropertiesChanged(this, e);
        }
        public event EventHandler PropertiesChanged;
        public IEnumerable<PropertyDescriptor> GetProperties()
        {
            foreach (PropertyDescriptor pd in source.GetItemProperties(null))
                yield return pd;
        }
        void grid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (IsGroupRow(e.RowIndex))
                e.Cancel = true;
        }
        protected override void Dispose(bool disposing)
        {
            DataGridView = null;
            source.Dispose();
            base.Dispose(disposing);
        }
        void grid_Sorted(object sender, EventArgs e)
        {
            this.capturedcollapsebox = new Point(-1, -1);
            ResetGrouping();
        }
        GroupingSource source = new GroupingSource();
        public GroupingSource GroupingSource
        {
            get
            {
                return source;
            }
        }
        public void RemoveGrouping()
        {
            if (GridUsesGroupSource)
                try
                {
                    grid.DataSource = source.DataSource;
                    grid.DataMember = source.DataMember;
                    source.RemoveGrouping();
                }
                catch { }
        }
        void source_GroupingChanged(object sender, EventArgs e)
        {
            OnGroupOnChanged();
        }
        public event EventHandler GroupingChanged;
        void OnGroupOnChanged()
        {
            if (GroupingChanged != null)
                GroupingChanged(this, EventArgs.Empty);
        }
        bool GridUsesGroupSource
        {
            get
            {
                return grid != null && grid.DataSource == source;
            }
        }
        public void ResetGrouping()
        {
            if (!GridUsesGroupSource) return;
            source.ResetGroup();
        }


        [DefaultValue(null)]
        public GroupingInfo GroupOn
        {
            get
            {
                return source.GroupOn;
            }
            set
            {
                if (GroupOn == value) return;
                if (value == null)
                    RemoveGrouping();
                else
                    CheckSource().GroupOn = value;
            }
        }
        public bool IsGrouped
        {
            get
            {
                return source.GroupOn != null;
            }
        }
        [DefaultValue(SortOrder.Ascending)]
        public SortOrder SortOrder
        {
            get
            {
                return source.GroupSortDirection;
            }
            set
            {
                source.GroupSortDirection = value;
            }
        }



        public void SetGroupOn(DataGridViewColumn col)
        {
            SetGroupOn(col == null ? null : col.DataPropertyName);
        }


        public void SetGroupOn(PropertyDescriptor Property)
        {
            CheckSource().SetGroupOn(Property);
        }
        public void SetGroupOn(GroupingDelegate gd)
        {
            CheckSource().SetGroupOn(gd);
        }
        public void SetGroupOnStartLetters(GroupingInfo g, int Letters)
        {
            CheckSource().SetGroupOnStartLetters(g, Letters);
        }
        public void SetGroupOnStartLetters(string Property, int Letters)
        {
            CheckSource().SetGroupOnStartLetters(Property, Letters);
        }
        public void SetGroupOn(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                RemoveGrouping();
            else
                CheckSource().SetGroupOn(Name);
        }
        GroupingSource CheckSource()
        {
            if (grid == null)
                throw new Exception("No target datagridview set");
            if (!GridUsesGroupSource)
            {
                source.DataSource = grid.DataSource;
                source.DataMember = grid.DataMember;
                grid.DataSource = source;
            }
            return source;
        }
        #region Painting
        void grid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (IsGroupRow(e.RowIndex))
            {
                e.Handled = true;
                PaintGroupRow(e);
            }
        }
        const int collapseboxwidth = 10;
        const int lineoffset = collapseboxwidth / 2;
        int HeaderOffset
        {
            get
            {
                if (grid.RowHeadersVisible) return grid.RowHeadersWidth - lineoffset;
                return lineoffset * 4;
            }
        }
        Pen linepen = Pens.SteelBlue;
        bool DrawExpandCollapseLines
        {
            get
            {
                return grid.RowHeadersVisible;
            }
        }
        void grid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (!DrawExpandCollapseLines || e.RowIndex >= source.Count) return;
            int next = e.RowIndex + 1;
            int r = grid.RowHeadersWidth;
            int x = HeaderOffset - lineoffset;
            int y = e.RowBounds.Top + e.RowBounds.Height / 2;
            e.Graphics.DrawLine(linepen, x, y, r, y);
            if (next < source.Count && !IsGroupRow(next))
                y = e.RowBounds.Bottom;
            e.Graphics.DrawLine(linepen, x, e.RowBounds.Top, x, y);


        }


        private bool showheader = true;
        [DefaultValue(true)]
        public bool ShowGroupName
        {
            get { return showheader; }
            set
            {
                if (showheader == value) return;
                showheader = value;
                if (grid != null) grid.Invalidate();
            }
        }
        private bool showcount = true;
        [DefaultValue(true)]
        public bool ShowCount
        {
            get { return showcount; }
            set
            {
                if (showcount == value) return;
                showcount = value;
                if (grid != null) grid.Invalidate();
            }
        }


        /// <summary>
        /// This event is fired when the group row has to be painted and the display values are requested
        /// </summary>
        public event EventHandler<GroupDisplayEventArgs> DisplayGroup;
        GroupDisplayEventArgs GetDisplayValues(DataGridViewRowPrePaintEventArgs pe)
        {
            IGroupRow row = source[pe.RowIndex] as IGroupRow;
            GroupDisplayEventArgs e = new GroupDisplayEventArgs(row, source.GroupOn);
            bool selected = selectedrows.Contains(pe.RowIndex);
            e.Selected = selected;
            e.BackColor = selected ? grid.DefaultCellStyle.SelectionBackColor : grid.DefaultCellStyle.BackColor;
            e.ForeColor = selected ? grid.DefaultCellStyle.SelectionForeColor : grid.DefaultCellStyle.ForeColor;
            e.Font = pe.InheritedRowStyle.Font;
            if (showcount)
                e.Summary = "(" + row.Count + ")";
            if (showheader)
                e.Header = source.GroupOn.ToString();
            e.GroupingInfo.SetDisplayValues(e);
            if (e.Cancel) return null;
            if (DisplayGroup != null)
            {
                DisplayGroup(this, e);
                if (e.Cancel) return null;
            }
            return e;
        }
        void PaintGroupRow(DataGridViewRowPrePaintEventArgs e)
        {
            var info = GetDisplayValues(e);
            if (info == null) return; //cancelled
            var r = e.RowBounds;
            r.X = 1;
            r.Height--;
            using (var b = new SolidBrush(info.BackColor))
                e.Graphics.FillRectangle(b, r);
            //line under the group row
            e.Graphics.DrawLine(Pens.SteelBlue, r.Left, r.Bottom, r.Right, r.Bottom);
            //collapse/expand symbol               
            {
                var cer = GetCollapseBoxBounds(e.RowBounds.Y);
                if (capturedcollapsebox.Y == e.RowIndex)
                    e.Graphics.FillEllipse(Brushes.Yellow, cer);
                e.Graphics.DrawEllipse(linepen, cer);
                bool collapsed = IsCollapsed(e.RowIndex);
                int cx;
                if (DrawExpandCollapseLines && !collapsed)
                {
                    cx = HeaderOffset - lineoffset;
                    e.Graphics.DrawLine(linepen, cx, cer.Bottom, cx, r.Bottom);
                }
                cer.Inflate(-2, -2);
                var cy = cer.Y + cer.Height / 2;
                e.Graphics.DrawLine(linepen, cer.X, cy, cer.Right, cy);
                if (collapsed)
                {
                    cx = cer.X + cer.Width / 2;
                    e.Graphics.DrawLine(linepen, cx, cer.Top, cx, cer.Bottom);
                }
            }
            //group value

            {
                r.X = HeaderOffset + 1;



                using (var fb = new SolidBrush(info.ForeColor))
                {
                    var sf = new StringFormat { LineAlignment = StringAlignment.Center };
                    if (info.Header != null)
                    {
                        var size = e.Graphics.MeasureString(info.Header, info.Font);
                        e.Graphics.DrawString(info.Header, info.Font, fb, r, sf);
                        r.Offset((int)size.Width + 5, 0);
                    }
                    if (info.DisplayValue != null)
                    {
                        using (var f = new Font(info.Font.FontFamily, info.Font.Size + 2, FontStyle.Bold))
                        {
                            var size = e.Graphics.MeasureString(info.DisplayValue, f);
                            e.Graphics.DrawString(info.DisplayValue, f, fb, r, sf);
                            r.Offset((int)size.Width + 10, 0);
                        }
                    }
                    if (info.Summary != null)
                    {
                        e.Graphics.DrawString(info.Summary, info.Font, fb, r, sf);
                    }
                }
            }
        }
        const int CollapseBox_Y_Offset = 5;
        private Rectangle GetCollapseBoxBounds(int Y_Offset)
        {
            return new Rectangle(HeaderOffset - collapseboxwidth, Y_Offset + CollapseBox_Y_Offset, collapseboxwidth, collapseboxwidth);
        }
        #endregion
        public bool CurrentRowIsGroupRow
        {
            get
            {
                if (grid == null || grid.CurrentCell == null) return false;
                return IsGroupRow(grid.CurrentCell.RowIndex);
            }
        }
    }
    public class GroupDisplayEventArgs : CancelEventArgs
    {
        public readonly IGroupRow Row;
        public readonly GroupingInfo GroupingInfo;
        public GroupDisplayEventArgs(IGroupRow Row, GroupingInfo Info)
        {
            this.Row = Row;
            this.GroupingInfo = Info;
        }
        public object Value { get { return Row.Value; } }
        public string DisplayValue { get; set; }
        public string Header { get; set; }
        public string Summary { get; set; }
        public Color BackColor { get; set; }
        public Color ForeColor { get; set; }
        public Font Font { get; set; }
        public bool Selected { get; internal set; }
    }
    public interface IGroupRow
    {
        int Index { get; }
        object Value { get; }
        int Count { get; }
        object[] Rows { get; }
    }


    [DefaultEvent("GroupingChanged")]
    public class GroupingSource : BindingSource
    {
        public GroupingSource()
        {
        }
        public GroupingSource(object DataSource)
            : this()
        {
            this.DataSource = DataSource;
        }
        public GroupingSource(object DataSource, string GroupOn)
            : this(DataSource)
        {
        }
        GroupingInfo groupon;
        [DefaultValue(null)]
        public GroupingInfo GroupOn
        {
            get
            {
                return groupon;
            }
            set
            {
                if (groupon == value) return;
                RemoveGrouping(value == null);
                if (value != null)
                {
                    if (value.Equals(groupon)) return;
                    groupon = value;
                    OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
                    OnGroupingChanged();
                }
            }
        }


        public void RemoveGrouping()
        {
            RemoveGrouping(true);
        }
        void RemoveGrouping(bool callListChanged)
        {
            if (groupon == null) return;
            groupon = null;
            ResetGroup();
            if (callListChanged)
            {
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
                OnGroupingChanged();
            }
        }
        public void SetGroupOn(string Property)
        {
            SetGroupOn(GetProperty(Property));
        }
        PropertyDescriptor GetProperty(string Name)
        {
            var pd = this.GetItemProperties(null)[Name];
            if (pd == null)
                throw new Exception(Name + " is not a valid property");
            return pd;
        }
        public void SetGroupOn(PropertyDescriptor p)
        {
            if (p == null) throw new ArgumentNullException();
            if (groupon == null || !groupon.IsProperty(p))
                GroupOn = new PropertyGrouper(p);
        }
        public void SetGroupOn(GroupingDelegate gd)
        {
            SetGroupOn(gd, null);
        }
        public void SetGroupOn(GroupingDelegate gd, string descr)
        {
            if (gd == null) throw new ArgumentNullException();
            GroupOn = new DelegateGrouper(gd, descr);
        }
        public void SetGroupOnStartLetters(GroupingInfo g, int Letters)
        {
            GroupOn = new StartLetterGrouper(g, Letters);
        }
        public void SetGroupOnStartLetters(string Property, int Letters)
        {
            SetGroupOnStartLetters(GetProperty(Property), Letters);
        }
        public bool IsGroupRow(int Index)
        {
            if (info == null) return false;
            if (Index < 0 || Index >= Count) return false;
            return info.Rows[Index] is GroupRow;
        }



        SortOrder order = SortOrder.Ascending;
        [DefaultValue(SortOrder.Ascending)]
        public SortOrder GroupSortDirection
        {
            get { return order; }
            set
            {
                if (order == value) return;
                order = value;
                ResetGroup();
            }
        }
        class GroupRow : IGroupRow
        {
            public int Index { get; set; }
            public object Value { get; set; }
            public object[] Rows { get; set; }
            public int Count
            {
                get { return Rows.Length; }
            }
            internal List<object> List = new List<object>();
            public void Finalize(int Index)
            {
                this.Index = Index;
                Rows = List.ToArray();
                List = null;
            }
        }
        public IEnumerable<IGroupRow> GetGroups()
        {
            foreach (IGroupRow g in Info.Groups.Values)
                yield return g;
        }
        class GroupInfo
        {
            public readonly GroupingSource Owner;


            public GroupInfo(GroupingSource Owner)
            {
                this.Owner = Owner;
                set();
            }
            public int TotalCount
            {
                get
                {
                    return Rows.Count;
                }
            }
            public IList Rows;
            //public List<GroupRow> Groups = new List<GroupRow>();
            public IDictionary<object, GroupRow> Groups;
            void set()
            {
                Groups = null;
                GroupingInfo gi = Owner.groupon;
                if (gi == null)
                {
                    Rows = Owner.List;
                    return;
                }
                if (Owner.GroupSortDirection == SortOrder.None)
                    Groups = new Dictionary<object, GroupRow>();
                else
                {
                    GenericComparer<object> comparer = new GenericComparer<object>();
                    comparer.Descending = Owner.GroupSortDirection == SortOrder.Descending;
                    Groups = new SortedDictionary<object, GroupRow>(comparer);
                }
                foreach (object row in Owner)
                {
                    object key = gi.GetGroupValue(row);
                    GroupRow gr;
                    if (!Groups.TryGetValue(key, out gr))
                    {
                        gr = new GroupRow();
                        gr.Value = key;
                        Groups.Add(key, gr);
                    }
                    gr.List.Add(row);
                }

                //var groups = Owner.Cast<object>().GroupBy<object, object>(o => gr.GetGroupValue(o));
                int i = 0;
                Rows = new List<object>(Groups.Count + Owner.BaseCount);
                foreach (GroupRow g in Groups.Values)
                {
                    g.Finalize(i++);
                    Rows.Add(g);
                    foreach (object row in g.Rows)
                        Rows.Add(row);
                }
            }




        }



        GroupInfo info;
        GroupInfo Info
        {
            get
            {
                if (info == null)
                {
                    info = new GroupInfo(this);
                    if (bsource != null)
                        SyncWithBSource();
                }
                return info;
            }
        }
        void OnGroupingChanged()
        {
            if (GroupingChanged != null)
                GroupingChanged(this, EventArgs.Empty);
        }
        public event EventHandler GroupingChanged;


        public void ResetGroup()
        {
            info = null;
        }
        BindingSource bsource;
        void DisposeBindingSourceEvents()
        {
            if (bsource == null) return;
            bsource.CurrentChanged -= new EventHandler(bsource_CurrentChanged);
        }
        protected override void Dispose(bool disposing)
        {
            DisposeBindingSourceEvents();
            base.Dispose(disposing);
        }
        protected override void OnDataSourceChanged(EventArgs e)
        {
            ResetGroup();
            DisposeBindingSourceEvents();
            bsource = DataSource as BindingSource;
            if (bsource != null)
            {
                bsource.CurrentChanged += new EventHandler(bsource_CurrentChanged);
                if (NeedSync) SyncWithBSource();
            }
            base.OnDataSourceChanged(e);
        }
        bool suspendlistchange;
        protected override void OnListChanged(ListChangedEventArgs e)
        {
            if (suspendlistchange) return;


            switch (e.ListChangedType)
            {
                case ListChangedType.ItemChanged:
                    if (groupon != null && groupon.IsProperty(e.PropertyDescriptor))
                        ResetGroup();
                    break;
                case ListChangedType.ItemAdded:
                    if (info != null)
                        info.Rows.Add(List[e.NewIndex]);
                    break;
                case ListChangedType.ItemDeleted:
                    ResetGroup();
                    break;
                case ListChangedType.Reset:
                    ResetGroup();
                    break;
                case ListChangedType.PropertyDescriptorAdded:
                case ListChangedType.PropertyDescriptorChanged:
                case ListChangedType.PropertyDescriptorDeleted:
                    props = null;
                    break;
            }
            base.OnListChanged(e);
        }
        public override object AddNew()
        {
            suspendlistchange = true;
            try
            {
                var res = base.AddNew();
                if (info != null)
                    info.Rows.Add(res);
                return res;
            }
            finally
            {
                suspendlistchange = false;
            }
        }
        public override void ApplySort(PropertyDescriptor property, ListSortDirection sort)
        {
            if (property is PropertyWrapper)
                property = (property as PropertyWrapper).Property;
            base.ApplySort(property, sort);
        }
        public override void ApplySort(ListSortDescriptionCollection sorts)
        {
            base.ApplySort(sorts);
        }





        public override void RemoveAt(int index)
        {
            if (info == null || groupon == null)
                base.RemoveAt(index);
            else if (!IsGroupRow(index))
            {
                var i = List.IndexOf(this[index]);
                suspendlistchange = true;
                try
                {
                    info.Rows.RemoveAt(index);
                    List.RemoveAt(i);
                    base.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
                }
                finally
                {
                    suspendlistchange = false;
                }
            }
        }



        public override void Remove(object value)
        {
            if (value is GroupRow) return;
            int index = this.IndexOf(value);
            if (index != -1)
            {
                RemoveAt(index);
            }
        }


        protected override void OnCurrentChanged(EventArgs e)
        {
            base.OnCurrentChanged(e);
            if (NeedSync && !(Current is GroupRow))
            {
                bsource.Position = bsource.IndexOf(Current);
            }
        }
        void bsource_CurrentChanged(object sender, EventArgs e)
        {
            if (NeedSync)
                SyncWithBSource();
        }


        bool NeedSync
        {
            get
            {
                if (bsource == null || suspendlistchange) return false;
                if (bsource.IsBindingSuspended) return false;
                return Current != bsource.Current;
            }
        }


        private void SyncWithBSource()
        {
            Position = IndexOf(bsource.Current);
        }
        public override int IndexOf(object value)
        {
            return Info.Rows.IndexOf(value);
        }


        public class PropertyWrapper : PropertyDescriptor
        {
            public readonly PropertyDescriptor Property;
            public readonly GroupingSource Owner;
            public PropertyWrapper(PropertyDescriptor Property, GroupingSource Owner)
                : base(Property)
            {
                this.Property = Property;
                this.Owner = Owner;
            }
            public override bool CanResetValue(object component)
            {
                return Property.CanResetValue(component);
            }
            public override Type ComponentType
            {
                get { return Property.ComponentType; }
            }
            public override object GetValue(object component)
            {
                if (component is GroupRow)
                {
                    if (Owner.groupon.IsProperty(Property))
                        return (component as GroupRow).Value;
                    return null;
                }
                return Property.GetValue(component);
            }
            public override bool IsReadOnly
            {
                get { return Property.IsReadOnly; }
            }
            public override Type PropertyType
            {
                get { return Property.PropertyType; }
            }
            public override void ResetValue(object component)
            {
                Property.ResetValue(component);
            }
            public override void SetValue(object component, object value)
            {
                Property.SetValue(component, value);
            }
            public override bool ShouldSerializeValue(object component)
            {
                return Property.ShouldSerializeValue(component);
            }
        }


        PropertyDescriptorCollection props;
        public override PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            if (listAccessors == null)
            {
                if (props == null)
                {
                    /*
                    props = new PropertyDescriptorCollection( 
                    base.GetItemProperties(null).Cast<PropertyDescriptor>()
                    .Select(pd => new PropertyWrapper(pd, this)).ToArray());*/
                    props = base.GetItemProperties(null);
                    PropertyDescriptor[] arr = new PropertyDescriptor[props.Count];
                    for (int i = 0; i < props.Count; i++)
                    {
                        arr[i] = new PropertyWrapper(props[i], this);
                    }
                    props = new PropertyDescriptorCollection(arr);
                }
                return props;
            }
            return base.GetItemProperties(listAccessors);
        }
        public int BaseCount
        {
            get
            {
                return base.Count;
            }
        }
        public object GetBaseRow(int Index)
        {
            return List[Index];
        }
        public override int Count
        {
            get
            {
                return Info.TotalCount;
            }
        }
        public override object this[int index]
        {
            get
            {
                return Info.Rows[index];
            }
            set
            {
                Info.Rows[index] = value;
            }
        }
    }
    #region Grouping Info objects
    public abstract class GroupingInfo
    {
        public abstract object GetGroupValue(object Row);
        public virtual bool IsProperty(PropertyDescriptor p)
        {
            return false;
        }
        public virtual bool IsProperty(string Name)
        {
            return Name == ToString();
        }
        public static implicit operator GroupingInfo(PropertyDescriptor p)
        {
            return new PropertyGrouper(p);
        }
        public virtual void SetDisplayValues(GroupDisplayEventArgs e)
        {
            var o = e.Value;
            e.DisplayValue = o == null ? "<Null>" : o.ToString();
        }
    }
    public class PropertyGrouper : GroupingInfo
    {
        public readonly PropertyDescriptor Property;
        public PropertyGrouper(PropertyDescriptor Property)
        {
            if (Property == null) throw new ArgumentNullException();
            this.Property = Property;
        }
        public override object GetGroupValue(object Row)
        {
            return Property.GetValue(Row);
        }
        public override string ToString()
        {
            return Property.Name;
        }
    }
    public delegate object GroupingDelegate(object Row);
    public class DelegateGrouper : GroupingInfo
    {
        public readonly string Name;
        public readonly GroupingDelegate GroupingDelegate;
        public DelegateGrouper(GroupingDelegate Delegate, string Name)
        {
            if (Delegate == null) throw new ArgumentNullException();
            this.Name = Name;
            if (Name == null) this.Name = Delegate.ToString();
            this.GroupingDelegate = Delegate;
        }
        public override string ToString()
        {
            return Name;
        }
        public override object GetGroupValue(object Row)
        {
            return GroupingDelegate(Row);
        }
    }



    public class StartLetterGrouper : GroupingInfo
    {
        public readonly GroupingInfo Grouper;
        public readonly int Letters;
        public StartLetterGrouper(GroupingInfo Grouper)
            : this(Grouper, 1)
        {
        }
        public StartLetterGrouper(GroupingInfo Grouper, int Letters)
        {
            if (Grouper == null) throw new ArgumentNullException();
            this.Grouper = Grouper;
            this.Letters = Letters;
        }
        public override string ToString()
        {
            return Grouper.ToString();
        }
        public override bool IsProperty(PropertyDescriptor p)
        {
            return Grouper.IsProperty(p);
        }
        public override object GetGroupValue(object Row)
        {
            var val = Grouper.GetGroupValue(Row);
            if (val == null) return null;
            var s = val.ToString();
            if (s.Length < Letters) return s;
            return s.Substring(0, Letters);
        }
    }
    #endregion
    #region Interfaces
    public interface IDataGridViewGrouperOwner
    {
        DataGridViewGrouper Grouper { get; }
    }
    public interface IGrouper
    {
        void SetGroupOn(string col);
        void SetGroupOn(PropertyDescriptor col);
        void RemoveGrouping();
        GroupingInfo GroupOn { get; set; }
        event EventHandler PropertiesChanged;
        event EventHandler GroupingChanged;
        IEnumerable<PropertyDescriptor> GetProperties();
    }
    #endregion
}


/*
* Added the Generic comparer here, for ease of use on blog posting ;)
*/

namespace Subro
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections;
    using System.ComponentModel;
    /// <summary>
    /// Comparer that tries to find the 'strongest' comparer for a type. 
    /// if the type implements a generic IComparable, that is used.
    /// otherwise if it implements a normal IComparable, that is used.
    /// If neither are implemented, the ToString versions are compared. 
    /// INullable structures are also supported.
    /// This way, the DefaultComparer can compare any object types and can be used for sorting any source.
    /// </summary>
    /// <example>Array.Sort(YourArray,new GenericComparer());</example>
    public class GenericComparer : IComparer
    {
        public GenericComparer()
        {
        }
        public GenericComparer(Type Type)
        {
            this.Type = Type;
        }
        Type type;
        public Type Type
        {
            get
            {
                return type;
            }
            set
            {
                if (value == null) throw new ArgumentNullException();
                type = value;
                comp = null;
            }
        }
        Type targettype;
        /// <summary>
        /// normally the same as the type, but can be set to a different type
        /// </summary>
        public Type TargetType
        {
            get
            {
                if (targettype == null) return type;
                return targettype;
            }
            set
            {
                if (TargetType == value) return;
                targettype = value;
                comp = null;
            }
        }



        IComparer comp;
        IComparer GetGenericComparer(Type From, Type To)
        {
            while (To != typeof(object))
            {
                if (typeof(IComparable<>).MakeGenericType(To).IsAssignableFrom(From))
                    return (IComparer)Activator.CreateInstance(typeof(StrongCompare<,>).MakeGenericType(From, To));
                To = To.BaseType;
            }
            return null;
        }
        public IComparer GetComparer(Type From, Type To)
        {
            var gen = GetGenericComparer(From, To);
            if (gen != null)
                return gen;
            else if (typeof(IComparable).IsAssignableFrom(type))
            {
                return (IComparer)Activator.CreateInstance(typeof(NonGenericCompare<>).MakeGenericType(type));
            }
            else if (type.IsGenericType && typeof(Nullable<>) == type.GetGenericTypeDefinition())
            {
                var basetype = type.GetGenericArguments()[0];
                return (IComparer)Activator.CreateInstance(typeof(NullableComparer<>).MakeGenericType(basetype),
                GetComparer(basetype, To == From ? basetype : To));
            }
            return new StringComparer();
        }
        class NullableComparer<T> : IComparer
        where T : struct
        {
            public readonly IComparer BaseComparer;
            public NullableComparer(IComparer BaseComparer)
            {
                this.BaseComparer = BaseComparer;
            }
            object getval(object o)
            {
                return ((Nullable<T>)o).Value;
            }
            public int Compare(object x, object y)
            {
                return BaseComparer.Compare(getval(x), getval(y));
            }
        }
        class StrongCompare<F, T> : IComparer
        where F : IComparable<T>
        {
            public int Compare(object x, object y)
            {
                return ((F)x).CompareTo((T)y);
            }
        }
        class NonGenericCompare<T> : IComparer
        where T : IComparable
        {
            public int Compare(object x, object y)
            {
                return ((T)x).CompareTo(y);
            }
        }
        class StringComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return string.Compare(x.ToString(), y.ToString());
            }
        }
        public bool Descending
        {
            get
            {
                return factor < 0;
            }
            set
            {
                factor = value ? -1 : 1;
            }
        }
        int factor = 1;
        int compare(object x, object y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            if (type == null)
                Type = x.GetType();
            if (comp == null)
                comp = GetComparer(type, TargetType);
            return comp.Compare(x, y);
        }
        public int Compare(object x, object y)
        {
            return factor * compare(x, y);
        }
    }


    public class GenericComparer<T> : GenericComparer, IComparer<T>
    {
        public GenericComparer()
            : base(typeof(T))
        { }
        public int Compare(T a, T b)
        {
            return base.Compare(a, b);
        }
    }



    public class PropertyDescriptorComparer : GenericComparer
    {
        public readonly PropertyDescriptor Prop;
        public PropertyDescriptorComparer(PropertyDescriptor Prop)
            : this(Prop, true)
        {
        }
        public PropertyDescriptorComparer(PropertyDescriptor Prop, bool Descending)
            : base(Prop.PropertyType)
        {
            this.Prop = Prop;
            this.Descending = Descending;
        }
    }
}
