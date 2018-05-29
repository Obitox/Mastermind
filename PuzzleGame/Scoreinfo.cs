using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PuzzleGame
{
    [Serializable]
    class Scoreinfo
    {
        private string username;
        private double score;

        private static List<Scoreinfo> listOfAllScores = new List<Scoreinfo>();

        public Scoreinfo(string username,double finishedTime,double totalTime)
        {
            double timeMultiplier = 1;
            if (totalTime == 80) timeMultiplier = 4;
            if (totalTime == 60) timeMultiplier = 7;
            if (totalTime == 40) timeMultiplier = 9;


            this.username = username;
            this.score = (totalTime - finishedTime) * timeMultiplier;

            loadScores();
        }

        public double getScore()
        {
            return score;
        }

        public void setScore(double value)
        {
            score = value;
        }

        public string getUsername()
        {
            return username;
        }

        public void setUsername(string name)
        {
            username = name;
        }

        public override string ToString()
        {
            return username + " " + score;
        }

        public static void Serialize(Scoreinfo score)
        {
            try
            {
                using (Stream stream = File.Open("Scoreboard.bin", FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    listOfAllScores.Add(score);
                    bin.Serialize(stream, listOfAllScores);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Error, can't serialize to file.");
            }
        }

        public static List<Scoreinfo> Deserialize()
        {
            try
            {
                using (Stream stream = File.Open("Scoreboard.bin", FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    List<Scoreinfo> listOfallScores = (List<Scoreinfo>) bin.Deserialize(stream);
                    return listOfallScores;
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Error, can't deserialize to file.");
            }
            return null;
        }

        public static void loadScores()
        {
            if (File.Exists("Scoreboard.bin"))
            {
                try
                {
                    using (Stream stream = File.Open("Scoreboard.bin", FileMode.Open))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        listOfAllScores = (List<Scoreinfo>)bin.Deserialize(stream);

                        sortScore();

                        foreach (Scoreinfo si in listOfAllScores)
                        {
                            EntryWindow.listBox.Items.Add(si);
                        }
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("Error, can't deserialize to file.");
                }
            }
        }

        public static void writeScore(Scoreinfo score)
        {
            Serialize(score);

            EntryWindow.listBox.Items.Clear();

            sortScore();

            foreach (Scoreinfo element in listOfAllScores)
            {
                if (EntryWindow.listBox.Items.Count == 10) break;
                EntryWindow.listBox.Items.Add(element);
            }
 
        }

        public static void sortScore()
        {
            for (int i = 0; i < listOfAllScores.Count - 1; i++)
            {
                for (int j = i + 1; j < listOfAllScores.Count; j++)
                {
                    if (listOfAllScores[i].getScore() < listOfAllScores[j].getScore())
                    {
                        double pomScore = listOfAllScores[i].getScore();
                        string pomUsername = listOfAllScores[i].getUsername();

                        listOfAllScores[i].setScore(listOfAllScores[j].getScore());
                        listOfAllScores[i].setUsername(listOfAllScores[j].getUsername());

                        listOfAllScores[j].setScore(pomScore);
                        listOfAllScores[j].setUsername(pomUsername);

                    }
                }
            }
        }

    }
}
