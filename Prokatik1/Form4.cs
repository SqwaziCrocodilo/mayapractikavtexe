using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Prokat
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            textBox4.PasswordChar = '*'; // Скрытие пароля
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Задаём административные данные
            const string adminSurname = "Васин";
            const string adminName = "Александр";
            const string adminPatronymic = "Константинович";
            const string adminPassword = "123";

            // Проверяем, совпадают ли введённые данные с административными
            if (textBox1.Text.Trim().Equals(adminSurname, StringComparison.OrdinalIgnoreCase) &&
                textBox2.Text.Trim().Equals(adminName, StringComparison.OrdinalIgnoreCase) &&
                textBox3.Text.Trim().Equals(adminPatronymic, StringComparison.OrdinalIgnoreCase) &&
                textBox4.Text.Trim() == adminPassword)
            {
                // Если администратор, открываем Form1
                Form1 adminForm = new Form1(textBox1.Text.Trim());
                adminForm.Show();
                this.Hide();
                return;
            }

            // Если данные не совпадают с административными, проверяем клиента через базу данных
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;"))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT Код_клиента FROM Клиенты WHERE Фамилия = @Surname AND Имя = @Name AND Отчество = @Patronymic AND Пароль = @Password", conn))
                {
                    cmd.Parameters.AddWithValue("@Surname", textBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@Name", textBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@Patronymic", textBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", textBox4.Text.Trim());

                    SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int clientID = Convert.ToInt32(reader["Код_клиента"]);
                        Form5 form5 = new Form5(clientID);
                        form5.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Неверные данные клиента.", "Ошибка входа");
                    }
                }
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();
            form6.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
