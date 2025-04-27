using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Prokat
{
    public partial class Form1 : Form
    {
        private string clientLogin;

        public Form1(string login)
        {
            InitializeComponent();
            clientLogin = login;

            // Настраиваем TextBox2 для поиска
            textBox2.TextChanged += textBox2_TextChanged;

            RefreshDatabase();
        }

        // Обновление данных для всех таблиц
        private void RefreshDatabase()
        {
            UpdateClients();      // DataGridView1: таблица "Клиенты"
            UpdateSuppliers();    // DataGridView2: таблица "Поставщики"
            UpdateSellers();      // DataGridView3: таблица "Продавцы"
            UpdatePriceList();    // DataGridView4: таблица "Прайс"
            UpdateJournal();      // DataGridView5: таблица "Журнал"
        }

        private void UpdateClients()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();
                // Изменяем запрос: исключаем поле "Пароль" и клиента с Код_клиента = 1
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(
                    "SELECT Код_клиента, Имя, Фамилия, Отчество, Номер FROM Клиенты WHERE Код_клиента != 1", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }


        private void UpdateSuppliers()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Поставщики", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView2.DataSource = dt;
            }
        }

        private void UpdateSellers()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Продавцы", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView3.DataSource = dt;
            }
        }

        private void UpdatePriceList()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Прайс", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView4.DataSource = dt;
            }
        }

        private void UpdateJournal()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM Журнал", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView5.DataSource = dt;
            }
        }

        // Поиск по Код_Клиента через TextBox
        private void textBoxClientCode_TextChanged(object sender, EventArgs e)
        {
            string clientCode = textBox1.Text.Trim();

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(
                    $"SELECT * FROM Клиенты WHERE Код_клиента LIKE '{clientCode}%'", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Columns.Contains("Пароль"))
                {
                    dt.Columns.Remove("Пароль");
                }

                dataGridView1.DataSource = dt;
            }
        }


        // Фильтрация через TextBox2
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string productCode = textBox2.Text.Trim(); // Получаем введённое значение

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(
                    $"SELECT * FROM Журнал WHERE Код_товара LIKE '{productCode}%'", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView5.DataSource = dt; // Обновляем DataGridView5
            }
        }

        // Обработчики кнопок
        private void button4_Click(object sender, EventArgs e)
        {
            RefreshDatabase();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int clientId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Код_клиента"].Value);
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Клиенты WHERE Код_клиента = @ClientID", conn);
                    cmd.Parameters.AddWithValue("@ClientID", clientId);
                    cmd.ExecuteNonQuery();
                }
                RefreshDatabase();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int supplierId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["Код_поставщика"].Value);
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Поставщики WHERE Код_поставщика = @SupplierID", conn);
                    cmd.Parameters.AddWithValue("@SupplierID", supplierId);
                    cmd.ExecuteNonQuery();
                }
                RefreshDatabase();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count > 0)
            {
                int productId = Convert.ToInt32(dataGridView4.SelectedRows[0].Cells["Код_товара"].Value);
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Прайс WHERE Код_товара = @ProductID", conn);
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.ExecuteNonQuery();
                }
                RefreshDatabase();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8();
            form8.Show();
        }


        private void button11_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                int sellerId = Convert.ToInt32(dataGridView3.SelectedRows[0].Cells["Код_продавца"].Value);
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Продавцы WHERE Код_продавца = @SellerID", conn);
                    cmd.Parameters.AddWithValue("@SellerID", sellerId);
                    cmd.ExecuteNonQuery();
                }
                RefreshDatabase();
                MessageBox.Show("Продавец успешно удалён.");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку с данными продавца для удаления.");
            }
        }




        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}
