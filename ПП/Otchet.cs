using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;


namespace ПП
{
    public partial class Otchet : Form
    {
        int min = Convert.ToInt32(info.min);
        int sec = Convert.ToInt32(info.sec);
        public static string connectionString = @"Data Source=LAPTOP-TI67Q6CF\SQLEXPRESS;Initial Catalog=Прокат инвентаря;Integrated Security=True";
        private SqlConnection MySqlcon = new SqlConnection(connectionString);
        private List<int> MAC = new List<int>();
        private float MAC_mean = 0;
        private List<string> Ingredients = new List<string>();
        private string Title;
        public Otchet()
        {
            InitializeComponent();
        }
        DataSet ds;
        SqlDataAdapter adapter;
        
        
        // выбор показа отчёта графиком, таблицей или всё вместе
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Всё")
            {
                label2.Visible = true;
                dateTimePicker1.Visible = true;
                dataGridView1.Visible = true;
                label3.Visible = true;
                dateTimePicker2.Visible = true;

                MySqlcon.Open();

                SqlCommand conn = new SqlCommand();
                conn.Connection = MySqlcon;

                conn.CommandText =
                    $"SELECT " +
                    $"Заказы.ID," +
                    $"Заказы.[Дата создания]," +
                    $"Клиент.Фамилия AS Клиент," +
                    $"Заказы.[Время заказа]," +
                    $"Заказы.Количество_услуг AS Услуги, " +
                    $"Статус.Наименование AS Статус, " +
                    $"Заказы.[Дата закрытия]" +
                    $"FROM Заказы " +
                    $"LEFT JOIN Статус ON Заказы.Статус = Статус.ID " +
                    $"LEFT JOIN Клиент ON Заказы.IDклиента = Клиент.id ";
                    
                SqlDataReader read = conn.ExecuteReader();
                while (read.Read())
                {
                    dataGridView1.Rows.Add(read["ID"], read["Клиент"], read["Дата создания"], read["Время заказа"], read["Услуги"], read["Статус"], read["Дата закрытия"]);
                    MAC.Add(Convert.ToInt32(read["Услуги"]));
                    Ingredients.Add(read["Дата создания"].ToString());
                }
                read.Close();
                MySqlcon.Close();
                diagram(MAC, Ingredients);
            }
            else if (comboBox1.Text == "График")
            {
                label2.Visible = true;
                dateTimePicker1.Visible = true;
                dataGridView1.Visible = false;
                label3.Visible = true;
                dateTimePicker2.Visible = true;
                chart1.Visible = true;
                MySqlcon.Open();

                SqlCommand conn = new SqlCommand();
                conn.Connection = MySqlcon;

                conn.CommandText =
                    $"SELECT " +
                    $"Заказы.ID," +
                    $"Заказы.[Дата создания]," +
                    $"Клиент.Фамилия AS Клиент," +
                    $"Заказы.[Время заказа]," +
                    $"Заказы.Количество_услуг AS Услуги, " +
                    $"Статус.Наименование AS Статус, " +
                    $"Заказы.[Дата закрытия]" +
                    $"FROM Заказы " +
                    $"LEFT JOIN Статус ON Заказы.Статус = Статус.ID " +
                    $"LEFT JOIN Клиент ON Заказы.IDклиента = Клиент.id ";

                SqlDataReader read = conn.ExecuteReader();
                while (read.Read())
                {
                    dataGridView1.Rows.Add(read["ID"], read["Клиент"], read["Дата создания"], read["Время заказа"], read["Услуги"], read["Статус"], read["Дата закрытия"]);
                    MAC.Add(Convert.ToInt32(read["Услуги"]));
                    Ingredients.Add(read["Дата создания"].ToString());
                }
                read.Close();
                MySqlcon.Close();
                diagram(MAC, Ingredients);   
            }
            else
            {
                label2.Visible = true;
                dateTimePicker1.Visible = true;
                dataGridView1.Visible = true;
                label3.Visible = true;
                dateTimePicker2.Visible = true;

                MySqlcon.Open();

                SqlCommand conn = new SqlCommand();
                conn.Connection = MySqlcon;

                conn.CommandText =
                    $"SELECT " +
                    $"Заказы.ID," +
                    $"Заказы.[Дата создания]," +
                    $"Клиент.Фамилия AS Клиент," +
                    $"Заказы.[Время заказа]," +
                    $"Заказы.Количество_услуг AS Услуги, " +
                    $"Статус.Наименование AS Статус, " +
                    $"Заказы.[Дата закрытия]" +
                    $"FROM Заказы " +
                    $"LEFT JOIN Статус ON Заказы.Статус = Статус.ID " +
                    $"LEFT JOIN Клиент ON Заказы.IDклиента = Клиент.id ";

                SqlDataReader read = conn.ExecuteReader();
                while (read.Read())
                {
                    dataGridView1.Rows.Add(read["ID"], read["Клиент"], read["Дата создания"], read["Время заказа"], read["Услуги"], read["Статус"], read["Дата закрытия"]);
                    MAC.Add(Convert.ToInt32(read["Услуги"]));
                    Ingredients.Add(read["Дата создания"].ToString());
                }
                read.Close();
                MySqlcon.Close();
            }
        }
        // Выбираем дату периода
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            button1.Visible = true;
            
        }
        // вывод в таблицу заказов у казанный период
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            chart1.Series.Clear();
            chart1.Titles[0].Text = "";
            button2.Visible = true;
            string time1 = dateTimePicker1.Value.ToShortDateString();
            string time2 = dateTimePicker2.Value.ToShortDateString();
            MySqlcon.Open();

            SqlCommand conn = new SqlCommand();
            conn.Connection = MySqlcon;

            conn.CommandText =
                $"SELECT " +
                $"Заказы.ID," +
                $"Заказы.[Дата создания]," +
                $"Клиент.Фамилия AS Клиент," +
                $"Заказы.[Время заказа]," +
                $"Заказы.Количество_услуг AS Услуги, " +
                $"Статус.Наименование AS Статус, " +
                $"Заказы.[Дата закрытия]" +
                $"FROM Заказы " +
                $"LEFT JOIN Статус ON Заказы.Статус = Статус.ID " +
                $"LEFT JOIN Клиент ON Заказы.IDклиента = Клиент.id " +
                $"where Заказы.[Дата создания] between '{time1}' and '{time2}'";

            SqlDataReader read = conn.ExecuteReader();
            while (read.Read())
            {
                dataGridView1.Rows.Add(read["ID"], read["Клиент"], read["Дата создания"], read["Время заказа"], read["Услуги"], read["Статус"], read["Дата закрытия"]);
                MAC.Add(Convert.ToInt32(read["Услуги"]));
                Ingredients.Add(read["Дата создания"].ToString());
                dataGridView1.Columns[0].Visible = false;
            }
            
            read.Close();
            MySqlcon.Close();
            diagram(MAC, Ingredients);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;
        }
        // импорт в PDF
        private void SaveToPDF2(string fileName)
        {
            try
            {
                using (Aspose.Pdf.Document doc = new Aspose.Pdf.Document())
                {
                    Aspose.Pdf.Page page = doc.Pages.Add();
                    SaveToPngTABLE(fileName);

                    Aspose.Pdf.Image картинка = new Aspose.Pdf.Image();
                    картинка.File = fileName;

                    page.Paragraphs.Add(картинка);
                    SaveToPngChart(fileName + "Q");

                    Aspose.Pdf.Image картинка2 = new Aspose.Pdf.Image();
                    картинка2.File = fileName + "Q";
                    page.Paragraphs.Add(картинка2);

                    doc.Save(fileName + @".pdf");
                }

                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Exists)
                    fileInfo.Delete();
                fileInfo = new FileInfo(fileName + "Q");
                if (fileInfo.Exists)
                    fileInfo.Delete();

                MessageBox.Show("TABLE AND CHART pdf save");
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "Ошибка");
            }
        }

        private void SaveToPDFChart(string fileName)
        {
            try
            {
                using (Aspose.Pdf.Document doc = new Aspose.Pdf.Document())
                {
                    Aspose.Pdf.Page page = doc.Pages.Add();
                    SaveToPngChart(fileName);

                    Aspose.Pdf.Image картинка = new Aspose.Pdf.Image();
                    картинка.File = fileName;
                    page.Paragraphs.Add(картинка);

                    doc.Save(fileName + @".pdf");
                }

                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Exists)
                    fileInfo.Delete();
                MessageBox.Show("chart pdf save");
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "Ошибка");
            }
        }

        private void SaveToPDFTABLE(string fileName)
        {
            try
            {
                using (Aspose.Pdf.Document doc = new Aspose.Pdf.Document())
                {
                    Aspose.Pdf.Page page = doc.Pages.Add();
                    SaveToPngTABLE(fileName);

                    Aspose.Pdf.Image картинка = new Aspose.Pdf.Image();
                    картинка.File = fileName;
                    page.Paragraphs.Add(картинка);

                    doc.Save(fileName + @".pdf");
                }

                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Exists)
                    fileInfo.Delete();
                MessageBox.Show("TABLE pdf save");
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "Ошибка");
            }
        }

        public void SaveToPngTABLE(string fileName)
        {
            //Resize DataGridView to full height.
            int height = dataGridView1.Height;
            dataGridView1.Height = dataGridView1.RowCount * dataGridView1.RowTemplate.Height;

            //Create a Bitmap and draw the DataGridView on it.
            Bitmap bitmap = new Bitmap(930, 422);
            dataGridView1.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));

            //Resize DataGridView back to original height.
            dataGridView1.Height = height;

            //Save the Bitmap to folder.

            bitmap.Save(Directory.GetCurrentDirectory() + @"\" + fileName);
        }

        public void SaveToPngChart(string fileName)
        {
            //Resize DataGridView to full height.
            int height = chart1.Height;

            //Create a Bitmap and draw the DataGridView on it.
            Bitmap bitmap = new Bitmap(495, 422);
            chart1.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, this.chart1.Width, this.chart1.Height));

            //Resize DataGridView back to original height.
            chart1.Height = height;

            //Save the Bitmap to folder.

            bitmap.Save(Directory.GetCurrentDirectory() + @"\" + fileName);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveToPDF2("Оба");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveToPDFTABLE("Table");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveToPDFChart("Chart");
        }
        // Вывод графика
        private void diagram(List<int> MAC_Level, List<string> Ingredients)
        {
            chart1.Titles.Add("Отчёт");
            chart1.Titles[0].Text = "Отчёт";
            chart1.Titles[0].Visible = true;
            chart1.Series.Clear();
            for (int i = 0; i < Ingredients.Count(); i++)
            {
                if (chart1.Series.IndexOf(Ingredients[i]) == -1)
                {
                    chart1.Series.Add(Ingredients[i]);
                    chart1.Series[chart1.Series.Count - 1].Points.Add(MAC_Level[i]);
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                }
                else
                {
                    chart1.Series[chart1.Series.IndexOf(Ingredients[i])].Points[0].YValues[0] += MAC_Level[i];
                }
            }
        }

        private void Otchet_FormClosed(object sender, FormClosedEventArgs e)
        {
            info.min = $"{min}";
            info.sec = $"{sec}";
            Admin admin = new Admin();
            admin.Show();
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
            else if (label4.Text == "0:0")
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
            label4.Text = $"{min}:{sec}";
        }
    }
}