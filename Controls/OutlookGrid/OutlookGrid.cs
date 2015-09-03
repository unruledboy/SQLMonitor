// Copyright 2006 Herre Kuijpers - <herre@xs4all.nl>
//
// This source file(s) may be redistributed, altered and customized
// by any means PROVIDING the authors name and all copyright
// notices remain intact.
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUR OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Xnlab.SQLMon.Controls.OutlookGrid
{
    #region implementation of the OutlookGrid!
    public partial class OutlookGrid : DataGridView
    {
        #region OutlookGrid constructor
        public OutlookGrid()
        {
            InitializeComponent();

            // very important, this indicates that a new default row class is going to be used to fill the grid
            // in this case our custom OutlookGridRow class
            base.RowTemplate = new OutlookGridRow();
            this._groupTemplate = new OutlookgGridDefaultGroup();

        }
        #endregion OutlookGrid constructor

        #region OutlookGrid property definitions
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewRow RowTemplate
        {
            get { return base.RowTemplate;}
        }

        private IOutlookGridGroup _groupTemplate;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IOutlookGridGroup GroupTemplate
        {
            get
            {
                return _groupTemplate;
            }
            set
            {
                _groupTemplate = value;
            }
        }

        private Image _iconCollapse;
        [Category("Appearance")]
        public Image CollapseIcon
        {
            get { return _iconCollapse; }
            set { _iconCollapse = value; }
        }

        private Image _iconExpand;
        [Category("Appearance")]
        public Image ExpandIcon
        {
            get { return _iconExpand; }
            set { _iconExpand = value; }
        }


        private DataSourceManager _dataSource;
        public new object DataSource
        {
            get
            {
                if (_dataSource == null) return null;

                // special case, datasource is bound to itself.
                // for client it must look like no binding is set,so return null in this case
                if (_dataSource.DataSource.Equals(this)) return null;

                // return the origional datasource.
                return _dataSource.DataSource;
            }
        }
        #endregion OutlookGrid property definitions

        #region OutlookGrid new methods
        public void CollapseAll()
        {
            SetGroupCollapse(true);
        }

        public void ExpandAll()
        {
            SetGroupCollapse(false);
        }

        public void ClearGroups()
        {
            _dataSource = null;
            _groupTemplate.Column = null; //reset
            //FillGrid(null);
        }

        public void BindData(object dataSource, string dataMember)
        {
            this.DataMember = DataMember;
            if (dataSource == null)
            {
                this._dataSource = null;
                Columns.Clear();
            }
            else
            {
                this._dataSource = new DataSourceManager(dataSource, dataMember);
                SetupColumns();
                FillGrid(null);
            }
        }
        public override void Sort(IComparer comparer)
        {
            if (_dataSource == null) // if no datasource is set, then bind to the grid itself
                _dataSource = new DataSourceManager(this, null);

            _dataSource.Sort(comparer);
            FillGrid(_groupTemplate);
        }

        
        public override void Sort(DataGridViewColumn dataGridViewColumn, ListSortDirection direction)
        {
            if (_dataSource == null) // if no datasource is set, then bind to the grid itself
                _dataSource = new DataSourceManager(this, null);

            _dataSource.Sort(new OutlookGridRowComparer(dataGridViewColumn.Index, direction));
            FillGrid(_groupTemplate);
        }
        #endregion OutlookGrid new methods

        #region OutlookGrid event handlers
        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            var row = (OutlookGridRow)base.Rows[e.RowIndex];
            if (row.IsGroupRow)
                e.Cancel = true;
            else
                base.OnCellBeginEdit(e);
        }

        protected override void OnCellDoubleClick(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                var row = (OutlookGridRow)base.Rows[e.RowIndex];
                if (row.IsGroupRow)
                {
                    row.Group.Collapsed = !row.Group.Collapsed;

                    //this is a workaround to make the grid re-calculate it's contents and backgroun bounds
                    // so the background is updated correctly.
                    // this will also invalidate the control, so it will redraw itself
                    row.Visible = false;
                    row.Visible = true;
                    return;
                }
            }
            base.OnCellClick(e);
        }

        // the OnCellMouseDown is overriden so the control can check to see if the
        // user clicked the + or - sign of the group-row
        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                base.OnCellMouseDown(e);
                return;
            }
            var row = (OutlookGridRow)base.Rows[e.RowIndex];
            if (row.IsGroupRow && row.IsIconHit(e))
            {
                Debug.WriteLine("OnCellMouseDown " + DateTime.Now.Ticks.ToString());
                row.Group.Collapsed = !row.Group.Collapsed;

                //this is a workaround to make the grid re-calculate it's contents and backgroun bounds
                // so the background is updated correctly.
                // this will also invalidate the control, so it will redraw itself
                row.Visible = false;
                row.Visible = true;
            }
            else
                base.OnCellMouseDown(e);
        }
        #endregion OutlookGrid event handlers

        #region Grid Fill functions
        private void SetGroupCollapse(bool collapsed)
        {
            if (Rows.Count == 0) return;
            if (_groupTemplate == null) return;

            // set the default grouping style template collapsed property
            _groupTemplate.Collapsed = collapsed;

            // loop through all rows to find the GroupRows
            foreach (OutlookGridRow row in Rows)
            {
                if (row.IsGroupRow)
                    row.Group.Collapsed = collapsed;
            }

            // workaround, make the grid refresh properly
            Rows[0].Visible = !Rows[0].Visible;
            Rows[0].Visible = !Rows[0].Visible;
        }

        private void SetupColumns()
        {
            ArrayList list;

            // clear all columns, this is a somewhat crude implementation
            // refinement may be welcome.
            Columns.Clear();

            // start filling the grid
            if (_dataSource == null)
                return;
            else
                list = _dataSource.Rows;
            if (list.Count <= 0) return;

            foreach (string c in _dataSource.Columns)
            {
                int index;
                var column = Columns[c];
                if (column == null)
                    index = Columns.Add(c, c);
                else
                    index = column.Index;
                Columns[index].SortMode = DataGridViewColumnSortMode.Programmatic; // always programmatic!
            }

        }

        /// <summary>
        /// the fill grid method fills the grid with the data from the DataSourceManager
        /// It takes the grouping style into account, if it is set.
        /// </summary>
        private void FillGrid(IOutlookGridGroup groupingStyle)
        {

            ArrayList list;
            OutlookGridRow row;

            this.Rows.Clear();

            // start filling the grid
            if (_dataSource == null) 
                return; 
            else
                list = _dataSource.Rows;
            if (list.Count <= 0) return;

            // this block is used of grouping is turned off
            // this will simply list all attributes of each object in the list
            if (groupingStyle == null)
            {
                foreach (DataSourceRow r in list)
                {
                    row = (OutlookGridRow) this.RowTemplate.Clone(); 
                    foreach (var val in r)
                    {
                        DataGridViewCell cell = new DataGridViewTextBoxCell();
                        cell.Value = val.ToString();
                        row.Cells.Add(cell);
                    }
                    Rows.Add(row);
                }
            }

            // this block is used when grouping is used
            // items in the list must be sorted, and then they will automatically be grouped
            else
            {
                IOutlookGridGroup groupCur = null;
                object result = null;
                var counter = 0; // counts number of items in the group

                foreach (DataSourceRow r in list)
                {
                    row = (OutlookGridRow)this.RowTemplate.Clone();
                    result = r[groupingStyle.Column.Index];
                    if (groupCur != null && groupCur.CompareTo(result) == 0) // item is part of the group
                    {
                        row.Group = groupCur;
                        counter++;
                    }
                    else // item is not part of the group, so create new group
                    {
                        if (groupCur != null)
                            groupCur.ItemCount = counter;

                        groupCur = (IOutlookGridGroup)groupingStyle.Clone(); // init
                        groupCur.Value = result;
                        row.Group = groupCur;
                        row.IsGroupRow = true;
                        row.Height = groupCur.Height;
                        row.CreateCells(this, groupCur.Value);
                        Rows.Add(row);

                        // add content row after this
                        row = (OutlookGridRow)this.RowTemplate.Clone();
                        row.Group = groupCur;
                        counter = 1; // reset counter for next group
                    }


                    foreach (var obj in r)
                    {
                        DataGridViewCell cell = new DataGridViewTextBoxCell();
                        cell.Value = obj.ToString();
                        row.Cells.Add(cell);
                    }
                    Rows.Add(row);
                    groupCur.ItemCount = counter;
                }
            }

        }
        #endregion Grid Fill functions
    }
    #endregion implementation of the OutlookGrid!
}
