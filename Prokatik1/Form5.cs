using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Prokat
{
    public partial class Form5 : Form
    {
        private int clientID;

        public Form5(int id)
        {
            InitializeComponent();
            clientID = id; // Сохранение переданного Код_клиента

            LoadData();
        }

        private void LoadData()
        {
            LoadTools();
            LoadRentedTools();
        }

        private void LoadTools()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Прайс", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void LoadRentedTools()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Журнал WHERE Код_клиента = @ClientID", conn);
                cmd.Parameters.AddWithValue("@ClientID", clientID);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Columns.Contains("Код_продажи"))
                {
                    dt.Columns.Remove("Код_продажи");
                }

                dataGridView2.DataSource = dt;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT Фамилия, Имя, Отчество FROM Клиенты WHERE Код_клиента = @ClientID", conn))
                {
                    cmd.Parameters.AddWithValue("@ClientID", clientID);
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string surname = reader["Фамилия"].ToString();
                        string name = reader["Имя"].ToString();
                        string patronymic = reader["Отчество"].ToString();

                        // Передача данных в Form3
                        Form3 form3 = new Form3(surname, name, patronymic, clientID);
                        form3.Show();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: данные клиента не найдены.");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            LoadData(); // Обновление данных
        }
    }
}
