using System;
using System.Windows.Forms;

namespace AstronomyCalculator
{
    public partial class astCalcForm : Form
    {
        private double getValueAsKg(string units)
        {
            switch (units)
            {
                case "Kilogram":
                    return 1.0;
                case "Metric Ton":
                    return 1000.0;
                case "Pound":
                    return 0.453592;
                case "Earth Mass":
                    return 5.972e+24;
                case "Jupiter Mass":
                    return 1.898e+27;
                case "Sol Mass":
                    return 1.989e+30;
            }
            return -1;
        }

        private void computeMassValues()
        {
            double inputValue = validateInputValue(tMassValue);
            double multiplier = getValueAsKg(listMass.Text);
            double mass = inputValue * multiplier;
            tKg.Text = mass.ToString();
            tTon.Text = (mass / 1000).ToString();
            tBus.Text = (mass / 16000).ToString();
            tLocomotive.Text = (mass / 196000).ToString();
            t747.Text = (mass / 439985).ToString();
            tSaturnV.Text = (mass / 2966000).ToString();
            tNimitz.Text = (mass / 100017118).ToString();
            tPlutoM.Text = (mass / 1.3105e+22).ToString();
            tMoonM.Text = (mass / 7.35e+22).ToString();
            tMercuryM.Text = (mass / 3.302e+23).ToString();
            tMarsM.Text = (mass / 6.4185e+23).ToString();
            tVenusM.Text = (mass / 4.8685e+24).ToString();
            tEarthM.Text = (mass / getValueAsKg("Earth Mass")).ToString();
            tUranusM.Text = (mass / 8.6832e+25).ToString();
            tNeptuneM.Text = (mass / 1.0243e+26).ToString();
            tSaturnM.Text = (mass / 5.6846e+26).ToString();
            tJupiterM.Text = (mass / getValueAsKg("Jupiter Mass")).ToString();
            tSolarM.Text = (mass / getValueAsKg("Sol Mass")).ToString();
        }

        private void tabMass_Entered(object sender, EventArgs e)
        {
            tabMass.Controls.Add(errorText);
            computeMassValues();
        }

        private void tMassValue_TextChanged(object sender, EventArgs e)
        {
            computeMassValues();
        }

        private void listMass_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeMassValues();
        }

        private void bKg_Click(object sender, EventArgs e)
        {
            setClipboard(tKg, lKg);
        }

        private void bTon_Click(object sender, EventArgs e)
        {
            setClipboard(tTon, lTon);
        }

        private void bBus_Click(object sender, EventArgs e)
        {
            setClipboard(tBus, lBus);
        }

        private void bLocomotive_Click(object sender, EventArgs e)
        {
            setClipboard(tLocomotive, lLocomotive);
        }

        private void b747_Click(object sender, EventArgs e)
        {
            setClipboard(t747, l747);
        }

        private void bSaturnV_Click(object sender, EventArgs e)
        {
            setClipboard(tSaturnV, lSaturnV);
        }

        private void bNimitz_Click(object sender, EventArgs e)
        {
            setClipboard(tNimitz, lNimitz);
        }

        private void bPlutoM_Click(object sender, EventArgs e)
        {
            setClipboard(tPlutoM, lPlutoM);
        }

        private void bMoonM_Click(object sender, EventArgs e)
        {
            setClipboard(tMoonM, lMoonM);
        }

        private void bMercuryM_Click(object sender, EventArgs e)
        {
            setClipboard(tMercuryM, lMercuryM);
        }

        private void bMarsM_Click(object sender, EventArgs e)
        {
            setClipboard(tMarsM, lMarsM);
        }

        private void bVenusM_Click(object sender, EventArgs e)
        {
            setClipboard(tVenusM, lVenusM);
        }

        private void bEarthM_Click(object sender, EventArgs e)
        {
            setClipboard(tEarthM, lEarthM);
        }

        private void bUranusM_Click(object sender, EventArgs e)
        {
            setClipboard(tUranusM, lUranusM);
        }

        private void bNeptuneM_Click(object sender, EventArgs e)
        {
            setClipboard(tNeptuneM, lNeptuneM);
        }

        private void bSaturnM_Click(object sender, EventArgs e)
        {
            setClipboard(tSaturnM, lSaturnM);
        }

        private void bJupiterM_Click(object sender, EventArgs e)
        {
            setClipboard(tJupiterM, lJupiterM);
        }

        private void bSolarM_Click(object sender, EventArgs e)
        {
            setClipboard(tSolarM, lSolarM);
        }

    }
}
