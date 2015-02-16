using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MonteCarloGMB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            int steps = int.Parse(textBox5.Text);
            int paths = int.Parse(textBox6.Text);
            double percentile=double.Parse(textBox7.Text)/100;
            int pos1 = (int)(paths *(1-percentile));
            int pos2 = pos1 + 1;
            
            double S_0 = double.Parse(textBox1.Text);
            double mu = double.Parse(textBox2.Text);
            double sigma = double.Parse(textBox3.Text);
            double dt = double.Parse(textBox4.Text) / steps;

            double max = 1.1 * S_0;
            double min = 0.8*S_0;
            Random rand = new Random();     

            for (int i = 0; i < paths; i++)
            {
                chart1.Series.Add(i.ToString());
                chart1.Series[i].ChartType = SeriesChartType.Line;
                chart1.Series[i].Points.AddXY(0, S_0);
                chart1.Series[i].IsVisibleInLegend = false;
                double prev = S_0;
                for (int j = 0; j < steps; j++)
                {
                    double u1 = rand.NextDouble(); //u1,u2 are uniform(0,1) random doubles
                    double u2 = rand.NextDouble();
                    double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                 Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                    double dS = mu*prev*dt + sigma*prev * randStdNormal*Math.Sqrt(dt);
                    prev += dS;
                    if (prev > max)
                        max = prev;
                    if (prev < min)
                        min = prev;
                    chart1.Series[i].Points.AddXY(j, prev);
                    
                }
            }
            ArrayList tops = new ArrayList();
            
            for (int count = 0; count <=pos2; count++)
            {
                tops.Add(max);
            }
            for (int k = 0; k < paths; k++)
            {
                int l = 0;
                bool inserted = false;
                while ((l <= pos2) && (!inserted))
                {
                    double a = chart1.Series[k].Points[steps].YValues[0];
                  
                    if (a <= (double)tops[l])
                    {
                        tops.Insert(l, a);
                        tops.RemoveAt(pos2+1);
                        inserted = true;
                    }
                    l++;
                }
            }
            listView1.Clear();
            for (int m = 0; m <tops.Count; m++)
            {
                listView1.Items.Add(tops[m].ToString());
            }
            double VaR=((double)tops[pos1] + (percentile - pos1) * ((double)tops[pos2] -(double)tops[pos1]));
            result1.Text =min.ToString("#.##");
            result2.Text=((S_0-VaR)/S_0).ToString("#.##");

            chart1.ChartAreas[0].AxisY.Maximum = max * 1.1;
            chart1.ChartAreas[0].AxisY.Minimum =min*0.9;

            chart1.ChartAreas[0].AxisX.Minimum = 0;

            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "#0.00";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            chart1.Series.Add("New");

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
