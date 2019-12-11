using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void OnNumBtn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            InputNumber(btn.Text);
        }

        private void InputNumber(string number)
        {
            txtBox_Number.AppendText(number);
        }

        private void btnChangeSign_Click(object sender, EventArgs e)
        {
            if (txtBox_Number.Text[0] == '-')
                txtBox_Number.Text = txtBox_Number.Text.Substring(1);
            else
                txtBox_Number.Text = $"-{txtBox_Number.Text}";
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (txtBox_Number.Text.Length > 0)
                txtBox_Number.Text = txtBox_Number.Text.Substring(0, txtBox_Number.Text.Length - 1);
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            txtBox_Number.Clear();
            txtBox_Expression.Clear();
        }

        private void OnOperationBtn_Click(object sender, EventArgs e)
        {
            string operation = (sender as Button).Text;
            InputOperation(operation);
        }

        private void InputOperation(string operation)
        {
            if (txtBox_Number.Text.Length > 0)
            {
                string operand = txtBox_Number.Text;
                txtBox_Expression.AppendText($"{operand} {operation} ");
                txtBox_Number.Clear();
            }
            else if (txtBox_Expression.Text.Length > 0)
            {
                char lastToken = txtBox_Expression.Text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Last()[0];
                bool lastCharIsOperation =
                    lastToken == '+' ||
                    lastToken == '-' ||
                    lastToken == '*' ||
                    lastToken == '/';
                if (lastCharIsOperation)
                    txtBox_Expression.Text = $"{txtBox_Expression.Text.Substring(0, txtBox_Expression.Text.Length - 2)}{operation} ";
            }
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            if (txtBox_Expression.Text.Length > 0)
            {
                if (txtBox_Number.Text.Length > 0)
                {
                    txtBox_Expression.AppendText(txtBox_Number.Text);
                }
                else
                {
                    txtBox_Expression.Text = txtBox_Expression.Text.Substring(0, txtBox_Expression.Text.Length-3);
                }
                
                string[] tokens = txtBox_Expression.Text.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                Queue<string> tokenQueue = new Queue<string>(tokens);
                
                double leftOp = double.Parse(tokenQueue.Dequeue()); // take first operand
                double res = leftOp;
                while (tokenQueue.Count > 0)
                {
                    string operation = tokenQueue.Dequeue();
                    double rightOp = double.Parse(tokenQueue.Dequeue());

                    switch (operation)
                    {
                        case "+":
                            res += rightOp;
                            break;
                        case "-":
                            res -= rightOp;
                            break;
                        case "*":
                            res *= rightOp;
                            break;
                        case "/":
                            // DIV by 0
                            if (rightOp == 0)
                            {
                                txtBox_Expression.Clear();
                                txtBox_Number.Text = "Can't divide by 0!";
                                return;
                            }
                            res /= rightOp;
                            break;
                    }
                }
                txtBox_Expression.Clear();
                txtBox_Number.Text = res.ToString();
            }
        }

        // keyboard controls
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // numbers
            if (e.KeyCode == Keys.NumPad0 || e.KeyCode == Keys.D0)
                InputNumber("0");
            else if (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1)
                InputNumber("1");
            else if (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2)
                InputNumber("2");
            else if (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3)
                InputNumber("3");
            else if (e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.D4)
                InputNumber("4");
            else if (e.KeyCode == Keys.NumPad5 || e.KeyCode == Keys.D5)
                InputNumber("5");
            else if (e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.D6)
                InputNumber("6");
            else if (e.KeyCode == Keys.NumPad7 || e.KeyCode == Keys.D7)
                InputNumber("7");
            else if (e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.D8)
                InputNumber("8");
            else if (e.KeyCode == Keys.NumPad9 || e.KeyCode == Keys.D9)
                InputNumber("9");
            else if (e.KeyCode == Keys.Decimal || e.KeyCode == Keys.Oemcomma || e.KeyCode == Keys.OemPeriod)
                InputNumber(",");
            // arithmetic operations
            else if (e.KeyCode == Keys.Add)
                InputOperation("+");
            else if (e.KeyCode == Keys.Subtract)
                InputOperation("-");
            else if (e.KeyCode == Keys.Multiply)
                InputOperation("*");
            else if (e.KeyCode == Keys.Divide)
                InputOperation("/");
            // control operations
            else if (e.KeyCode == Keys.Delete)
                btnC_Click(btnC, EventArgs.Empty);
            else if (e.KeyCode == Keys.Back)
                btnBackspace_Click(btnBackspace, EventArgs.Empty);
            else if (e.KeyCode == Keys.Enter)
                btnEquals_Click(btnEquals, EventArgs.Empty);

        }

        
    }
}
