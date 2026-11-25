using System;
using System.Globalization;
using System.Windows.Forms;

namespace SimpleCalculatorWinForms
{
    public partial class Form1 : Form
    {
        private readonly TextBox txtNum1 = new TextBox();
        private readonly TextBox txtNum2 = new TextBox();
        private readonly Label lblResult = new Label();

        public Form1()
        {
            InitializeComponent();
            BuildCalculatorUI();
        }

        private void BuildCalculatorUI()
        {
            Text = "Калькулятор";
            Width = 320;
            Height = 220;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            var lblNum1 = new Label { Text = "Число 1:", Left = 20, Top = 20, Width = 70 };
            txtNum1.SetBounds(95, 17, 185, 25);

            var lblNum2 = new Label { Text = "Число 2:", Left = 20, Top = 55, Width = 70 };
            txtNum2.SetBounds(95, 52, 185, 25);

            lblResult.Text = "Результат:";
            lblResult.Left = 20;
            lblResult.Top = 140;
            lblResult.Width = 260;

            Controls.AddRange(new Control[] { lblNum1, txtNum1, lblNum2, txtNum2, lblResult });

            (string op, Func<double, double, string> calc)[] operations =
            {
                ("+", (a, b) => (a + b).ToString(CultureInfo.CurrentCulture)),
                ("-", (a, b) => (a - b).ToString(CultureInfo.CurrentCulture)),
                ("*", (a, b) => (a * b).ToString(CultureInfo.CurrentCulture)),
                ("/", (a, b) => b == 0 ? "ділення на 0" : (a / b).ToString(CultureInfo.CurrentCulture))
            };

            for (int i = 0; i < operations.Length; i++)
            {
                var calc = operations[i].calc;
                var btn = new Button
                {
                    Text = operations[i].op,
                    Left = 20 + i * 65,
                    Top = 95,
                    Width = 55,
                    Height = 32
                };
                btn.Click += (_, _) => Calculate(calc);
                Controls.Add(btn);
            }
        }

        private void Calculate(Func<double, double, string> operation)
        {
            if (!TryGetInputs(out double a, out double b))
                return;

            lblResult.Text = $"Результат: {operation(a, b)}";
        }

        private bool TryGetInputs(out double num1, out double num2)
        {
            bool ok = TryParseDouble(txtNum1.Text, out num1) & TryParseDouble(txtNum2.Text, out num2);
            if (!ok)
                lblResult.Text = "Результат: введіть коректні числа";

            return ok;
        }

        private static bool TryParseDouble(string input, out double value) =>
            double.TryParse(
                input.Trim().Replace(',', '.'),
                CultureInfo.InvariantCulture,
                out value
            );
    }
}