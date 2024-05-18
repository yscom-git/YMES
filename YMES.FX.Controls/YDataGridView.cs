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

namespace YMES.FX.Controls
{
    [ToolboxBitmap(typeof(DataGridView))]
    public partial class YDataGridView: DataGridView, Base.IYGrid
    {
        public enum GridModeEnum
        {
            QueryNomal
            , UserSetting
        }
        private GridModeEnum m_GridMode = GridModeEnum.QueryNomal;

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
                    dt.Columns.Add(col);
                    vals.Add(((IDictionary)val)[key]);
                }
                dt.Rows.Add(vals.ToArray());
                this.DataSource = dt;
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
    }
}
