using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Lab6_Switcher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        private Panel sidebar;
        private Panel contentPanel;
        private Form activeChild;
        private Label lblTitle;

        public MainForm()
        {
            SetupMainUI();
        }

        private void SetupMainUI()
        {
            this.Text = "Лабораторная работа №6 - Гаврилов Артём, Ис24";
            this.Size = new Size(950, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(850, 600);
            this.BackColor = Color.White;

            sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(40, 44, 52)
            };

            var lblHeader = new Label
            {
                Text = "ВЫБОР ВАРИАНТА",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(150, 150, 150),
                Location = new Point(20, 20),
                AutoSize = true
            };
            sidebar.Controls.Add(lblHeader);

            Button btnCalc = CreateSidebarButton("Калькулятор", 20, 60);
            btnCalc.Click += (s, e) => SwitchTo(new CalculatorForm(), "Вариант 1: Калькулятор");

            Button btnNotes = CreateSidebarButton("Заметки", 20, 120);
            btnNotes.Click += (s, e) => SwitchTo(new NotesForm(), "Вариант 2: Менеджер заметок");

            sidebar.Controls.Add(btnCalc);
            sidebar.Controls.Add(btnNotes);

            Panel topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.White
            };

            lblTitle = new Label
            {
                Text = "Выберите вариант в меню слева",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                Location = new Point(20, 12),
                AutoSize = true
            };
            topBar.Controls.Add(lblTitle);

            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 245, 245),
                Padding = new Padding(15)
            };

            this.Controls.Add(contentPanel);
            this.Controls.Add(topBar);
            this.Controls.Add(sidebar);
        }

        private Button CreateSidebarButton(string text, int x, int y)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(210, 45),
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                BackColor = Color.FromArgb(55, 60, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
        }

        private void SwitchTo(Form child, string title)
        {
            if (activeChild != null)
            {
                activeChild.Close();
                activeChild.Dispose();
                activeChild = null;
            }

            lblTitle.Text = title;
            activeChild = child;
            activeChild.TopLevel = false;
            activeChild.FormBorderStyle = FormBorderStyle.None;
            activeChild.Dock = DockStyle.Fill;
            activeChild.BackColor = Color.White;

            contentPanel.Controls.Add(activeChild);
            activeChild.Show();
        }
    }

    public class CalculatorForm : Form
    {
        private TextBox txtDisplay;
        private double firstOperand = 0;
        private string currentOperator = "";
        private bool isNewEntry = true;
        private readonly List<string> history = new List<string>();
        private Panel buttonPanel;

        public CalculatorForm()
        {
            SetupCalculatorUI();
        }

        private void SetupCalculatorUI()
        {
            this.Size = new Size(400, 600);
            this.Padding = new Padding(15);
            this.BackColor = Color.White;

            txtDisplay = new TextBox
            {
                Location = new Point(15, 15),
                Size = new Size(360, 70),
                Font = new Font("Segoe UI", 32F, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(20, 20, 20),
                Text = "0",
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(txtDisplay);

            Button btnHistory = new Button
            {
                Location = new Point(15, 95),
                Size = new Size(360, 45),
                Text = "История операций",
                Font = new Font("Segoe UI", 11F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(230, 230, 230),
                ForeColor = Color.FromArgb(40, 40, 40),
                Cursor = Cursors.Hand
            };
            btnHistory.Click += (s, e) => MessageBox.Show(
                history.Count == 0 ? "История пуста." : string.Join("\n", history),
                "История", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Controls.Add(btnHistory);

            buttonPanel = new Panel
            {
                Location = new Point(15, 155),
                Size = new Size(360, 420),
                BackColor = Color.Transparent
            };
            this.Controls.Add(buttonPanel);

            CreateButtons();
        }

        private void CreateButtons()
        {
            int btnWidth = 80;
            int btnHeight = 70;
            int gap = 10;
            int startX = 0;
            int startY = 0;

            CreateButton("C", startX + 0 * (btnWidth + gap), startY, btnWidth, btnHeight, Color.FromArgb(200, 200, 200));
            CreateButton("←", startX + 1 * (btnWidth + gap), startY, btnWidth, btnHeight, Color.FromArgb(200, 200, 200));
            CreateButton(".", startX + 2 * (btnWidth + gap), startY, btnWidth, btnHeight, Color.White);
            CreateButton("/", startX + 3 * (btnWidth + gap), startY, btnWidth, btnHeight, Color.FromArgb(255, 150, 50));

            CreateButton("7", startX + 0 * (btnWidth + gap), startY + 1 * (btnHeight + gap), btnWidth, btnHeight, Color.White);
            CreateButton("8", startX + 1 * (btnWidth + gap), startY + 1 * (btnHeight + gap), btnWidth, btnHeight, Color.White);
            CreateButton("9", startX + 2 * (btnWidth + gap), startY + 1 * (btnHeight + gap), btnWidth, btnHeight, Color.White);
            CreateButton("*", startX + 3 * (btnWidth + gap), startY + 1 * (btnHeight + gap), btnWidth, btnHeight, Color.FromArgb(255, 150, 50));

            CreateButton("4", startX + 0 * (btnWidth + gap), startY + 2 * (btnHeight + gap), btnWidth, btnHeight, Color.White);
            CreateButton("5", startX + 1 * (btnWidth + gap), startY + 2 * (btnHeight + gap), btnWidth, btnHeight, Color.White);
            CreateButton("6", startX + 2 * (btnWidth + gap), startY + 2 * (btnHeight + gap), btnWidth, btnHeight, Color.White);
            CreateButton("-", startX + 3 * (btnWidth + gap), startY + 2 * (btnHeight + gap), btnWidth, btnHeight, Color.FromArgb(255, 150, 50));

            CreateButton("1", startX + 0 * (btnWidth + gap), startY + 3 * (btnHeight + gap), btnWidth, btnHeight, Color.White);
            CreateButton("2", startX + 1 * (btnWidth + gap), startY + 3 * (btnHeight + gap), btnWidth, btnHeight, Color.White);
            CreateButton("3", startX + 2 * (btnWidth + gap), startY + 3 * (btnHeight + gap), btnWidth, btnHeight, Color.White);
            CreateButton("+", startX + 3 * (btnWidth + gap), startY + 3 * (btnHeight + gap), btnWidth, btnHeight, Color.FromArgb(255, 150, 50));

            CreateButton("0", startX + 0 * (btnWidth + gap), startY + 4 * (btnHeight + gap), btnWidth * 2 + gap, btnHeight, Color.White);
            CreateButton("=", startX + 2 * (btnWidth + gap) + gap, startY + 4 * (btnHeight + gap), btnWidth, btnHeight, Color.FromArgb(255, 150, 50));
        }

        private void CreateButton(string text, int x, int y, int width, int height, Color backColor)
        {
            bool isOp = "+-*/=".Contains(text);

            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = backColor,
                ForeColor = isOp ? Color.White : Color.FromArgb(30, 30, 30),
                Cursor = Cursors.Hand
            };

            btn.Click += Btn_Click;
            buttonPanel.Controls.Add(btn);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string t = btn.Text;

            try
            {
                if (t == "C") ClearAll();
                else if (t == "←") Backspace();
                else if (t == "=") Calculate();
                else if (t == "." || t == ",")
                {
                    if (!txtDisplay.Text.Contains(".") && !txtDisplay.Text.Contains(","))
                    {
                        if (isNewEntry) txtDisplay.Text = "0";
                        txtDisplay.Text += ".";
                        isNewEntry = false;
                    }
                }
                else if (double.TryParse(t, out _))
                {
                    txtDisplay.Text = isNewEntry ? t : txtDisplay.Text + t;
                    isNewEntry = false;
                }
                else if ("+-*/".Contains(t))
                {
                    SetOperator(t);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ClearAll();
            }
        }

        private void SetOperator(string op)
        {
            if (!string.IsNullOrEmpty(currentOperator) && !isNewEntry)
                Calculate();
            firstOperand = ParseDisplayValue();
            currentOperator = op;
            isNewEntry = true;
        }

        private void Calculate()
        {
            if (string.IsNullOrEmpty(currentOperator)) return;
            double secondOperand = ParseDisplayValue();
            double result = 0;

            switch (currentOperator)
            {
                case "+": result = firstOperand + secondOperand; break;
                case "-": result = firstOperand - secondOperand; break;
                case "*": result = firstOperand * secondOperand; break;
                case "/":
                    if (secondOperand == 0) throw new DivideByZeroException("Деление на ноль");
                    result = firstOperand / secondOperand;
                    break;
            }

            string resStr = result % 1 == 0 ? result.ToString("0") : result.ToString("G");
            txtDisplay.Text = resStr;
            history.Add($"{FormatForHistory(firstOperand)} {currentOperator} {FormatForHistory(secondOperand)} = {resStr}");
            currentOperator = "";
            isNewEntry = true;
        }

        private void Backspace()
        {
            if (!isNewEntry && txtDisplay.Text.Length > 1)
                txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
            else { txtDisplay.Text = "0"; isNewEntry = true; }
        }

        private void ClearAll()
        {
            txtDisplay.Text = "0";
            firstOperand = 0;
            currentOperator = "";
            isNewEntry = true;
        }

        private double ParseDisplayValue()
        {
            string input = txtDisplay.Text.Replace(',', '.');
            return double.Parse(input, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        private string FormatForHistory(double val) => val % 1 == 0 ? val.ToString("0") : val.ToString("G");
    }

    public class NotesForm : Form
    {
        private ListBox lstNotes;
        private TextBox txtNote;
        private Button btnAdd, btnUpdate, btnDelete;
        private List<string> notes = new List<string>();
        private string filePath = "notes_data.txt";

        public NotesForm()
        {
            SetupNotesUI();
            LoadNotes();
        }

        private void SetupNotesUI()
        {
            this.Size = new Size(700, 520);
            this.Padding = new Padding(15);

            lstNotes = new ListBox
            {
                Location = new Point(15, 15),
                Size = new Size(220, 420),
                Font = new Font("Segoe UI", 10F),
                SelectionMode = SelectionMode.One,
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle
            };
            lstNotes.SelectedIndexChanged += (s, e) =>
            {
                if (lstNotes.SelectedIndex >= 0)
                    txtNote.Text = lstNotes.SelectedItem.ToString();
            };
            this.Controls.Add(lstNotes);

            txtNote = new TextBox
            {
                Location = new Point(250, 15),
                Size = new Size(420, 420),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 11F),
                AcceptsReturn = true,
                AcceptsTab = true,
                BackColor = Color.White,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(txtNote);

            int btnY = 445;
            int btnX = 250;
            int btnW = 120;
            int btnH = 40;
            int btnGap = 10;

            btnAdd = CreateButton("Добавить", btnX, btnY, Color.FromArgb(40, 167, 69));
            btnAdd.Click += (s, e) => AddNote();
            this.Controls.Add(btnAdd);

            btnUpdate = CreateButton("Изменить", btnX + btnW + btnGap, btnY, Color.FromArgb(255, 193, 7));
            btnUpdate.Click += (s, e) => UpdateNote();
            this.Controls.Add(btnUpdate);

            btnDelete = CreateButton("Удалить", btnX + (btnW + btnGap) * 2, btnY, Color.FromArgb(220, 53, 69));
            btnDelete.Click += (s, e) => DeleteNote();
            this.Controls.Add(btnDelete);
        }

        private Button CreateButton(string text, int x, int y, Color color)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(120, 40),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
        }

        private void AddNote()
        {
            string text = txtNote.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("Введите текст заметки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            notes.Add(text);
            lstNotes.Items.Add(text);
            SaveNotes();
            txtNote.Clear();
            MessageBox.Show("Заметка добавлена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateNote()
        {
            if (lstNotes.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите заметку для изменения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string text = txtNote.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("Введите текст заметки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            notes[lstNotes.SelectedIndex] = text;
            lstNotes.Items[lstNotes.SelectedIndex] = text;
            SaveNotes();
            MessageBox.Show("Заметка изменена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteNote()
        {
            if (lstNotes.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите заметку для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить эту заметку?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                notes.RemoveAt(lstNotes.SelectedIndex);
                lstNotes.Items.RemoveAt(lstNotes.SelectedIndex);
                SaveNotes();
                txtNote.Clear();
                MessageBox.Show("Заметка удалена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadNotes()
        {
            notes.Clear();
            lstNotes.Items.Clear();
            try
            {
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            notes.Add(line);
                            lstNotes.Items.Add(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveNotes()
        {
            try { File.WriteAllLines(filePath, notes.ToArray()); }
            catch (Exception ex) { MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
