using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessLayer;

namespace InventoryEntryForm
{
    public partial class Companies : Form
    {
        public Companies()
        {
            InitializeComponent();

            #region DataGrid
            dgCompanies.DataSource = Utility.GetCompanies();
            dgCompanies.Columns["ID"].Visible = false;

            DataGridViewButtonColumn updateColumn = new DataGridViewButtonColumn();
            updateColumn.HeaderText = "Update Entry";
            updateColumn.Text = "Update";
            updateColumn.Name = "updateButton";
            updateColumn.UseColumnTextForButtonValue = true;
            dgCompanies.Columns.Add(updateColumn);

            DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
            deleteColumn.HeaderText = "Delete Entry";
            deleteColumn.Text = "Delete";
            deleteColumn.Name = "deleteButton";
            deleteColumn.UseColumnTextForButtonValue = true;
            dgCompanies.Columns.Add(deleteColumn);
            #endregion
        }

        private void dgCompanies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow currRow = dgCompanies.Rows[e.RowIndex];
            int compID = int.Parse(currRow.Cells["ID"].Value.ToString());

            if (dgCompanies.SelectedCells.Count == 1 && dgCompanies.SelectedCells[0] is DataGridViewButtonCell)
            {
                DataGridViewButtonCell selectedCell = (DataGridViewButtonCell)dgCompanies.SelectedCells[0];
                if (selectedCell.Value.Equals("Update"))
                {
                    string name = currRow.Cells["CompanyName"].Value.ToString();
                    string number = currRow.Cells["ContactNumber"].Value.ToString();
                    string address = currRow.Cells["Address"].Value.ToString();

                    Utility.UpdateCompany(compID, name, number, address);
                }
                else if (selectedCell.Value.Equals("Delete"))
                {
                    Utility.DeleteCompany(compID);
                }
                dgCompanies.DataSource = Utility.GetCompanies();
            }
        }
    }
}
