﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TicketSelling
{
    public partial class SignUpForm : Form
    {
        private SQL sql;
        private List<TextBox> tbList;
        private List<Label> lbList;
        public SignUpForm(SQL sql)
        {
            this.sql = sql;
            InitializeComponent();
            init();
        }
        private void init()
        {
            tbList = new List<TextBox>()
            {
                FirstNameTb, LastNameTb, AddressTb, PhoneTb, EmailTb, UsTb, PwTb, PwCfTb
            };

            lbList = new List<Label>()
            {
                FirstNameLb, LastNameLb, AddressLb, PhoneLb, EmailLb, UsernameLb, PasswordLb, confirmpwlb
            };
        }
        private void PwCfTb_TextChanged(object sender, EventArgs e)
        {
            if (PwTb.Text != PwCfTb.Text)
            {
                confirmpwlb.ForeColor = Color.Red;
                checkPwLb.Visible = true;
            }
            else
            {
                confirmpwlb.ForeColor = Color.Black;
                checkPwLb.Visible = false;
            }
        }
        private Boolean IsBlank(TextBox t, int idx)
        {
            if (string.IsNullOrWhiteSpace(t.Text))
            {
                lbList[idx].ForeColor = Color.Red;
                MissInfLb.Show();
                return true;
            }
            else
            {
                lbList[idx].ForeColor = Color.Black;
            }
            return false;
        }

        private void PhoneTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    if (e.KeyCode != Keys.Back)
                    {
                        e.SuppressKeyPress = true;
                    }
                }
            }
        }

        private void Confirm_click(object sender, EventArgs e)
        {
            //Check blank name
            bool blank = false;
            int index = 0;
            foreach (TextBox tb in tbList)
            {
                if (IsBlank(tb, index++))
                {
                    blank = true;
                }
                tb.Text = sql.checkQuote(tb.Text);
            }

            if (GenderListBox.SelectedItem == null)
            {
                GenderLb.ForeColor = Color.Red;
                blank = true;
            }
            if (blank) return;
            else MissInfLb.Hide();

            if (!sql.IsValidEmail(EmailTb.Text))
            {
                MessageBox.Show("Wrong Email format");
                return;
            }



            //Check dumplicate username
            string query = string.Format("SELECT Username FROM ACCOUNT WHERE username='{0}' COLLATE Latin1_General_CS_AS", UsTb.Text.ToString());
            string check = sql.Read(0, query);
            if (!string.IsNullOrEmpty(check))
            {
                UsernameLb.ForeColor = Color.Red;
                return;
            }

            //Adding user infomations into database
            query = string.Format("INSERT INTO USERS VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','User')",
                FirstNameTb.Text,
                LastNameTb.Text,
                PhoneTb.Text,
                AddressTb.Text,
                EmailTb.Text,
                GenderListBox.SelectedItem.ToString(),
                DoBTb.Value.ToString("yyyy-MM-dd"));
            sql.Add(query);

            //Get user ID
            query = String.Format("SELECT TOP 1 * FROM USERS ORDER BY ID DESC;");
            string ID = sql.Read(1, query);

            //Add user login infomation into Account table
            query = String.Format("INSERT INTO ACCOUNT VALUES ('{0}','{1}','{2}')",
            UsTb.Text, PwTb.Text, ID);
            sql.Add(query);
            this.Close();
            MessageBox.Show("Sign up successfull");
        }

    }
}
