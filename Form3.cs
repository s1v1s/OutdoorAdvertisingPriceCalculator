using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ad_calculator
{
    public partial class Form3 : Form
    {
        int multiplier = 1;
        bool dataChanged, comboBox1Changed, comboBox2Changed, comboBox3Changed, changedByVoid = false;

        DataSet priceSet = new DataSet("PriceSet");
        DataTable materials = new DataTable("Materials");
        DataTable side = new DataTable("Side");
        DataTable back = new DataTable("Back");
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            dbStart();
            comboBox4.SelectedItem = "ММ";
            foreach (DataRow row in materials.Rows)
            {
                comboBox1.Items.Add(row["Name"]);
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataChanged == true || checkBox1.Checked == true)
            {
                priceSet.WriteXml("db2.xml");
            }
        }

        private void UpdatePrice()
        {
            numericUpDown7.Value = numericUpDown1.Value * numericUpDown4.Value * multiplier;
            numericUpDown8.Value = numericUpDown2.Value * numericUpDown5.Value * multiplier;
            numericUpDown9.Value = numericUpDown3.Value * numericUpDown6.Value * multiplier;
            numericUpDown10.Value = numericUpDown7.Value + numericUpDown8.Value + numericUpDown9.Value;
        }

        private void SaveChanges()
        {
            if (changedByVoid == false)
            {
                if (comboBox1Changed == true && comboBox2Changed == true && comboBox3Changed == true)
                {
                    // проверяем строки таблицы Materials
                    foreach (DataRow rowM in materials.Rows)
                    {
                        //Соотносим Name в таблице и выбранное значение Name
                        if (rowM["Name"].ToString() == comboBox1.SelectedItem.ToString())
                        {
                            // проверяем нужно ли изменять CutPrice для выбранного Name в таблице
                            if (Convert.ToDecimal(rowM["CutPrice"]) != numericUpDown4.Value)
                            {
                                rowM["CutPrice"] = numericUpDown4.Value;
                            }
                            // в таблице side по id находим строку и проверяем нужно ли изменять PricePerMeter для этого id
                            foreach (DataRow rowS in side.Rows)
                            {
                                if (rowS["MaterialID"].ToString() == rowM["ID"].ToString() && Convert.ToDecimal(rowS["PricePerMeter"]) != numericUpDown5.Value)
                                {
                                    rowS["PricePerMeter"] = numericUpDown5.Value;
                                }
                            }

                            // в таблице back по id находим строку и проверяем нужно ли изменять PricePerMeter для этого id
                            foreach (DataRow rowB in back.Rows)
                            {
                                if (rowB["MaterialID"].ToString() == rowM["ID"].ToString() && Convert.ToDecimal(rowB["PricePerMeter"]) != numericUpDown6.Value)
                                {
                                    rowB["PricePerMeter"] = numericUpDown6.Value;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                changedByVoid = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveChanges();
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
            SaveChanges();            
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
            SaveChanges();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrice();
            SaveChanges();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1Changed = true;
            comboBox2.SelectedItem = "";
            comboBox3.SelectedItem = "";
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();

            foreach (DataRow rowM in materials.Rows)
            {
                if (rowM["Name"].ToString() == comboBox1.SelectedItem.ToString())
                {
                    changedByVoid = true;
                    numericUpDown4.Value = Convert.ToDecimal(rowM["CutPrice"]);

                    foreach (DataRow rowS in side.Rows)
                    {
                        if (rowS["MaterialID"].ToString() == rowM["ID"].ToString())
                        {
                            comboBox2.Items.Add(rowS["ThicknessMM"]);
                        }
                    }
                    foreach (DataRow rowB in back.Rows)
                    {
                        if (rowB["MaterialID"].ToString() == rowM["ID"].ToString())
                        {
                            comboBox3.Items.Add(rowB["ThicknessMM"]);
                        }
                    }
                }
            }
            UpdatePrice();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2Changed = true;
            foreach (DataRow rowM in materials.Rows)
            {
                if (rowM["Name"].ToString() == comboBox1.SelectedItem.ToString())
                {
                    foreach (DataRow rowS in side.Rows)
                    {
                        if (rowS["MaterialID"].ToString() == rowM["ID"].ToString())
                        {
                            changedByVoid = true;
                            numericUpDown5.Value = Convert.ToDecimal(rowS["PricePerMeter"]);
                        }
                    }
                }
            }
            UpdatePrice();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3Changed = true;
            foreach (DataRow rowM in materials.Rows)
            {
                if (rowM["Name"].ToString() == comboBox1.SelectedItem.ToString())
                {
                    foreach (DataRow rowB in back.Rows)
                    {
                        if (rowB["MaterialID"].ToString() == rowM["ID"].ToString())
                        {
                            changedByVoid = true;
                            numericUpDown6.Value = Convert.ToDecimal(rowB["PricePerMeter"]);
                        }
                    }
                }
            }
            UpdatePrice();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox4.SelectedItem)
            {
                case "ММ":
                    multiplier = 1;
                    UpdatePrice();
                    break;
                case "СМ":
                    multiplier = 10;
                    UpdatePrice();
                    break;
                case "ДМ":
                    multiplier = 100;
                    UpdatePrice();
                    break;
                case "М":
                    multiplier = 1000;
                    UpdatePrice();
                    break;
            }

        }

        public void dbStart()
        {            
            priceSet.Tables.Add(materials);
            priceSet.Tables.Add(side);
            priceSet.Tables.Add(back);

            // таблица materials
            materials.Columns.Add("ID", Type.GetType("System.Int32"));
            materials.Columns.Add("Name", Type.GetType("System.String"));
            materials.Columns.Add("CutPrice", Type.GetType("System.Int32"));
            //materials.PrimaryKey = new[] { materials.Columns[0] };
            // таблица side
            back.Columns.Add("MaterialID", Type.GetType("System.Int32"));
            back.Columns.Add("ThicknessMM", Type.GetType("System.Int32"));
            back.Columns.Add("PricePerMeter", Type.GetType("System.Int32"));

            // таблица back
            side.Columns.Add("MaterialID", Type.GetType("System.Int32"));
            side.Columns.Add("ThicknessMM", Type.GetType("System.Int32"));
            side.Columns.Add("PricePerMeter", Type.GetType("System.Int32"));

            // запись базы данных в xml файл
            //priceSet.WriteXml("db1.xml");
            DataSet ds = new DataSet();

            try 
            { 
                ds.ReadXml("db2.xml"); 
                priceSet.Merge(ds, true, MissingSchemaAction.Ignore);
            }
            catch (System.IO.FileNotFoundException) 
            {
                fillDefault();
            };
        }

        /*           
        priceSet.Merge(ds, true); //, MissingSchemaAction.Ignore
        priceSet.WriteXml("db2.xml");
        // при объединении возникает ошибка, как её решить не понимаю. Если вставить третий аргумент, то данные плюсуются, вся база удваивается.
        // Понял из-за чего ошибка: при считывании xml файла всем параметрам даётся СТРОКОВЫЙ тип
        // Отмена MERGE всегда плюсует и не перезаписывает*/

        // Идея - есть 2 датасета: дефолтный и из XML. Если xml нет в папке с программой, то берётся дефолтный, и наоборот
        // Если в папке есть xml - записываем данные из него в датасет2 и объединяем с пустым датасетом,
        // а если нет, то пустой датасет1 заполняется дефолтными значениями

        /*https://stackoverflow.com/questions/1346132/how-do-i-extract-data-from-a-datatable
        https://docs.microsoft.com/ru-ru/dotnet/api/system.data.datatable.rows?view=net-5.0
        https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/keywords/switch */


        public void fillDefault()
        {
            // заполнение талбиц данными
            materials.Rows.Add(new object[] { 1, "ПВХ + Oracal", 10 });
            materials.Rows.Add(new object[] { 2, "Акрил", 20 });
            materials.Rows.Add(new object[] { 3, "ПВХ + Краска", 30 });
            materials.Rows.Add(new object[] { 4, "АКП", 40 });

            back.Rows.Add(new object[] { 1, "8", 50 });
            back.Rows.Add(new object[] { 2, "7", 40 });
            back.Rows.Add(new object[] { 3, "6", 30 });
            back.Rows.Add(new object[] { 4, "5", 20 });

            side.Rows.Add(new object[] { 1, "8", 50 });
            side.Rows.Add(new object[] { 2, "7", 40 });
            side.Rows.Add(new object[] { 3, "6", 30 });
            side.Rows.Add(new object[] { 4, "5", 20 });
        }
    }
}
