using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AstronomyCalculator
{
    public partial class astCalcForm : Form
    {
        private IDictionary<string, string> spaceInfo = null;

        private void tabInfo_Enter(object sender, EventArgs e)
        {
            if (null == spaceInfo)
            {
                spaceInfo = new Dictionary<string, string>();
                loadInfoText();
            }
        }

        private void listInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayInfo(listInfo.Text.Trim());
        }

        private void displayInfo(string item)
        {
            string info;
            try
            {
                info = spaceInfo[item];
            } catch (KeyNotFoundException)
            {
                info = "Data Not Found for " + item;
            }
            tInfo.Text = info;
            tInfo.SelectionStart = 0;
            tInfo.ScrollToCaret();
        }

        private void tInfoSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
            e.Handled = true;
            e.SuppressKeyPress = true;

            string searchTerm = tInfoSearch.Text.ToLower();
            tInfo.Text = "Searching for " + searchTerm + Environment.NewLine;
            bool found = false;
            foreach (KeyValuePair<string, string> kvp in spaceInfo)
            {
                if (kvp.Value.ToLower().Contains(searchTerm))
                {
                    found = true;
                    string[] lines = kvp.Value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    for (var i = 0; i< lines.Length; i++)
                    {
                        StringBuilder block = new StringBuilder(lines[i]);
                        int idx = i;
                        // Need to find "blocks" of text e.g.
                        // Albedo	    0.367 geometric
                        //              0.306 Bond
                        // Should be treated as one item.
                        while (idx+1<lines.Length && (lines[idx+1].StartsWith(" ")||lines[idx+1].StartsWith("\t"))) // lookahead
                        {
                            block.Append(Environment.NewLine).Append("\t\t\t").Append(lines[idx + 1]);
                            idx++;
                        }
                        if (block.ToString().ToLower().Contains(searchTerm))
                        {
                            tInfo.AppendText(kvp.Key + "\t\t" + block.ToString() + Environment.NewLine);
                        }
                        i = idx;
                    }
                }
            }
            if (!found)
            {
                tInfo.AppendText(Environment.NewLine + "\tSearch term not found");
            }
        }

        private void loadInfoText()
        {

            Assembly assembly = typeof(astCalcForm).Assembly;
            Regex sequenced = new Regex(@"^\d\d\d");
            string[] resourceNames = assembly.GetManifestResourceNames();
            Array.Sort(resourceNames);
            int datFiles = resourceNames.Count(r => r.EndsWith(".dat"));
            object[] infoNames = new object[datFiles];
            int idx = 0;

            foreach (string resource in resourceNames)
            {
                //Console.WriteLine("Found resource: " + resource);

                string[] tokens = resource.Split('.');
                string name = tokens[tokens.Length - 2];
                string ext = tokens[tokens.Length - 1];
                if (ext != "dat")
                {
                    continue;
                }
                // Check to see if we have a sequencing prefix on the resources e.g. 000Sol 010Mercury
                bool subItem = true;
                if (sequenced.Match(name).Success)
                {
                    if (name.Substring(2, 1).Equals("0")) // ends with a 0 is not a subitem
                    {
                        subItem = false;
                    }
                    name = name.Substring(3);
                } else
                {
                    subItem = false; // not subitem if no prefix
                }
                if (subItem)
                {
                    infoNames[idx++] = "  " + name;
                } else
                {
                    infoNames[idx++] = name;
                }
                StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(resource));
                string fileText = sr.ReadToEnd();
                spaceInfo.Add(name, fileText);

            }
            listInfo.Items.AddRange(infoNames);

        }

    }
}
