using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace zakharchenko_kn_3_2_lab5
{
    public partial class Form1 : Form
    {
        private const int B = 11; // значення B
        private const string C = "помаранчевий"; // значення C

        private CancellationTokenSource cancellationTokenSource;

        private NumericUpDown numUpDownIterations;
        private NumericUpDown numUpDownThreads;
        private TextBox textBoxComments;
        private ProgressBar progressBar;

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            Text = "ЛР № 5, автор: Захарченко Альона кн-3-2";

            // Label та NumericUpDown для кількості ітерацій J
            Label labelIterations = new Label
            {
                Text = "Кількість ітерацій J:",
                Location = new System.Drawing.Point(10, 10),
            };
            numUpDownIterations = new NumericUpDown
            {
                Minimum = 100,
                Maximum = 1000000,
                Value = 10000,
                Location = new System.Drawing.Point(150, 8),
            };

            // Label та NumericUpDown для кількості потоків T
            Label labelThreads = new Label
            {
                Text = "Кількість потоків T:",
                Location = new System.Drawing.Point(10, 40),
            };
            numUpDownThreads = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 100,
                Value = 1,
                Location = new System.Drawing.Point(150, 38),
            };

            // Кнопка для запуску паралельних обчислень
            Button btnStart = new Button
            {
                Text = "Запустити паралельні обчислення",
                Location = new System.Drawing.Point(10, 70),
            };
            btnStart.Click += BtnStart_Click;

            // Багаторядкове текстове поле для виведення коментарів
            textBoxComments = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Location = new System.Drawing.Point(10, 100),
                Size = new System.Drawing.Size(300, 150),
            };

            // Кнопки для управління обчисленнями
            Button btnCancel = new Button
            {
                Text = "Скасувати обчислення",
                Location = new System.Drawing.Point(10, 260),
            };
            btnCancel.Click += BtnCancel_Click;

            Button btnPause = new Button
            {
                Text = "Призупинити обчислення",
                Location = new System.Drawing.Point(10, 290),
            };
            btnPause.Click += BtnPause_Click;

            Button btnResume = new Button
            {
                Text = "Відновити обчислення",
                Location = new System.Drawing.Point(10, 320),
            };
            btnResume.Click += BtnResume_Click;

            // Індикатор виконання
            progressBar = new ProgressBar
            {
                Location = new System.Drawing.Point(10, 350),
                Size = new System.Drawing.Size(300, 20),
                Maximum = 100,
                Minimum = 0,
            };

            // Додавання елементів на форму
            Controls.Add(labelIterations);
            Controls.Add(numUpDownIterations);
            Controls.Add(labelThreads);
            Controls.Add(numUpDownThreads);
            Controls.Add(btnStart);
            Controls.Add(textBoxComments);
            Controls.Add(btnCancel);
            Controls.Add(btnPause);
            Controls.Add(btnResume);
            Controls.Add(progressBar);
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            int iterations = (int)numUpDownIterations.Value;
            int threadsCount = (int)numUpDownThreads.Value;

            cancellationTokenSource = new CancellationTokenSource();

            progressBar.Value = 0;

            progressBar.ForeColor = System.Drawing.Color.Orange;

            // Замінено textBoxComments на параметр методу
            LogMessage($"Початок паралельних обчислень. Кількість потоків: {threadsCount}. Загальна кількість ітерацій: {iterations}.");

            try
            {
                await RunParallelCalculations(iterations, threadsCount, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                LogMessage("Обчислення скасовано.");
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }

        private async void BtnPause_Click(object sender, EventArgs e)
        {
            await Task.Delay(100); // Затримка, щоб не блокувати інтерфейс
            LogMessage("Обчислення призупинено.");
        }

        private async void BtnResume_Click(object sender, EventArgs e)
        {
            await Task.Delay(100); // Затримка, щоб не блокувати інтерфейс
            LogMessage("Відновлення обчислень.");
        }

        private async Task RunParallelCalculations(int iterations, int threadsCount, CancellationToken cancellationToken)
        {
            double result = 0;

            await Task.Run(() =>
            {
                Parallel.For(0, threadsCount, new ParallelOptions { CancellationToken = cancellationToken }, i =>
                {
                    int start = i * (iterations / threadsCount) + 1;
                    int end = (i == threadsCount - 1) ? iterations : (i + 1) * (iterations / threadsCount);

                    for (int j = start; j <= end; j++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        // Реалізувати обчислення за формулою A
                        result += j;

                        // Засинання на B мілісекунд
                        Thread.Sleep(B);

                        // Оновлення прогресу
                        double progress = (double)j / iterations * 100;
                        progressBar.Invoke(new Action(() => { progressBar.Value = (int)progress; }));
                    }
                });
            });

            CheckResult(result);
     
            LogMessage($"Обчислення завершено. Результат: {result}.");
        }

        private void CheckResult(double result)
        {
            // Логіка перевірки точності результату
            double expectedValue = CalculateExpectedValue();
            double tolerance = 0.0001;

            if (Math.Abs(result - expectedValue) < tolerance)
            {
                LogMessage("Результат вірний.");
            }
            else
            {
                LogMessage("Результат невірний.");
            }
        }

        private double CalculateExpectedValue()
        {
            // Логіка для обчислення очікуваного значення A 
            int iterations = (int)numUpDownIterations.Value;
            return (iterations * (iterations + 1)) / 2.0;
        }

        private void LogMessage(string message)
        {
            textBoxComments.AppendText($"{message}\n");

            // Автоматична прокрутка до останнього результату
            textBoxComments.SelectionStart = textBoxComments.Text.Length;
            textBoxComments.ScrollToCaret();
        }
    }
}

