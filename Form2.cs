using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ad_calculator
{
    public partial class Form2 : Form
    {
        DataTable back = new DataTable("Back");
        DataTable front = new DataTable("Front");
        DataTable materials = new DataTable("Materials");
        DataSet priceSet = new DataSet("PriceSet");
        public Form2()
        {
            InitializeComponent();
        }

        public void dbStart()
        {
            priceSet.Tables.Add(materials);
            priceSet.Tables.Add(back);
            priceSet.Tables.Add(front);

            // таблица materials
            materials.Columns.Add("ID", Type.GetType("System.Int32"));
            materials.Columns.Add("Type", Type.GetType("System.String"));
            materials.Columns.Add("SquarePrice", Type.GetType("System.Int32"));
            materials.Columns.Add("CutPriceFront", Type.GetType("System.Int32"));
            materials.Columns.Add("CutPriceBack", Type.GetType("System.Int32"));

            // таблица back
            back.Columns.Add("ThicknessMM", Type.GetType("System.Int32"));
            back.Columns.Add("SquarePriceMM", Type.GetType("System.Int32"));

            // таблица front (ПВХ)
            front.Columns.Add("ID", Type.GetType("System.Int32"));
            front.Columns.Add("ThicknessMM", Type.GetType("System.Int32"));
            front.Columns.Add("SquarePriceMM", Type.GetType("System.Int32"));

            DataSet ds = new DataSet();

            try
            {
                ds.ReadXml("Database.xml");
                priceSet.Merge(ds, true, MissingSchemaAction.Ignore);
            }
            catch (System.IO.FileNotFoundException)
            {
                fillDefault();
            };
        }

        public void fillDefault()
        {
            // заполнение талбиц данными
            materials.Rows.Add(new object[] { 1, "ПВХ + Oracal", null, 10, 10 });
            materials.Rows.Add(new object[] { 2, "Акрил", 20, 200, 20 });
            materials.Rows.Add(new object[] { 3, "ПВХ + Краска", null, 30, 30 });
            materials.Rows.Add(new object[] { 4, "АКП", 40, 400, 40 });

            back.Rows.Add(new object[] { "5", 50 });
            back.Rows.Add(new object[] { "8", 80 });
            back.Rows.Add(new object[] { "0", 0 });

            front.Rows.Add(new object[] { 1, "3", 150 });
            front.Rows.Add(new object[] { 1, "5", 140 });
            front.Rows.Add(new object[] { 1, "8", 130 });
            front.Rows.Add(new object[] { 1, "19", 120 });

            front.Rows.Add(new object[] { 3, "3", 350 });
            front.Rows.Add(new object[] { 3, "5", 340 });
            front.Rows.Add(new object[] { 3, "8", 330 });
            front.Rows.Add(new object[] { 3, "19", 320 });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            priceSet.WriteXml("Database.xml");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedItem = "";
            comboBox2.Items.Clear();

            if (comboBox1.SelectedIndex != -1)
            {
                if (comboBox1.SelectedItem.ToString().Contains("ПВХ") == true)
                {
                    label1.Visible = true;
                    comboBox2.Visible = true;

                    foreach (DataRow rowM in materials.Rows)
                    {
                        if (rowM["Type"].ToString() == comboBox1.SelectedItem.ToString())
                        {
                            foreach (DataRow rowS in front.Rows)
                            {
                                numericUpDown3.Value = 0;
                                numericUpDown7.Value = Convert.ToDecimal(rowM["CutPriceFront"]);
                                numericUpDown14.Value = Convert.ToDecimal(rowM["CutPriceBack"]);

                                numericUpDown7.BackColor = Color.FromName("Info");
                                numericUpDown14.BackColor = Color.FromName("Info");

                                if (rowS["ID"].ToString() == rowM["ID"].ToString())
                                {
                                    comboBox2.Items.Add(rowS["ThicknessMM"]);
                                }
                            }
                        }
                    }
                }
                else
                {
                    label1.Visible = false;
                    comboBox2.Visible = false;
                    foreach (DataRow rowM in materials.Rows)
                    {
                        if (rowM["Type"].ToString() == comboBox1.SelectedItem.ToString())
                        {
                            numericUpDown3.Value = Convert.ToDecimal(rowM["SquarePrice"]);
                            numericUpDown7.Value = Convert.ToDecimal(rowM["CutPriceFront"]);
                            numericUpDown14.Value = Convert.ToDecimal(rowM["CutPriceBack"]);

                            numericUpDown3.BackColor = Color.FromName("Info");
                            numericUpDown7.BackColor = Color.FromName("Info");
                            numericUpDown14.BackColor = Color.FromName("Info");
                        }
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (DataRow rowM in materials.Rows)
            {
                if (rowM["Type"].ToString() == comboBox1.SelectedItem.ToString())
                {
                    foreach (DataRow rowS in front.Rows)
                    {
                        if (rowS["ID"].ToString() == rowM["ID"].ToString() && rowS["ThicknessMM"].ToString() == comboBox2.SelectedItem.ToString())
                        {
                            numericUpDown3.Value = Convert.ToDecimal(rowS["SquarePriceMM"]);

                            numericUpDown3.BackColor = Color.FromName("Info");
                        }
                    }
                }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (DataRow rowB in back.Rows)
            {
                if (rowB["ThicknessMM"].ToString() == comboBox3.SelectedItem.ToString())
                {
                    numericUpDown11.Value = Convert.ToDecimal(rowB["SquarePriceMM"]);

                    numericUpDown11.BackColor = Color.FromName("Info");
                }
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            dbStart();
            label1.Visible = false;
            comboBox2.Visible = false;
            foreach (DataRow rowM in materials.Rows)
            {
                comboBox1.Items.Add(rowM["Type"]);
            }
            foreach (DataRow rowB in back.Rows)
            {
                comboBox3.Items.Add(rowB["ThicknessMM"]);
            }
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void UpdatePrice()
        {

            textBox1.Text = Convert.ToString(numericUpDown1.Value * numericUpDown2.Value * numericUpDown3.Value);
            textBox3.Text = Convert.ToString(numericUpDown6.Value * numericUpDown7.Value);
            textBox4.Text = Convert.ToString(numericUpDown9.Value * numericUpDown10.Value * numericUpDown11.Value);
            textBox5.Text = Convert.ToString(numericUpDown13.Value * numericUpDown14.Value);
            textBox2.Text = Convert.ToString(Convert.ToDecimal(textBox1.Text) + Convert.ToDecimal(textBox3.Text) + Convert.ToDecimal(textBox4.Text) + Convert.ToDecimal(textBox5.Text));

            textBox2.BackColor = Color.FromName("Info");
        }
    }
}
