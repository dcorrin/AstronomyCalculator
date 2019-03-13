using System;
using System.Windows.Forms;

namespace AstronomyCalculator
{
    public partial class astCalcForm : Form
    {
        private double getValueAsMas(string units)
        {
            switch (units)
            {
                case "Degree":
                    return 3600000.0;
                case "Radian":
                    return 206264806.2471;
                case "Arcminute":
                    return 60000.0;
                case "Arcsecond":
                    return 1000.0;
                case "Milliarcsecond (mas)":
                    return 1.0;
            }
            return -1;
        }

        private void computeAngleValues()
        {
            double inputValue = validateInputValue(tAngleValue);
            double multiplier = getValueAsMas(listAngle.Text);
            double angle = inputValue * multiplier;
            tDegree.Text = (angle / getValueAsMas("Degree")).ToString();
            tRadian.Text = (angle / getValueAsMas("Radian")).ToString();
            tArcMin.Text = (angle / getValueAsMas("Arcminute")).ToString();
            tArcSec.Text = (angle / getValueAsMas("Arcsecond")).ToString();
            tMas.Text = angle.ToString();
            double distanceValue = validateInputValue(tDistAngle);
            double distMultiplier = getValueAsKm(listDistAngle.Text);
            double twiceDist = 2 * distanceValue * distMultiplier; // in Km
            double subtended = twiceDist*Math.Tan(angle/(2*getValueAsMas("Radian")));
            tMetersAngle.Text = (subtended*1000).ToString();
            tAUAngle.Text = (subtended / getValueAsKm("AU")).ToString();
        }

        private void tabAngle_Entered(object sender, EventArgs e)
        {
            tabAngle.Controls.Add(errorText);
            computeAngleValues();
        }

        private void tAngleValue_TextChanged(object sender, EventArgs e)
        {
            computeAngleValues();
        }

        private void listDistAngle_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeAngleValues();
        }

        private void listAngle_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeAngleValues();
        }

        private void tDistAngle_TextChanged(object sender, EventArgs e)
        {
            computeAngleValues();
        }

        private void bDegree_Click(object sender, EventArgs e)
        {
            setClipboard(tDegree, lDegree);
        }

        private void bRadian_Click(object sender, EventArgs e)
        {
            setClipboard(tRadian, lRadian);
        }

        private void bArcMin_Click(object sender, EventArgs e)
        {
            setClipboard(tArcMin, lArcMin);
        }

        private void bArcSec_Click(object sender, EventArgs e)
        {
            setClipboard(tArcSec, lArcSec);
        }

        private void bMas_Click(object sender, EventArgs e)
        {
            setClipboard(tMas, lMas);
        }

        private void bMetersAngle_Click(object sender, EventArgs e)
        {
            setClipboard(tMetersAngle, lMetersAngle);
        }

        private void bAUAngle_Click(object sender, EventArgs e)
        {
            setClipboard(tAUAngle, lAUAngle);
        }

    }
}
