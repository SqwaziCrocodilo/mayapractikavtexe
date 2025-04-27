using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Prokat
{
    public class productType
    {
        public int id;
        public string name;

        public productType(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }

    public partial class Form3 : Form
    {
        private int clientID; // Идентификатор клиента
        List<productType> product = new List<productType>();

        // Обновлённая строка подключения с параметрами Pooling и BusyTimeout
        private readonly string connString = "Data Source=E:\\Prokatik21\\Prokatik2\\Prokatik1\\BAZA\\BAZA.db;Pooling=True;BusyTimeout=5000;";

        public Form3(string surname, string name, string patronymic, int id)
        {
            InitializeComponent();
            clientID = id;
            // Отображаем ФИО клиента
            textBox1.Text = $"{surname} {name} {patronymic}";
            LoadProducts();
        }

        private void LoadProducts()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT Код_товара, Название_товара FROM Прайс", conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                product.Add(new productType(
                                    Convert.ToInt32(reader["Код_товара"]),
                                    reader["Название_товара"].ToString()));
                            }
                        }
                    }

                    foreach (productType i in product)
                    {
                        comboBox2.Items.Add(i.name);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке товаров: " + ex.Message);
                }
            }
        }

        private int GetProductPrice(string productName)
        {
            int price = 0;
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT Цена FROM Прайс WHERE Название_товара = @Product", conn))
                    {
                        cmd.Parameters.AddWithValue("@Product", productName);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                price = Convert.ToInt32(reader["Цена"]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка получения цены: " + ex.Message);
                }
            }
            return price;
        }

        private int GetRandomSellerId()
        {
            int maxSellerId = 0;

            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT MAX(Код_продавца) AS MaxId FROM Продавцы", conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                maxSellerId = Convert.ToInt32(reader["MaxId"]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка получения максимального Код_продавца: " + ex.Message);
                }
            }

            // Генерируем случайное число от 1 до maxSellerId
            Random rand = new Random();
            return rand.Next(1, maxSellerId + 1); // Верхняя граница включена
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, выбран ли товар
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите товар.", "Ошибка");
                return;
            }

            string selectedProduct = comboBox2.SelectedItem.ToString();
            int quantity;
            // Проверка корректности введенного количества
            if (string.IsNullOrWhiteSpace(textBox3.Text) ||
                !int.TryParse(textBox3.Text, out quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество товара в поле 'Количество'.", "Ошибка");
                return;
            }

            int pricePerUnit = GetProductPrice(selectedProduct);
            if (pricePerUnit <= 0)
            {
                MessageBox.Show("Не удалось получить цену товара.", "Ошибка");
                return;
            }

            int totalSum = quantity * pricePerUnit;
            textBox2.Text = totalSum.ToString();
            bool insertSuccess = false;

            int randomSellerId = GetRandomSellerId(); // Генерируем случайный Код_продавца

            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                try
                {
                    conn.Open();
                    string sql = "INSERT INTO \"Журнал\" (\"Код_товара\", \"Количество\", \"Дата_продажи\", \"Сумма_продажи\", \"Код_клиента\", \"Код_продавца\") " +
                                 "VALUES (@ProductID, @Quantity, @SaleDate, @TotalSum, @ClientID, @SellerID)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        // Находим выбранный товар по его имени в списке productType
                        productType selectProd = new productType(-1, "N/A");
                        foreach (productType i in product)
                        {
                            if (i.name == selectedProduct)
                            {
                                selectProd = i;
                                break;
                            }
                        }
                        cmd.Parameters.AddWithValue("@ProductID", selectProd.id);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@SaleDate", DateTime.Today.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@TotalSum", totalSum);
                        cmd.Parameters.AddWithValue("@ClientID", clientID);
                        cmd.Parameters.AddWithValue("@SellerID", randomSellerId); // Добавляем случайный Код_продавца
                        cmd.ExecuteNonQuery();
                        insertSuccess = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при записи в журнал: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (insertSuccess)
            {
                MessageBox.Show("Инструмент успешно арендован и добавлен в журнал!", "Успех");
                ResetForm();
            }
        }

        private void ResetForm()
        {
            comboBox2.SelectedItem = null;
            textBox3.Clear(); // Очищаем поле "Количество"
            textBox2.Clear(); // Очищаем поле "Сумма продажи"
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close(); // Закрытие формы
        }
    }
}
