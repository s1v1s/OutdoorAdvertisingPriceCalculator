using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Ad_calculator
{
    public partial class Form1 : Form
    {
        public DataTable back = new DataTable("Back");
        public DataTable front = new DataTable("Front");
        public DataTable materials = new DataTable("Materials");
        public DataSet priceSet = new DataSet("PriceSet");
        int meter = 1000;
        int comboBox4PerviousIndex;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            DataBaseStart();
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

            comboBox4.Items.Add("М");
            comboBox4.Items.Add("ММ");
            comboBox4.SelectedIndex = 0;
            comboBox4PerviousIndex = comboBox4.SelectedIndex;
        }

        private void DataBaseStart()
        {
            priceSet.Tables.Add(materials);
            priceSet.Tables.Add(front);
            priceSet.Tables.Add(back);

            // таблица materials
            materials.Columns.Add("ID", Type.GetType("System.Int32"));
            materials.Columns.Add("Type", Type.GetType("System.String"));
            materials.Columns.Add("SquarePriceMM", Type.GetType("System.Int32"));
            materials.Columns.Add("CutPriceFront", Type.GetType("System.Int32"));

            // таблица front (ПВХ)
            front.Columns.Add("ID", Type.GetType("System.Int32"));
            front.Columns.Add("ThicknessMM", Type.GetType("System.Int32"));
            front.Columns.Add("SquarePriceMM", Type.GetType("System.Int32"));
            front.Columns.Add("CutPriceFront", Type.GetType("System.Int32"));

            // таблица back
            back.Columns.Add("ThicknessMM", Type.GetType("System.Int32"));
            back.Columns.Add("SquarePriceMM", Type.GetType("System.Int32"));
            back.Columns.Add("CutPriceBack", Type.GetType("System.Int32"));

            DataSet ds = new DataSet();

            try
            {
                try
                {
                    ds.ReadXml("PriceDataBase.xml");
                    priceSet.Merge(ds, true, MissingSchemaAction.Ignore);
                }
                catch(System.Xml.XmlException)
                {
                    File.Delete("PriceDataBase.xml");
                    FillDefault();
                }
            }
            catch (FileNotFoundException)
            {
                FillDefault();
            };
        }

        private void FillDefault()
        {
            // заполнение талбиц данными
            materials.Rows.Add(new object[] { 1, "ПВХ + Oracal", null, null });
            materials.Rows.Add(new object[] { 2, "Акрил", 1700, 80  });
            materials.Rows.Add(new object[] { 3, "ПВХ + Краска", null, null });
            materials.Rows.Add(new object[] { 4, "АКП", 1500 , 60  });

            back.Rows.Add(new object[] { "5", 800 , 70  });
            back.Rows.Add(new object[] { "8", 1000 , 80  });
            back.Rows.Add(new object[] { "0", 0, 0 });

            front.Rows.Add(new object[] { 1, "3", 600 , 60  });
            front.Rows.Add(new object[] { 1, "5", 900 , 60  });
            front.Rows.Add(new object[] { 1, "10", 1800 , 90  });
            front.Rows.Add(new object[] { 1, "19", 6400 , 120  });

            front.Rows.Add(new object[] { 3, "3", 600 , 60  });
            front.Rows.Add(new object[] { 3, "5", 900 , 60  });
            front.Rows.Add(new object[] { 3, "10", 1800 , 90  });
            front.Rows.Add(new object[] { 3, "19", 6400 , 120  });
        }

        private void UpdatePrice()
        {
            textBox1.Text = Convert.ToString(numericUpDown1.Value * numericUpDown2.Value * numericUpDown3.Value);
            textBox3.Text = Convert.ToString(numericUpDown6.Value * numericUpDown7.Value);
            textBox4.Text = Convert.ToString(numericUpDown9.Value * numericUpDown10.Value * numericUpDown11.Value);
            textBox5.Text = Convert.ToString(numericUpDown13.Value * numericUpDown14.Value);
            textBox2.Text = Convert.ToString(Convert.ToDecimal(textBox1.Text) + Convert.ToDecimal(textBox3.Text) + Convert.ToDecimal(textBox4.Text) + Convert.ToDecimal(textBox5.Text));
            ChangeColor();
        }

        private void ChangeColor()
        {
            switch (numericUpDown3.Value)
            {
                case 0:
                    numericUpDown3.BackColor = Color.FromName("Control");
                    break;
                default:
                    numericUpDown3.BackColor = Color.FromName("Info");
                    break;
            }

            switch (numericUpDown7.Value)
            {
                case 0:
                    numericUpDown7.BackColor = Color.FromName("Control");
                    break;
                default:
                    numericUpDown7.BackColor = Color.FromName("Info");
                    break;
            }

            switch (numericUpDown11.Value)
            {
                case 0:
                    numericUpDown11.BackColor = Color.FromName("Control");
                    break;
                default:
                    numericUpDown11.BackColor = Color.FromName("Info");
                    break;
            }

            switch (numericUpDown14.Value)
            {
                case 0:
                    numericUpDown14.BackColor = Color.FromName("Control");
                    break;
                default:
                    numericUpDown14.BackColor = Color.FromName("Info");
                    break;
            }

            switch (textBox2.Text)
            {
                case "0":
                    textBox2.BackColor = Color.FromName("Control");
                    break;
                default:
                    textBox2.BackColor = Color.FromName("Info");
                    break;
            }
        }

        private void CleanScreen()
        {
            numericUpDown3.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown11.Value = 0;
            numericUpDown14.Value = 0;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            ChangeColor();
        }

        public void SaveDataSetToFile()
        {
            TempColumnClean();
            priceSet.WriteXml("PriceDataBase.xml");
            TempColumnAdd();
            if (File.Exists("PriceDataBase.xml"))
            {
                MessageBox.Show("Файл PriceDataBase.xml сохранён");
            }
            else { MessageBox.Show("Произошла ошибка"); }
        }
        public void TempColumnAdd()
        {
            front.Columns.Add("Type", Type.GetType("System.String"));
            foreach (DataRow rowS in front.Rows)
            {
                foreach (DataRow rowM in materials.Rows)
                {
                    if (rowM["ID"].ToString() == rowS["ID"].ToString())
                    {
                        rowS["Type"] = rowM["Type"];
                    }
                }
            }
        }

        public void TempColumnClean()
        {
            try
            {
                front.Columns.Remove("Type");
            }
            catch { Exception e; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveDataSetToFile();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CleanScreen();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form Form1 = new Form2(back, front, materials);
            Form1.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = -1;
            comboBox2.Items.Clear();

            if (comboBox1.SelectedIndex != -1)
            {
                if (comboBox1.SelectedItem.ToString().Contains("ПВХ") == true)
                {
                    numericUpDown7.Value = 0;
                    label1.Visible = true;
                    comboBox2.Visible = true;

                    foreach (DataRow rowM in materials.Rows)
                    {
                        if (rowM["Type"].ToString() == comboBox1.SelectedItem.ToString())
                        {
                            foreach (DataRow rowS in front.Rows)
                            {
                                numericUpDown3.Value = 0;

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
                            numericUpDown3.Value = Convert.ToDecimal(rowM["SquarePriceMM"]) / meter;
                            numericUpDown7.Value = Convert.ToDecimal(rowM["CutPriceFront"]) / meter;
                            ChangeColor();
                        }
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1)
            {
                foreach (DataRow rowM in materials.Rows)
                {
                    if (rowM["Type"].ToString() == comboBox1.SelectedItem.ToString())
                    {
                        foreach (DataRow rowS in front.Rows)
                        {
                            if (rowS["ID"].ToString() == rowM["ID"].ToString() && rowS["ThicknessMM"].ToString() == comboBox2.SelectedItem.ToString())
                            {
                                numericUpDown3.Value = Convert.ToDecimal(rowS["SquarePriceMM"]) / meter;
                                numericUpDown7.Value = Convert.ToDecimal(rowS["CutPriceFront"]) / meter;
                                ChangeColor();
                            }
                        }
                    }
                }
            }                
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex != -1)
            {
                foreach (DataRow rowB in back.Rows)
                {
                    if (rowB["ThicknessMM"].ToString() == comboBox3.SelectedItem.ToString())
                    {
                        numericUpDown11.Value = Convert.ToDecimal(rowB["SquarePriceMM"]) / meter;
                        numericUpDown14.Value = Convert.ToDecimal(rowB["CutPriceBack"]) / meter;
                        ChangeColor();
                    }
                }
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox4.SelectedIndex)
            {
                case 0:
                    if (comboBox4PerviousIndex != 0)
                    {
                        numericUpDown3.Value *= 1000;
                        numericUpDown7.Value *= 1000;
                        numericUpDown11.Value *= 1000;
                        numericUpDown14.Value *= 1000;
                        comboBox4PerviousIndex = 0;

                        label1.Text = "Толщина ПВХ лицевой стороны (м)";
                        label2.Text = "Площадь лицевой стороны (м)";
                        label3.Text = "Цена (руб/м)";
                        label6.Text = "Длина фрезеровки лицевой стороны (м)";
                        label7.Text = "Цена (руб/м)";
                        label9.Text = "Толщина ПВХ задника (м)";
                        label10.Text = "Площадь задника (м)";
                        label12.Text = "Цена (руб/м)";
                        label14.Text = "Длина фрезеровки задника (м)";
                        label15.Text = "Цена (руб/м)";
                    }
                    meter = 1;
                    break;
                case 1:
                    if(comboBox4PerviousIndex != 1)
                    {
                        numericUpDown3.Value /= 1000;
                        numericUpDown7.Value /= 1000;
                        numericUpDown11.Value /= 1000;
                        numericUpDown14.Value /= 1000;
                        comboBox4PerviousIndex = 1;

                        label1.Text = "Толщина ПВХ лицевой стороны (мм)";
                        label2.Text = "Площадь лицевой стороны (мм)";
                        label3.Text = "Цена (руб/мм)";
                        label6.Text = "Длина фрезеровки лицевой стороны (мм)";
                        label7.Text = "Цена (руб/мм)";
                        label9.Text = "Толщина ПВХ задника (мм)";
                        label10.Text = "Площадь задника (мм)";
                        label12.Text = "Цена (руб/мм)";
                        label14.Text = "Длина фрезеровки задника (мм)";
                        label15.Text = "Цена (руб/мм)";
                    }
                    meter = 1000;
                    break;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
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
    }
}
