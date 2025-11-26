using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZakiLaundryHouse.ucAdmin
{
    public partial class ucSales : UserControl
    {
        private DataTable originalSalesData;

        public ucSales()
        {
            InitializeComponent();
            LoadSales();
            this.dateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            btnFilter.Click += btnFilter_Click;
            DataGridViewStyler.ApplyNonSelectableStyle(dtgSales);

            MouseUp += (s, e) =>
            {
                if (expand && !panelExportDropdown.Bounds.Contains(e.Location))
                {
                    ExportDroprdownTimer.Start();
                }
            };

            foreach (Control control in Controls)
            {

                if (control != panelExportDropdown && control.Name != btnExportAs.Name)
                {
                    control.MouseUp += (s, e) =>
                    {
                        if (expand && !panelExportDropdown.Bounds.Contains(e.Location))
                        {
                            ExportDroprdownTimer.Start();
                        }
                    };
                }
            }
            this.VisibleChanged += ucSales_VisibleChanged;

        }
        private void ucSales_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {

                ResetForm();
                LoadSales();
            }
        }

        public void ResetForm()
        {
            Totallbl.Text = string.Empty;
            dateFrom.Value = DateTime.Today;
            dateTo.Value = DateTime.Today;
            panelExportDropdown.Height = panelExportDropdown.MinimumSize.Height;
            expand = false;
            ExportDroprdownTimer.Stop();
            dtgSales.DataSource = originalSalesData;
        }


        private void LoadSales()
        {
            string connectionString = DbConnection.ConnectionString;
            string query = @"
SELECT 
    strftime('%Y-%m-%d', p.PaymentDate) AS 'Date',
    o.InvoiceNo AS 'Invoice No',
    c.CustomerName AS 'Customer Name',
    GROUP_CONCAT(i.ItemName || ' (x' || od.Quantity || ')', ', ') AS 'Items',
    o.Remarks AS 'Remarks',
    p.PaymentMethod AS 'Payment Method',
    p.AmountPaid AS 'Amount Paid'
FROM Payments p
INNER JOIN Orders o ON p.OrderID = o.OrderID
INNER JOIN Customers c ON o.CustomerID = c.CustomerID
INNER JOIN OrderDetails od ON o.OrderID = od.OrderID
INNER JOIN Items i ON od.ItemID = i.ItemID
WHERE o.Status = 'Completed'
GROUP BY 
    p.PaymentDate,
    o.InvoiceNo,
    c.CustomerName,
    p.PaymentMethod,
    p.AmountPaid,
    o.Remarks
ORDER BY p.PaymentDate DESC;";

            using (var conn = new SQLiteConnection(connectionString))
            {
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                originalSalesData = dt;
                dtgSales.DataSource = dt;

                // Format Amount Paid as currency
                if (dtgSales.Columns.Contains("Amount Paid"))
                {
                    dtgSales.Columns["Amount Paid"].DefaultCellStyle.Format = "C2"; // ₱1,234.00
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtgSales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CbExit_Click(object sender, EventArgs e)
        {
            formLogin loginForm = new formLogin();
            loginForm.Show();
            Form parentForm = this.FindForm(); // find the parent form
            if (parentForm != null)
            {
                parentForm.Close(); // close the whole form instead of just the user control
            }
        }

        private void CbMax_Click(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                if (parentForm.WindowState == FormWindowState.Normal)
                {
                    parentForm.WindowState = FormWindowState.Maximized;
                }
                else if (parentForm.WindowState == FormWindowState.Maximized)
                {
                    parentForm.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void CbMin_Click(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.WindowState = FormWindowState.Minimized;
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dateFrom.Value.Date;
            DateTime toDate = dateTo.Value.Date;

            if (fromDate > toDate)
            {
                MessageBox.Show(
                    "The start date cannot be later than the end date. Please select a valid date range before proceeding.",
                    "Invalid Date Range",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }


            if (originalSalesData == null)
                return;

            DataView dv = new DataView(originalSalesData);
            dv.RowFilter = $"[Date] >= '{fromDate:yyyy-MM-dd}' AND [Date] <= '{toDate:yyyy-MM-dd}'";

            dtgSales.DataSource = dv;


            decimal totalAmount = 0;
            foreach (DataRowView row in dv)
            {
                decimal.TryParse(row["Amount Paid"].ToString(), out decimal amount);
                totalAmount += amount;
            }

            Totallbl.Text = totalAmount.ToString("C2", CultureInfo.CreateSpecificCulture("en-PH"));
        }


        private void ExportToExcel()
        {
            try
            {
                if (dtgSales.Rows.Count == 0)
                {
                    MessageBox.Show("No sales records to export.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DateTime fromDate = dateFrom.Value.Date;
                DateTime toDate = dateTo.Value.Date;

                var filteredRows = dtgSales.Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells["Date"].Value != null &&
                                DateTime.TryParse(r.Cells["Date"].Value.ToString(), out DateTime d) &&
                                d.Date >= fromDate && d.Date <= toDate)
                    .ToList();

                if (filteredRows.Count == 0)
                {
                    MessageBox.Show("No sales found for the selected date range.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                    FileName = $"Sales_Report_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}.xlsx"
                };

                if (sfd.ShowDialog() != DialogResult.OK) return;

                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Sales Report");
                    int currentRow = 1;

                    // Title
                    ws.Range(currentRow, 1, currentRow, 7).Merge();
                    ws.Cell(currentRow, 1).Value = "Zaki's Laundry House Sales Report";
                    ws.Cell(currentRow, 1).Style.Font.Bold = true;
                    ws.Cell(currentRow, 1).Style.Font.FontSize = 16;
                    ws.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    currentRow++;

                    ws.Range(currentRow, 1, currentRow, 7).Merge();
                    ws.Cell(currentRow, 1).Value = $"Exported on: {DateTime.Now:yyyy-MM-dd hh:mm:ss tt}";
                    ws.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    currentRow++;

                    ws.Range(currentRow, 1, currentRow, 7).Merge();
                    ws.Cell(currentRow, 1).Value = fromDate == toDate
                        ? $"Date: {fromDate:yyyy-MM-dd}"
                        : $"Date Range: {fromDate:yyyy-MM-dd} to {toDate:yyyy-MM-dd}";
                    ws.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    currentRow++;

                    ws.Range(currentRow, 1, currentRow, 7).Merge();
                    ws.Cell(currentRow, 1).Value = "Block 5 Lot 10 Phase 1, Berkeley Heights Subdivision, Pulong Santa Cruz, Santa Rosa, Laguna, 4026";
                    ws.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    currentRow++;

                    ws.Range(currentRow, 1, currentRow, 7).Merge();
                    ws.Cell(currentRow, 1).Value = "Phone: +63 917 124 6364 | Email: info@zakilaundry.com";
                    ws.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    currentRow++;

                    ws.Cell(currentRow++, 1).Value = ""; // Empty line

                    // Column headers
                    string[] headers = { "Invoice No", "Date", "Customer Name", "Items", "Remarks", "Payment Method", "Amount Paid" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        ws.Cell(currentRow, i + 1).Value = headers[i];
                        ws.Cell(currentRow, i + 1).Style.Font.Bold = true;
                        ws.Cell(currentRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                        ws.Cell(currentRow, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }
                    currentRow++;

                    double grandTotal = 0;

                    foreach (var row in filteredRows)
                    {
                        string invoiceNo = row.Cells["Invoice No"].Value?.ToString() ?? "";
                        string date = row.Cells["Date"].Value?.ToString() ?? "";
                        string customer = row.Cells["Customer Name"].Value?.ToString() ?? "";
                        string items = row.Cells["Items"].Value?.ToString() ?? "";
                        string remarks = row.Cells["Remarks"].Value?.ToString() ?? "";
                        string paymentMethod = row.Cells["Payment Method"].Value?.ToString() ?? "";
                        double.TryParse(row.Cells["Amount Paid"].Value?.ToString(), out double amountPaid);

                        ws.Cell(currentRow, 1).Value = invoiceNo;
                        ws.Cell(currentRow, 2).Value = date;
                        ws.Cell(currentRow, 3).Value = customer;
                        ws.Cell(currentRow, 4).Value = items;
                        ws.Cell(currentRow, 5).Value = remarks;
                        ws.Cell(currentRow, 6).Value = paymentMethod;
                        ws.Cell(currentRow, 7).Value = amountPaid;
                        ws.Cell(currentRow, 7).Style.NumberFormat.Format = "₱#,##0.00";

                        grandTotal += amountPaid;
                        currentRow++;
                    }

                    // Grand total
                    ws.Cell(currentRow, 6).Value = "Grand Total:";
                    ws.Cell(currentRow, 6).Style.Font.Bold = true;
                    ws.Cell(currentRow, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Cell(currentRow, 6).Style.Fill.BackgroundColor = XLColor.LightGreen;

                    ws.Cell(currentRow, 7).Value = grandTotal;
                    ws.Cell(currentRow, 7).Style.Font.Bold = true;
                    ws.Cell(currentRow, 7).Style.NumberFormat.Format = "₱#,##0.00";
                    ws.Cell(currentRow, 7).Style.Fill.BackgroundColor = XLColor.LightGreen;

                    currentRow += 2;

                    // Generated by
                    ws.Range(currentRow, 1, currentRow, 7).Merge();
                    ws.Cell(currentRow, 1).Value = "Generated By: Glenn Austria Paglaoman";
                    ws.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    ws.Columns().AdjustToContents();
                    workbook.SaveAs(sfd.FileName);
                }

                MessageBox.Show("Excel Exported Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting Excel:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dtgSales.Rows.Count == 0) return;

            DateTime fromDate = dateFrom.Value.Date;
            DateTime toDate = dateTo.Value.Date;

            var filteredRows = dtgSales.Rows.Cast<DataGridViewRow>()
                .Where(r => r.Cells["Date"].Value != null &&
                            DateTime.TryParse(r.Cells["Date"].Value.ToString(), out DateTime d) &&
                            d.Date >= fromDate && d.Date <= toDate)
                .ToList();

            if (filteredRows.Count == 0)
            {
                MessageBox.Show("No sales found for the selected period.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = $"Sales_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}.pdf"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                using (var stream = new FileStream(sfd.FileName, FileMode.Create))
                {
                    var pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 36, 36, 50, 50);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    var titleFont = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD);
                    var subtitleFont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLDITALIC);
                    var headerFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
                    var normalFont = FontFactory.GetFont("Arial", 10);

                    // Title
                    Paragraph title = new Paragraph("Zaki's Laundry House Sales Report", titleFont) { Alignment = Element.ALIGN_CENTER };
                    pdfDoc.Add(title);

                    Paragraph exportedOn = new Paragraph($"Exported on: {DateTime.Now:yyyy-MM-dd hh:mm:ss tt}", normalFont) { Alignment = Element.ALIGN_CENTER };
                    pdfDoc.Add(exportedOn);

                    Paragraph dateRange = new Paragraph(fromDate == toDate
                        ? $"Date: {fromDate:yyyy-MM-dd}"
                        : $"Date Range: {fromDate:yyyy-MM-dd} to {toDate:yyyy-MM-dd}", normalFont)
                    { Alignment = Element.ALIGN_CENTER };
                    pdfDoc.Add(dateRange);

                    Paragraph address = new Paragraph("Block 5 Lot 10 Phase 1, Berkeley Heights Subdivision, Pulong Santa Cruz, Santa Rosa, Laguna, 4026", normalFont) { Alignment = Element.ALIGN_CENTER };
                    pdfDoc.Add(address);

                    Paragraph contact = new Paragraph("Phone: +63 917 124 6364 | Email: info@zakilaundry.com", normalFont) { Alignment = Element.ALIGN_CENTER };
                    pdfDoc.Add(contact);

                    pdfDoc.Add(new Paragraph("\n"));

                    // Table
                    string[] headers = { "Invoice No", "Date", "Customer Name", "Items", "Remarks", "Payment Method", "Amount Paid" };
                    PdfPTable pdfTable = new PdfPTable(headers.Length) { WidthPercentage = 100 };

                    foreach (var h in headers)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(h, headerFont))
                        {
                            BackgroundColor = new BaseColor(220, 220, 220),
                            HorizontalAlignment = Element.ALIGN_CENTER
                        };
                        pdfTable.AddCell(cell);
                    }

                    decimal grandTotal = 0;
                    foreach (var row in filteredRows)
                    {
                        pdfTable.AddCell(row.Cells["Invoice No"].Value?.ToString() ?? "");
                        pdfTable.AddCell(row.Cells["Date"].Value?.ToString() ?? "");
                        pdfTable.AddCell(row.Cells["Customer Name"].Value?.ToString() ?? "");
                        pdfTable.AddCell(row.Cells["Items"].Value?.ToString() ?? "");
                        pdfTable.AddCell(row.Cells["Remarks"].Value?.ToString() ?? "");
                        pdfTable.AddCell(row.Cells["Payment Method"].Value?.ToString() ?? "");

                        decimal.TryParse(row.Cells["Amount Paid"].Value?.ToString(), out decimal amountPaid);
                        grandTotal += amountPaid;

                        PdfPCell amountCell = new PdfPCell(new Phrase(amountPaid.ToString("₱#,##0.00"), normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT };
                        pdfTable.AddCell(amountCell);
                    }

                    // Grand total row
                    for (int i = 0; i < headers.Length - 2; i++)
                        pdfTable.AddCell(new PdfPCell(new Phrase("")));

                    PdfPCell grandTotalCell = new PdfPCell(new Phrase("Grand Total: " + grandTotal.ToString("₱#,##0.00"), headerFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        BackgroundColor = new BaseColor(144, 238, 144), // light green
                        Colspan = 2
                    };
                    pdfTable.AddCell(grandTotalCell);

                    pdfDoc.Add(pdfTable);

                    pdfDoc.Add(new Paragraph("\n\n"));
                    Paragraph generatedBy = new Paragraph("Generated By: Glenn Austria Paglaoman", normalFont) { Alignment = Element.ALIGN_LEFT };
                    pdfDoc.Add(generatedBy);

                    pdfDoc.Close();
                    stream.Close();
                }

                MessageBox.Show("PDF Exported Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting PDF: " + ex.Message);
            }
        }






        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        bool expand = false;
        private void ExportDroprdownTimer_Tick(object sender, EventArgs e)
        {
            if (expand == false)
            {
                panelExportDropdown.Height += 15;
                if (panelExportDropdown.Height >= panelExportDropdown.MaximumSize.Height)
                {
                    ExportDroprdownTimer.Stop();
                    expand = true;
                }
            }
            else
            {
                panelExportDropdown.Height -= 15;
                if (panelExportDropdown.Height <= panelExportDropdown.MinimumSize.Height)
                {
                    ExportDroprdownTimer.Stop();
                    expand = false;
                }

            }
        }

        private void btnExportAs_Click(object sender, EventArgs e)
        {
            ExportDroprdownTimer.Start();
        }
    }
}

