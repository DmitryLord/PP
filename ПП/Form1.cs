using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Image = System.Drawing.Image;

namespace ПП
{
    public partial class Form1 : Form
    {
        private string text = String.Empty;
        public Form1()
        {
            InitializeComponent();
        }
        int min = 3;
        int sec = 0;
        int min1 = 10;
        int sec1 = 0;

        DataSet ds;
        SqlDataAdapter adapter;
        int kol;
        string connectionString = @"Data Source=LAPTOP-TI67Q6CF\SQLEXPRESS;Initial Catalog=Прокат инвентаря;Integrated Security=True";
        string pass;
        string log;

        private void button1_Click(object sender, EventArgs e)
        {
            string prov = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            if (textBox1.Text == "" | textBox2.Text == "")
            {
                MessageBox.Show("Вы не ввели данные! Пожалуйста введите данные", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (this.textBox1.Text.Length == 0)
            {
                MessageBox.Show("Введите коректный E-mail!!", "Info");
            }
            else if (!Regex.IsMatch(textBox1.Text, prov))
            {
                MessageBox.Show("Введите коректный E-mail!!", "Info");
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string passcheck = "";
                    string logcheck = "";
                    string stat = "";
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    string query = $"select id from Сотрудник where Пароль='{pass}'";
                    cmd.CommandText = query;
                    cmd.Connection = connection;
                    SqlDataReader myreader = cmd.ExecuteReader()/*ВыполнитьРидер*/;
                    while (myreader.Read())
                    {
                        passcheck = $"{myreader.GetValue(0)}";
                    }
                    myreader.Close();
                    string query2 = $"select id from Сотрудник where Логин='{log}'";
                    cmd.CommandText = query2;
                    cmd.Connection = connection;
                    myreader = cmd.ExecuteReader()/*ВыполнитьРидер*/;
                    while (myreader.Read())
                    {
                        logcheck = $"{myreader.GetValue(0)}";
                    }
                    myreader.Close();

                    if (passcheck != "" & logcheck != "")
                    {
                        string query3 = $"select IDДолжности from Сотрудник where Логин='{log}' and Пароль='{pass}'";
                        cmd.CommandText = query3;
                        cmd.Connection = connection;
                        myreader = cmd.ExecuteReader()/*ВыполнитьРидер*/;
                        while (myreader.Read())
                        {
                            stat = $"{myreader.GetValue(0)}";
                        }
                        myreader.Close();
                        string query4 = $"select id from Сотрудник where Логин='{log}' and Пароль='{pass}'";
                        cmd.CommandText = query4;
                        cmd.Connection = connection;
                        myreader = cmd.ExecuteReader()/*ВыполнитьРидер*/;
                        while (myreader.Read())
                        {
                            info.ID = $"{myreader.GetValue(0)}";
                        }
                        myreader.Close();

                        if (stat == "1")
                        {
                            info.dol = stat;
                            info.log = log;
                            string query6 = $"select ФИО from Сотрудник where Логин='{log}' and Пароль='{pass}'";
                            cmd.CommandText = query6;
                            cmd.Connection = connection;
                            myreader = cmd.ExecuteReader()/*ВыполнитьРидер*/;
                            while (myreader.Read())
                            {
                                info.Name = $"{myreader.GetValue(0)}";
                            }
                            myreader.Close();
                            
                            string sqlupdate = $"update Сотрудник set Последний_вход= '{DateTime.Now.ToShortDateString()}' WHERE id = '{info.ID}'";
                            using (SqlConnection connection1 = new SqlConnection(connectionString))
                            {
                                connection1.Open();
                                adapter = new SqlDataAdapter(sqlupdate, connection1);
                                ds = new DataSet();
                                adapter.Fill(ds);
                            }

                            string sqlupdate1 = $"update Сотрудник set IDТипВхода = '1' WHERE id = '{info.ID}'";
                            using (SqlConnection connection1 = new SqlConnection(connectionString))
                            {
                                connection1.Open();
                                adapter = new SqlDataAdapter(sqlupdate, connection1);
                                ds = new DataSet();
                                adapter.Fill(ds);
                            }

                            MessageBox.Show($"Добро пожаловать {info.Name}", "Авторизация", MessageBoxButtons.OK);
                            string minute = Convert.ToString(min1);
                            string second = Convert.ToString(sec1);
                            info.min = minute;
                            info.sec = second;
                            LK adm = new LK();
                            adm.Show();
                            this.Hide();
                        }
                        else if (stat == "2")
                        {
                            info.dol = stat;
                            info.log = log;
                            string query6 = $"select ФИО from Сотрудник where Логин='{log}' and Пароль='{pass}'";
                            cmd.CommandText = query6;
                            cmd.Connection = connection;
                            myreader = cmd.ExecuteReader()/*ВыполнитьРидер*/;
                            while (myreader.Read())
                            {
                                info.Name = $"{myreader.GetValue(0)}";
                            }

                            string sqlupdate = $"update Сотрудник set Последний_вход= '{DateTime.Now.ToShortDateString()}' WHERE id = '{info.ID}'";
                            using (SqlConnection connection1 = new SqlConnection(connectionString))
                            {
                                connection1.Open();
                                adapter = new SqlDataAdapter(sqlupdate, connection1);
                                ds = new DataSet();
                                adapter.Fill(ds);
                            }

                            string sqlupdate1 = $"update Сотрудник set IDТипВхода = '1' WHERE id = '{info.ID}'";
                            using (SqlConnection connection1 = new SqlConnection(connectionString))
                            {
                                connection1.Open();
                                adapter = new SqlDataAdapter(sqlupdate1, connection1);
                                ds = new DataSet();
                                adapter.Fill(ds);
                            }
                            
                            MessageBox.Show($"Добро пожаловать {info.Name}", "Авторизация", MessageBoxButtons.OK);
                            string minute = Convert.ToString(min1);
                            string second = Convert.ToString(sec1);
                            info.min = minute;
                            info.sec = second;
                            LK sale = new LK();
                            sale.Show();
                            this.Hide();
                        }
                        else
                        {
                            info.dol = stat;
                            info.log = log;
                            string query6 = $"select ФИО from Сотрудник where Логин='{log}' and Пароль='{pass}'";
                            cmd.CommandText = query6;
                            cmd.Connection = connection;
                            myreader = cmd.ExecuteReader()/*ВыполнитьРидер*/;
                            while (myreader.Read())
                            {
                                info.Name = $"{myreader.GetValue(0)}";
                            }

                            string sqlupdate = $"update Сотрудник set Последний_вход = '{DateTime.Now.ToShortDateString()}' WHERE id = '{info.ID}'";
                            using (SqlConnection connection1 = new SqlConnection(connectionString))
                            {
                                connection1.Open();
                                adapter = new SqlDataAdapter(sqlupdate, connection1);
                                ds = new DataSet();
                                adapter.Fill(ds);
                            }
                            string sqlupdate1 = $"update Сотрудник set IDТипВхода = '1' WHERE id = '{info.ID}'";
                            using (SqlConnection connection1 = new SqlConnection(connectionString))
                            {
                                connection1.Open();
                                adapter = new SqlDataAdapter(sqlupdate, connection1);
                                ds = new DataSet();
                                adapter.Fill(ds);
                            }

                            MessageBox.Show($"Добро пожаловать {info.Name}", "Авторизация", MessageBoxButtons.OK);
                            string minute = Convert.ToString(min1);
                            string second = Convert.ToString(sec1);
                            info.min = minute;
                            info.sec = second;
                            LK st = new LK();
                            st.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не правильный логин или пароль", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        kol++;
                        if (kol == 2)
                        {
                            button1.Enabled = false;
                            pictureBox2.Visible = true;
                            textBox3.Visible = true;
                            button2.Visible = true;
                            button3.Visible = true;
                            pictureBox2.Image = this.CreateImage(pictureBox2.Width, pictureBox2.Height);
                        }
                        else if (kol == 3)
                        {
                            MessageBox.Show("Неправильный логин или пароль! Вы заблокированы на 10 сек!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            kol = 0;

                            textBox1.Text = "";
                            textBox1.Enabled = false;
                            textBox2.Text = "";
                            textBox2.Enabled = false;
                            button1.Enabled = false;

                            timer1.Interval = 1000;
                            label3.Text = "10";
                            timer1.Start();
                        }
                        string sqlupdate = $"update Сотрудник set IDТипВхода = '2' WHERE Логин = '{log}'";
                        using (SqlConnection connection1 = new SqlConnection(connectionString))
                        {
                            connection1.Open();
                            adapter = new SqlDataAdapter(sqlupdate, connection1);
                            ds = new DataSet();
                            adapter.Fill(ds);
                        }
                    }
                }
            }
        }
        public Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();

            //Создадим изображение
            Bitmap result = new Bitmap(Width, Height);

            //Вычислим позицию текста
            int Xpos = 10;
            int Ypos = 10;

            //Добавим различные цвета ддя текста
            Brush[] colors = {
            Brushes.Black,
            Brushes.Red,
            Brushes.RoyalBlue,
            Brushes.Green,
            Brushes.Yellow,
            Brushes.White,
            Brushes.Tomato,
            Brushes.Sienna,
            Brushes.Pink };

            //Добавим различные цвета линий
            Pen[] colorpens = {
            Pens.Black,
            Pens.Red,
            Pens.RoyalBlue,
            Pens.Green,
            Pens.Yellow,
            Pens.White,
            Pens.Tomato,
            Pens.Sienna,
            Pens.Pink };

             //Делаем случайный стиль текста
            FontStyle[] fontstyle = {
            FontStyle.Bold,
            FontStyle.Italic,
            FontStyle.Regular,
            FontStyle.Strikeout,
            FontStyle.Underline};

            //Добавим различные углы поворота текста
            Int16[] rotate = { 1, -1, 2, -2, 3, -3, 4, -4, 5, -5, 6, -6 };

            //Укажем где рисовать
            Graphics g = Graphics.FromImage((Image)result);

            //Пусть фон картинки будет серым
            g.Clear(Color.Gray);

            //Делаем случайный угол поворота текста
            g.RotateTransform(rnd.Next(rotate.Length));

            //Генерируем текст
            text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";

                for (int i = 0; i < 5; ++i)
                text += ALF[rnd.Next(ALF.Length)];

            //Нарисуем сгенирируемый текст
            g.DrawString(text, new Font("Arial", 25, fontstyle[rnd.Next(fontstyle.Length)]), colors[rnd.Next(colors.Length)], new PointF(Xpos, Ypos));

            //Добавим немного помех
            //Линии из углов
            g.DrawLine(colorpens[rnd.Next(colorpens.Length)],
            new Point(0, 0),
            new Point(Width - 1, Height - 1));
            g.DrawLine(colorpens[rnd.Next(colorpens.Length)],
            new Point(0, Height - 1),
            new Point(Width - 1, 0));

             //Белые точки
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);

            return result;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            log = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            pass = textBox2.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            textBox2.UseSystemPasswordChar = true;
            if (info.min == "0" && info.sec == "0")
            {
                MessageBox.Show("Вы заблокированы на 3 минуты!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "";
                textBox1.Enabled = false;
                textBox2.Text = "";
                textBox2.Enabled = false;
                button1.Enabled = false;

                timer2.Interval = 1000;
                timer2.Start();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = this.CreateImage(pictureBox2.Width, pictureBox2.Height);
            textBox3.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == text)
            {
                MessageBox.Show("Верно!","CAPTCHA",MessageBoxButtons.OK,MessageBoxIcon.Information);
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                pictureBox2.Visible = false;
                textBox3.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button1.Enabled = true;
            }
            else
            {
                MessageBox.Show("Ошибка!", "CAPTCHA",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }  
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int time = Convert.ToInt32(label3.Text);
            if (time == 1)
            {
                timer1.Stop();
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Enabled = true;

            }
            label3.Text = Convert.ToString(time - 1);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (sec == 0)
            {
                min = min - 1;
                sec = 59;

            }
            else if (label3.Text == "0:0")
            {
                timer1.Stop();
                MessageBox.Show("Вы разблокированы!", "Время", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Enabled = true;
            }
            sec = sec - 1;
            label3.Text = $"{min}:{sec}";
        }
    }
}