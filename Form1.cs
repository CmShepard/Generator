using System;
using System.Threading;
using System.Windows.Forms;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Layout.Properties;

namespace Generator
{
    public partial class EnterForm : Form
    {
        //Variables
        bool addition = true, substitution = true, multiplication = true, devision = true;
        int maxMultF = 10, maxMultS = 10, maxDivS = 10, maxDivA = 10, deviderType = 1, mode = 0, xpos = 1,
            representationMode = 0, minMultF = 2, minMultS = 2, minDivS = 2, minDivA = 2, maxAddF = 100, maxAddS = 100,
            minAddF = 10, minAddS = 10, maxSubS = 100, maxSubA = 100, minSubS = 10, minSubA = 10;



        string multSign, divSign, eqnDevidider;

        Document doc;
        Table table;
        string DEST = "Примеры.pdf"; //Temp variable

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            mode = comboBox1.SelectedIndex;
            if (comboBox1.SelectedIndex == 1) {
                comboBox2.SelectedIndex = 0;
            }
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e) {
            DEST = saveFileDialog1.FileName;
        }

        private void button3_Click(object sender, EventArgs e) {//Открыть настройки
            Settings settForm = new Settings();

            settForm.Show();

        }
               
        public EnterForm(){
            InitializeComponent();
        }

        private void CheckTimer_Tick(object sender, EventArgs e){//Check that at least one operation is checked
            if(!checkBox1.Checked  && !checkBox2.Checked && !checkBox3.Checked && !checkBox4.Checked)
            {
                label2.Text = "Хотя бы одна из опций должна быть отмечена";//Red lable under checkboxes
                label2.Visible = true;
                button2.Enabled = false;
            } else {
                label2.Visible = false;
                button2.Enabled = true;
            }
            if(comboBox2.SelectedIndex == 1) {
                comboBox1.SelectedIndex = 0;
            }
            if(comboBox1.SelectedIndex == 1) {
                comboBox2.SelectedIndex = 0;
            }
        }


        private void button1_Click(object sender, EventArgs e){//Clear textBox field
            help helpForm = new help();

            helpForm.Show();
        }

        private void button2_Click(object sender, EventArgs e){// Generate equations
            if(!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked && !checkBox4.Checked){
                MessageBox.Show("Хотя бы одна из опций должна быть отмечена!");
            }else{
                richTextBox1.Clear();

                ReadSettings();

                addition = checkBox1.Checked;
                substitution = checkBox2.Checked;
                multiplication = checkBox3.Checked;
                devision = checkBox4.Checked;
                representationMode = comboBox2.SelectedIndex;

                WriteToSettings();

                saveFileDialog1.AddExtension = true;
                saveFileDialog1.DefaultExt = ".pdf";
                saveFileDialog1.FileName = "Примеры";
                saveFileDialog1.Filter = "PDF file (*.pdf)| *.pdf";
                saveFileDialog1.ShowDialog();

                //Creates PDF document
                CreatePDFDocument();

                //doc.Add(new Paragraph("!!"));
                                
                richTextBox1.Text = Generate(checkBox1.Checked, checkBox2.Checked, checkBox3.Checked, checkBox4.Checked,
                    (int)numericUpDown2.Value, deviderType, representationMode);

                Clipboard.SetText(richTextBox1.Rtf, TextDataFormat.Rtf); // Copy text to clipboard

                doc.Add(table);
                doc.Close();

            }
        }

        private void EnterForm_Load(object sender, EventArgs e){//Loading from settings is here
            checkBox1.Checked = Properties.Settings.Default.addition;
            checkBox2.Checked = Properties.Settings.Default.subtraction;
            checkBox3.Checked = Properties.Settings.Default.multiplication;
            checkBox4.Checked = Properties.Settings.Default.devision;
            comboBox1.SelectedIndex = Properties.Settings.Default.mode;
            comboBox2.SelectedIndex = Properties.Settings.Default.representation_mode;
            numericUpDown2.Value = Properties.Settings.Default.number_of_equations;
            ReadSettings();
        }

        void ReadSettings() { // Reads settings
            eqnDevidider = Properties.Settings.Default.user_devider_type;
            deviderType = Properties.Settings.Default.devider_type;
            multSign = Properties.Settings.Default.multiplication_sign;
            divSign = Properties.Settings.Default.devision_sign;

            maxMultF = Properties.Settings.Default.max_multiplicator1;
            maxMultS = Properties.Settings.Default.max_multiplicator2;

            minMultF = Properties.Settings.Default.min_multiplicator1;
            minMultS = Properties.Settings.Default.min_multiplicator2;

            maxDivS = Properties.Settings.Default.max_divider;
            maxDivA = Properties.Settings.Default.max_quotient;

            minDivS = Properties.Settings.Default.min_divider;
            minDivA = Properties.Settings.Default.min_quotient;
            
            maxAddF = Properties.Settings.Default.max_summand1;
            maxAddS = Properties.Settings.Default.max_summand2;


            minAddF= Properties.Settings.Default.min_summand1;
            minAddS = Properties.Settings.Default.min_summand2;


            maxSubS = Properties.Settings.Default.max_subtrahend;
            maxSubA = Properties.Settings.Default.max_difference;


            minSubS = Properties.Settings.Default.min_subtrahend;
            minSubA = Properties.Settings.Default.min_difference;
        }

        void WriteToSettings(){// Write settings
            Properties.Settings.Default.addition = checkBox1.Checked;
            Properties.Settings.Default.subtraction = checkBox2.Checked;
            Properties.Settings.Default.multiplication = checkBox3.Checked;
            Properties.Settings.Default.devision = checkBox4.Checked;
            Properties.Settings.Default.mode = comboBox1.SelectedIndex;
            Properties.Settings.Default.number_of_equations = (int)numericUpDown2.Value;
            Properties.Settings.Default.representation_mode = comboBox2.SelectedIndex;

            Properties.Settings.Default.Save();
        }





        string Generate(bool add, bool sub, bool mult, bool dev, int num, int deviderType, int representationMode){//Generate equations
            string Assembled_Text = "";
            progressBar1.Maximum = num;
            progressBar1.Value = 0;

            //Create font
            PdfFont font = PdfFontFactory.CreateFont(FontConstants.COURIER);
            
            //Init table
            if(representationMode == 0) {
                if( (maxMultF.ToString().Length + maxMultS.ToString().Length < 8 && mult) || 
                    (maxAddF.ToString().Length + maxAddS.ToString().Length < 8 && add) ||
                        (maxSubS.ToString().Length + maxSubA.ToString().Length < 8 && sub)  ||
                        (maxDivS.ToString().Length + maxDivA.ToString().Length < 8 && dev)){
                    table = new Table(5);
                }else{
                    table = new Table(4);
                }
                
            } else {
                table = new Table(7);
            }
            
            table.SetFont(font);
            table.SetFontSize(10);


            for (int i = 0; i < num; i++){
                progressBar1.Value++;
                Random rnd = new Random(DateTime.Now.Millisecond + Cursor.Position.X);
                
                int opr = 0;

                opr = generator.ChooseOperation(add, sub, mult, dev);


                Thread.Sleep(4);

                int first = 0, second = 0;

                //Generate addition equations
                if (add && opr == 0){
                    first = rnd.Next(minAddF, maxAddF);
                    second = rnd.Next(minAddS, maxAddS);

                    if( mode == 1 && first > second) {//If equations with x
                        int temp = first;
                        first = second;
                        second = temp;
                        if (rnd.Next(1, 3) == 1) {
                            xpos = 1;
                        } else {
                            xpos = 2;
                        }
                    }
                }
                //Generate substraction equations
                if (sub && opr == 1){
                    int ans = rnd.Next(minSubA, maxSubA);
                    second = rnd.Next(minSubS, maxSubS);
                    first = ans + second;
                    if(mode == 1) {
                        if (rnd.Next(1, 3) == 1) {
                            xpos = 1;
                            //first = second - first;
                        } else {
                            xpos = 2;
                        }
                    }
                }
                //Generate multiplication equations
                if (mult && opr == 2){
                    //make sure first and second do not exceed maxMultF & maxMultS
                        first = rnd.Next(minMultF, maxMultF + 1);

                        second = rnd.Next(minMultS, maxMultS + 1);

                    if (mode == 1) {// If equation 
                        if (rnd.Next(1, 3) == 1) {
                            xpos = 1;
                        } else {
                            xpos = 2;
                        }
                        second = first * second;
                    }

                }

                //Generate devision equations
                if (dev && opr == 3){
                    int ans = 0;
                       
                            second = rnd.Next(minDivS, maxDivS);

                            ans = rnd.Next(minDivA,maxDivA);

                    first = second * ans;

                    if (mode == 1) {//If equations
                        if (rnd.Next(1, 3) == 1) {
                            xpos = 1;
                            first = ans;
                        } else {
                            xpos = 2;                            
                        }
                    }

                }
                
                if (representationMode == 0) {//Adds string equations to the assembled text and table of PDF document
                    string temtText = AssembleStringEquation(first, second, opr, i, num, deviderType);
                    Assembled_Text += temtText;
                    Cell cell;
                    if (mode == 0) {
                        cell = new Cell().Add(new Paragraph(temtText));
                    } else {
                        temtText += "\n";
                        cell = new Cell().Add(new Paragraph(temtText));
                    }
                    
                    cell.SetKeepTogether(true);
                    cell.SetBorder(new SolidBorder(Color.WHITE, 0));

                    table.AddCell(cell);
                } else {//Adds column equations to the richTextBox and PFD documents
                    string tempText = AssembleColumnEquation(first, second, opr, i, num);
                    Assembled_Text += tempText;

                    Cell cell = new Cell().Add(new Paragraph(tempText));
                    
                    cell.SetKeepTogether(true);
                    cell.SetTextAlignment(TextAlignment.RIGHT);
                    //cell.Se
                    cell.SetBorder(new SolidBorder(Color.WHITE, 0));

                    table.AddCell(cell);
                }
                
            }
            return Assembled_Text;
        }

        string AssembleColumnEquation(int first, int second, int op, int i, int num) {//Generate complete equation in column form
            string operation = "";
            string Text = "";
            switch (op) { // Change the default operation signs to column ones
                case 0:
                    operation = "+";
                    break;
                case 1:
                    operation = "_";
                    break;
                case 2:
                    operation = "×";
                    break;
                case 3:
                    operation = "|";
                    break;
            };
            if (i == num - 1) {
                if (mode == 0) {
                    if (first.ToString().Length == second.ToString().Length) {

                        if (op == 2) {
                            string compensator1 = "";
                            for (int k = 0; k < (second.ToString().Length); k++) {
                                compensator1 += "\n";
                            }
                            Text += operation + first.ToString() + "\n "  + second.ToString() + compensator1 + "\n\n";
                        } else if (op == 3) {
                            string compensator = "";
                            for (int k = 0; k < (first.ToString().Length); k++) {
                                compensator += "\n";
                            }
                            Text += " " + first.ToString() + operation + second.ToString() + compensator;
                        } else {
                            Text += operation + first.ToString() + "\n " + second.ToString();
                        }
                        
                    } else {//else for if (first.ToString().Length == second.ToString().Length)
                        string compensator = "";
                        if(first < second) {
                            int temp = first;
                            first = second;
                            second = temp;
                        }

                        for(int k = 0; k < (first.ToString().Length - second.ToString().Length); k++) {
                            compensator += " ";
                        }

                        if (op == 2) {
                            string compensator1 = "";
                            for (int k = 0; k < (second.ToString().Length); k++) {
                                compensator1 += "\n";
                            }
                            Text += operation + first.ToString() + "\n " + compensator + second.ToString() + compensator1 + "\n\n";
                        } else if (op == 3) {
                            compensator = "";
                            for (int k = 0; k < (first.ToString().Length); k++) {
                                compensator += "\n";
                            }
                            Text += " " + first.ToString() + operation + second.ToString() + compensator;
                        } else {
                            Text += operation + first.ToString() + "\n " + compensator + second.ToString();
                        }

                    }
                }
            } else {//else for if (i==j-1)
                if (mode == 0) {
                    if (first.ToString().Length == second.ToString().Length) {

                        if (op ==2 ) {
                            string compensator = "";
                            for (int k = 0; k < (second.ToString().Length); k++) {
                                compensator += "\n";
                            }
                            Text += operation + first.ToString() + "\n " + second.ToString() + compensator + "\n\n\n";
                        } else if (op == 3) {
                            string compensator = "";
                            for (int k = 0; k < (first.ToString().Length); k++) {
                                compensator += "\n";
                            }
                            Text += " " + first.ToString() + operation + second.ToString() + compensator + "\n\n\n";
                        } else {
                            Text += operation + first.ToString() + "\n " + second.ToString() + "\n\n\n";
                        }


                    } else {
                        string compensator = "";
                        if (first < second) {
                            int temp = first;
                            first = second;
                            second = temp;
                        }

                        for (int k = 0; k < (first.ToString().Length - second.ToString().Length); k++) {
                            compensator += " ";
                        }

                        if (op == 2) {//If multiplication
                            string compensator1 = "";
                            for (int k = 0; k < (second.ToString().Length); k++) {
                                compensator1 += "\n";
                            }
                            Text += operation + first.ToString() + "\n " + compensator + second.ToString() + compensator1 + "\n\n";
                        } else if(op ==3) {//If division
                            compensator = "";
                            for (int k = 0; k < (first.ToString().Length); k++) {
                                compensator += "\n";
                            }
                            Text += " " + first.ToString() + operation + second.ToString() + compensator + "\n\n";
                        } else {
                            Text += operation + first.ToString() + "\n " + compensator + second.ToString() + "\n\n\n";
                        }
                    }
                    }
                }
            
            return Text;
        }

        string AssembleStringEquation(int first, int second, int op, int i, int j, int devideType){//Generate complete equation in string form
            string operation = "";
            string Text = "";
            switch (op){
                case 0:
                    operation = " + ";
                    break;
                case 1:
                    operation = " - ";
                    break;
                case 2:
                    operation = " " + multSign + " ";
                    break;
                case 3:
                    operation = " " + divSign + " ";
                    break;
            };
            if (i ==j-1){
                if (mode == 0) {
                    Text += first.ToString() + operation + second.ToString() + " =";
                } else if (mode ==1) {
                    if(xpos == 1)
                        Text += "x" + operation + first.ToString() + " = " + second.ToString();
                    else
                        Text += first.ToString() + operation + "x = " + second.ToString();
                }
            }else{
                if (mode == 0) {

                    Text += first.ToString() + operation + second.ToString() + " =";

                } else if(mode == 1) {
                    if (xpos == 1) {

                         Text += "x" + operation + first.ToString() + " = " + second.ToString();

                    } else {
                        
                        Text += first.ToString() + operation + "x = " + second.ToString();
                    }
                }
                switch (devideType) {
                    case 1:
                        Text += "\n\n";
                        break;
                    case 2:
                        Text += "\n";
                        break;
                    case 3:
                        Text += "\t";
                        break;
                    case 4:
                        Text += eqnDevidider;
                        break;
                    default:
                        Text += "\n\n";
                        break;
                };
            }

            return Text;
        }

        void CreatePDFDocument() {//Create PDF document for results

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            //Initialize PDF writer
            PdfWriter writer = new PdfWriter(DEST);
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(writer);
            // Initialize document
            doc = new Document(pdf);
        }


    }
}
