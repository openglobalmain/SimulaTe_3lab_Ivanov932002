using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string[] positions = new string[] { "000", "001", "010", "011", "100", "101", "110", "111" };
        char[] cellRules;
        int btclick = 0;
        bool start = true; 
        int rowCounter = 0; 

        private char[] acceptRules(int rule)
        {
            char[] result;

            string binaryCode = Convert.ToString(rule, 2);

            int binaryLength = binaryCode.Length;
            if (binaryLength != 8)
            {
                for (int i = 0; i < 8 - binaryLength; i++)
                {
                    binaryCode = "0" + binaryCode;
                }
            }

            result = binaryCode.ToCharArray();

            return result;
        }

        private char calculateLayerCellValue(char[] xyz)
        {
            char result;

            string code = new string(xyz);

            int index = Array.IndexOf(positions, code);

            result = cellRules[index];

            return result;
        }

        private void btCreate_Click(object sender, EventArgs e)
        {
            numrule.Enabled = false;
            numcol.Enabled = false;
            btStart.Enabled = true;
            btclick = 0; 
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            for (int i = 0; i < (int)numcol.Value; i++)
            {
                dataGridView1.Columns.Add("", "");
            }

            dataGridView1.Rows.Add();

            int rule = (int)numrule.Value;
            cellRules = acceptRules(rule); 
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            if (btclick % 2 == 0)
            {
                btCreate.Enabled = false;
                start = false;
                rowCounter = 0; 
                timer1.Start(); 
            }
            else
            {
                timer1.Stop(); 
                btCreate.Enabled = true;
                start = true;
                numcol.Enabled = true;
                numrule.Enabled = true;
                btStart.Enabled = false;
            }
            btclick++; 
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (start)
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Green;
                        dataGridView1.ClearSelection();
                        break;
                    case MouseButtons.Right:
                        dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
                        dataGridView1.ClearSelection();
                        break;
                }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            char[] previousLayer = new char[dataGridView1.Columns.Count];

            char[] currentLayer = new char[dataGridView1.Columns.Count];

            char[] xyz = new char[3];

            for (int i = 0; i < previousLayer.Length; i++)
            {
                if (dataGridView1[i, rowCounter].Style.BackColor == Color.Green) previousLayer[i] = '1';
                else previousLayer[i] = '0';
            }

            dataGridView1.Rows.Add();
            rowCounter++;

            for (int i = 0; i < currentLayer.Length; i++)
            {
                xyz[0] = previousLayer[(i + previousLayer.Length - 1) % previousLayer.Length];
                xyz[1] = previousLayer[i];
                xyz[2] = previousLayer[(i + previousLayer.Length + 1) % previousLayer.Length];
                currentLayer[i] = calculateLayerCellValue(xyz);
            }

            for (int i = 0; i < currentLayer.Length; i++)
            {
                if (currentLayer[i] == '0') dataGridView1[i, rowCounter].Style.BackColor = Color.White;
                else dataGridView1[i, rowCounter].Style.BackColor = Color.Green;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)sender).SelectedCells[0].Selected = false;
            }
            catch { }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
