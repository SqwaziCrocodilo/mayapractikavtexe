using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Prokat
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            textBox4.PasswordChar = '*'; // Только пароль скрывается
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            if (!textBox5.Text.StartsWith("+7") || textBox5.Text.Length != 12 || !System.Text.RegularExpressions.Regex.IsMatch(textBox5.Text.Substring(2), @"^\d+$"))
            {
                MessageBox.Show("Номер телефона должен начинаться с '+7', содержать 12 символов и только цифры.");
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();

                // Получение максимального значения Код_клиента
                int maxClientCode = GetMaxClientCode(conn);

                // Добавление нового клиента с правильным идентификатором
                using (SQLiteCommand cmd = new SQLiteCommand(
                    "INSERT INTO Клиенты (Код_клиента, Фамилия, Имя, Отчество, Пароль, Номер) VALUES (@ClientID, @Surname, @Name, @Patronymic, @Password, @Phone)", conn))
                {
                    cmd.Parameters.AddWithValue("@ClientID", maxClientCode + 1); // Увеличиваем идентификатор на 1
                    cmd.Parameters.AddWithValue("@Surname", textBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@Name", textBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@Patronymic", textBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", textBox4.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", textBox5.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Клиент успешно добавлен!");
            this.Hide();
        }

        private int GetMaxClientCode(SQLiteConnection conn)
        {
            // Получение текущего максимального значения Код_клиента из таблицы
            using (SQLiteCommand cmdMax = new SQLiteCommand("SELECT MAX(Код_клиента) FROM Клиенты", conn))
            {
                object result = cmdMax.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
