using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessLayer;

namespace InventoryEntryForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            #region Data Binding
            try
            {
                Organization.CategoriesDataTable dtCatTable = Utility.GetCategories();
                cmbType.DataSource = dtCatTable;
                cmbType.DisplayMember = dtCatTable.CategoryColumn.ColumnName;
                cmbType.ValueMember = dtCatTable.IDColumn.ColumnName;
            }
            catch (SqlException e)
            {
                MessageBox.Show("SQL error. Cannot get Categories data from server.\n" + e.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unhandled error. Contact your administrator.\n" + e.Message);
            }

            try
            {
                Organization.CategoriesDataTable dtCatTable = Utility.GetCategories();
                cmbSearchType.DataSource = dtCatTable;
                cmbSearchType.DisplayMember = dtCatTable.CategoryColumn.ColumnName;
                cmbSearchType.ValueMember = dtCatTable.IDColumn.ColumnName;
                cmbSearchType.SelectedIndex = -1;
                cmbSearchType.Text = "";
            }
            catch (SqlException e)
            {
                MessageBox.Show("SQL error. Cannot get Categories data from server.\n" + e.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unhandled error. Contact your administrator.\n" + e.Message);
            }

            try
            {
                Organization.CompaniesDataTable dtCompTable = Utility.GetCompanies();
                cmbCompany.DataSource = dtCompTable;
                cmbCompany.DisplayMember = dtCompTable.CompanyNameColumn.ColumnName;
                cmbCompany.ValueMember = dtCompTable.IDColumn.ColumnName;
            }
            catch (SqlException e)
            {
                MessageBox.Show("SQL error. Cannot get Companies data from server.\n" + e.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unhandled error. Contact your administrator.\n" + e.Message);
            }
            #endregion

            #region DataGrid
            try
            {
                dgInventory.DataSource = Utility.GetInventory();
                dgInventory.Columns["ID"].Visible = false;
                dgInventory.Columns["CompanyID"].Visible = false;
                dgInventory.Columns["Length"].Visible = false;
                dgInventory.Columns["Width"].Visible = false;
                dgInventory.Columns["Height"].Visible = false;
                dgInventory.Columns["Weight"].Visible = false;
                dgInventory.Columns["ISBN"].Visible = false;
                dgInventory.Columns["Author"].Visible = false;
                dgInventory.Columns["BookType"].Visible = false;
                dgInventory.Columns["PackagingDate"].Visible = false;
                dgInventory.Columns["ExpirationDate"].Visible = false;
                dgInventory.Columns["GroceryCategory"].Visible = false;

                DataGridViewButtonColumn updateColumn = new DataGridViewButtonColumn();
                updateColumn.HeaderText = "Update Entry";
                updateColumn.Text = "Update";
                updateColumn.Name = "updateButton";
                updateColumn.UseColumnTextForButtonValue = true;
                dgInventory.Columns.Add(updateColumn);

                DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
                deleteColumn.HeaderText = "Delete Entry";
                deleteColumn.Text = "Delete";
                deleteColumn.Name = "deleteButton";
                deleteColumn.UseColumnTextForButtonValue = true;
                dgInventory.Columns.Add(deleteColumn);
            }
            catch (SqlException e)
            {
                MessageBox.Show("SQL error. Cannot get Inventory data from server.\n" + e.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unhandled error. Contact your administrator.\n" + e.Message);
            }
            
            #endregion
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (cmbType.SelectedIndex < 0)
            {
                cmbType.BackColor = Color.Yellow;
                MessageBox.Show("Choose an item type");
                return;
            }
            if (gboxBook.Visible && cmbGenre.SelectedIndex < 0)
            {
                cmbGenre.BackColor = Color.Yellow;
                MessageBox.Show("Choose a genre");
                return;
            }
            if (gboxGrocery.Visible && cmbCategory.SelectedIndex < 0)
            {
                cmbCategory.BackColor = Color.Yellow;
                MessageBox.Show("Choose a category");
                return;
            }
            if (gboxGrocery.Visible && dateExpiry.Value <= datePack.Value)
            {
                errDate.SetError(dateExpiry, "Expiration needs to be after Packaging");
                return;
            }
            if (string.IsNullOrEmpty(txtDesc.Text))
            {
                errDesc.SetError(txtDesc, "You should enter a description");
                return;
            }
            else
            {
                errDesc.SetError(txtDesc, string.Empty);
                errDate.SetError(dateExpiry, string.Empty);
            }

            if (dgInventory.SelectedRows.Count == 1)
            {
                try
                {
                    Utility.UpdateInventory(int.Parse(dgInventory.SelectedRows[0].Cells["ID"].Value.ToString()), txtItem.Text, int.Parse(cmbType.SelectedValue.ToString()), float.Parse(txtPrice.Text), txtDesc.Text);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("SQL error. Cannot get Inventory data from server.\n" + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unhandled error. Contact your administrator.\n" + ex.Message);
                }
            }
            else
            {
                //string itemInfo = "Item: " + txtItem.Text + "\r\nDescription: " + txtDesc.Text + "\r\nPrice: $" + txtPrice.Text;
                switch (cmbType.SelectedIndex)
                {
                    case 0:
                        //itemInfo += "\r\nAuthor: " + txtAuthor.Text + "\r\nGenre: " + cmbGenre.SelectedItem + "\r\nISBN: " + txtISBN.Text;
                        try
                        {
                            Utility.SaveInventoryItem(int.Parse(txtQty.Text), txtItem.Text, int.Parse(cmbType.SelectedValue.ToString()), float.Parse(txtPrice.Text), txtDesc.Text, int.Parse(cmbCompany.SelectedValue.ToString()), txtISBN.Text, txtAuthor.Text, cmbGenre.SelectedIndex);
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("SQL error. Cannot get Inventory data from server.\n" + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unhandled error. Contact your administrator.\n" + ex.Message);
                        }
                        break;
                    case 1:
                        //itemInfo += "\r\nDimensions: " + txtLength.Text + "L x " + txtHeight.Text + "H x " + txtWidth.Text + "W\r\nWeight: " + txtWeight.Text;
                        try
                        {
                            Utility.SaveInventoryItem(int.Parse(txtQty.Text), txtItem.Text, int.Parse(cmbType.SelectedValue.ToString()), float.Parse(txtPrice.Text), txtDesc.Text, int.Parse(cmbCompany.SelectedValue.ToString()), float.Parse(txtLength.Text), float.Parse(txtWidth.Text), float.Parse(txtHeight.Text), float.Parse(txtWeight.Text));
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("SQL error. Cannot get Inventory data from server.\n" + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unhandled error. Contact your administrator.\n" + ex.Message);
                        }
                        break;
                    case 2:
                        //itemInfo += "\r\nCategory: " + cmbCategory.SelectedItem + "\r\nPack Date: " + datePack.Value.ToString("d") + "\r\nExpiry Date: " + dateExpiry.Value.ToString("d");
                        try
                        {
                            Utility.SaveInventoryItem(int.Parse(txtQty.Text), txtItem.Text, int.Parse(cmbType.SelectedValue.ToString()), float.Parse(txtPrice.Text), txtDesc.Text, int.Parse(cmbCompany.SelectedValue.ToString()), datePack.Value, dateExpiry.Value, cmbCategory.SelectedIndex);
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("SQL error. Cannot get Inventory data from server.\n" + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unhandled error. Contact your administrator.\n" + ex.Message);
                        }
                        break;
                }
                //txtInfo.Text = itemInfo;
            }
            try
            {
                dgInventory.DataSource = Utility.GetInventory();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL error. Cannot get Inventory data from server.\n" + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unhandled error. Contact your administrator.\n" + ex.Message);
            }
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbType.BackColor = default;
            switch (cmbType.SelectedIndex)
            {
                case 0:
                    gboxFurniture.Visible = false;
                    gboxGrocery.Visible = false;
                    gboxBook.Visible = true;
                    break;
                case 1:
                    gboxBook.Visible = false;
                    gboxGrocery.Visible = false;
                    gboxFurniture.Visible = true;
                    break;
                case 2:
                    gboxBook.Visible = false;
                    gboxGrocery.Visible = true;
                    gboxFurniture.Visible = false;
                    break;
                default:
                    gboxGrocery.Visible = false;
                    gboxFurniture.Visible = false;
                    gboxBook.Visible = false;
                    break;
            }
        }

        private void cmbGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbGenre.BackColor = default;
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbCategory.BackColor = default;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                tb.Text = string.Empty;
            }
            foreach (TextBox tb in this.gboxFurniture.Controls.OfType<TextBox>())
            {
                tb.Text = string.Empty;
            }
            foreach (TextBox tb in this.gboxBook.Controls.OfType<TextBox>())
            {
                tb.Text = string.Empty;
            }
            cmbType.SelectedIndex = -1;
            cmbType.Text = "(Pick one)";
            cmbCategory.SelectedIndex = -1;
            cmbCategory.Text = "(Pick one)";
            cmbGenre.SelectedIndex = -1;
            cmbGenre.Text = "(Pick one)";
            datePack.Value = DateTime.Today;
            dateExpiry.Value = DateTime.Today;
            cmbCompany.Text = "(Pick one)";
            txtQty.Text = "1";
        }

        private void dgInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow currRow = dgInventory.Rows[e.RowIndex];
            int invID = int.Parse(currRow.Cells["ID"].Value.ToString());

            if (e.ColumnIndex == -1)
            {
                txtItem.Text = currRow.Cells["ItemTitle"].Value.ToString();
                cmbType.SelectedValue = currRow.Cells["Category"].Value;
                txtDesc.Text = currRow.Cells["Description"].Value.ToString();
                txtPrice.Text = currRow.Cells["Price"].Value.ToString();
                cmbCompany.SelectedValue = currRow.Cells["CompanyID"].Value;
                txtAuthor.Text = string.IsNullOrEmpty(currRow.Cells["Author"].Value.ToString()) ? string.Empty : currRow.Cells["Author"].Value.ToString();
                //cmbGenre.SelectedValue = currRow.Cells["BookType"].Value;
                cmbGenre.SelectedIndex = string.IsNullOrEmpty(currRow.Cells["BookType"].Value.ToString()) ? -1 : int.Parse(currRow.Cells["BookType"].Value.ToString());
                txtISBN.Text = string.IsNullOrEmpty(currRow.Cells["ISBN"].Value.ToString()) ? string.Empty : currRow.Cells["ISBN"].Value.ToString();
                txtLength.Text = string.IsNullOrEmpty(currRow.Cells["Length"].Value.ToString()) ? string.Empty : currRow.Cells["Length"].Value.ToString();
                txtWidth.Text = string.IsNullOrEmpty(currRow.Cells["Width"].Value.ToString()) ? string.Empty : currRow.Cells["Width"].Value.ToString();
                txtHeight.Text = string.IsNullOrEmpty(currRow.Cells["Height"].Value.ToString()) ? string.Empty : currRow.Cells["Height"].Value.ToString();
                txtWeight.Text = string.IsNullOrEmpty(currRow.Cells["Weight"].Value.ToString()) ? string.Empty : currRow.Cells["Weight"].Value.ToString();
                //cmbCategory.SelectedValue = currRow.Cells["GroceryCategory"].Value;
                cmbCategory.SelectedIndex = string.IsNullOrEmpty(currRow.Cells["GroceryCategory"].Value.ToString()) ? -1 : int.Parse(currRow.Cells["GroceryCategory"].Value.ToString());
                datePack.Value = string.IsNullOrEmpty(currRow.Cells["PackagingDate"].Value.ToString()) ? DateTime.Today : (DateTime)currRow.Cells["PackagingDate"].Value;
                dateExpiry.Value = string.IsNullOrEmpty(currRow.Cells["ExpirationDate"].Value.ToString()) ? DateTime.Today : (DateTime)currRow.Cells["ExpirationDate"].Value;
            }

            if (dgInventory.SelectedCells.Count == 1 && dgInventory.SelectedCells[0] is DataGridViewButtonCell)
            {
                DataGridViewButtonCell selectedCell = (DataGridViewButtonCell)dgInventory.SelectedCells[0];
                if (selectedCell.Value.Equals("Update"))
                {
                    string title = currRow.Cells["ItemTitle"].Value.ToString();
                    int category = int.Parse(currRow.Cells["Category"].Value.ToString());
                    float price = float.Parse(currRow.Cells["Price"].Value.ToString());
                    string description = currRow.Cells["Description"].Value.ToString();

                    try
                    {
                        Utility.UpdateInventory(invID, title, category, price, description);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("SQL error. Cannot get Inventory data from server.\n" + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unhandled error. Contact your administrator.\n" + ex.Message);
                    }
                }
                else if (selectedCell.Value.Equals("Delete"))
                {
                    try
                    {
                        Utility.DeleteInventory(invID);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("SQL error. Cannot get Inventory data from server.\n" + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unhandled error. Contact your administrator.\n" + ex.Message);
                    }
                }
                try
                {
                    dgInventory.DataSource = Utility.GetInventory();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("SQL error. Cannot get Inventory data from server.\n" + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unhandled error. Contact your administrator.\n" + ex.Message);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgInventory.DataSource = Utility.FilterInventory(txtSearchItem.Text, cmbSearchType.SelectedIndex + 1);
        }

        private void btnSearchClear_Click(object sender, EventArgs e)
        {
            txtSearchItem.Text = string.Empty;
            cmbSearchType.SelectedIndex = -1;
            cmbSearchType.Text = "";
        }
    }
}
