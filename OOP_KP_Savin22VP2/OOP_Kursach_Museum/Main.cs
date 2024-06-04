using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OOP_Kursach_09
{
    /// <summary>
    /// Основная форма приложения для управления.
    /// </summary>
    public partial class Main : Form
    {
        private List<Competition> users = new List<Competition>();
        private ContextMenuStrip contextMenuStrip;


        /// <summary>
        /// Конструктор формы.
        /// </summary>
        public Main()
        {
            InitializeComponent();
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;
            buttonDeleteDatabase.Click += buttonDeleteDatabase_Click;
            button2.Click += button2_Click; // Добавляем обработчик для button2
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            textBox1.TextChanged += TextBox1_TextChanged;
            ComboBoxFilterByName.SelectedIndexChanged += FilterComboBoxes_Changed;
            ComboBoxFilterByYear.SelectedIndexChanged += FilterComboBoxes_Changed;

            contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("Удалить").Click += Delete_Click;
            dataGridView1.ContextMenuStrip = contextMenuStrip;
            dataGridView1.MouseDown += DataGridView1_MouseDown;

        }

        /// <summary>
        /// Обработчик события загрузки формы.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DataTable table = new DataTable();
            DataTable table2 = new DataTable();
            table.Columns.Add("Название конкурса", typeof(string));
            table.Columns.Add("Год проведения", typeof(int));
            table.Columns.Add("Кол-во конкурсантов", typeof(int));
            table.Columns.Add("Кол-во посетителей", typeof(int));
            table.Columns.Add("Кол-во работ", typeof(int));

            table2.Columns.Add("Кол-во конкурсов в год", typeof(int));
            table2.Columns.Add("Год проведения", typeof(int));

            users = FileManager.ReadFromFile();
            foreach (var user in users)
            {
                table.Rows.Add(user.GetName(), user.GetYear(), user.GetMem(), user.GetWorks(), user.GetVisitors());
            }
            Dictionary<int, int> yearCounts = new Dictionary<int, int>();
            foreach (var  number in users)
            {
                int key = number.GetYear();
                if (yearCounts.ContainsKey(key))
                {
                    yearCounts[key]++;
                }
                else
                {
                    yearCounts[key] = 1;
                }
            }
            foreach (var entry in yearCounts.OrderBy(x => x.Key))
            {
                DataRow row = table2.NewRow();
                row["Кол-во конкурсов в год"] = entry.Value;
                row["Год проведения"] = entry.Key;
                table2.Rows.Add(row);
            }

            dataGridView1.DataSource = table;
            dataGridView2.DataSource = table2;
            UpdateFilterComboBoxes();
            UpdateExhibitCount();
        }

        /// <summary>
        /// Обновляет значения в комбобоксах фильтров.
        /// </summary>
        private void UpdateFilterComboBoxes()
        {
            ComboBoxFilterByName.Items.Clear();
            ComboBoxFilterByName.Items.Add("");
            foreach (var user in users)
            {
                if (!ComboBoxFilterByName.Items.Contains(user.GetName()))
                {
                    ComboBoxFilterByName.Items.Add(user.GetName());
                }
            }

            ComboBoxFilterByYear.Items.Clear();
            ComboBoxFilterByYear.Items.Add("");
            foreach (var user in users)
            {
                if (!ComboBoxFilterByYear.Items.Contains(user.GetYear().ToString()))
                {
                    ComboBoxFilterByYear.Items.Add(user.GetYear().ToString());
                }
            }
        }

        /// <summary>
        /// Обработчик клика по ячейке DataGridView.
        /// </summary>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBoxName.Text = row.Cells["Название конкурса"].Value.ToString();
                textBoxYear.Text = row.Cells["Год проведения"].Value.ToString();
                textBoxConk.Text = row.Cells["Кол-во конкурсантов"].Value.ToString();
                textBoxWorks.Text= row.Cells["Кол-во посетителей"].Value.ToString();
                textBoxMem.Text = row.Cells["Кол-во работ"].Value.ToString();
            }
        }

        /// <summary>
        /// Обработчик клика по кнопке добавления.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            if (int.TryParse(textBoxYear.Text, out int year) && int.TryParse(textBoxMem.Text, out int mem) && int.TryParse(textBoxConk.Text, out int conk) && int.TryParse(textBoxWorks.Text, out int works))
            {
                if (year>=0 && mem>=0 &&conk>=0&&works>=0) {
                    int currentYear = DateTime.Now.Year;
                    if (year <= currentYear)
                    {
                        var comp = new Competition(name, year, conk, mem, works);
                        users.Add(comp);
                        UpdateData(users);
                        FileManager.AppendToFile(comp);
                        UpdateExhibitCount();
                    }
                    else
                    {
                        MessageBox.Show("Год не может быть больше текущего года");
                    }
                }
                else
                {
                    MessageBox.Show("Параметры не могут быть отрицательными");
                }
            }
            else
            {
                MessageBox.Show("Все параметры кроме названия должны быть числом");
            }
            UpdateFilterComboBoxes();
        }

        /// <summary>
        /// Записывает данные пользователей в файл.
        /// </summary>
        private void WriteDataToFile()
        {
            FileManager.WriteToFile(users);
            UpdateFilterComboBoxes();
        }

        /// <summary>
        /// Обработчик изменения фильтров.
        /// </summary>
        private void FilterComboBoxes_Changed(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Обработчик изменения текста в текстовом поле поиска.
        /// </summary>
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Применяет фильтры к списку пользователей.
        /// </summary>
        private void ApplyFilters()
        {
            string filterByName = ComboBoxFilterByName.Text.Trim();
            string filterByYearText = ComboBoxFilterByYear.Text.Trim();
            int filterByYear;
            bool filterByYearEnabled = int.TryParse(filterByYearText, out filterByYear);

            var filteredUsers = users;
            if (!string.IsNullOrWhiteSpace(filterByName))
            {
                filteredUsers = filteredUsers.Where(u => u.GetName().Contains(filterByName)).ToList();
            }

            if (filterByYearEnabled)
            {
                filteredUsers = filteredUsers.Where(u => u.GetYear() == filterByYear).ToList();
            }
            string searchText = textBox1.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                filteredUsers = filteredUsers.Where(u => u.GetName().Contains(searchText) || u.GetYear().ToString().Contains(searchText) || 
                u.GetVisitors().ToString().Contains(searchText) || u.GetMem().ToString().Contains(searchText) || u.GetWorks().ToString().Contains(searchText)).ToList();
            }

            UpdateData(filteredUsers);
            UpdateExhibitCount();
        }

        /// <summary>
        /// Обновляет данные в DataGridView.
        /// </summary>
        private void UpdateData(List<Competition> filteredUsers)
        {
            DataTable table = (DataTable)dataGridView1.DataSource;
            DataTable table2 = (DataTable)dataGridView2.DataSource;
            table.Rows.Clear();
            table2.Rows.Clear();
            foreach (var user in filteredUsers)
            {
                table.Rows.Add(user.GetName(), user.GetYear(), user.GetMem(), user.GetVisitors(), user.GetWorks());
            }
            Dictionary<int, int> yearCounts = new Dictionary<int, int>();
            foreach (var number in users)
            {
                int key = number.GetYear();
                if (yearCounts.ContainsKey(key))
                {
                    yearCounts[key]++;
                }
                else
                {
                    yearCounts[key] = 1;
                }
            }
            foreach (var entry in yearCounts.OrderBy(x => x.Key))
            {
                DataRow row = table2.NewRow();
                row["Кол-во конкурсов в год"] = entry.Value;
                row["Год проведения"] = entry.Key;
                table2.Rows.Add(row);
            }
        }

        /// <summary>
        /// Обновляет количество конкурсов.
        /// </summary>
        private void UpdateExhibitCount()
        {
            labelCount.Text = $"Количество конкурсов: {dataGridView1.Rows.Count}";
        }

        /// <summary>
        /// Обработчик клика по кнопке удаления базы данных.
        /// </summary>
        private void buttonDeleteDatabase_Click(object sender, EventArgs e)
        {
            users.Clear();
            DataTable table = (DataTable)dataGridView1.DataSource;
            DataTable table2 = (DataTable)dataGridView2.DataSource;
            table.Rows.Clear();
            table2.Rows.Clear();
            FileManager.DeleteFile();
            UpdateExhibitCount();
            UpdateFilterComboBoxes();
        }

        /// <summary>
        /// Обработчик изменения значения ячейки DataGridView.
        /// </summary>
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string name = dataGridView1.Rows[e.RowIndex].Cells["Название конкурса"].Value.ToString();
                int year = (int)dataGridView1.Rows[e.RowIndex].Cells["Год проведения"].Value;
                int conk = (int)dataGridView1.Rows[e.RowIndex].Cells["Кол-во конкурсантов"].Value;
                int mem = (int)dataGridView1.Rows[e.RowIndex].Cells["Кол-во посетителей"].Value;
                int work = (int)dataGridView1.Rows[e.RowIndex].Cells["Кол-во работ"].Value;

                users[e.RowIndex] = new Competition(name, year, conk,mem,work);
                WriteDataToFile();
                UpdateExhibitCount();
            }
        }

        /// <summary>
        /// Обработчик изменения состояния редактируемой ячейки DataGridView.
        /// </summary>
        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            if (dataGridView2.IsCurrentCellDirty)
            {
                dataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// Обработчик нажатия правой кнопкой мыши на DataGridView.
        /// </summary>
        private void DataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hit = dataGridView1.HitTest(e.X, e.Y);
                dataGridView1.ClearSelection();
                if (hit.RowIndex >= 0)
                {
                    dataGridView1.Rows[hit.RowIndex].Selected = true;
                    contextMenuStrip.Show(dataGridView1, e.Location);
                }
            }
        }

        /// <summary>
        /// Обработчик клика по пункту контекстного меню "Удалить".
        /// </summary>
        private void Delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                users.RemoveAt(index);
                dataGridView1.Rows.RemoveAt(index);
                WriteDataToFile();
                UpdateExhibitCount();
                UpdateFilterComboBoxes();
            }
        }

        /// <summary>
        /// Обработчик клика по кнопке редактирования.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Index >= 0)
            {
                int selectedIndex = dataGridView1.CurrentRow.Index;
                string previousYear = dataGridView1.CurrentRow.Cells["Год проведения"].Value.ToString();

                if (int.TryParse(textBoxYear.Text, out int year) && int.TryParse(textBoxMem.Text, out int mem) && int.TryParse(textBoxConk.Text, out int conk) && int.TryParse(textBoxWorks.Text, out int works))
                {
                    // Валидация года                
                    int currentYear = DateTime.Now.Year;
                    if (year > currentYear)
                    {
                        MessageBox.Show("Год создания не может быть больше текущего года.");
                        textBoxYear.Text = previousYear; // Возвращаем предыдущее значение
                        return;
                    }

                    // Обновление DataTable
                    DataTable table = (DataTable)dataGridView1.DataSource;
                    table.Rows[selectedIndex]["Название конкурса"] = textBoxName.Text;
                    table.Rows[selectedIndex]["Год проведения"] = year;
                    table.Rows[selectedIndex]["Кол-во конкурсантов"] =conk;
                    table.Rows[selectedIndex]["Кол-во посетителей"] = mem;
                    table.Rows[selectedIndex]["Кол-во работ"] = works;

                    // Обновление списка users
                    users[selectedIndex] = new Competition(textBoxName.Text, year, conk, mem,works);

                    // Запись обновленных данных обратно в файл
                    WriteDataToFile();

                    // Обновление количества 
                    UpdateExhibitCount();
                }
                else
                {
                    MessageBox.Show("Все параметры кроме названия должны быть числами");
                    textBoxYear.Text = previousYear; // Возвращаем предыдущее значение
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для редактирования.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
