using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Utility
    {
        public static Organization.CategoriesDataTable GetCategories()
        {
            OrganizationTableAdapters.CategoriesTableAdapter catAdapter = new OrganizationTableAdapters.CategoriesTableAdapter();
            Organization.CategoriesDataTable dtCatTable = new Organization.CategoriesDataTable();
            try
            {
                catAdapter.Fill(dtCatTable);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            return dtCatTable;
        }

        public static Organization.CompaniesDataTable GetCompanies()
        {
            OrganizationTableAdapters.CompaniesTableAdapter compAdapter = new OrganizationTableAdapters.CompaniesTableAdapter();
            Organization.CompaniesDataTable dtCompTable = new Organization.CompaniesDataTable();
            try
            {
                compAdapter.Fill(dtCompTable);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            return dtCompTable;
        }

        public static Organization.InventoryItemsDataTable GetInventory()
        {
            OrganizationTableAdapters.InventoryItemsTableAdapter invAdapter = new OrganizationTableAdapters.InventoryItemsTableAdapter();
            Organization.InventoryItemsDataTable dtInvTable = new Organization.InventoryItemsDataTable();
            try
            {
                invAdapter.Fill(dtInvTable);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            return dtInvTable;
        }

        public static Organization.InventoryItemsDataTable FilterInventory(string title, int category)
        {
            OrganizationTableAdapters.InventoryItemsTableAdapter invAdapter = new OrganizationTableAdapters.InventoryItemsTableAdapter();
            Organization.InventoryItemsDataTable dtInvTable = new Organization.InventoryItemsDataTable();
            if (category > 0)
            {
                dtInvTable.DefaultView.RowFilter = string.Format("ItemTitle LIKE '%{0}%' AND Category = {1}", title, category);
            }
            else
            {
                dtInvTable.DefaultView.RowFilter = string.Format("ItemTitle LIKE '%{0}%'", title);
            }
            invAdapter.Fill(dtInvTable);
            return dtInvTable;
        }

        public static void SaveInventoryItem(int qty, string title, int type, float price, string desc, int company, float length, float width, float height, float weight)
        {
            OrganizationTableAdapters.InventoryItemsTableAdapter itemAdapter = new OrganizationTableAdapters.InventoryItemsTableAdapter();
            Organization.InventoryItemsDataTable dtItemTable = new Organization.InventoryItemsDataTable();
            itemAdapter.Fill(dtItemTable);

            for (int i = 0; i < qty; i++)
            {
                Organization.InventoryItemsRow newRow = dtItemTable.NewInventoryItemsRow();
                newRow.ItemTitle = title;
                newRow.Category = type;
                newRow.Price = price;
                newRow.Description = desc;
                newRow.CompanyID = company;
                newRow.Length = length;
                newRow.Width = width;
                newRow.Height = height;
                newRow.Weight = weight;

                dtItemTable.AddInventoryItemsRow(newRow);
                itemAdapter.Update(dtItemTable);
            }
        }

        public static void SaveInventoryItem(int qty, string title, int type, float price, string desc, int company, DateTime pack, DateTime expiry, int grocery)
        {
            OrganizationTableAdapters.InventoryItemsTableAdapter itemAdapter = new OrganizationTableAdapters.InventoryItemsTableAdapter();
            Organization.InventoryItemsDataTable dtItemTable = new Organization.InventoryItemsDataTable();
            itemAdapter.Fill(dtItemTable);

            for (int i = 0; i < qty; i++)
            {
                Organization.InventoryItemsRow newRow = dtItemTable.NewInventoryItemsRow();
                newRow.ItemTitle = title;
                newRow.Category = type;
                newRow.Price = price;
                newRow.Description = desc;
                newRow.CompanyID = company;
                newRow.PackagingDate = pack;
                newRow.ExpirationDate = expiry;
                newRow.GroceryCategory = grocery;

                dtItemTable.AddInventoryItemsRow(newRow);
                itemAdapter.Update(dtItemTable);
            }
        }

        public static void SaveInventoryItem(int qty, string title, int type, float price, string desc, int company, string ISBN, string author, int genre)
        {
            OrganizationTableAdapters.InventoryItemsTableAdapter itemAdapter = new OrganizationTableAdapters.InventoryItemsTableAdapter();
            Organization.InventoryItemsDataTable dtItemTable = new Organization.InventoryItemsDataTable();
            itemAdapter.Fill(dtItemTable);

            for (int i = 0; i < qty; i++)
            {
                Organization.InventoryItemsRow newRow = dtItemTable.NewInventoryItemsRow();
                newRow.ItemTitle = title;
                newRow.Category = type;
                newRow.Price = price;
                newRow.Description = desc;
                newRow.CompanyID = company;
                newRow.ISBN = ISBN;
                newRow.Author = author;
                newRow.BookType = genre;

                dtItemTable.AddInventoryItemsRow(newRow);
                itemAdapter.Update(dtItemTable);
            }
        }

        public static void UpdateInventory(int invID, string title, int category, float price, string description)
        {
            OrganizationTableAdapters.InventoryItemsTableAdapter invAdapter = new OrganizationTableAdapters.InventoryItemsTableAdapter();
            Organization.InventoryItemsDataTable dtInvTable = new Organization.InventoryItemsDataTable();
            invAdapter.Fill(dtInvTable);

            foreach (Organization.InventoryItemsRow row in dtInvTable)
            {
                if (row.ID == invID)
                {
                    row.ItemTitle = title;
                    row.Category = category;
                    row.Price = price;
                    row.Description = description;
                }
            }

            invAdapter.Update(dtInvTable);
        }

        public static void DeleteInventory(int invID)
        {
            OrganizationTableAdapters.InventoryItemsTableAdapter invAdapter = new OrganizationTableAdapters.InventoryItemsTableAdapter();
            Organization.InventoryItemsDataTable dtInvTable = new Organization.InventoryItemsDataTable();
            invAdapter.Fill(dtInvTable);

            foreach (Organization.InventoryItemsRow row in dtInvTable)
            {
                if (row.ID == invID)
                {
                    row.Delete();
                }
            }

            invAdapter.Update(dtInvTable);
        }

        public static void UpdateCompany(int compID, string name, string number, string address)
        {
            Organization.CompaniesDataTable dtCompTable = new Organization.CompaniesDataTable();
            OrganizationTableAdapters.CompaniesTableAdapter compAdapter = new OrganizationTableAdapters.CompaniesTableAdapter();
            compAdapter.Fill(dtCompTable);

            foreach (Organization.CompaniesRow row in dtCompTable)
            {
                if (row.ID == compID)
                {
                    row.CompanyName = name;
                    row.ContactNumber = number;
                    row.Address = address;
                }
            }

            compAdapter.Update(dtCompTable);
        }

        public static void DeleteCompany(int compID)
        {
            Organization.CompaniesDataTable dtCompTable = new Organization.CompaniesDataTable();
            OrganizationTableAdapters.CompaniesTableAdapter compAdapter = new OrganizationTableAdapters.CompaniesTableAdapter();
            compAdapter.Fill(dtCompTable);

            foreach (Organization.CompaniesRow row in dtCompTable)
            {
                if (row.ID == compID)
                {
                    row.Delete();
                }
            }

            compAdapter.Update(dtCompTable);
        }
    }
}
