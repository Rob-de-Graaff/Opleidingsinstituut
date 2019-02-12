using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opleidingsinstituut
{
    class Subject
    {
        private string name;
        private double tuition;
        private double learningMaterials;
        private double discount5Plus;
        private double discoount19Min;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public double Tuition
        {
            get { return tuition; }
            set { tuition = value; }
        }

        public double LearningMaterials
        {
            get { return learningMaterials; }
            set { learningMaterials = value; }
        }

        public double Discount5Plus
        {
            get { return discount5Plus; }
            set { discount5Plus = value; }
        }

        public double Discount19Min
        {
            get { return discoount19Min; }
            set { discoount19Min = value; }
        }

        public Subject(string subjectName, double subjectTuition, double subjectLearningMaterials, double subjectDiscount5Plus, double subjectDiscoount19Min)
        {
            name = subjectName;
            tuition = subjectTuition;
            learningMaterials = subjectLearningMaterials;
            discount5Plus = subjectDiscount5Plus;
            discoount19Min = subjectDiscoount19Min;
        }
    }
}
