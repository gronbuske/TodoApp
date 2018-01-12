using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TodoApp
{
    public partial class Form1 : Form
    {
        public struct rowControls
        {
            public CheckBox cb;
            public TextBox text;
            public Button deleteButton;
        }

        public List<rowControls> rowList;

        public Form1()
        {
            rowList = new List<rowControls>();

            InitializeComponent();
            comboBox1.SelectedItem = comboBox1.Items[0];
            textBox1.KeyUp += TextBoxKeyUp;
            label1.Text = "";

            tableLayoutPanel1.RowCount = 0;
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.AutoScroll = true;
        }
        

        private void UpdateTable()
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowCount = rowList.Count;
            int nrNotChecked = 0;
            int index = comboBox1.SelectedIndex;
            int counter = 0;
            for (int i = 0; i < rowList.Count; i++)
            {
                if (rowList[i].cb.Checked == false)
                    nrNotChecked++;
                if (index == 1 && rowList[i].cb.Checked)
                    continue;
                else if (index == 2 && !rowList[i].cb.Checked)
                    continue;
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                tableLayoutPanel1.Controls.Add(rowList[i].cb, 0, counter);
                tableLayoutPanel1.Controls.Add(rowList[i].text, 1, counter);
                tableLayoutPanel1.Controls.Add(rowList[i].deleteButton, 2, counter);
                counter++;
            }
            label1.Text = nrNotChecked.ToString() + " things left to do";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                foreach (var v in rowList)
                    v.cb.Checked = true;
            }
            else
            {
                foreach (var v in rowList)
                    v.cb.Checked = false;
            }
            UpdateTable();
        }
        
        private void itemChecked(object sender, EventArgs e)
        {
            UpdateTable();
        }

        private void RemoveButtonPressed(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            for (int i = 0; i < rowList.Count; i++)
            {
                if (b == rowList[i].deleteButton)
                {
                    rowList.RemoveAt(i);
                    break;
                }
            }
            UpdateTable();
        }

        private void itemNameChange(object sender, EventArgs e)
        {
            TextBox l = (TextBox)sender;
            l.ReadOnly = false;
        }

        private void finishedEditingName(object sender, EventArgs e)
        {
            ((TextBox)sender).ReadOnly = true;
            rowList.Sort((s1, s2) => String.Compare(s1.text.Text, s2.text.Text));
            UpdateTable();
        }

        private void checkForEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ((TextBox)sender).ReadOnly = true;
                rowList.Sort((s1, s2) => String.Compare(s1.text.Text, s2.text.Text));
                UpdateTable();
            }
        }

        private void TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                CheckBox newcb = new CheckBox() { Text = "" };
                newcb.CheckedChanged += itemChecked;
                Button newB = new Button() { Text = "Remove" };
                newB.Click += RemoveButtonPressed;
                TextBox newL = new TextBox() { Text = textBox1.Text };
                newL.DoubleClick += itemNameChange;
                newL.ReadOnly = true;
                newL.LostFocus += finishedEditingName;
                newL.KeyUp += checkForEnter;
                rowList.Add(new rowControls()
                {
                    cb = newcb,
                    text = newL,
                    deleteButton = newB
                });
                textBox1.Clear();
                rowList.Sort((s1, s2) => String.Compare(s1.text.Text, s2.text.Text));
                UpdateTable();
                e.Handled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTable();
        }

        private void textBox1_TextChanged(object sender, EventArgs e){}
        private void label1_Click(object sender, EventArgs e){}
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
    }
}
