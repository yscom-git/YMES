using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Xml.Linq;
using YMES.FX.Controls.Base;
using System.Runtime.CompilerServices;

namespace YMES.FX.Controls
{
    [ToolboxBitmap(typeof(DataGridView))]
    public partial class YDataGridView: DataGridView, Base.IYGrid
    {
        private string m_Key = string.Empty;
        private DataGridViewContentAlignment m_HeaderAlignment = DataGridViewContentAlignment.MiddleCenter;
        private GridModeEnum m_GridMode = GridModeEnum.QueryNomal;
        private bool m_FixedSort = true;
        public enum GridModeEnum
        {
            QueryNomal
            , UserSetting
        }
        [Category(Common.CN_CATEGORY_APP)]
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }
        [Category(Common.CN_CATEGORY_APP)]
        public bool FixedSort
        {
            get { return m_FixedSort; }
            set
            {
                m_FixedSort = value;
                RefreshCtl();
            }
        }
        [Category(Common.CN_CATEGORY_APP)]
        public DataGridViewContentAlignment HeaderAlignment
        {
            get { return m_HeaderAlignment; }
            set 
            { 
                m_HeaderAlignment = value;
                AlignHeader();
            }
        }
        [Category(Common.CN_CATEGORY_APP)]
        public GridModeEnum GridMode
        {
            get { return m_GridMode; }
            set { m_GridMode = value; }
        }
        public YDataGridView()
        {
            InitializeComponent();          
        }
        private void AlignHeader()
        {
            foreach(DataGridViewColumn col in this.Columns)
            {
                col.HeaderCell.Style.Alignment = HeaderAlignment;
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.AutoGenerateColumns = false;
            RefreshCtl();
            
        }
        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
        {
            base.OnDataBindingComplete(e);
            AlignHeader();
            ClearSelection();
        }

        #region Interface Define

        public void ClearValue()
        {
            this.DataSource = null;
        }

        public object GetCellValue(int row, string colBindNM)
        {
            try
            {
                string colName = GetColNM(colBindNM);
                if (!this.Columns.Contains(colName))
                {
                    return null;
                }
                if (row >= 0 && row < Rows.Count)
                {
                    if (this.Rows[row].Cells[colName].Value != null)
                    {
                        return this.Rows[row].Cells[colName].Value;
                    }
                }
                return null;
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);
                return null;
            }
        }

        public object GetCellValue(int row, int col)
        {
            try
            {
                if (row >= 0 && row < Rows.Count)
                {
                    if (this.Rows[row].Cells[col].Value != null)
                    {
                        return this.Rows[row].Cells[col].Value;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception eLog)
            {
                System.Diagnostics.Debug.WriteLine("[" + System.Reflection.MethodBase.GetCurrentMethod().Name + "]" + eLog.Message);

            }
            return null;
        }

        public string GetColNM(string colBindNM)
        {
            for (int i = 0; i < Columns.Count; i++)
            {
                if (Columns[i].DataPropertyName == colBindNM)
                {
                    return Columns[i].Name;
                }
            }
            return "";
        }

        public string GetColNM(int col)
        {
            return Columns[col].Name;
        }

        public object GetValue()
        {
            return this.DataSource;
        }

        public void SetValue(object val)
        {
            SetValue(val, "Value");
        }
        public void SetValue(object val, string colName = "Value")
        {
            if(Columns.Count <=0)
            {
                AutoGenerateColumns = true;
            }
            if (val is DataTable)
            {
                this.DataSource = val;
            }
            else if (val is DataSet)
            {
                this.DataSource = ((DataSet)val).Tables[0];
            }
            else if(val is IDictionary)
            {
                DataTable dt = new DataTable();
                List<object> vals = new List<object>();
                foreach (var item in ((IDictionary)val).Keys)
                {
                    var key = item;
                    DataColumn col = new DataColumn();
                    col.ColumnName = key.ToString();
                    dt.Columns.Add(col);
                    vals.Add(((IDictionary)val)[key]);
                }
                dt.Rows.Add(vals.ToArray());
                this.DataSource = dt;
                foreach(DataGridViewColumn dvcol in this.Columns)
                {
                    dvcol.HeaderText = dvcol.Name;
                }
            }
            else
            {
                DataColumn dc = new DataColumn(colName);
                DataTable dt = new DataTable();
                dt.Columns.Add(dc);
                DataRow dr = dt.NewRow();
                dr[0] = val;
                dt.Rows.Add(dr);
                this.DataSource = dt;
            }
        }

        public void RefreshCtl()
        {
            foreach (DataGridViewColumn col in this.Columns)
            {
                if (m_FixedSort)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                else
                {
                    col.SortMode = DataGridViewColumnSortMode.Automatic;
                }
            }

            if (GridMode == GridModeEnum.QueryNomal)
            {

                this.MultiSelect = false;
                this.RowHeadersVisible = false;
                this.ReadOnly = true;
                this.MultiSelect = false;
                this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                this.AllowUserToAddRows = false;
                this.AllowUserToOrderColumns = false;
                this.AllowUserToResizeColumns = false;
                this.AllowUserToResizeRows = false;
                this.AllowUserToOrderColumns = false;
                this.RowTemplate.Height = 40;
                this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                this.ColumnHeadersHeight = 30;
                this.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
                this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            }
        }
        #endregion

    }
}
