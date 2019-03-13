using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AstronomyCalculator
{
    public partial class astCalcForm : Form
    {
        public astCalcForm()
        {
            InitializeComponent();
            listDistance.SelectedIndex = 1;
            tDistValue.Text = "1";
            listMass.SelectedIndex = 0;
            tMassValue.Text = "1";
            listTime.SelectedIndex = 1;
            tTimeValue.Text = "1";
            listAngle.SelectedIndex = 0;
            tAngleValue.Text = "1";
            listDistAngle.SelectedIndex = 9;
            tDistAngle.Text = "1";
        }

        private double validateInputValue(TextBox inputField)
        {
            double inputValue = 1.0;
            Regex validate = new Regex(@"^-?\d+\.?\d*(e[+-]\d+)?$");
            Match validInput = validate.Match(inputField.Text);
            if (validInput.Success)
            {
                inputValue = double.Parse(inputField.Text);
                errorText.Visible = false;
            } else if (inputField.Text.Length > 0)
            {
                errorText.Text = "Invalid characters in input";
                errorText.Visible = true;
            }
            if (inputValue < 0)
            {
                errorText.Text = "Error in unit lookup";
                errorText.Visible = true;
            }
            return inputValue;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            errorText.Visible = false;
        }
 
        private void setClipboard(TextBox button, Label desc)
        {
            Clipboard.SetText(button.Text);
            errorText.Text = "Copied " + desc.Text + " value " + button.Text + " to Clipboard";
            errorText.Visible = true;
        }

        private void tabSelector_KeyDown(object sender, KeyEventArgs e)
        {
            if (tabSelector.SelectedIndex == 5)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                switch (e.KeyData)
                {
                    case Keys.D0:
                    case Keys.NumPad0:
                        bZero_Click(sender, e); break;
                    case Keys.D1:
                    case Keys.NumPad1:
                        bOne_Click(sender, e); break;
                    case Keys.D2:
                    case Keys.NumPad2:
                        bTwo_Click(sender, e); break;
                    case Keys.D3:
                    case Keys.NumPad3:
                        bThree_Click(sender, e); break;
                    case Keys.D4:
                    case Keys.NumPad4:
                        bFour_Click(sender, e);
                        break;
                    case Keys.D5:
                    case Keys.NumPad5:
                        bFive_Click(sender, e);
                        break;
                    case Keys.D6:
                    case Keys.NumPad6:
                        bSix_Click(sender, e);
                        break;
                    case Keys.D7:
                    case Keys.NumPad7:
                        bSeven_Click(sender, e);
                        break;
                    case Keys.D8:
                    case Keys.NumPad8:
                        bEight_Click(sender, e);
                        break;
                    case Keys.D9:
                    case Keys.NumPad9:
                        bNine_Click(sender, e);
                        break;
                    case Keys.Decimal:
                        bDot_Click(sender, e);
                        break;
                    case Keys.Divide:
                        bDivide_Click(sender, e);
                        break;
                    case Keys.Multiply:
                        bTimes_Click(sender, e);
                        break;
                    case Keys.Subtract:
                        bMinus_Click(sender, e);
                        break;
                    case Keys.Add:
                        bPlus_Click(sender, e);
                        break;
                    case Keys.Enter:
                        Console.WriteLine("Enter");
                        bEnter_Click(sender, e);
                        break;
                    case Keys.E:
                        bExp_Click(sender, e);
                        break;
                    case Keys.Back:
                        bBack_Click(sender, e);
                        break;
                }

            }

        }
    }
}
