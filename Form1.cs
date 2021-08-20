using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ad_calculator
{
    public partial class Form1 : Form
    {
        public Form1(DataTable back, DataTable front, DataTable materials)
        {
            InitializeComponent();


            //dataGridView1[1,1].Value=
            dataGridView1.DataSource = materials;
            dataGridView1.Update();

            dataGridView1.Columns[1].HeaderText = "Тип материала";
            dataGridView1.Columns[2].HeaderText = "Цена материала (руб/м)";
            dataGridView1.Columns[3].HeaderText = "Цена фрезеровки (руб/м)";

            dataGridView1.Update();

            dataGridView2.DataSource = front;
            dataGridView2.Update();

            dataGridView2.Columns[1].HeaderText = "Толщина (мм)";
            dataGridView2.Columns[2].HeaderText = "Цена материала (руб/м)";
            dataGridView2.Columns[3].HeaderText = "Цена фрезеровки (руб/м)";

            dataGridView2.Update();
            
            dataGridView3.DataSource = back;
            dataGridView3.Update();
            dataGridView3.Columns[0].HeaderText = "Толщина (мм)";
            dataGridView3.Columns[1].HeaderText = "Цена материала (руб/м)";
            dataGridView3.Columns[2].HeaderText = "Цена фрезеровки (руб/м)";

            dataGridView3.Update();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
