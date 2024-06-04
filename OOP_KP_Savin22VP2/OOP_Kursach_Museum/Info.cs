using System;
using System.Windows.Forms;

namespace OOP_Kursach_09
{
    /// <summary>
    /// Класс формы Info, отображающей информацию с автоматическим закрытием по таймеру.
    /// </summary>
    public partial class Info : Form
    {
        private Timer inactivityTimer;

        /// <summary>
        /// Инициализирует новый экземпляр класса Info.
        /// </summary>
        public Info()
        {
            InitializeComponent();

            // Инициализация таймера
            inactivityTimer = new Timer();
            inactivityTimer.Interval = 10000; // 10 секунд
            inactivityTimer.Tick += InactivityTimer_Tick;

            this.Shown += Info_Shown; // Событие, когда форма отображена
            this.MouseMove += ResetInactivityTimer; // Событие движения мыши
            this.KeyPress += ResetInactivityTimer; // Событие нажатия клавиш
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Предотвращение изменения размера формы
            this.MaximizeBox = false; // Отключить кнопку разворачивания
        }

        /// <summary>
        /// Обработчик события отображения формы.
        /// Запускает таймер при отображении формы.
        /// </summary>
        private void Info_Shown(object sender, EventArgs e)
        {
            // Запуск таймера при отображении формы
            inactivityTimer.Start();
        }

        /// <summary>
        /// Сбрасывает таймер неактивности при взаимодействии с формой.
        /// </summary>
        private void ResetInactivityTimer(object sender, EventArgs e)
        {
            // Перезапуск таймера при взаимодействии
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }

        /// <summary>
        /// Обработчик события тика таймера.
        /// Закрывает текущую форму и открывает главную форму при истечении времени.
        /// </summary>
        private void InactivityTimer_Tick(object sender, EventArgs e)
        {
            // Закрытие формы при истечении времени
            Main mainForm = new Main(); // Создание объекта главной формы
            mainForm.Show(); // Отображение главной формы
            this.Hide(); // Скрытие текущей формы
            inactivityTimer.Stop(); // Остановка таймера при закрытии формы
        }

        /// <summary>
        /// Обработчик события клика по кнопке.
        /// Закрывает текущую форму и открывает главную форму.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            Main mainForm = new Main(); // Создание объекта главной формы
            mainForm.Show(); // Отображение главной формы
            this.Hide(); // Скрытие текущей формы
            inactivityTimer.Stop(); // Остановка таймера при закрытии формы
        }

        private void Info_Load(object sender, EventArgs e)
        {

        }
    }
}
