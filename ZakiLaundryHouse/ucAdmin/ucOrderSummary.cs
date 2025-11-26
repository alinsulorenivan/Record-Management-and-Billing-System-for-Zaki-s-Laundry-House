using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QRCoder;
using System.Drawing.Printing;
using System.Collections.Generic;// for printing
using System.Text.RegularExpressions;
using System.Text;



namespace ZakiLaundryHouse.ucAdmin
{

    public partial class ucOrderSummary : UserControl
    {

        private int pageStartY = 0;
        private PictureBox pbLogo;
        private PictureBox pbQRCode;
        private Label lblQRCodeNote;
        private PrintDocument printDocument; //for printing
        Bitmap controlBitmap;// not used for printing now but kept for compatibility
        private string[] WordWrapMonospace(string text, int maxCharsPerLine = 13)
        {
            List<string> lines = new List<string>();
            StringBuilder currentLine = new StringBuilder();
            string[] words = text.Split(' ');  // Split the text by spaces to process each word

            foreach (var word in words)
            {
                // If the word contains parentheses, handle them carefully
                if (word.Contains("(") || word.Contains(")"))
                {
                    // If there's a space before parentheses, remove it
                    if (currentLine.Length > 0 && currentLine[currentLine.Length - 1] == ' ')
                    {
                        currentLine.Length--;  // Remove the trailing space
                    }
                }

                // Check if adding the word fits in the line
                if (currentLine.Length + word.Length + (currentLine.Length > 0 ? 1 : 0) <= maxCharsPerLine)
                {
                    if (currentLine.Length > 0)
                        currentLine.Append(" ");  // Add space between words if it's not the first word

                    currentLine.Append(word);
                }
                else
                {
                    // If the word doesn't fit, push the current line and start a new one
                    lines.Add(currentLine.ToString());
                    currentLine.Clear();
                    currentLine.Append(word);
                }
            }

            // Add the last line if there's any leftover text
            if (currentLine.Length > 0)
            {
                lines.Add(currentLine.ToString());
            }

            return lines.ToArray();
        }

        public string InvoiceNo { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public decimal Cash { get; set; }
        public decimal Change { get; set; }
        public int LoggedInUserId { get; set; }
        public string CashierName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public string ReferenceNumber { get; set; }
        public string PaymentMethod { get; set; }




        // Declare controls
        private Label lblBusinessName, lblAddress, lblContact;
        private Label lblOrderNo, lblCashier, lblDateTime;
        private Label lblCustomerName, lblCustomerContact;

        private Panel pnlItems;

        private Label lblSubtotal, lblTotal, lblCash, lblChange;
        private Label lblFooter;

        // Receipt printing constants
        private const int Dpi = 203; // target printer DPI
        private const float MmPerInch = 25.4f;
        private const int PrintableWidthMm = 48; // 48 mm printable area inside 58mm roll
        private const int PaperWidthMm = 58; // 58 mm roll width
        private readonly int printableWidthPx;
        private readonly int paperWidthPx;

        // Flattened draw list for printing & pagination
        private class DrawItem
        {
            public enum ItemType { Label, Image }
            public ItemType Type;
            public int X;
            public int Y; // absolute Y relative to top of control
            public int Width;
            public int Height;
            public string Text;
            public Font Font;
            public ContentAlignment TextAlign;
            public Image Image;
            public Color ForeColor;
        }
        private List<DrawItem> drawItems = new List<DrawItem>();
        /* private int pageStartY = 0; */// pagination offset between PrintPage calls

        public ucOrderSummary()
        {
            InitializeComponent();


            // Calculate pixel widths from mm at target DPI
            printableWidthPx = (int)Math.Round(PrintableWidthMm / MmPerInch * Dpi); // ~384 px
            paperWidthPx = (int)Math.Round(PaperWidthMm / MmPerInch * Dpi); // ~464 px

            BuildLayout();
            this.Load += ucOrderSummary_Load;
        }

        private void ucOrderSummary_Load(object sender, EventArgs e)
        {
            // ===== HEADER =====
            lblBusinessName.Text = "Zaki's Laundry House";
            lblAddress.Text = "BCN Construction Compound, Sitio Aratan, Brgy\n" +
                "Pulong Sta. Cruz | Blk 5 Lot 10 Ph1 Berkeley\n" +
                "Heights Subd, Pulong Sta. Cruz, \n" +
                "Sta. Rosa, Laguna";
            lblContact.Text = "+63 917 124 6364   |   Zakis.laundryhouse@gmail.com ";

            // ===== ORDER INFO =====
            lblOrderNo.Text = "Invoice No: " + InvoiceNo;
            lblDateTime.Text = "Date: " + DateTime.Now.ToString("MMM dd, yyyy hh:mm tt");

            // ✅ Get cashier name based on logged-in user ID
            // ✅ Get cashier name based on the most recent logged-in user from LogTrail
            string cashierName = "";
            string connStr = DbConnection.ConnectionString;

            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                string query = @"
       SELECT U.Name
            FROM LogTrail L
            INNER JOIN Users U ON L.UserID = U.UserID
            WHERE L.ActionType = 'Login'
            ORDER BY L.DateTime DESC
            LIMIT 1";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        cashierName = result.ToString();
                    }
                }
            }

            // Show cashier name
            lblCashier.Text = "Cashier: " + (string.IsNullOrEmpty(cashierName) ? "N/A" : cashierName);


            // ✅ Get Customer Name & Contact Number based on InvoiceNo
            string customerName = "";
            string customerContact = "";

            using (SQLiteConnection conn = new SQLiteConnection(DbConnection.ConnectionString))
            {
                conn.Open();
                string query = @"
           SELECT C.CustomerName, C.ContactNumber
            FROM Orders O
            INNER JOIN Customers C ON O.CustomerID = C.CustomerID
            WHERE O.InvoiceNo = @InvoiceNo";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customerName = reader["CustomerName"].ToString();
                            customerContact = reader["ContactNumber"].ToString();
                        }
                    }
                }
            }

            // Show customer info
            lblCustomerName.Text = "Customer Name: " + (string.IsNullOrEmpty(customerName) ? "N/A" : customerName);
            lblCustomerContact.Text = "Contact No: " + (string.IsNullOrEmpty(customerContact) ? "N/A" : customerContact);

            // ===== ITEMS =====
            LoadOrderItems();

            // ===== QR CODE =====
            string fbPageUrl = "https://web.facebook.com/profile.php?id=61562217321879";
            GenerateQRCode(fbPageUrl);

            // ===== FOOTER =====
            lblFooter.Text = "This is not an official receipt.";
        }


        private void GenerateQRCode(string qrText)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    Bitmap qrCodeImage = qrCode.GetGraphic(4);
                    pbQRCode.Image = qrCodeImage;
                }
            }
        }

        private void LoadOrderItems()
        {
            string connStr = DbConnection.ConnectionString;
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();
                string remarks = "";

                // ===== Fetch order remarks =====
                using (SQLiteCommand cmdRemarks = new SQLiteCommand(
                    "SELECT Remarks FROM Orders WHERE InvoiceNo = @InvoiceNo", conn))
                {
                    cmdRemarks.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
                    object result = cmdRemarks.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        remarks = result.ToString();
                }

                // ===== Fetch order items =====
               string query = @"
    SELECT 
        I.ItemID,
        I.Category, 
        I.ItemName, 
        OD.Quantity, 
        OD.Weight, 
        OD.Price, 
        OD.Subtotal AS Amount
    FROM OrderDetails OD
    INNER JOIN Items I ON OD.ItemID = I.ItemID
    INNER JOIN Orders O ON OD.OrderID = O.OrderID
    WHERE O.InvoiceNo = @InvoiceNo";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        pnlItems.Controls.Clear();
                        int y = 0;

                        // ===== Header Lines =====
                        Label lblLineTop = MakeDashedLine();
                        lblLineTop.Location = new Point(10, y);
                        pnlItems.Controls.Add(lblLineTop);
                        y += lblLineTop.Height + 5;

                        Label lblHeader = new Label()
                        {
                            AutoSize = false,
                            Width = pnlItems.Width - 20,
                            Font = new Font("Consolas", 10, FontStyle.Bold),
                            Text = string.Format("{0,-13}{1,5}{2,6}{3,9}{4,9}",
    "Category", "Qty", "Wgt", "Price", "Amount"),

                            Location = new Point(10, y),
                            Height = 25,
                            TextAlign = ContentAlignment.MiddleLeft
                        };

                        pnlItems.Controls.Add(lblHeader);
                        y += lblHeader.Height + 3;

                        Label lblLineUnderHeader = MakeDashedLine();
                        lblLineUnderHeader.Location = new Point(10, y);
                        pnlItems.Controls.Add(lblLineUnderHeader);
                        y += lblLineUnderHeader.Height + 5;

                        // ===== Group items by category =====
                        var categories = new Dictionary<string, List<string>>();

                        while (reader.Read())
                        {
                            string category = reader["Category"].ToString();
                            string itemName = reader["ItemName"].ToString();
                            int qty = Convert.ToInt32(reader["Quantity"]);
                            decimal weight = reader["Weight"] != DBNull.Value ? Convert.ToDecimal(reader["Weight"]) : 0;
                            decimal price = Convert.ToDecimal(reader["Price"]);

                            string[] noWeightCategories = { "bleach", "detergent", "fabcon", "other" };
                            bool hideWeight = noWeightCategories.Contains(category.ToLower());

                            decimal total = 0;
                            decimal amount = 0;
                            string pricingType = "";
                            decimal minWeight = 0, maxWeight = 9999;

                            // ===== Split long item names into lines =====
                            string fullItemName = itemName.Trim();
                            int nameCharLimit = 14;
                            List<string> nameLines = new List<string>();
                            for (int i = 0; i < fullItemName.Length; i += nameCharLimit)
                            {
                                int len = Math.Min(nameCharLimit, fullItemName.Length - i);
                                nameLines.Add(fullItemName.Substring(i, len));
                            }
                            if (nameLines.Count > 0)
                            {
                                nameLines[0] = nameLines[0].PadRight(nameCharLimit);
                            }

                            // ===== Fetch pricing for this item =====
                            int itemId = Convert.ToInt32(reader["ItemID"]);
                            using (SQLiteConnection conn2 = new SQLiteConnection(DbConnection.ConnectionString))
                            {
                                conn2.Open();
                                string priceQuery = @"
                                SELECT Price, PricingType, MinWeight, MaxWeight
                                FROM Pricing
                                WHERE ItemID = @ItemID
                                ORDER BY
                                    CASE 
                                        WHEN PricingType = 'Per KG' THEN 1
                                        WHEN PricingType = 'Per Load' THEN 2
                                        WHEN PricingType = 'Per Minimum' THEN 3
                                        WHEN PricingType = 'Fixed' THEN 4
                                        ELSE 5
                                    END
                                LIMIT 1";

                                using (SQLiteCommand cmd2 = new SQLiteCommand(priceQuery, conn2))
                                {
                                    cmd2.Parameters.AddWithValue("@ItemID", itemId);
                                    using (SQLiteDataReader priceReader = cmd2.ExecuteReader())
                                    {
                                        if (priceReader.Read())
                                        {
                                            price = Convert.ToDecimal(priceReader["Price"]);
                                            pricingType = priceReader["PricingType"].ToString();
                                            if (priceReader["MinWeight"] != DBNull.Value)
                                                minWeight = Convert.ToDecimal(priceReader["MinWeight"]);
                                            if (priceReader["MaxWeight"] != DBNull.Value)
                                                maxWeight = Convert.ToDecimal(priceReader["MaxWeight"]);
                                        }
                                    }
                                }
                            }

                            // ===== Calculate subtotal based on pricing type =====
                            switch (pricingType)
                            {
                                case "Per KG":
                                    total = (price * weight) * qty;
                                    break;
                                case "Per Load":
                                    total = price * qty;
                                    break;
                                case "Per Minimum":
                                    total = price * qty;
                                    break;
                                case "Fixed":
                                    total = price * qty;
                                    break;
                                default:
                                    total = price * qty;
                                    break;
                            }

                            amount = total;

                            // ===== Build printable item line =====
                            string itemLine;
                            if (hideWeight)
                            {
                                itemLine = string.Format("{0,-13}{1,5}{2,10:0.00}{3,10:0.00}",
             nameLines[0], qty, price, amount);
                            }
                            else
                            {
                                string weightStr = weight > 0 ? weight.ToString("0.00") + "kg" : "";
                                itemLine = string.Format("{0,-13}{1,5}{2,10}{3,9:0.00}{4,10:0.00}",
                                    nameLines[0], qty, weightStr, price, amount);
                            }

                            // ===== Add to category dictionary =====
                            if (!categories.ContainsKey(category))
                            {
                                categories[category] = new List<string>();
                            }
                            categories[category].Add(itemLine);

                            // Add wrapped lines if name too long
                            for (int i = 1; i < nameLines.Count; i++)
                            {
                                categories[category].Add(nameLines[i]);
                            }
                        }

                        // ===== Display categories & items =====
                        foreach (var category in categories)
                        {
                            Label lblCategory = new Label()
                            {
                                AutoSize = false,
                                Width = pnlItems.Width - 6,
                                Font = new Font("Consolas", 9, FontStyle.Bold),
                                Text = category.Key,
                                Location = new Point(10, y),
                                Height = 25,
                                TextAlign = ContentAlignment.MiddleLeft
                            };
                            pnlItems.Controls.Add(lblCategory);
                            y += lblCategory.Height + 5;

                            int indent = 0;
                            foreach (var itemLine in category.Value)
                            {
                                Label lblItem = new Label()
                                {
                                    AutoSize = true,
                                    Font = new Font("Consolas", 9),
                                    Location = new Point(10 + indent, y),
                                    Text = itemLine
                                };
                                pnlItems.Controls.Add(lblItem);
                                y += lblItem.Height + 3;
                            }
                        }

                        // ===== Dashed line bottom =====
                        Label lblLineBottom = MakeDashedLine();
                        lblLineBottom.Location = new Point(lblLineTop.Location.X, y);
                        lblLineBottom.Width = lblLineTop.Width;             // ✅ same width
                        lblLineBottom.Font = lblLineTop.Font;               // ✅ same font (dash spacing)
                        lblLineBottom.Text = lblLineTop.Text;               // ✅ same number of dashes
                        pnlItems.Controls.Add(lblLineBottom);
                        y += lblLineBottom.Height + 8;

                        // ===== Remarks =====
                        Label lblRemarks = new Label()
                        {
                            AutoSize = true,
                            MaximumSize = new Size(pnlItems.Width / 2, 0),
                            Font = new Font("Consolas", 10, FontStyle.Regular),
                            TextAlign = ContentAlignment.MiddleLeft,
                            Text = "Remarks: " + remarks,
                            Location = new Point(10, y)
                        };
                        pnlItems.Controls.Add(lblRemarks);
                        y += lblRemarks.Height + 5;
                        // ===== Summary =====
                        int labelWidth = pnlItems.Width - 20;
                        int xCenter = 10;

                        Label MakeSummaryLabel(string text, bool isBold = false, float fontSize = 8.5f)
                        {
                            return new Label()
                            {
                                Width = labelWidth,
                                Height = 22,
                                AutoSize = false,
                                Font = new Font("Consolas", fontSize, isBold ? FontStyle.Bold : FontStyle.Regular),
                                TextAlign = ContentAlignment.MiddleRight,
                                Text = text,
                                Location = new Point(xCenter, y)
                            };
                        }

                        decimal displayChange = Change < 0 ? 0.00m : Change;

                        // TOTAL
                        pnlItems.Controls.Add(MakeSummaryLabel(FormatSummaryLine("Total:", Total), isBold: true, fontSize: 11f));
                        y += 24;

                        // ✅ PAYMENT METHOD (Cash / Gcash / Maya / Others)
                        pnlItems.Controls.Add(MakeSummaryLabel(FormatSummaryLine($"{PaymentMethod}:", Cash)));
                        y += 24;

                        // CHANGE
                        pnlItems.Controls.Add(MakeSummaryLabel(FormatSummaryLine("Change:", displayChange)));
                        y += 24;

                        // ✅ REFERENCE NUMBER (only show if not empty)
                        if (!string.IsNullOrEmpty(ReferenceNumber))
                        {
                            pnlItems.Controls.Add(MakeSummaryLabel($"Ref No: {ReferenceNumber}", false, 9f));
                            y += 24;
                        }




                        // ===== QR Code & Footer =====
                        pbQRCode.Top = y + 10;
                        lblQRCodeNote.Top = y + 10;
                        lblQRCodeNote.Left = 0;
                        lblQRCodeNote.Width = pnlItems.Width;
                        lblQRCodeNote.TextAlign = ContentAlignment.MiddleCenter;
                        lblQRCodeNote.Text = "For concerns / feedback, please scan QR below\n" +
                                             "to message our FB page. Thank you!";
                        pnlItems.Controls.Add(lblQRCodeNote);
                        lblQRCodeNote.BringToFront();
                        y = lblQRCodeNote.Bottom + 5;

                        if (pbQRCode.Image == null)
                        {
                            GenerateQRCode("https://web.facebook.com/profile.php?id=61562217321879");
                        }

                        pbQRCode.Top = y;
                        pbQRCode.Left = (pnlItems.Width - pbQRCode.Width) / 2;
                        pbQRCode.SizeMode = PictureBoxSizeMode.StretchImage;
                        pnlItems.Controls.Add(pbQRCode);
                        pbQRCode.BringToFront();
                        y = pbQRCode.Bottom + 5;

                        lblFooter.Top = y;
                        lblFooter.Left = 0;
                        lblFooter.Width = pnlItems.Width;
                        lblFooter.TextAlign = ContentAlignment.MiddleCenter;
                        lblFooter.Text = "This is not an official receipt.";
                        pnlItems.Controls.Add(lblFooter);
                        lblFooter.BringToFront();
                        y = lblFooter.Bottom + 20;

                        // ✅ Add Complaint & Disposal Notice (following your layout)
                        Label lblNotice = new Label();
                        lblNotice.Top = y;
                        lblNotice.Left = 0;
                        lblNotice.Width = pnlItems.Width;
                        lblNotice.Height = 80;
                        lblNotice.TextAlign = ContentAlignment.MiddleCenter;
                        lblNotice.Font = new Font("Segoe UI", 8, FontStyle.Regular);
                        lblNotice.Text = "Complaints will only be entertained within 24 hours from the\n" +
                                            "date of release.\n" +
                                            "Our shop reserves the right to dispose (donate/discard) items\n" +
                                            "if unclaimed after 30 calendar days.\n" +
                                            "Our shop will not be liable for any loss or damage once the\n" +
                                            "items have been disposed or donated.";
                        pnlItems.Controls.Add(lblNotice);
                        lblNotice.BringToFront();
                        y = lblNotice.Bottom + 10;



                        pnlItems.Height = y;
                        this.Height = pnlItems.Bottom + 10;
                        pnlItems.Left = (this.Width - pnlItems.Width) / 2;
                        this.Refresh();
                    }
                }
            }
        }


        private string FormatSummaryLine(string label, decimal value)
        {
            // Right-align label (12 characters) and value (8 characters with 2 decimals)
            return string.Format("{0,12} ₱{1,10:N2}", label, value);

        }

        private Label MakeDashedLine()
        {
            return new Label()
            {
                AutoSize = false,
                Width = pnlItems.Width - 20,
                Height = 15,
                Font = new Font("Consolas", 9, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Text = new string('-', 64)
            };
        }

        private void BuildLayout()
        {
            this.BackColor = Color.White;
            this.Width = 384;
            this.AutoScroll = false;
            this.Dock = DockStyle.Top;

            int currentTop = 0;

            // ✅ 1. Logo
            //pbLogo = new PictureBox()
            //{
            //    Size = new Size(85, 85),
            //    SizeMode = PictureBoxSizeMode.Zoom,
            //    Image = Properties.Resources.zakilogo,
            //    Left = (this.Width - 85) / 2,
            //    Top = currentTop
            //};
            //this.Controls.Add(pbLogo);
            //currentTop += pbLogo.Height + 5;

            // ✅ 2. Business Name
            lblBusinessName = new Label()
            {
                Text = "Zaki's Laundry House",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Width = 384,
                Height = 40,
                Top = currentTop,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White
            };
            this.Controls.Add(lblBusinessName);
            currentTop += lblBusinessName.Height;

            // ✅ 3. Address
            lblAddress = new Label()
            {
                Text =
                "BCN Construction Compound, Sitio Aratan, Brgy\n" +
                "Pulong Sta. Cruz | Blk 5 Lot 10 Ph1 Berkeley\n" +
                "Heights Subd, Pulong Sta. Cruz,\n" +
                "Sta. Rosa, Laguna",
                Font = new Font("Segoe UI", 9),
                Width = 384,
                Height = 70,
                Top = currentTop,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White
            };
            this.Controls.Add(lblAddress);
            currentTop += lblAddress.Height;

            // ✅ 4. Contact
            lblContact = new Label()
            {
                Text = "+63 917 124 6364 | Zakis.laundryhouse@gmail.com ",
                Font = new Font("Segoe UI", 8),
                Width = 384,
                Height = 20,
                Top = currentTop,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White
            };
            this.Controls.Add(lblContact);
            currentTop += lblContact.Height + 10;

            // ✅ 5. Order Info
            lblOrderNo = new Label()
            {
                Width = 384,
                Height = 20,
                Top = currentTop,
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lblOrderNo);
            currentTop += lblOrderNo.Height;

            lblDateTime = new Label()
            {
                Width = 384,
                Height = 20,
                Top = currentTop,
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lblDateTime);
            currentTop += lblDateTime.Height;

            lblCashier = new Label()
            {
                Width = 384,
                Height = 20,
                Top = currentTop,
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lblCashier);
            currentTop += lblCashier.Height;

            lblCustomerName = new Label()
            {
                Width = 384,
                Height = 20,
                Top = currentTop,
                TextAlign = ContentAlignment.MiddleLeft,

            };
            this.Controls.Add(lblCustomerName);
            currentTop += lblCustomerName.Height;

            // ✅ Make sure it inherits the same font and rendering


            lblCustomerContact = new Label()
            {
                Width = 384,
                Height = 20,
                Top = currentTop,
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lblCustomerContact);
            currentTop += lblCustomerContact.Height + 5;

            // ✅ 6. Items Panel
            pnlItems = new Panel()
            {
                Width = 384,
                AutoSize = true,
                BackColor = Color.White,
                Top = currentTop,
                Left = 0
            };
            this.Controls.Add(pnlItems);
            currentTop += pnlItems.Height + 10;

            // ✅ 7. QR Code
            pbQRCode = new PictureBox()
            {
                Size = new Size(100, 100),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                Left = (384 - 100) / 2,
                Top = currentTop
            };
            this.Controls.Add(pbQRCode);
            currentTop += pbQRCode.Height + 5;

            // ✅ 8. QR Note
            lblQRCodeNote = new Label()
            {
                Text = "For concerns / feedback, please scan QR below\n" +
                "to message our FB page. Thank you!",
                Height = 44,
                Width = 384,
                Top = currentTop,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9, FontStyle.Italic)
            };
            this.Controls.Add(lblQRCodeNote);
            currentTop += lblQRCodeNote.Height + 5;

            // ✅ 9. Footer
            lblFooter = new Label()
            {
                Width = 384,
                Top = currentTop,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "This is not an official receipt."
            };
            this.Controls.Add(lblFooter);

            // ✅ Auto-center the top labels dynamically
            this.Layout += (s, e) =>
            {
                int centerX = (this.ClientSize.Width - lblBusinessName.Width) / 2;
                lblBusinessName.Left = centerX;
                lblAddress.Left = centerX;
                lblContact.Left = centerX;
            };
            // ✅ Center the entire layout inside the parent form
            this.Anchor = AnchorStyles.None;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            this.ParentChanged += (s, e) =>
            {
                if (this.Parent != null)
                {
                    this.Parent.Resize += (s2, e2) =>
                    {
                        this.Left = (this.Parent.ClientSize.Width - this.Width) / 2;
                        this.Top = (this.Parent.ClientSize.Height - this.Height) / 2;
                    };
                }
            };

        }


        /// <summary>
        /// Build flattened draw list from current controls (keeps original positions)
        /// </summary>
        private void BuildDrawItems()
        {
            drawItems.Clear();


            // Add top-level controls in the order they appear in Controls collection
            foreach (Control c in this.Controls)
            {
                if (!c.Visible) continue;

                if (c is Panel pnl)
                {
                    foreach (Control child in pnl.Controls)
                    {
                        if (!child.Visible) continue;

                        if (child is Label lbl)
                        {
                            // ✅ Detect dashed-line labels
                            bool isDashedLine = lbl.Text.Trim().StartsWith("-");

                            drawItems.Add(new DrawItem
                            {
                                Type = DrawItem.ItemType.Label,
                                X = pnl.Left + child.Left,
                                Y = pnl.Top + child.Top,
                                Width = child.Width,
                                Height = child.Height,
                                // ✅ Store marker text instead of raw dashed text
                                Text = isDashedLine ? "DASHED_LINE" : lbl.Text,
                                Font = lbl.Font,
                                TextAlign = lbl.TextAlign,
                                ForeColor = lbl.ForeColor
                            });

                            continue;
                        }
                        else if (child is PictureBox childPb && childPb.Image != null)
                        {
                            drawItems.Add(new DrawItem
                            {
                                Type = DrawItem.ItemType.Image,
                                X = pnl.Left + childPb.Left,
                                Y = pnl.Top + childPb.Top,
                                Width = childPb.Width,
                                Height = childPb.Height,
                                Image = childPb.Image
                            });
                        }
                    }
                }
                else if (c is Label lblTop)
                {
                    bool isDashedLine = lblTop.Text.Trim().StartsWith("-");
                    drawItems.Add(new DrawItem
                    {
                        Type = DrawItem.ItemType.Label,
                        X = c.Left,
                        Y = c.Top,
                        Width = c.Width,
                        Height = c.Height,
                        Text = isDashedLine ? "DASHED_LINE" : lblTop.Text,
                        Font = lblTop.Font,
                        TextAlign = lblTop.TextAlign,
                        ForeColor = lblTop.ForeColor
                    });
                }
                else if (c is PictureBox pbTop && pbTop.Image != null && pbTop != pbLogo)
                {
                    drawItems.Add(new DrawItem
                    {
                        Type = DrawItem.ItemType.Image,
                        X = c.Left,
                        Y = c.Top,
                        Width = c.Width,
                        Height = c.Height,
                        Image = pbTop.Image
                    });
                }
            }
        }
        public void PrintReceipt()
        {
            // ✅ Ensure layout and all child controls are created
            this.SuspendLayout();
            this.PerformLayout();
            this.Refresh();

            pnlItems.PerformLayout();
            pnlItems.Refresh();

            // Force re-measure child controls
            foreach (Control c in pnlItems.Controls)
            {
                c.PerformLayout();
                c.Refresh();
            }
            this.ResumeLayout();
            Application.DoEvents();

            // 🟢 Check if pnlItems actually has content
            if (pnlItems.Controls.Count == 0)
            {
                // Reload order items if empty
                LoadOrderItems();
                pnlItems.PerformLayout();
                pnlItems.Refresh();
            }

            // ✅ Get cashier name based on most recent login in LogTrail
            string cashierName = "";
            string connStr = DbConnection.ConnectionString;

            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                conn.Open();

                string query = @"
                SELECT U.Name
                FROM LogTrail L
                INNER JOIN Users U ON L.UserID = U.UserID
                WHERE L.ActionType = 'Login'
                ORDER BY L.DateTime DESC
                LIMIT 1";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        cashierName = result.ToString();
                    }
                }
            }


            // ✅ Make sure header labels are set
            lblOrderNo.Text = "Invoice No: " + InvoiceNo;
            lblCashier.Text = "Cashier: " + (string.IsNullOrEmpty(cashierName) ? "N/A" : cashierName);
            lblDateTime.Text = "Date: " + DateTime.Now.ToString("MMM dd, yyyy hh:mm tt");
            lblCustomerName.Text = "Customer: " + CustomerName;
            lblCustomerContact.Text = "Contact: " + CustomerContact;



            // ✅ Flatten all visible controls
            BuildDrawItems();
            pageStartY = 0;


            // === SET UP PRINT DOCUMENT ===
            printDocument = new PrintDocument();
            printDocument.PrintController = new StandardPrintController();
            printDocument.PrinterSettings = new PrinterSettings();
            printDocument.PrinterSettings.PrinterName = "XP-58"; // change to your actual printer name if needed
            // ✅ Paper size for 58mm thermal printer
            PaperSize ps = new PaperSize("Receipt58mm", paperWidthPx, 10000);
            printDocument.DefaultPageSettings.PaperSize = ps;

            // === EVENT HOOKS ===
            printDocument.PrintPage += PrintDocument_PrintPage_Direct;
            printDocument.EndPrint += (s, e) => { pageStartY = 0; };

            // === PRINT TWO COPIES WITH DELAY ===
            for (int i = 0; i < 2; i++)
            {
                printDocument.Print();
                if (i == 0) System.Threading.Thread.Sleep(6000);
            }

        }
        /// <summary>
        /// PrintPage handler that draws the flattened items, handling pagination.
        /// </summary>
        private void PrintDocument_PrintPage_Direct(object sender, PrintPageEventArgs e)
        {
            float scaleFactor = 1.5f;

            // === Graphics setup ===
            e.Graphics.PageUnit = GraphicsUnit.Pixel;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            // === Use full page width instead of margin bounds ===
            int pageLeft = e.PageBounds.Left;         // No 1-inch margin
            int pageTop = e.PageBounds.Top;
            int pageHeight = e.PageBounds.Height;
            int pageBottomY = pageStartY + pageHeight;
            int printableWidth = e.PageBounds.Width;  // Full width (edge-to-edge)
            int layoutWidth = drawItems.Max(i => i.X + i.Width);
            int centerOffsetX = -20;                   // Start at the true left edge

            foreach (var item in drawItems)
            {
                if (item.Y < pageStartY) continue;
                if (item.Y >= pageStartY + pageHeight) continue;

                // === Scale position and size ===
                int drawX = (int)(centerOffsetX + item.X * scaleFactor);
                int drawY = (int)(pageTop + (item.Y - pageStartY) * scaleFactor);
                int drawWidth = (int)(item.Width * scaleFactor);
                int drawHeight = (int)(item.Height * scaleFactor);

                // === Font setup ===
                Font baseFont = item.Font ?? new Font("Consolas", 10, FontStyle.Regular);
                string text = item.Text ?? string.Empty;
                string textLower = text.ToLowerInvariant();

                // === Type detection ===


                bool isHeaderInfo =
 textLower.StartsWith("invoice") ||
 textLower.StartsWith("cashier") ||
 textLower.StartsWith("date") ||
 textLower.Contains("laundry") ||    // business name
 textLower.Contains("address") ||
 textLower.Contains("contact") ||
 textLower.StartsWith("customer") ||    // ✅ covers "CustomerName"
    textLower.Contains("customer");      // ✅ covers any "Customer:" or "CustomerName"

                bool isTableHeader =
                    Regex.IsMatch(textLower, @"category\s+qty\s+(wgt\s+)?price\s+amount");


                bool isSummary =
    textLower.Contains("total") ||
    textLower.Contains("cash:") ||
    textLower.Contains("maya") ||
    textLower.Contains("bpi") ||
    textLower.Contains("change") ||
    textLower.Contains("ref no") ||     // ✅ add
    textLower.Contains("reference");    // ✅ add



                bool isRemarks =
    textLower.StartsWith("remarks:");

                bool isQrOrAddress =
     // ===== Footer / QR notes =====
     textLower.Contains("qr") ||
     textLower.Contains("scan") ||
     textLower.Contains("feedback") ||
     textLower.Contains("fb page") ||
     //textLower.Contains("thank") ||
     //textLower.Contains("trusting") ||          // <-- catches “Thank you for trusting…”
     textLower.Contains("official receipt") ||  // <-- matches “this is not official receipt”

     // ===== Address keywords =====
     textLower.Contains("compound") ||
     textLower.Contains("sitio") ||
     textLower.Contains("blk") ||
     textLower.Contains("lot") ||
     textLower.Contains("subd") ||
     textLower.Contains("brgy") ||
     textLower.Contains("barangay") ||
     textLower.Contains("sta. rosa") ||
     textLower.Contains("laguna") ||
     textLower.Contains("heights") ||
     textLower.Contains("construction") ||
     textLower.Contains("berkeley") ||
     textLower.Contains("|");

                bool noticeTex =
                  textLower.Contains("complaints") ||
                  textLower.Contains("reserves") ||
                  textLower.Contains("liable");


                bool isSectionHeader =
                    !isHeaderInfo &&
                    !isSummary &&
                    !isTableHeader &&
                    Regex.IsMatch(text, @"^[A-Za-z\s]{3,25}$") &&  // letters & spaces only, length < 25
                    !text.Contains(":") &&
                    !Regex.IsMatch(text, @"\d");

                bool isTableItem =
                !isHeaderInfo &&
                !isSummary &&
                !isTableHeader &&
                !isSectionHeader &&
                !isQrOrAddress;

                // === Font scaling ===
                float fontScale;
                if (isTableItem) fontScale = 1.2f;
                else if (isSummary && textLower.Contains("total")) fontScale = 1.5f;
                else fontScale = scaleFactor;



                using (Font scaledFont = new Font(baseFont.FontFamily, Math.Max(6f, baseFont.Size * fontScale), baseFont.Style))
                {
                    if (item.Type == DrawItem.ItemType.Label)
                    {
                        TextFormatFlags flags = TextFormatFlags.NoPadding;
                        Rectangle drawRect;
                        if (item.Text == "DASHED_LINE")
                        {
                            // ✅ Use a Pen with a DashStyle to create the dashed effect, and make it bold by increasing the width
                            using (Pen dashedPen = new Pen(Color.Black, 1))  // Increased width to make it bold
                            {
                                dashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                                // Measure the space for the dashed line (mimicking the dashed text approach)
                                int lineY = drawY + drawHeight / 2;

                                // Calculate how many dash segments will fit across the width
                                int dashLength = 15;  // Dash length (you can adjust this)
                                int gapLength = 8;    // Gap between dashes (you can adjust this)
                                int fullLineWidth = printableWidth - 20; // Subtracting some space for margins (optional)

                                // Use a loop to draw a series of dashed segments
                                int currentX = pageLeft + 10; // Start drawing with some left margin
                                while (currentX < pageLeft + fullLineWidth)
                                {
                                    // Draw a bold dash from the current position to the next
                                    e.Graphics.DrawLine(dashedPen, currentX, lineY, currentX + dashLength, lineY);

                                    // Move the position forward by dashLength + gapLength
                                    currentX += dashLength + gapLength;
                                }
                            }

                            continue; // ✅ Skip normal text rendering
                        }




                        if (isHeaderInfo)
                        {
                            flags = TextFormatFlags.Left | TextFormatFlags.NoPadding;

                            int leftAlignedX = pageLeft + 5;
                            int safeTop = Math.Max(drawY, pageTop + 25);

                            if (text.Contains("Zaki's Laundry House")) // Assuming the business name is fixed text
                            {
                                // Move the business name slightly to the right
                                leftAlignedX = pageLeft + 40; // Example: Move 20px to the right

                            }

                            drawRect = new Rectangle(
                                leftAlignedX,
                                safeTop,
                                (int)(printableWidth * 0.9),
                                drawHeight
                            );

                            TextRenderer.DrawText(
                                e.Graphics,
                                text,
                                scaledFont,
                                drawRect,
                                item.ForeColor == default(Color) ? Color.Black : item.ForeColor,
                                flags
                            );
                        }
                        else if (noticeTex)
                        {
                            string[] lines = text.Split('\n');
                            int yPos = drawY;

                            const int receiptWidth = 384;   // <-- FIXED EXACT WIDTH

                            foreach (string rawLine in lines)
                            {
                                string line = rawLine.Trim();

                                if (string.IsNullOrEmpty(line))
                                {
                                    yPos += scaledFont.Height + 2;
                                    continue;
                                }

                                // Measure width WITHOUT padding
                                Size padded = TextRenderer.MeasureText(
                                    e.Graphics,
                                    line,
                                    scaledFont,
                                    new Size(int.MaxValue, int.MaxValue),
                                    TextFormatFlags.NoPadding
                                );

                                int trueWidth = padded.Width - 6;  // remove TextRenderer’s padding

                                if (trueWidth < 0) trueWidth = padded.Width; // safety fallback

                                // ===== TRUE CENTERING USING EXACT 384px RECEIPT WIDTH =====
                                int x = pageLeft + (receiptWidth - trueWidth) / 2;

                                TextRenderer.DrawText(
                                    e.Graphics,
                                    line,
                                    scaledFont,
                                    new Point(x, yPos),
                                    Color.Black,
                                    TextFormatFlags.NoPadding
                                );

                                yPos += padded.Height + 2;
                            }

                            continue;
                        }







                        else if (isRemarks)
                        {
                            flags = TextFormatFlags.Left;
                            int verticalOffset = 2; // add slight vertical space

                            drawRect = new Rectangle(
                                pageLeft + 5,
                                drawY + verticalOffset, // shift a bit lower to avoid collision
                                printableWidth / 2 - 10,
                                drawHeight
                            );
                            // Calculate available width (half of receipt)
                            int maxWidth = printableWidth / 2 - 10;

                            // Measure text
                            Size textSize = TextRenderer.MeasureText(text, scaledFont, new Size(maxWidth, int.MaxValue),
                                TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);

                            // Draw wrapped remarks text
                            TextRenderer.DrawText(
                                e.Graphics,
                                text,
                                scaledFont,
                                new Rectangle(pageLeft + 5, drawY, maxWidth, textSize.Height),
                                item.ForeColor == default(Color) ? Color.Black : item.ForeColor,
                                TextFormatFlags.WordBreak | TextFormatFlags.Left
                            );

                            // Add vertical space based on how many lines were drawn
                            drawY += textSize.Height + 5;
                        }
                        else if (isSummary)
                        {
                            text = Regex.Replace(text, @"₱\s+", "₱"); // removes all spaces after ₱
                            flags |= TextFormatFlags.Right;
                            int verticalOffset = 0; // optional, you can adjust slightly if needed

                            drawRect = new Rectangle(
                                (int)(pageLeft + printableWidth - drawWidth - 80),
                                drawY + verticalOffset,
                                drawWidth,
                                drawHeight
                            );

                            TextRenderer.DrawText(
                                e.Graphics,
                                text,
                                scaledFont,
                                drawRect,
                                item.ForeColor == default(Color) ? Color.Black : item.ForeColor,
                                flags
                            );
                        }


                        else if (isTableHeader || isTableItem)
                        {
                            TextFormatFlags flagsBase = TextFormatFlags.NoPadding | TextFormatFlags.WordBreak | TextFormatFlags.Left;

                            int tableLeft = pageLeft + 5;
                            int tableWidth = printableWidth - 60;
                            drawRect = new Rectangle(tableLeft, drawY, tableWidth, drawHeight);

                            using (Font monoFont = new Font("Consolas", Math.Max(6f, baseFont.Size * 1.2f), baseFont.Style))
                            {
                                string formattedText = text;

                                // ✅ Smarter splitting — keeps parentheses content as part of the item name
                                string pattern = @"^(?<name>.+?)\s+(?<qty>\d+)\s+(?:(?<wgt>[A-Za-z0-9.]+)\s+)?(?<price>\d+(\.\d{1,2})?)\s+(?<amount>\d+(\.\d{1,2})?)$";
                                var match = Regex.Match(text, pattern);

                                if (isTableHeader)
                                {
                                    formattedText = string.Format("{0,-13}{1,5}{2,6}{3,9}{4,9}",
                                        "Category", "Qty", "Wgt", "Price", "Amount");
                                }
                                else if (match.Success)
                                {
                                    string itemName = match.Groups["name"].Value;
                                    string qty = match.Groups["qty"].Value;
                                    string wgt = match.Groups["wgt"].Success ? match.Groups["wgt"].Value : ""; // optional
                                    string price = match.Groups["price"].Value;
                                    string amount = match.Groups["amount"].Value;

                                    // Clean up item name
                                    itemName = itemName.Replace(" (", "(").Replace(") ", ")").Trim();

                                    if (itemName.StartsWith("DISCOUNT", StringComparison.OrdinalIgnoreCase))
                                    {
                                        // Remove decimals
                                        price = price.Replace(".00", "");
                                        amount = amount.Replace(".00", "");

                                        // Format the line
                                        formattedText = string.Format("{0,-13}{1,6}{2,10}{3,9}{4,9}",
                                            itemName.PadRight(13), qty, wgt, price, amount);
                                    }

                                    else
                                    {
                                        if (itemName.Length > 13)
                                            itemName = itemName.Substring(0, 12) + ".";
                                        else
                                            itemName = itemName.PadRight(13);

                                        formattedText = string.Format("{0,-13}{1,6}{2,10}{3,9}{4,9}",
                                            itemName, qty, wgt, price, amount);
                                    }
                                }
                                else
                                {
                                    formattedText = text; // fallback
                                }

                                Size textSize = TextRenderer.MeasureText(formattedText, monoFont);
                                if (textSize.Width <= tableWidth)
                                {
                                    TextRenderer.DrawText(
                                        e.Graphics,
                                        formattedText,
                                        monoFont,
                                        drawRect,
                                        item.ForeColor == default(Color) ? Color.Black : item.ForeColor,
                                        flagsBase
                                    );
                                }
                                else
                                {
                                    // Use WordWrapMonospace to handle wrapping for both labels and table items
                                    string[] wrappedLines = WordWrapMonospace(formattedText, 13); // Wrap text to 13 characters
                                    int baseLineHeight = monoFont.Height;  // Base height for table items
                                    int labelLineHeight = scaledFont.Height;  // Line height for labels (could be adjusted)

                                    int currentLineHeight = isTableItem ? baseLineHeight : labelLineHeight;  // Dynamic height based on type

                                    // Create a small buffer in the line height to avoid excessive gaps
                                    int lineBuffer = 2; // Adjust the buffer for line spacing

                                    for (int i = 0; i < wrappedLines.Length; i++)
                                    {
                                        Rectangle lineRect;

                                        if (isTableItem)
                                        {
                                            // For table items, reduce the height slightly to save space
                                            lineRect = new Rectangle(
                                                tableLeft,
                                                drawY + (i * (baseLineHeight - lineBuffer)),
                                                tableWidth,
                                                baseLineHeight - lineBuffer
                                            );
                                        }
                                        else
                                        {
                                            // For labels, adjust the height similarly
                                            lineRect = new Rectangle(
                                                pageLeft + 5,
                                                drawY + (i * (labelLineHeight - lineBuffer)),
                                                printableWidth - 10,
                                                labelLineHeight - lineBuffer
                                            );
                                        }

                                        // Draw the wrapped text, using different fonts for labels and table items
                                        TextRenderer.DrawText(
                                            e.Graphics,
                                            wrappedLines[i],
                                            isTableItem ? monoFont : scaledFont,
                                            lineRect,
                                            item.ForeColor == default(Color) ? Color.Black : item.ForeColor,
                                            flags
                                        );
                                    }

                                    continue;

                                }
                            }
                        }

                        else if (isSectionHeader)
                        {
                            TextFormatFlags flagsBase = TextFormatFlags.NoPadding | TextFormatFlags.Left;

                            drawRect = new Rectangle(pageLeft + 5, drawY, printableWidth - 60, drawHeight);

                            TextRenderer.DrawText(
                                e.Graphics,
                                text,
                                scaledFont,
                                drawRect,
                                item.ForeColor == default(Color) ? Color.Black : item.ForeColor,
                                flagsBase
                            );
                        }
                        else
                        {
                            TextFormatFlags flagsBase = TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding;

                            int centerShiftX = -40;
                            drawRect = new Rectangle(pageLeft + centerShiftX, drawY, printableWidth, drawHeight);

                            TextRenderer.DrawText(
                                e.Graphics,
                                text,
                                scaledFont,
                                drawRect,
                                item.ForeColor == default(Color) ? Color.Black : item.ForeColor,
                                flagsBase
                            );
                        }


                    }

                    else if (item.Type == DrawItem.ItemType.Image && item.Image != null)
                    {
                        // ❌ Skip drawing if this is the logo
                        if (item.Width <= 90 && item.Height <= 90)
                            continue;

                        int centerShiftX = -50;
                        int centeredX = pageLeft + (printableWidth - drawWidth) / 2 + centerShiftX;
                        e.Graphics.DrawImage(item.Image, centeredX, drawY, drawWidth, drawHeight);
                    }

                }

            }

            // === Pagination Handling ===
            var lastItemOnPage = drawItems
                .Where(i => i.Y < pageBottomY && i.Y >= pageStartY)
                .OrderByDescending(i => i.Y)
                .FirstOrDefault();

            var maxItem = drawItems.OrderByDescending(i => i.Y).FirstOrDefault();
            int totalHeight = (maxItem != null ? maxItem.Y + maxItem.Height : 0);

            if (lastItemOnPage != null && (lastItemOnPage.Y + lastItemOnPage.Height) < totalHeight)
            {
                pageStartY = lastItemOnPage.Y + lastItemOnPage.Height;
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
                pageStartY = 0;
            }
        }


    }
}
