using System;
using System.Windows.Forms;

namespace AstronomyCalculator
{
    /// <summary>
    /// Contains the Form for the Distance Tab
    /// </summary>
    public partial class astCalcForm : Form
    {
        /// <summary>
        /// Given text from dropdown returns distance
        /// </summary>
        /// <param name="units"></param>
        /// <returns>value of parameter in Km</returns>
        private double getValueAsKm(string units)
        {
            switch (units)
            {
                case "Meter":
                    return 0.001;
                case "Kilometer":
                    return 1.0;
                case "Miles":
                    return 1.60934;
                case "AU":
                    return 149597870.7;
                case "Light-Second":
                    return 299792;
                case "Light-Minute":
                    return 1.799e+7;
                case "Light-Hour":
                    return 1.079e+9;
                case "Light-Day":
                    return 2.59e+10;
                case "Light-Year":
                    return 9460730472580.8;
                case "Parsec":
                    return 3.085677581e+13;
            }
            return -1;
        }
        /// <summary>
        /// Fills in all the textbox with the computed distances based on input
        /// </summary>
        private void computeDistanceValues()
        {
            double inputValue = validateInputValue(tDistValue);
            double multiplier = getValueAsKm(listDistance.Text);
            double distance = inputValue * multiplier;
            tMeter.Text = (distance * 1000).ToString();
            tKm.Text = distance.ToString();
            tEarthD.Text = (distance / 12742).ToString();
            tEarthMoon.Text = (distance / 384400).ToString();
            tJupiterD.Text = (distance / 139820).ToString();
            tJupiterO.Text = (distance / 778500000).ToString();
            tMarsO.Text = (distance / 227900000).ToString();
            tSolD.Text = (distance / 1391000).ToString();
            tNeptuneO.Text = (distance / 4495000000).ToString();
            tAU.Text = (distance / getValueAsKm("AU")).ToString();
            tLS.Text = (distance / getValueAsKm("Light-Second")).ToString();
            tLM.Text = (distance / getValueAsKm("Light-Minute")).ToString();
            tLH.Text = (distance / getValueAsKm("Light-Hour")).ToString();
            tLD.Text = (distance / getValueAsKm("Light-Day")).ToString();
            tLY.Text = (distance / getValueAsKm("Light-Year")).ToString();
            tParsec.Text = (distance / getValueAsKm("Parsec")).ToString();
        }

        private void tabDistance_Entered(object sender, EventArgs e)
        {
            tabDistance.Controls.Add(errorText);
            computeDistanceValues();
        }

        private void tDistValue_TextChanged(object sender, EventArgs e)
        {
            computeDistanceValues();
        }

        private void listDistance_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeDistanceValues();
        }
        private void bMeter_Click(object sender, EventArgs e)
        {
            setClipboard(tMeter, lMeter);
        }

        private void bKm_Click(object sender, EventArgs e)
        {
            setClipboard(tKm, lKm);
        }

        private void bEarthD_Click(object sender, EventArgs e)
        {
            setClipboard(tEarthD, lEarthD);
        }

        private void bJupiterD_Click(object sender, EventArgs e)
        {
            setClipboard(tJupiterD, lJupiterD);
        }

        private void bLS_Click(object sender, EventArgs e)
        {
            setClipboard(tLS, lLS);
        }

        private void bEarthMoon_Click(object sender, EventArgs e)
        {
            setClipboard(tEarthMoon, lEarthMoon);
        }

        private void bSolD_Click(object sender, EventArgs e)
        {
            setClipboard(tSolD, lSolD);
        }

        private void bLM_Click(object sender, EventArgs e)
        {
            setClipboard(tLM, lLM);
        }

        private void bAU_Click(object sender, EventArgs e)
        {
            setClipboard(tAU, lAU);
        }

        private void bMarsO_Click(object sender, EventArgs e)
        {
            setClipboard(tMarsO, lMarsO);
        }

        private void bJupiterO_Click(object sender, EventArgs e)
        {
            setClipboard(tJupiterO, lJupiterO);
        }

        private void bLH_Click(object sender, EventArgs e)
        {
            setClipboard(tLH, lLH);
        }

        private void bNeptuneO_Click(object sender, EventArgs e)
        {
            setClipboard(tNeptuneO, lNeptuneO);
        }

        private void bLD_Click(object sender, EventArgs e)
        {
            setClipboard(tLD, lLD);
        }

        private void bLY_Click(object sender, EventArgs e)
        {
            setClipboard(tLY, lLY);
        }

        private void bParsec_Click(object sender, EventArgs e)
        {
            setClipboard(tParsec, lParsec);
        }

    }
}
