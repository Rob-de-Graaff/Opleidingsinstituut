using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Opleidingsinstituut
{
    public partial class Form1 : Form
    {
        private Subject _newSubject1, _newSubject2, _newSubject3;
        private Dictionary<string, Subject> subjectDict;
        private List<string> registrationList;
        private double _priceTotal;
        private int ageStudent;
        private int ageStudentSet = 19;
        private int totalSubjectAmount = 5;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _newSubject1 = new Subject("schriftelijke vak", 50, 50, 0.95, 0.98);
            _newSubject2 = new Subject("mondelinge vak", 150, 50, 0.95, 0.98);
            _newSubject3 = new Subject("practische vak", 150, 125, 0.95, 0.98);

            subjectDict = new Dictionary<string, Subject>();
            registrationList = new List<string>();

            subjectDict.Add(_newSubject1.Name, _newSubject1);
            subjectDict.Add(_newSubject2.Name, _newSubject3);
            subjectDict.Add(_newSubject3.Name, _newSubject3);

            // Displays name property
            listBoxSubjects.DataSource = new BindingSource(subjectDict, null);
            listBoxSubjects.DisplayMember = "Key";

            // Fills numericupdowns
            foreach (NumericUpDown control in panelYear.Controls)
            {
                control.Maximum = DateTime.Today.Year;
                control.Minimum = DateTime.Today.Year - 100;
                control.Value = DateTime.Today.Year;
            }

            foreach (NumericUpDown control in panelMonth.Controls)
            {
                control.Maximum = DateTime.Today.Month;
                control.Value = DateTime.Today.Month;
            }

            foreach (NumericUpDown control in panelDay.Controls)
            {
                control.Maximum = DateTime.Today.Day;
                control.Value = DateTime.Today.Day;
            }

            // Displays calculation, total
            labelTicketsTotal.Text = $"(((((Lesgeld*korting)+studiemat)*#les)+\n(((Lesgeld*korting)+studiemat)*#les)+\n(((Lesgeld*korting)+studiemat)*#les)) *korting";
            labelPriceTotal.Text = $@"Totaal: € {Math.Round(_priceTotal, 2):0.00},-";
        }

        private void ButtonAddClass_Click(object sender, EventArgs e)
        {
           string subjectName;
           subjectName = listBoxSubjects.SelectedItem.ToString();
           int indexStart = subjectName.IndexOf("[") + 1;
           int indexEnd = subjectName.IndexOf(",");
           int lenght = indexEnd - indexStart;
           subjectName = subjectName.Substring(indexStart, lenght);
                
           registrationList.Add(subjectName + ":" + numericUpDownAmount.Value);

           // Displays name property
           listBoxRegistrations.DataSource = null;
           listBoxRegistrations.DataSource = registrationList;
        }

        private void ButtonRemoveClass_Click(object sender, EventArgs e)
        {
            string subjectName;
            subjectName = listBoxSubjects.SelectedItem.ToString();
            int indexStart = subjectName.IndexOf("[") + 1;
            int indexEnd = subjectName.IndexOf(",");
            int lenght = indexEnd - indexStart;
            subjectName = subjectName.Substring(indexStart, lenght);
                
            registrationList.Remove(subjectName + ":" + numericUpDownAmount.Value);

            // Displays name property
            listBoxRegistrations.DataSource = null;
            listBoxRegistrations.DataSource = registrationList;
        }

        private void ButtonCalculate_Click(object sender, EventArgs e)
        {
            Subject tempSubject = null;
            double tuitionSubject1 = 0;
            double tuitionSubject2 = 0;
            double tuitionSubject3 = 0;
            double discount5Plus = 1;
            double discountAge = 1;
            double learningMaterialsSubject1 = 0;
            double learningMaterialsSubject2 = 0;
            double learningMaterialsSubject3 = 0;
            //double tuitionTotal = 0;
            //double learningMaterialsTotal = 0;
            
            int subjectsTotal = 0;
            int amountSubject1 = 0;
            int amountSubject2 = 0;
            int amountSubject3 = 0;

            if (listBoxRegistrations.Items.Count > 0)
            {
                if (ValidateDate())
                {
                    foreach (string registration in registrationList)
                    {
                        int indexStart = 0;
                        int indexEnd = registration.IndexOf(":");
                        int lenght = indexEnd - indexStart;
                        string subjectName = registration.Substring(indexStart, lenght);
                        indexStart = registration.IndexOf(":")+1;
                        indexEnd = registration.Length;
                        lenght = indexEnd - indexStart;
                        int registrationAmount = int.Parse(registration.Substring(indexStart, lenght));

                        switch (subjectName)
                        {
                            case "schriftelijke vak":
                                tempSubject = _newSubject1;
                                tuitionSubject1 = _newSubject1.Tuition;
                                amountSubject1 += registrationAmount;
                                break;
                            case "mondelinge vak":
                                tempSubject = _newSubject2;
                                tuitionSubject2 = _newSubject2.Tuition;
                                amountSubject2 += registrationAmount;
                                break;
                            case "practische vak":
                                tempSubject = _newSubject3;
                                tuitionSubject3 = _newSubject3.Tuition;
                                amountSubject3 += registrationAmount;
                                break;
                        }

                        subjectsTotal += registrationAmount;
                    }

                    // Checks if total amount of subjects >= 5, gives 5% discount on tuition
                    if (subjectsTotal >= totalSubjectAmount)
                    {
                        discount5Plus = tempSubject.Discount5Plus;
                    }
                    tuitionSubject1 = tuitionSubject1 * discount5Plus;
                    tuitionSubject2 = tuitionSubject2 * discount5Plus;
                    tuitionSubject3 = tuitionSubject3 * discount5Plus;

                    // Checks if the student age < 19, gives 2% discount on total course fee
                    if (ageStudent < ageStudentSet)
                    {
                        discountAge = tempSubject.Discount19Min;
                    }

                    // Checks if student follows course through mediation of employment office 
                    if (!checkBoxEmploymentOffice.Checked)
                    {
                        learningMaterialsSubject1 = _newSubject1.LearningMaterials;
                        learningMaterialsSubject2 = _newSubject2.LearningMaterials;
                        learningMaterialsSubject3 = _newSubject3.LearningMaterials;
                    }

                    //_priceTotal += (tuitionTotal + learningMaterialsTotal)  * discountAge;

                    _priceTotal += ((((tuitionSubject1 * discount5Plus) + learningMaterialsSubject1) * amountSubject1) +
                                    (((tuitionSubject2 * discount5Plus) + learningMaterialsSubject2) * amountSubject2) +
                                    (((tuitionSubject3 * discount5Plus) + learningMaterialsSubject3) *
                                     amountSubject3)) * discountAge;

                    // Displays calculation, total
                    labelTicketsTotal.Text = $"((((Lesgeld € {tuitionSubject1} * korting {(1-discount5Plus)*100}%) + studiemat € {learningMaterialsSubject1}) * #les {amountSubject1}) + \n(((Lesgeld € {tuitionSubject2} * korting {(1-discount5Plus)*100}%) + studiemat € {learningMaterialsSubject2}) * #les {amountSubject2}) + \n(((Lesgeld € {tuitionSubject3} * korting {(1-discount5Plus)*100}%) + studiemat € {learningMaterialsSubject3}) * #les {amountSubject3})) * korting {(1-discountAge)*100}%";
                    labelPriceTotal.Text = $@"Totaal: € {Math.Round(_priceTotal, 2):0.00},-";
                    
                    //Resets display and variables
                    registrationList.Clear();
                    _priceTotal = 0;
                }
                else
                {
                    MessageBox.Show($@"Please select the correct day of the month");
                }
            }
            else
            {
                MessageBox.Show("You must have registered for class.");
            }
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            numericUpDownAmount.Value = 1;
            listBoxRegistrations.DataSource = null;
            registrationList.Clear();
            _priceTotal = 0;
        }

        private bool ValidateDate()
        {
            int checkDay = DateTime.DaysInMonth((int)numericUpDownYear1.Value, (int)numericUpDownMonth1.Value);

            if ((int)numericUpDownDay1.Value <= checkDay)
            {
                DateTime dateOfBirth1 = new DateTime((int) numericUpDownYear1.Value,
                    (int) numericUpDownMonth1.Value, (int) numericUpDownDay1.Value);
                ageStudent = CalculateAge(dateOfBirth1);

                return true;
            }
            else
            {
                return false;
            }
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = DateTime.Now.Year - dateOfBirth.Year;  
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)  
                age = age - 1;  
  
            return age; 
        }
    }
}
