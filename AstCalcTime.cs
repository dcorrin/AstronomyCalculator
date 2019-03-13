using System;
using System.Windows.Forms;

namespace AstronomyCalculator
{
    public partial class astCalcForm : Form
    {

        private double getValueAsSecond(string units)
        {
            switch (units)
            {
                case "Millisecond":
                    return 0.001;
                case "Second":
                    return 1.0;
                case "Minute":
                    return 60.0;
                case "Hour":
                    return 3600.0;
                case "Earth Day":
                    return 86400.0;
                case "Mars Day":
                    return 88620.0;
                case "Month":
                    return 2.628e+6;
                case "Mercury Year":
                    return 7600608.0;
                case "Venus Year":
                    return 19414080.0;
                case "Earth Year":
                    return 31558464.0;
                case "Mars Year":
                    return 59355072.0;
                case "Jupiter Year":
                    return 374355648.0;
                case "Saturn Year":
                    return 929292480.0;
                case "Uranus Year":
                    return 2651369760.0;
                case "Neptune Year":
                    return 5200418592.0;
                case "Pluto Year":
                    return 7824384000.0;
            }
            return -1;
        }

        private void computeTimeValues()
        {
            double inputValue = validateInputValue(tTimeValue);
            double multipier = getValueAsSecond(listTime.Text);
            double time = inputValue * multipier;
            tMillisec.Text = (time * 1000).ToString();
            tSecond.Text = time.ToString();
            tMinute.Text = (time / 60).ToString();
            tHour.Text = (time / 3600).ToString();
            double days = time / getValueAsSecond("Earth Day");
            tDay.Text = days.ToString();
            tMonth.Text = (time / getValueAsSecond("Month")).ToString();
            tMercuryY.Text = (time / getValueAsSecond("Mercury Year")).ToString();
            tVenusY.Text = (time / getValueAsSecond("Venus Year")).ToString();
            tEarthY.Text = (time / getValueAsSecond("Earth Year")).ToString();
            tMarsY.Text = (time / getValueAsSecond("Mars Year")).ToString();
            tMarsDay.Text = ((time/getValueAsSecond("Earth Day"))/ 1.025957).ToString();
            tJupiterY.Text = (time / getValueAsSecond("Jupiter Year")).ToString();
            tSaturnY.Text = (time / getValueAsSecond("Saturn Year")).ToString();
            tUranusY.Text = (time / getValueAsSecond("Uranus Year")).ToString();
            tNeptuneY.Text = (time / getValueAsSecond("Neptune Year")).ToString();
            tPlutoY.Text = (time / getValueAsSecond("Pluto Year")).ToString();
            int wholeDays = (int)Math.Truncate(days);
            tPDay.Text = wholeDays.ToString();
            double remainder = (days - (double)wholeDays)*24;
            int wholeHours = (int)Math.Truncate(remainder);
            tPHour.Text = wholeHours.ToString();
            remainder = (remainder - (double)wholeHours) * 60;
            int wholeMinutes = (int)Math.Truncate(remainder);
            tPMinute.Text = wholeMinutes.ToString();
            remainder = (remainder - (double)wholeMinutes) * 60;
            tPSecond.Text = remainder.ToString();

        }

        private void tabTime_Entered(object sender, EventArgs e)
        {
            tabTime.Controls.Add(errorText);
            computeTimeValues();
        }

        private void tTimeValue_TextChanged(object sender, EventArgs e)
        {
            computeTimeValues();
        }

        private void listTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            computeTimeValues();
        }

        private void bMillisec_Click(object sender, EventArgs e)
        {
            setClipboard(tMillisec, lMillisec);
        }

        private void bSecond_Click(object sender, EventArgs e)
        {
            setClipboard(tSecond, lSecond);
        }

        private void bMinute_Click(object sender, EventArgs e)
        {
            setClipboard(tMinute, lMinute);
        }

        private void bHour_Click(object sender, EventArgs e)
        {
            setClipboard(tHour, lHour);
        }

        private void bDay_Click(object sender, EventArgs e)
        {
            setClipboard(tDay, lDay);
        }

        private void bMonth_Click(object sender, EventArgs e)
        {
            setClipboard(tMonth, lMonth);
        }

        private void bMercuryY_Click(object sender, EventArgs e)
        {
            setClipboard(tMercuryY, lMercuryY);
        }

        private void bVenusY_Click(object sender, EventArgs e)
        {
            setClipboard(tVenusY, lVenusY);
        }

        private void bEarthY_Click(object sender, EventArgs e)
        {
            setClipboard(tEarthY, lEarthY);
        }

        private void bMarsDay_Click(object sender, EventArgs e)
        {
            setClipboard(tMarsDay, lMarsDay);
        }

        private void bMarsY_Click(object sender, EventArgs e)
        {
            setClipboard(tMarsY, lMarsY);
        }

        private void bJupiterY_Click(object sender, EventArgs e)
        {
            setClipboard(tJupiterY, lJupiterY);
        }

        private void bSaturnY_Click(object sender, EventArgs e)
        {
            setClipboard(tSaturnY, lSaturnY);
        }

        private void bNeptuneY_Click(object sender, EventArgs e)
        {
            setClipboard(tNeptuneY, lNeptuneY);
        }

        private void bUranusY_Click(object sender, EventArgs e)
        {
            setClipboard(tUranusY, lUranusY);
        }

        private void bPlutoY_Click(object sender, EventArgs e)
        {
            setClipboard(tPlutoY, lPlutoY);
        }
    }
}
