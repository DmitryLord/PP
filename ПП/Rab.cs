using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ПП
{
    public partial class Rab : Form
    {
        int min = Convert.ToInt32(info.min);
        int sec = Convert.ToInt32(info.sec);
        DataSet ds;
        SqlDataAdapter adapter;
        public static string connectionString = @"Data Source=LAPTOP-TI67Q6CF\SQLEXPRESS;Initial Catalog=Прокат инвентаря;Integrated Security=True";
        SqlConnection MySqlCon = new SqlConnection(connectionString);

        public Rab()
        {
            InitializeComponent();
            Conclusion();
        }

        private void Conclusion()
        {
            #region [Инвентарь]
            MySqlCon.Open();
            string Model = $"SELECT Наименование from Инвентарь";
            SqlCommand con = new SqlCommand(Model, MySqlCon);
            SqlDataReader read = con.ExecuteReader();
            while (read.Read())
            {
                comboBox1.Items.Add(read.GetValue(0));
            }
            read.Close();
            MySqlCon.Close();
            //comboBox1.SelectedIndex = 0;
            #endregion

            #region [Склад]
            dataGridView1.ClearSelection();
            string Stock = $"select Инвентарь = Инвентарь.Наименование, Склад.Стоимость, Склад.Количество, Склад.Дата " +
            $"from Склад join Инвентарь on Склад.IDинвентаря = Инвентарь.ID ";

            SqlDataAdapter StockC = new SqlDataAdapter(Stock, MySqlCon);
            DataSet Sk = new DataSet();
            StockC.Fill(Sk);
            dataGridView1.DataSource = Sk.Tables[0];
            #endregion

        }

        private void Rab_Load(object sender, EventArgs e)
        {
            label5.Text = $"{info.min}:{info.sec}";
            timer1.Interval = 1000;
            timer1.Start();
        }

        private void Rab_FormClosed(object sender, FormClosedEventArgs e)
        {
            info.min = $"{min}";
            info.sec = $"{sec}";
            new Saler().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "" | textBox1.Text == "" | textBox2.Text == "")
            {
                MessageBox.Show("Вы не ввели данные! Пожалуйста введите данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sqlupdate = $"INSERT Склад (IDИнвентаря, Стоимость, Количество, Дата) values ('{comboBox1.SelectedIndex + 1}', '{textBox2.Text}', '{textBox1.Text}', '{dateTimePicker1.Value}')";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    adapter = new SqlDataAdapter(sqlupdate, connection);
                    ds = new DataSet();
                    adapter.Fill(ds);
                }
                Conclusion();
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
            label5.Text = $"{min}:{sec}";
        }
    }
}
