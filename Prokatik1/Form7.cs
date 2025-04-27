using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Prokat
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
            LoadSuppliersIntoComboBox();
        }

        // Метод для заполнения комбобокса поставщиков данными из таблицы "Поставщики".
        // Теперь выбирается только "Код_поставщика", который и будет отображаться.
        private void LoadSuppliersIntoComboBox()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();
                // Запрос выбирает только столбец "Код_поставщика"
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT Код_поставщика FROM Поставщики", conn))
                {
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "Код_поставщика";
                    comboBox1.ValueMember = "Код_поставщика";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверка заполнения полей:
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            int price;
            if (!int.TryParse(textBox2.Text, out price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену.");
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();

                // Получение текущего максимального значения "Код_товара"
                int maxCode = 1; // Если таблица пустая, начнем с 1
                using (SQLiteCommand getMaxCodeCommand = new SQLiteCommand("SELECT MAX(Код_товара) FROM Прайс", conn))
                {
                    object result = getMaxCodeCommand.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        maxCode = Convert.ToInt32(result) + 1; // Увеличиваем на 1
                    }
                }

                // Добавление нового товара
                using (SQLiteCommand cmd = new SQLiteCommand(
                    "INSERT INTO Прайс (Код_товара, Название_товара, Цена, Код_поставщика) VALUES (@ProductCode, @ProductName, @Price, @SupplierID)", conn))
                {
                    cmd.Parameters.AddWithValue("@ProductCode", maxCode); // Используем вычисленный код
                    cmd.Parameters.AddWithValue("@ProductName", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@SupplierID", comboBox1.SelectedValue);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Товар успешно добавлен!");
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Возвращаемся на Form1
            Form1 form1 = new Form1("логин"); // Здесь нужно передать подходящий логин клиента
            this.Close();
        }
    }
}
