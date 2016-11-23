using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Generator {
    public partial class Settings : Form {


        public int prop = 0, iteration = 0;

        public Settings() {
            InitializeComponent();
        }

        void writeToSettings() {//Change settings for equation devider
            if (radioButton1.Checked) {
                Properties.Settings.Default.devider_type = 1;
            } else if (radioButton2.Checked) {
                Properties.Settings.Default.devider_type = 2;
            } else if (radioButton3.Checked) {
                Properties.Settings.Default.devider_type = 3;
            } else if (radioButton4.Checked) {
                Properties.Settings.Default.devider_type = 4;
            }
            Properties.Settings.Default.user_devider_type = richTextBox1.Text;

            if (radioButton5.Checked) {//Change settings for multiplication sign
                Properties.Settings.Default.multiplication_sign = "×";
            } else if(radioButton6.Checked) {
                Properties.Settings.Default.multiplication_sign = "*";
            } else if (radioButton10.Checked){
                Properties.Settings.Default.multiplication_sign = "·";
            }

            if (radioButton7.Checked) {// Change settings for devision sign
                Properties.Settings.Default.devision_sign = "÷";
            } else if (radioButton8.Checked) {
                Properties.Settings.Default.devision_sign = "/";
            } else if (radioButton9.Checked) {
                Properties.Settings.Default.devision_sign = ":";
            }

            Properties.Settings.Default.max_multiplicator1 = (int)numericUpDown1.Value;
            Properties.Settings.Default.max_multiplicator2 = (int)numericUpDown2.Value;

            Properties.Settings.Default.min_multiplicator1 = (int)numericUpDown6.Value;
            Properties.Settings.Default.min_multiplicator2 = (int)numericUpDown5.Value;

            Properties.Settings.Default.max_divider = (int)numericUpDown4.Value;
            Properties.Settings.Default.max_quotient = (int)numericUpDown3.Value;

            Properties.Settings.Default.min_divider = (int)numericUpDown8.Value;
            Properties.Settings.Default.min_quotient = (int)numericUpDown7.Value;


            Properties.Settings.Default.max_summand1 = (int)numericUpDown10.Value;
            Properties.Settings.Default.max_summand2 = (int)numericUpDown9.Value;

            Properties.Settings.Default.min_summand1 = (int)numericUpDown16.Value;
            Properties.Settings.Default.min_summand2 = (int)numericUpDown15.Value;

            Properties.Settings.Default.max_subtrahend = (int)numericUpDown14.Value;
            Properties.Settings.Default.max_difference = (int)numericUpDown13.Value;

            Properties.Settings.Default.min_subtrahend = (int)numericUpDown12.Value;
            Properties.Settings.Default.min_difference = (int)numericUpDown11.Value;


            Properties.Settings.Default.Save(); // Save all changes
        }

        void ReadRadioButtons() {//Sets up all the radiobuttons on form opening

            prop = Properties.Settings.Default.devider_type;

            if (prop == 1) {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;
            } else if (prop == 2) {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
                radioButton3.Checked = false;
                radioButton4.Checked = false;
            } else if (prop == 3) {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = true;
                radioButton4.Checked = false;
            } else if (prop == 4) {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = true;
            }

            if (Properties.Settings.Default.multiplication_sign == "×") {
                radioButton5.Checked = true;
                radioButton6.Checked = false;
                radioButton10.Checked = false;
            } else if(Properties.Settings.Default.multiplication_sign == "*") {
                radioButton5.Checked = false;
                radioButton6.Checked = true;
                radioButton10.Checked = false;
            } else {
                radioButton5.Checked = false;
                radioButton6.Checked = false;
                radioButton10.Checked = true;
            }

            if (Properties.Settings.Default.devision_sign == "÷") {
                radioButton7.Checked = true;
                radioButton8.Checked = false;
                radioButton9.Checked = false;
            } else if (Properties.Settings.Default.devision_sign == "/") {
                radioButton7.Checked = false;
                radioButton8.Checked = true;
                radioButton9.Checked = false;
            } else {
                radioButton7.Checked = false;
                radioButton8.Checked = false;
                radioButton9.Checked = true;
            }
        }

        private void Settings_Load(object sender, EventArgs e) { // In case there are no profiles, values used for last
                                                                 // generation are used

            ReadRadioButtons();

            numericUpDown1.Value = Properties.Settings.Default.max_multiplicator1;
            numericUpDown2.Value = Properties.Settings.Default.max_multiplicator2;

            numericUpDown6.Value = Properties.Settings.Default.min_multiplicator1;
            numericUpDown5.Value = Properties.Settings.Default.min_multiplicator2;

            numericUpDown3.Value = Properties.Settings.Default.max_quotient;
            numericUpDown4.Value = Properties.Settings.Default.max_divider;

            numericUpDown8.Value = Properties.Settings.Default.min_divider;
            numericUpDown7.Value = Properties.Settings.Default.min_quotient;

            numericUpDown10.Value = Properties.Settings.Default.max_summand1;
            numericUpDown9.Value  = Properties.Settings.Default.max_summand2;

            numericUpDown16.Value = Properties.Settings.Default.min_summand1;
            numericUpDown15.Value = Properties.Settings.Default.min_summand2;

            numericUpDown14.Value = Properties.Settings.Default.max_subtrahend;
            numericUpDown13.Value = Properties.Settings.Default.max_difference;

            numericUpDown12.Value = Properties.Settings.Default.min_subtrahend;
            numericUpDown11.Value = Properties.Settings.Default.min_difference;


            richTextBox1.Text = Properties.Settings.Default.user_devider_type.ToString();


            if (Directory.Exists("Profiles")) { // Check if there are any profiles
                int num = new DirectoryInfo("Profiles").GetFiles().Length; // How many of them?

                for (int i = 0; i < num; i++) { // Add all the profiles to comboBox
                    string fileName = "Profiles\\profile_";
                    fileName += i + ".gensavef";

                    StreamReader file = new StreamReader(fileName);

                    string line = "";

                    file.ReadLine(); // load settings file version (No need to use now)

                    line = file.ReadLine(); // Name of profile
                    comboBox1.Items.Add(line);

                    file.Close();
                }
            }

            if (comboBox1.Items.Count > Properties.Settings.Default.profile) { // Select profile, which was used last time
                comboBox1.SelectedIndex = Properties.Settings.Default.profile;
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e) {

            // Checks that min values are smaller then max values
            // and gives error if they don't

            if(numericUpDown10.Value < numericUpDown16.Value || numericUpDown9.Value < numericUpDown15.Value ||
                numericUpDown13.Value < numericUpDown11.Value || numericUpDown14.Value < numericUpDown12.Value) {
                    label18.Visible = true;
                    label20.Visible = true;
            } else if (numericUpDown1.Value < numericUpDown6.Value || numericUpDown2.Value < numericUpDown5.Value ||
                numericUpDown3.Value < numericUpDown7.Value || numericUpDown4.Value < numericUpDown8.Value) {
                    label19.Visible = true;
                    label20.Visible = true;
            } else {
                writeToSettings();
                label19.Visible = false;
                label18.Visible = false;
                label20.Visible = false;
            }

            if(comboBox1.Items.Count < 1) {
                button1.Enabled = false;
                button3.Enabled = false;
            } else {
                button1.Enabled = true;
                button3.Enabled = true;
            }
                
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            // Switches profile then chousen from comboBox

            Properties.Settings.Default.profile = comboBox1.SelectedIndex;

            ReadFromFile(comboBox1.SelectedIndex);

        }

        private void button2_Click(object sender, EventArgs e) {// New profile
            comboBox1.Items.Add("Новый профиль");
            textBox1.Text = "Новый профиль";
            SaveProfile(1);
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
        }

        private void button3_Click(object sender, EventArgs e) { // Delete profile
            if(comboBox1.Items.Count == 0) {
                return;
            } else if (comboBox1.Items.Count == 1){// If there is only 1 profile
                File.Delete("Profiles\\profile_0.gensavef");
                comboBox1.Items.Clear();
                textBox1.Clear();
            } else {                                // If more then 1 profile

                string fileName = "Profiles\\profile_" + comboBox1.SelectedIndex + ".gensavef";

                int num = new DirectoryInfo("Profiles").GetFiles().Length;  //TODO: Check if filename starts with profile_
                                                                            // and ends with .gensavef

                File.Delete(fileName);

                fileName = "Profiles\\profile_";

                for (int i = comboBox1.SelectedIndex + 1; i <num; i++) {
                    File.Move(fileName + i + ".gensavef", fileName + (i - 1) + ".gensavef");
                }
                int index = comboBox1.SelectedIndex;

                if(index >= 1) {
                    comboBox1.SelectedIndex -= 1;
                } else {
                    comboBox1.SelectedIndex += 1;
                }

                comboBox1.Items.RemoveAt(index);

                if(comboBox1.Items.Count < 1) {
                    button3.Enabled = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) { // Save profile
            SaveProfile(0);
        }

        void SaveProfile(int saveType) {
            if (!Directory.Exists("Profiles")) { // If there is no Profiles folder

                Directory.CreateDirectory("Profiles");

                string file = "";

                file = fillFile(file);

                File.WriteAllText("Profiles\\profile_0.gensavef", file);

            } else { // If Profiles folder exists
                int num = new DirectoryInfo("Profiles").GetFiles().Length;// Get number of profile files

                if (num == 0) {
                    string file = "";

                    file = fillFile(file);

                    File.WriteAllText("Profiles\\profile_0.gensavef", file);
                } else {
                    string file = "";

                    file = fillFile(file);
                    string fileName = "";
                    if (saveType == 0)  // 0 if it is not a new profile
                        fileName = "Profiles\\profile_" + comboBox1.SelectedIndex + ".gensavef";
                    else if (saveType == 1) {   // 1 if it is a new profile being saved

                        fileName = "Profiles\\profile_" + (comboBox1.Items.Count - 1) + ".gensavef";
                    }

                    File.WriteAllText(fileName, file);

                }
                if(saveType == 0)
                    ReadFromFile(comboBox1.SelectedIndex);
                else if (saveType == 1) {
                    ReadFromFile(comboBox1.SelectedIndex + 1);
                }
            }
        }

        string fillFile(string file) {

            file += "1\n"; // Profile file version

            file += textBox1.Text; // Name of profile
            file += "\n";

            if (radioButton1.Checked) { // Type of equation devider
                file += "1\n";
            } else if (radioButton2.Checked) {
                file += "2\n";
            } else if (radioButton3.Checked) {
                file += "3\n";
            } else if (radioButton4.Checked) {
                file += "4\n";
            }

            file += richTextBox1.Lines.Count();// Number of own equation devider
            file += "\n";

            file += richTextBox1.Text; // Own equation devider
            file += "\n";

            if (radioButton5.Checked) {// Multiplication sign
                file += "×\n";
            } else if (radioButton6.Checked) {
                file += "*\n";
            } else if (radioButton10.Checked) {
                file += "·\n";
            }

            if (radioButton7.Checked) {// Devision sign
                file += "÷\n";
            } else if (radioButton8.Checked) {
                file += "/\n";
            } else if (radioButton9.Checked) {
                file += ":\n";
            }

            file += numericUpDown1.Value; // Max multiplicator 1
            file += "\n";

            file += numericUpDown2.Value; // Max multiplicator 2
            file += "\n";

            file += numericUpDown6.Value; // Min multiplicator 1
            file += "\n";

            file += numericUpDown5.Value; // Min multiplicator 2
            file += "\n";

            file += numericUpDown4.Value; // Max devider
            file += "\n";

            file += numericUpDown3.Value; // Max quotient
            file += "\n";

            file += numericUpDown8.Value; // Min devider
            file += "\n";

            file += numericUpDown7.Value; // Min quotient
            file += "\n";



            file += numericUpDown10.Value; // Max summand 1
            file += "\n";
            
            file += numericUpDown9.Value; // Max summand 2
            file += "\n";                        
            
            file += numericUpDown16.Value; // Min summand 1
            file += "\n";                        
            
            file += numericUpDown15.Value; // Min summand 2
            file += "\n";

            file += numericUpDown14.Value; // Max subtrahend
            file += "\n";

            file += numericUpDown13.Value; // Max difference
            file += "\n";

            file += numericUpDown12.Value; // Min subtrahend
            file += "\n";

            file += numericUpDown11.Value; // Min difference

            return file;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e) {

        }

        void ReadFromFile(int index) {
            string fileName = "Profiles\\profile_";
            fileName += index.ToString() + ".gensavef";

            StreamReader file = new StreamReader(fileName);

            string line;

            while (!file.EndOfStream) {
                file.ReadLine(); // Save file version (Not usable now)

                line = file.ReadLine(); // Name of profile
                textBox1.Text = line;

                if (iteration == 0) {
                    iteration++;
                    comboBox1.Items[index] = line;
                }

                line = file.ReadLine(); // Equation deviderType
                int deviderType = Convert.ToInt32(line);

                if (deviderType == 1) {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                } else if (deviderType == 2) {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                } else if (deviderType == 3) {
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = true;
                    radioButton4.Checked = false;
                } else if (deviderType == 4) {
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = true;
                }

                line = file.ReadLine(); // Number of own equation devider
                int n = Convert.ToInt32(line);
                richTextBox1.Text = "";
                for(int i = 0; i < n - 1; i++) {
                    richTextBox1.Text += file.ReadLine() + "\n";
                }
                richTextBox1.Text += file.ReadLine();


                line = file.ReadLine(); // Multiplication Symbol

                if (line == "×") {
                    radioButton5.Checked = true;
                    radioButton6.Checked = false;
                    radioButton10.Checked = false;
                } else if (line == "*") {
                    radioButton5.Checked = false;
                    radioButton6.Checked = true;
                    radioButton10.Checked = false;
                } else {
                    radioButton5.Checked = false;
                    radioButton6.Checked = false;
                    radioButton10.Checked = true;
                }

                line = file.ReadLine(); // Devision Symbol
                
                if (line == "÷") {
                    radioButton7.Checked = true;
                    radioButton8.Checked = false;
                    radioButton9.Checked = false;
                } else if (line == "/") {
                    radioButton7.Checked = false;
                    radioButton8.Checked = true;
                    radioButton9.Checked = false;
                } else {
                    radioButton7.Checked = false;
                    radioButton8.Checked = false;
                    radioButton9.Checked = true;
                }

                line = file.ReadLine(); // Max multiplicator 1
                numericUpDown1.Value = Convert.ToInt32(line);

                line = file.ReadLine(); // Max multiplicator 2
                numericUpDown2.Value = Convert.ToInt32(line);

                line = file.ReadLine(); // Min multiplicator 1
                numericUpDown6.Value = Convert.ToInt32(line);

                line = file.ReadLine(); // Min multiplicator 2
                numericUpDown5.Value = Convert.ToInt32(line);


                line = file.ReadLine(); // Max devider
                numericUpDown4.Value = Convert.ToInt32(line);

                line = file.ReadLine(); // Max quotient
                numericUpDown3.Value = Convert.ToInt32(line);

                line = file.ReadLine(); // Min devider
                numericUpDown8.Value = Convert.ToInt32(line);

                line = file.ReadLine(); // Min quotient
                numericUpDown7.Value = Convert.ToInt32(line);


                line = file.ReadLine(); // Max summond 1
                numericUpDown10.Value = Convert.ToInt32(line);

                line = file.ReadLine(); // Max summond 2
                numericUpDown9.Value = Convert.ToInt32(line);

                line = file.ReadLine(); // Min summond 1
                numericUpDown16.Value = Convert.ToInt32(line);

                line = file.ReadLine(); // Min summond 2
                numericUpDown15.Value = Convert.ToInt32(line);


                line = file.ReadLine(); // Max subtrahend
                numericUpDown14.Value = Convert.ToInt32(line);

                line = file.ReadLine(); // Max difference
                numericUpDown13.Value = Convert.ToInt32(line);

                line = file.ReadLine(); // Min subtrahend
                numericUpDown12.Value = Convert.ToInt32(line);

                line = file.ReadLine(); // Min difference
                numericUpDown11.Value = Convert.ToInt32(line);

                file.Close();

                iteration = 0;

                return;
            }
        }

    }
}
