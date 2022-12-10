using System.Text.Json.Serialization;
using System.Text.Json;
using System.ComponentModel;

namespace oop_lab
{
    public partial class Form1 : Form
    {
        private int _row = 0;
        private int _col = 6;
        private int _rowHeaderSize = 311;

        private OpenFileDialog ofd;
        private SaveFileDialog sfd;


        List<Helper> gpu = new List<Helper>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeDataGridView(_row, _col);
            ofd = new OpenFileDialog();
            sfd = new SaveFileDialog();
        }
        private void HeaderTable()
        {
            var nameCol = new List<string> { "Name", "Vendor", "GB", "Flops", "Price", "Year" };
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.HeaderText = nameCol[col.Index];
            }
        }
        private void InitializeDataGridView(int rowCount, int columnCount)
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowCount = rowCount;
            dataGridView1.ColumnCount = columnCount;

            HeaderTable();

            dataGridView1.AutoResizeRows();
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.RowHeadersWidth = _rowHeaderSize;
            dataGridView1.Visible = true;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        //sort button
        private void button3_Click(object sender, EventArgs e)//by gb
        {
            dataGridView1.Sort(dataGridView1.Columns[2], ListSortDirection.Ascending);
        }

        private void button4_Click(object sender, EventArgs e)//by tflops
        {
            dataGridView1.Sort(dataGridView1.Columns[3], ListSortDirection.Ascending);

        }

        private void button5_Click(object sender, EventArgs e)//by price
        {
            dataGridView1.Sort(dataGridView1.Columns[4], ListSortDirection.Ascending);

        }


        //add/del button
        private void button1_Click(object sender, EventArgs e)//add
        {
            float _price = float.Parse(textBox5.Text);
            float _flops = float.Parse(textBox4.Text);
            int _gb = Convert.ToInt32(textBox3.Text);
            int _year = Convert.ToInt32(textBox6.Text);
            int _nameSize = textBox1.Text.Length;
            int _vendorSize = textBox2.Text.Length;

            string _name = textBox1.Text.ToString();
            string _vendor = textBox2.Text.ToString();

            bool flagName = char.IsDigit(_name[0]);
            bool flagVendor = char.IsDigit(_vendor[0]);

            if (_gb <= 0 || _price <= 0 || _flops <= 0 || _year <= 2000 || _year >=2022 || _nameSize <= 0 || _vendorSize <= 0
                || flagName || _name[0] == '-' || _name[0] == '+' || _name[0] == '='
                || flagVendor || _vendor[0] == '-' || _vendor[0] == '+' || _vendor[0] == '=')
            {
                string msg = "You enter somethin wrong param or did not enter any parameter\n" +
                    "Name - string; without: '-', '+', '='\n" +
                    "Vendor - string; without: '-', '+', '='\n" +
                    "Gb - int\n" +
                    "Year - int; diapazon 2000 - 2022\n" +
                    "Price - float\n" +
                    "Flops - float\n" +
                    "All values must be greater than 0";
                MessageBox.Show(msg, "Error");
            }
            else
            {
                bool flag = true;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (textBox2.Text == dataGridView1.Rows[i].Cells[1].Value.ToString())
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    dataGridView1.Rows.Add(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text);
                }
                else
                {
                    MessageBox.Show("THIS PRODUCT IS ALREADY IN THE TABLE");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)//del
        {
            int countRow = dataGridView1.RowCount;
            int countColumn = dataGridView1.ColumnCount;
            string deleteRowInfo = "you can't choose less than 0";
            string HeaderOfMesageBox = "Warning";
            string deleteShure = "Are you shure?";

            DialogResult result = MessageBox.Show(deleteShure, HeaderOfMesageBox, MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                if (countRow > 0)
                {
                    InitializeDataGridView(countRow - 1, countColumn);
                }
                else
                {
                    MessageBox.Show(deleteRowInfo, HeaderOfMesageBox);
                }
            }
        }


        //nav bar menu
        private async void openToolStripMenuItem_Click(object sender, EventArgs e)//open file
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                {
                    gpu = await JsonSerializer.DeserializeAsync<List<Helper>>(fs);
                    foreach (Helper hgpu in gpu)
                    {
                        dataGridView1.Rows.Add(hgpu.Name.ToString(), hgpu.Vendor.ToString(), hgpu.GB.ToString(), hgpu.Flops.ToString(), hgpu.Price.ToString(), hgpu.Year.ToString());
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)//save file
        {
            if (dataGridView1.Rows.Count <= 0)
            {
                string head = "Message";
                string tail = "This tabe empty";
                MessageBox.Show(tail, head);
            }
            else
            {
                if (sfd.ShowDialog() == DialogResult.OK) {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                    };
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        string n = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        string v = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        int g = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);
                        float f = Convert.ToSingle(dataGridView1.Rows[i].Cells[3].Value);
                        float p = Convert.ToSingle(dataGridView1.Rows[i].Cells[4].Value);
                        int y = Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value);
                        gpu.Add(new Helper(n, v, g, f, p, y));
                    }
                    using (FileStream fstream = new FileStream(sfd.FileName, FileMode.Create))
                    {
                        JsonSerializer.SerializeAsync(fstream, gpu, options);
                    }
                }
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)//open help menu
        {
            string head = "Help";
            string tail = "This lab make student Yeugen Solovey group K - 26\n" +
                "Sort by GB - int\n" +
                "Sort by Flops - float\n" +
                "Sort by price - float\n" +
                "Add/delete - you can add or delete element\n" +
                "Open/Save - you can open or save json file\n";
            MessageBox.Show(tail, head);
        }

        
        private void textBox1_TextChanged(object sender, EventArgs e)//name
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)//vendor
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)//gb
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)//flops
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)//price
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)//year
        {

        }
    }
}