﻿using BLL;
using DTO;
using GUI.Employee;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TheArtOfDevHtmlRenderer.Adapters.RGraphicsPath;

namespace GUI
{
    public partial class frmSale : Form
    {
        NhanVien nv;
        TabPage currentTabPage = new TabPage();
        PhanLoaiTDBLL pltd_bll = new PhanLoaiTDBLL();
        ThucDonBLL td_bll = new ThucDonBLL();
        HinhThucThanhToanBLL httt_bll = new HinhThucThanhToanBLL();
        List<ChiTietHDBH> cthdbh = new List<ChiTietHDBH>();
        HoaDonBanHangBLL hdbh_bll = new HoaDonBanHangBLL();
        ChiTietHDBHBLL cthdbh_bll = new ChiTietHDBHBLL();
        ThamSoBLL ts_bll = new ThamSoBLL();

        public frmSale(NhanVien nv)
        {
            InitializeComponent();
            this.nv = nv;
            this.Load += frmSale_Load;
            tabOrder.SelectedIndexChanged += tabOrder_SelectedIndexChanged;
            dgvOrder.CellContentClick += dgvOrder_CellContentClick;
            btnOrder.Click += btnOrder_Click;
            btnClear.Click += btnClear_Click;
            btnOrderList.Click += btnOrderList_Click;
            btnExit.Click += btnExit_Click;
        }

        private void frmSale_Load(object sender, EventArgs e)
        {
            tabOrder_Load();
            if (tabOrder.TabCount > 0)
            {
                currentTabPage = tabOrder.TabPages[0];
            }
            dgvOrder_Load();
            cboPayment_Load();
        }

        private void tabOrder_Load()
        {
            PhanLoaiTD[] pltd = pltd_bll.GetList(new PhanLoaiTD());
            if (pltd == null) { return; }
            foreach (PhanLoaiTD category in pltd)
            {
                // Create new TabPage
                TabPage newPage = new TabPage(category.TenLoai);
                newPage.Name = String.Format("tpCategory{0}", category.MaLoai);
                newPage.BackgroundImage = global::GUI.Properties.Resources.mF_Background;
                newPage.BackgroundImageLayout = ImageLayout.Stretch;
                Guna2VScrollBar scroll = new Guna2VScrollBar();
                scroll.BindingContainer = newPage;
                scroll.AutoRoundedCorners = true;

                // Load menu of each TabPage
                ThucDon td_find = new ThucDon();
                td_find.MaLoai = category.MaLoai;
                ThucDon[] td = td_bll.GetList(td_find);

                // Add TableLayoutPanel to TabPage
                TableLayoutPanel newTable = new TableLayoutPanel();
                newTable.AutoSize = true;
                newTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                newTable.BackColor = Color.Transparent;
                newTable.Dock = DockStyle.Top;
                newPage.Controls.Add(newTable);

                // If this catogery has no dish added
                if (td == null)
                {
                    newTable.Controls.Add(new Label()
                    //newPage.Controls.Add(new Label()
                    {
                        Anchor = AnchorStyles.Top,
                        AutoSize = true,
                        Text = "Chưa có món ăn nào được thêm vào phân loại thực đơn này!",
                        Font = new Font("Times New Roman", 14F, FontStyle.Bold)
                    });
                    tabOrder.Controls.Add(newPage);
                    // Skip to next category
                    continue;
                }

                //

                // Generate columns
                ThamSo ts_MenuColumns = new ThamSo();
                ts_MenuColumns.TenTS = "MenuColumns";
                ThamSo[] ts = ts_bll.GetList(ts_MenuColumns);
                if (ts == null)
                {
                    ShowError("Bạn chưa cài đặt số cột hiển thị cho thực đơn!");
                    return;
                }
                int numberOfColumns = ts[0].GiaTri;
                newTable.ColumnCount = numberOfColumns;
                float columnWidth = (float)(100 / numberOfColumns);
                for (int i = 0; i < numberOfColumns; i++)
                {
                    newTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, columnWidth));
                }
                //newTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
                //newTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
                //newTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

                int columnIndex = 0;
                foreach (ThucDon dish in td)
                {
                    if (columnIndex == numberOfColumns)
                    {
                        columnIndex = 0;
                    }
                    if (columnIndex % numberOfColumns == 0)
                    {
                        newTable.RowCount++;
                        newTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 300));
                    }

                    // Add TableLayoutPanel for each cell of menu
                    TableLayoutPanel cell = new TableLayoutPanel();
                    cell.AutoSize = true;
                    cell.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    cell.BackColor = Color.Transparent;
                    cell.Dock = DockStyle.Fill;
                    cell.ColumnCount = 1;
                    cell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                    cell.RowCount = 3;
                    cell.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

                    Guna2PictureBox img = new Guna2PictureBox();
                    img.Image = new Bitmap(dish.AnhMon);
                    img.SizeMode = PictureBoxSizeMode.StretchImage;
                    img.Anchor = AnchorStyles.None;

                    cell.Controls.Add(img, 0, 0);
                    cell.Controls.Add(new Guna2HtmlLabel()
                    {
                        Anchor = AnchorStyles.None,
                        Text = dish.TenMon,
                        Font = new Font("Times New Roman", 16F, FontStyle.Bold),
                    }, 0, 1);

                    TableLayoutPanel order = new TableLayoutPanel();
                    order.AutoSize = true;
                    order.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    order.BackColor = Color.Transparent;
                    order.Dock = DockStyle.Fill;
                    order.ColumnCount = 3;
                    order.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
                    order.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                    order.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
                    order.RowCount = 1;

                    order.Controls.Add(new Guna2HtmlLabel()
                    {
                        Anchor = AnchorStyles.Left,
                        Text = String.Format("{0} VNĐ", dish.GiaBan.ToString()),
                        Font = new Font("Times New Roman", 13F)
                    }, 0, 0);

                    Guna2NumericUpDown numQuantity = new Guna2NumericUpDown()
                    {
                        //Name = String.Format("numQuantity{0}", dish.MaMon),
                        Tag = dish.MaMon,
                        Anchor = AnchorStyles.None,
                        Height = 30,
                        BorderRadius = 5,
                        ForeColor = Color.Black,
                        Font = new Font("Times New Roman", 13F)
                    };
                    order.Controls.Add(numQuantity, 1, 0);

                    Guna2Button btnAdd = new Guna2Button()
                    {
                        //Name = String.Format("btnAdd{0}", dish.MaMon),
                        Tag = dish,
                        Anchor = AnchorStyles.None,
                        Height = 30,
                        BorderRadius = 5,
                        Text = "Thêm",
                        Font = new Font("Times New Roman", 13F)
                    };
                    btnAdd.Click += new EventHandler(btnAdd_Click);
                    order.Controls.Add(btnAdd, 2, 0);

                    cell.Controls.Add(order, 0, 2);

                    newTable.Controls.Add(cell, columnIndex, newTable.RowCount - 1);

                    columnIndex++;
                }

                // Add new TabPage to TabControl 'tabOrder'
                tabOrder.Controls.Add(newPage);
            }
        }

        private void tabOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tab = (TabControl)sender;
            currentTabPage = tab.TabPages[tab.SelectedIndex];
        }

        private void dgvOrder_Load()
        {
            int dgvWidth = dgvOrder.Width;
            dgvOrder.Columns[0].FillWeight = (float)(dgvWidth * 25 / 100);
            dgvOrder.Columns[1].FillWeight = (float)(dgvWidth * 20 / 100);
            dgvOrder.Columns[2].FillWeight = (float)(dgvWidth * 20 / 100);
            dgvOrder.Columns[3].FillWeight = (float)(dgvWidth * 25 / 100);
            dgvOrder.Columns[4].FillWeight = (float)(dgvWidth * 10 / 100);
        }

        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvOrder.Columns[e.ColumnIndex].Name == "Delete")
            {
                cthdbh.RemoveAt(e.RowIndex);
                dgvOrder.Rows.RemoveAt(e.RowIndex);
                UpdateTotal();
            }
        }

        private void dgvOrder_Clear()
        {
            dgvOrder.Rows.Clear();
            UpdateTotal();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Guna2Button btn = (Guna2Button)sender;
            ThucDon td = (ThucDon)btn.Tag;
            List<Control> cellList = new List<Control>();
            cellList.AddRange(currentTabPage.Controls[0].Controls.OfType<Control>());
            foreach (Control ctr in cellList)
            {
                Guna2NumericUpDown num = (Guna2NumericUpDown)ctr.Controls[2].Controls[1];
                if (int.Parse(num.Tag.ToString()) == td.MaMon)
                {
                    int quantity = int.Parse(num.Value.ToString());
                    if (quantity == 0)
                    {
                        ShowMessage("Vui lòng chọn số lượng món ăn!");
                        return;
                    }

                    // If this dish has already been added into order => just increase the quantity, not create new row
                    bool added = false;
                    for (int i = 0; i < cthdbh.Count; i++)
                    {
                        if (td.MaMon == cthdbh[i].MaMon)
                        {
                            added = true;
                            int currentQuantity = int.Parse(dgvOrder.Rows[i].Cells["Quantity"].Value.ToString());
                            dgvOrder.Rows[i].Cells["Quantity"].Value = currentQuantity + quantity;

                            // Update quantity of ChiTietHDBH list
                            cthdbh[i].SoLuong += quantity;
                            cthdbh[i].ThanhTien = cthdbh[i].SoLuong * cthdbh[i].DonGia;
                            dgvOrder.Rows[i].Cells["TotalPrice"].Value = cthdbh[i].ThanhTien;
                        }
                    }

                    // If this dish hasn't been added into order yet => create new row
                    if (!added)
                    {
                        dgvOrder.Rows.Add(new object[]
                        {
                            td.TenMon, td.GiaBan, quantity, td.GiaBan * quantity
                        });
                        
                        // Add new ChiTietHDBH to list
                        ChiTietHDBH cthdbh_new = new ChiTietHDBH();
                        cthdbh_new.MaMon = td.MaMon;
                        cthdbh_new.SoLuong = quantity;
                        cthdbh_new.DonGia = td.GiaBan;
                        cthdbh_new.ThanhTien = cthdbh_new.SoLuong * cthdbh_new.DonGia;
                        cthdbh.Add(cthdbh_new);
                    }
                    UpdateTotal();
                    num.Value = 0;
                    break;
                }
            }
        }

        private void UpdateTotal()
        {
            int total = 0;
            foreach (ChiTietHDBH item in cthdbh)
            {
                total += item.ThanhTien;
            }
            lblTotal.Text = String.Format("{0} VNĐ", total.ToString());
        }

        private void cboPayment_Load()
        {
            HinhThucThanhToan[] httt = httt_bll.GetList(new HinhThucThanhToan());
            cboPayment.DisplayMember = "TenHTTT";
            cboPayment.ValueMember = "MaHTTT";
            cboPayment.DataSource = httt;
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrder.RowCount > 0)
            {
                if (!cthdbh_bll.CheckStorage(cthdbh))
                {
                    ShowError("Nguyên liệu tồn kho không đủ!");
                    return;
                }
                HoaDonBanHang hdbh = new HoaDonBanHang();
                hdbh.MaNV = nv.MaNV;
                hdbh.ThanhToan = int.Parse(cboPayment.SelectedValue.ToString());
                int result = hdbh_bll.Create(hdbh);
                if (result > 0)
                {
                    int orderID = hdbh_bll.GetLatest();
                    for (int i = 0; i < cthdbh.Count; i++)
                    {
                        cthdbh[i].MaHD = orderID;
                        int result2 = cthdbh_bll.Create(cthdbh[i]);
                        if (!(result2 > 0))
                        {
                            cthdbh_bll.Delete(cthdbh[i]);
                            hdbh_bll.Delete(hdbh);
                            ShowError("Đặt hàng thất bại!");
                            return;
                        }
                    }
                    ShowMessage("Đặt hàng thành công!");
                    dgvOrder_Clear();
                    UpdateTotal();
                }
                else
                {
                    ShowError("Đặt hàng thất bại!");
                }
            }
            else
            {
                ShowMessage("Vui lòng chọn món ăn!");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dgvOrder_Clear();
            cthdbh.Clear();
        }

        private void btnOrderList_Click(object sender, EventArgs e)
        {
            new frmOrderManagement(nv).ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowError(string error)
        {
            MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowMessage(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
