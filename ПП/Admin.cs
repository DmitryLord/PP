using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ПП
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }
        int min = Convert.ToInt32(info.min);
        int sec = Convert.ToInt32(info.sec);
        DataSet ds;
        SqlDataAdapter adapter;

        string connectionString = @"Data Source=LAPTOP-TI67Q6CF\SQLEXPRESS;Initial Catalog=Прокат инвентаря;Integrated Security=True";
        string sql = "select Сотрудник.id, Сотрудник.Логин, Сотрудник.Последний_вход, ТипВхода.Наименование " +
            "from Сотрудник inner join ТипВхода on Сотрудник.IDТипВхода = ТипВхода.id";
        private void Admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            info.min = $"{min}";
            info.sec = $"{sec}";
            LK fr = new LK();
            fr.Show();
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            label3.Text = $"{info.min}:{info.sec}";
            timer1.Interval = 1000;
            timer1.Start();

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);

                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].Visible = false;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {

                    if (dataGridView1[3, i].Value == DBNull.Value)
                    {
                        dataGridView1[7, i].Value = dataGridView1[6, i].Value;
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            string log = $"select Сотрудник.id, Сотрудник.Логин, Сотрудник.Последний_вход, ТипВхода.Наименование " +
                $"from Сотрудник inner join ТипВхода on Сотрудник.IDТипВхода = ТипВхода.id where Логин like '{textBox1.Text}%'";
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(log, connection);

                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {

                    if (dataGridView1[3, i].Value == DBNull.Value)
                    {
                        dataGridView1[7, i].Value = dataGridView1[6, i].Value;
                    }
                }
            }
            dataGridView1.Columns[0].Visible = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string ex = $"{dateTimePicker1.Value.ToShortDateString()}";
            dataGridView1.Columns.Clear();
            string date = $"select Сотрудник.id, Сотрудник.Логин, Сотрудник.Последний_вход, ТипВхода.Наименование " +
                $"from Сотрудник inner join ТипВхода on Сотрудник.IDТипВхода = ТипВхода.id where Последний_вход = '{ex}'";
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(date, connection);

                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1[3, i].Value == DBNull.Value)
                    {
                        dataGridView1[7, i].Value = dataGridView1[6, i].Value;
                    }
                }
            }
            dataGridView1.Columns[0].Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sec == 0)
            {
                min = min - 1;
                sec = 59;

            }
            if (min == 5)
            {
                MessageBox.Show("У вас осталось 5 минут до окончания сеанса", "Сеанс", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (label3.Text == "0:0")
            {
                timer1.Stop();
                MessageBox.Show("Ваше время вышло!", "Время", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                info.min = $"{min}";
                info.sec = $"{sec}";
                Form1 fr = new Form1();
                fr.Show();
                this.Hide();
            }
            sec = sec - 1;
            label3.Text = $"{min}:{sec}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            info.min = $"{min}";
            info.sec = $"{sec}";
            Form ot = new Otchet();
            ot.Show();
            this.Hide();
        }
    }
}
