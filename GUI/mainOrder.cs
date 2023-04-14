﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class MainOrder : Form
    {
        public MainOrder()
        {
            InitializeComponent();
        }

        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnl2.Controls.Add(childForm);
            pnl2.Tag = childForm;
            pnl2.Visible = true;
            childForm.BringToFront();
            childForm.Show();
        }

        private void ptb_dot1_Click(object sender, EventArgs e)
        {
            openChildForm(new OrderHamburger());
            pnl3.Visible = true;

        }

        private void ptb_dot2_Click(object sender, EventArgs e)
        {
            openChildForm(new OrderChicken());
            pnl3.Visible=true;

        }

        private void ptb_dot3_Click(object sender, EventArgs e)
        {
            openChildForm(new Order_Noodles());
            pnl3.Visible = true;

        }

        private void ptb_dot4_Click(object sender, EventArgs e)
        {
            openChildForm(new OrderSidedish());
            pnl3.Visible = true;

        }

        private void ptb_close_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
