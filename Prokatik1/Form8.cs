using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Prokat
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверка заполнения полей
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();

                // Получение максимального Код_продавца
                int maxSellerCode = 1; // Если таблица пустая, начнем с 1
                using (SQLiteCommand cmdMax = new SQLiteCommand("SELECT MAX(Код_продавца) FROM Продавцы", conn))
                {
                    object result = cmdMax.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        maxSellerCode = Convert.ToInt32(result) + 1;
                    }
                }

                // Добавление нового продавца
                using (SQLiteCommand cmdInsert = new SQLiteCommand(
                    "INSERT INTO Продавцы (Код_продавца, Фамилия, Имя, Отчество, Контактный_номер) VALUES (@SellerID, @Surname, @Name, @Patronymic, @Phone)", conn))
                {
                    cmdInsert.Parameters.AddWithValue("@SellerID", maxSellerCode);
                    cmdInsert.Parameters.AddWithValue("@Surname", textBox1.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@Name", textBox2.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@Patronymic", textBox3.Text.Trim());
                    cmdInsert.Parameters.AddWithValue("@Phone", textBox4.Text.Trim());
                    cmdInsert.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Продавец успешно добавлен!");
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
