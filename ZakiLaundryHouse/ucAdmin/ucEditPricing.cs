using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Globalization;

namespace ZakiLaundryHouse.ucAdmin
{
    public partial class ucEditPricing : UserControl
    {
        private dashboardAdmin mainForm;
        private Form popup;
        private string previousForm;

        public ucEditPricing(dashboardAdmin form, Form popupForm, string cameFrom,
                      string priceId, string itemName, string category,
                      string weight, string pricingType, string price)
        {
            InitializeComponent();
            mainForm = form;
            popup = popupForm;
            previousForm = cameFrom;
            LoadPricingDetails(priceId, itemName, category, weight, pricingType, price);
            WireEnterToSave();
            LoadDropdowns();
            tbxCategory.Text = tbxCategory.Text; // already loaded from LoadPricingDetails
            cbxPricingType.Text = cbxPricingType.Text;
            UpdateWeightTextboxState();
            tbxCategory.Text = category; // set selected category
            cbxPricingType.Text = pricingType; // set selected pricing type
            UpdateEditableControlsState();
        }


        public void LoadPricingDetails(string priceId, string itemName, string category, string weight, string pricingType, string price)
        {
            lblPriceId.Text = "Pricing #" + priceId;
            tbxItem.Text = itemName;
            tbxCategory.Text = category;
            tbxWeight.Text = weight;
            cbxPricingType.Text = pricingType;
            tbxPrice.Text = price;
        }

        private static decimal ParseDecimalFlexible(string input)
        {
            var s = (input ?? "").Trim();

            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out var v))
                return v;

            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out v))
                return v;

            s = s.Replace(',', '.');
            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out v))
                return v;

            return 0m;
        }

        private void WireEnterToSave()
        {
            void handler(object _, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    btnSave.PerformClick();
                }
            }

            tbxItem.KeyDown += handler;
            tbxCategory.KeyDown += handler;
            tbxWeight.KeyDown += handler;
            tbxItem.KeyDown += handler;
            tbxPrice.KeyDown += handler;

            if (this.Parent is Form f) f.AcceptButton = btnSave;
        }

        public event EventHandler PricingUpdated;


        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            if (this.Parent is Form parentForm)
            {
                parentForm.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.ValidateChildren();
                this.ActiveControl = null;

                string priceIdText = lblPriceId.Text.Replace("Pricing #", "").Trim();
                int priceId = int.Parse(priceIdText);

                string itemName = tbxItem.Text.Trim();
                decimal price = ParseDecimalFlexible(tbxPrice.Text);

                string weightInput = tbxWeight.Text;
                decimal? minWeight = null;
                decimal? maxWeight = null;

                if (!string.IsNullOrWhiteSpace(weightInput))
                {
                    string[] weightParts = weightInput.Split(new[] { '-', '–' }, StringSplitOptions.RemoveEmptyEntries);

                    if (weightParts.Length == 2)
                    {
                        minWeight = ParseDecimalFlexible(weightParts[0]);
                        maxWeight = ParseDecimalFlexible(weightParts[1].Replace("kg", "").Trim());
                    }
                    else
                    {
                        minWeight = ParseDecimalFlexible(weightInput.Replace("kg", "").Trim());
                        maxWeight = minWeight;
                    }
                }

                using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
                {
                    conn.Open();

                    // 1️⃣ Update Pricing table
                    string queryPricing = @"
                UPDATE Pricing
                SET MinWeight = @MinWeight,
                    MaxWeight = @MaxWeight,
                    Price = @Price
                WHERE PriceID = @PriceID";

                    using (SQLiteCommand cmd = new SQLiteCommand(queryPricing, conn))
                    {
                        cmd.Parameters.AddWithValue("@PriceID", priceId);
                        cmd.Parameters.AddWithValue("@MinWeight", minWeight.HasValue ? (object)minWeight.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaxWeight", maxWeight.HasValue ? (object)maxWeight.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.ExecuteNonQuery();
                    }

                    // 2️⃣ Get ItemID linked to this PriceID
                    int itemId = 0;
                    string getItemIdQuery = "SELECT ItemID FROM Pricing WHERE PriceID = @PriceID";
                    using (SQLiteCommand cmd = new SQLiteCommand(getItemIdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PriceID", priceId);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                            itemId = Convert.ToInt32(result);
                        else
                            throw new Exception("ItemID not found for this pricing.");
                    }

                    // 3️⃣ Update Items table
                    string queryItem = "UPDATE Items SET ItemName = @ItemName WHERE ItemID = @ItemID";
                    using (SQLiteCommand cmd = new SQLiteCommand(queryItem, conn))
                    {
                        cmd.Parameters.AddWithValue("@ItemID", itemId);
                        cmd.Parameters.AddWithValue("@ItemName", itemName);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Item name, weight, and price updated successfully!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PricingUpdated?.Invoke(this, EventArgs.Empty);

                if (this.Parent is Form parentForm)
                    parentForm.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDropdowns()
        {
            using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
            {
                conn.Open();

                string pricingTypeQuery = "SELECT DISTINCT PricingType FROM Pricing ORDER BY PricingType";
                using (SQLiteCommand cmd = new SQLiteCommand(pricingTypeQuery, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    cbxPricingType.Items.Clear();
                    while (reader.Read())
                    {
                        cbxPricingType.Items.Add(reader["PricingType"].ToString());
                    }
                }
            }
        }
        private void UpdateWeightTextboxState()
        {
            // Only enable weight if pricing type is "Minimum"
            if (cbxPricingType.Text.Equals("Per Minimum", StringComparison.OrdinalIgnoreCase))
            {
                tbxWeight.ReadOnly = false;
            }
            else
            {
                tbxWeight.ReadOnly = true;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void cbxPricingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWeightTextboxState();
            UpdateEditableControlsState();
        }

        private void UpdateEditableControlsState()
        {
            // Only editable for Full Service or Self Service
            if (tbxCategory.Text.Equals("Full Service", StringComparison.OrdinalIgnoreCase) ||
                tbxCategory.Text.Equals("Self Service", StringComparison.OrdinalIgnoreCase))
            {
                cbxPricingType.Enabled = true;

                // Weight is editable only if PricingType is Per Minimum
                tbxWeight.ReadOnly = !cbxPricingType.Text.Equals("Per Minimum", StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                cbxPricingType.Enabled = false;
                tbxWeight.ReadOnly = true;
            }
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEditableControlsState();

        }

        private void ucEditPricing_Load(object sender, EventArgs e)
        {

        }
    }
}
