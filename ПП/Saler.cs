using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using CheckBox = System.Windows.Forms.CheckBox;
using Word = Microsoft.Office.Interop.Word;

namespace ПП
{
    public partial class Saler : Form
    {
        public Saler()
        {
            InitializeComponent();
            Vivood();
        }
        int min = Convert.ToInt32(info.min);
        int sec = Convert.ToInt32(info.sec);
        DataSet ds;
        SqlDataAdapter adapter;
        string connectionString = @"Data Source=LAPTOP-TI67Q6CF\SQLEXPRESS;Initial Catalog=Прокат инвентаря;Integrated Security=True";
        string sql = $"select * from Клиент";
        int num = 0;

        private void Saler_FormClosed(object sender, FormClosedEventArgs e)
        {
            info.min = $"{min}";
            info.sec = $"{sec}";
            LK fr = new LK();
            fr.Show();
        }

        private void Saler_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string prov = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            if (textBox7.Text == "" | textBox2.Text == "" | textBox3.Text == "" | textBox4.Text == "" | textBox5.Text == "" | textBox6.Text == "")
            {
                MessageBox.Show("Вы не ввели данные! Пожалуйста введите данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (this.textBox7.Text.Length == 0)
            {
                MessageBox.Show("Вы не ввели данные E-mail!!", "Info");
            }
            else if (!Regex.IsMatch(textBox7.Text, prov))
            {
                MessageBox.Show("Введите коректный E-mail!!", "Info");
            }
            else
            {
                string sqlupdate = $"INSERT Клиент (Фамилия, Имя, Отчество, Паспортные_данные, Дата_рождения, Адрес, Email) values " +
                    $"('{textBox2.Text}', '{textBox3.Text}', '{textBox4.Text}', '{textBox5.Text}', '{dateTimePicker1.Value}', '{textBox6.Text}', '{textBox7.Text}')";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    adapter = new SqlDataAdapter(sqlupdate, connection);
                    ds = new DataSet();
                    adapter.Fill(ds);
                }
                MessageBox.Show("Клиент добавлен!", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
                dateTimePicker1.Value = DateTime.Now;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AllowUserToAddRows = false;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    adapter = new SqlDataAdapter(sql, connection);

                    ds = new DataSet();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1[7, i].Value == DBNull.Value)
                        {
                            dataGridView1[7, i].Value = dataGridView1[6, i].Value;
                        }
                    }
                }
                dataGridView1.Columns[0].Visible = false;
            }
        }

        private void Vivood ()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);

                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1[7, i].Value == DBNull.Value)
                    {
                        dataGridView1[7, i].Value = dataGridView1[6, i].Value;
                    }
                }
            }
            dataGridView1.Columns[0].Visible = false;
        }

        private void Saler_Load(object sender, EventArgs e)
        {
            label14.Text = $"{info.min}:{info.sec}";
            timer1.Interval = 1000;
            timer1.Start();
            if (info.dol == "2")
            {
                button4.Visible = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            string log = $"select * from Клиент where Фамилия like '{textBox1.Text}%'";
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

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            string n = (dataGridView1.CurrentRow.Cells[1].Value.ToString());
            string n1 = (dataGridView1.CurrentRow.Cells[2].Value.ToString());
            string n2 = (dataGridView1.CurrentRow.Cells[3].Value.ToString());
            string fio = n +" "+n1+" "+n2;
            panel1.Visible = false;
            panel2.Visible = true;
            textBox8.Text = fio;
        }

        private double summ = 0;

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {            
            summ = 0;
            summ += (checkBox1.Checked ? 1200 : 0);
            summ += (checkBox2.Checked ? 500 : 0);
            summ += (checkBox3.Checked ? 500 : 0);
            summ += (checkBox4.Checked ? 1000 : 0);
            summ += (checkBox5.Checked ? 300 : 0);
            summ += (checkBox6.Checked ? 800 : 0);
            summ += (checkBox7.Checked ? 800 : 0);
            summ += (checkBox8.Checked ? 100 : 0);
            summ += (checkBox9.Checked ? 300 : 0);
            summ += (checkBox10.Checked ? 450 : 0);
            summ += (checkBox11.Checked ? 300 : 0);
            summ += (checkBox12.Checked ? 400 : 0);
            summ += (checkBox13.Checked ? 500 : 0);            
            textBox10.Text = summ.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked || checkBox2.Checked || checkBox3.Checked || checkBox4.Checked || checkBox5.Checked || checkBox6.Checked || checkBox7.Checked)
            {
                string sqlupdate = $"INSERT Заказы ([Дата создания], IDКлиента, [Время заказа], Количество_услуг, Статус, [Дата закрытия]) values " +
                    $"('{dateTimePicker2.Value}', '{dataGridView1.CurrentRow.Cells[0].Value.ToString()}', '{DateTime.Now.ToString("HH:mm")}', '3', '1', '{dateTimePicker3.Value}')";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    adapter = new SqlDataAdapter(sqlupdate, connection);
                    ds = new DataSet();
                    adapter.Fill(ds);
                }

                //Создаём объект документа
                Word.Document doc = null;
                try
                {
                    // Создаём объект приложения
                    Word.Application app = new Word.Application();
                    // Путь до шаблона документа
                    string source = Path.Combine(Directory.GetCurrentDirectory(), "Чек.docx");
                    // Открываем
                    doc = app.Documents.Add(source);
                    doc.Activate();

                    // Добавляем информацию
                    // wBookmarks содержит все закладки
                    Word.Bookmarks wBookmarks = doc.Bookmarks;
                    Word.Range wRange;
                    int i = 0;
                    num++;
                    string nm = Convert.ToString(num);
                    string[] data = new string[5] { DateTime.Now.ToShortDateString(), textBox9.Text, nm , textBox10.Text, textBox8.Text };
                    foreach (Word.Bookmark mark in wBookmarks)
                    {

                        wRange = mark.Range;
                        wRange.Text = data[i];
                        i++;
                    }

                    // Закрываем документ
                    MessageBox.Show("Заказ успешно оплачен!","Оплата",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    doc.Close();
                    doc = null;

                    info.min = $"{min}";
                    info.sec = $"{sec}";
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
                MessageBox.Show("Вы ничего не заказали!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string minute = Convert.ToString(min);
            string second = Convert.ToString(sec);
            info.min = minute;
            info.sec = second;
            new Rab().Show();Hide();
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
            label14.Text = $"{min}:{sec}";
        }
    }
}