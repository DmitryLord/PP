using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ПП
{
    public partial class LK : Form
    {
        public LK()
        {
            InitializeComponent();
        }
        int min = Convert.ToInt32(info.min);
        int sec = Convert.ToInt32(info.sec);
        DataSet ds;
        SqlDataAdapter adapter;
        OpenFileDialog ofd = new OpenFileDialog();
        public static string connectionString = @"Data Source=LAPTOP-TI67Q6CF\SQLEXPRESS;Initial Catalog=Прокат инвентаря;Integrated Security=True";
        private SqlConnection MySqlCon = new SqlConnection(connectionString);

        private void LK_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();
        }

        private void LK_Load(object sender, EventArgs e)
        {
            label5.Text = $"{info.min}:{info.sec}";
            timer1.Interval = 1000;
            timer1.Start();

            MySqlCon.Open();
            SqlCommand Profile = new SqlCommand($"SELECT Логин, Пароль, ФИО FROM Сотрудник WHERE Логин = '{info.log}'", MySqlCon);
            SqlDataReader reade = Profile.ExecuteReader();
            while (reade.Read())
            {
                textBox3.Text = reade.GetString(0);
                textBox4.Text = reade.GetString(1);
                textBox1.Text = reade.GetString(2);
            }
            reade.Close();
            MySqlCon.Close();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    string query = $"select Фото from Сотрудник where Логин='{info.log}'";
                    cmd.CommandText = query;
                    cmd.Connection = connection;
                    string cmdres = cmd.ExecuteScalar().ToString();
                    pictureBox1.Image = new Bitmap(cmdres);

                    query = $"select Наименование from Должность where id='{info.dol}'";
                    cmd.CommandText = query;
                    cmd.Connection = connection;
                    cmdres = cmd.ExecuteScalar().ToString();
                    textBox2.Text = cmdres;
                }
                catch
                {
                    MessageBox.Show("УПС!", "Личный кабинет", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }   }

        private void button2_Click(object sender, EventArgs e)
        {
            string sqlupdate = $"update Сотрудник set ФИО ='{textBox1.Text}', Логин = '{textBox3.Text}', Пароль = '{textBox4.Text}', Фото = '{ofd.FileName}' WHERE Логин = '{info.log}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sqlupdate, connection);
                ds = new DataSet();
                adapter.Fill(ds);
            }
            MessageBox.Show("Данные успешно изменены");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ofd.Filter = "Image Files(*.JPG;*.JPEG;)|*.JPG;*.JPEG; | All files(*.*) | *.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = new Bitmap(ofd.FileName);
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть выбранный файл", "Ошибка");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (info.dol == "1")
            {
                info.min = $"{min}";
                info.sec = $"{sec}";
                Admin adm = new Admin();
                adm.Show();
                this.Hide();
            }
            else if (info.dol == "2")
            {
                info.min = $"{min}";
                info.sec = $"{sec}";
               Saler sale = new Saler();
                sale.Show();
                this.Hide();
            }
            else
            {
                info.min = $"{min}";
                info.sec = $"{sec}";
                new Saler().Show();Hide();
            }
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
            else if (label5.Text == "0:0")
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
            label5.Text = $"{min}:{sec}"; 
        }
    }
}
