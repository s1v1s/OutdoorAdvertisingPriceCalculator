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
    public partial class Form2 : Form
    {
        public Form2(DataTable back, DataTable front, DataTable materials)
        {
            InitializeComponent();

            if (Application.OpenForms["Form1"] != null)
            {
                (Application.OpenForms["Form1"] as Form1).TempColumnAdd();
            }

            DataGridView dg1 = dataGridView1;
            DataGridView dg2 = dataGridView2;
            DataGridView dg3 = dataGridView3;

            dg1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dg2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dg3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dg1.AllowUserToAddRows = false;
            dg2.AllowUserToAddRows = false;
            dg3.AllowUserToAddRows = false;
            dg1.AllowUserToDeleteRows = false;
            dg2.AllowUserToDeleteRows = false;
            dg3.AllowUserToDeleteRows = false;

            dg1.DataSource = materials;
            dg2.DataSource = front;
            dg3.DataSource = back;
            dg1.Update();
            dg2.Update();
            dg3.Update();

            dg1.Columns[0].ReadOnly = true;
            dg1.Columns[1].ReadOnly = true;
            dg1.Columns[0].DefaultCellStyle.BackColor = Color.FromName("Control");
            dg1.Columns[1].DefaultCellStyle.BackColor = Color.FromName("Control");
            dg1.Columns[1].HeaderText = "Тип материала";
            dg1.Columns[2].HeaderText = "Цена материала (руб/м)";
            dg1.Columns[3].HeaderText = "Цена фрезеровки (руб/м)";

            dg2.Columns[0].ReadOnly = true;
            dg2.Columns[1].ReadOnly = true;
            dg2.Columns[4].ReadOnly = true;
            dg2.Columns[0].DefaultCellStyle.BackColor = Color.FromName("Control");
            dg2.Columns[1].DefaultCellStyle.BackColor = Color.FromName("Control");
            dg2.Columns[4].DefaultCellStyle.BackColor = Color.FromName("Control");
            dg2.Columns[1].HeaderText = "Толщина (мм)";
            dg2.Columns[2].HeaderText = "Цена материала (руб/м)";
            dg2.Columns[3].HeaderText = "Цена фрезеровки (руб/м)";
            dg2.Columns[4].HeaderText = "Тип материала";
            dg2.Columns[4].DisplayIndex = 1;

            dg3.Columns[0].DefaultCellStyle.BackColor = Color.FromName("Control");
            dg3.Columns[0].ReadOnly = true;
            dg3.Columns[0].HeaderText = "Толщина (мм)";
            dg3.Columns[1].HeaderText = "Цена материала (руб/м)";
            dg3.Columns[2].HeaderText = "Цена фрезеровки (руб/м)";

            dg1.Update();
            dg2.Update();
            dg3.Update();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Если нажать кнопку 'Сохранить расценки в XML файл',\nто в папке создастся файл PriceDataBase.xml.\nПри новом запуске программа возьмёт из него цены.\nЧтобы вернуть цены по умолчанию, нужно удалить\nфайл PriceDataBase.xml");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Form1"] != null)
            {
                (Application.OpenForms["Form1"] as Form1).SaveDataSetToFile();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {   
                if (File.Exists("PriceDataBase.xml"))
                {    
                    File.Delete("PriceDataBase.xml");
                    MessageBox.Show("Файл PriceDataBase.xml удалён\nПерезапустите программу");
                }
                else MessageBox.Show("Файл PriceDataBase.xml не найден");
            }
            catch (IOException ioExp)
            {
                MessageBox.Show(ioExp.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Автор программы: Сивушков Иван Дмитриевич\nEmail:sivushkovivan@gmail.com\nViber,Whatsapp:+79996421446");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Application.OpenForms["Form1"] != null)
            {
                (Application.OpenForms["Form1"] as Form1).TempColumnClean();
            }
            
        }
        private void dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            MessageBox.Show("В ячейку введён не правильный тип данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            e.Cancel = false;
        }
    }
}
