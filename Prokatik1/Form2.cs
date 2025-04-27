using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Prokat
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверка заполнения полей
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Заполните поля: Название поставщика и Телефон.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();

                // Получение максимального Код_поставщика
                int maxSupplierCode = 1; // Если таблица пустая, начнем с 1
                using (SQLiteCommand cmdMax = new SQLiteCommand("SELECT MAX(Код_поставщика) FROM Поставщики", conn))
                {
                    object result = cmdMax.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        maxSupplierCode = Convert.ToInt32(result) + 1;
                    }
                }

                // Добавление нового поставщика
                using (SQLiteCommand cmdInsert = new SQLiteCommand(
                    "INSERT INTO Поставщики (Код_поставщика, Название_поставщика, Телефон) VALUES (@SupplierID, @Name, @Phone)", conn))
                {
                    cmdInsert.Parameters.AddWithValue("@SupplierID", maxSupplierCode);
                    cmdInsert.Parameters.AddWithValue("@Name", textBox1.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@Phone", textBox2.Text.Trim());
                    cmdInsert.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Поставщик успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
