using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadManager
{
    // Just in case the file doesn't exist, we'll create it
    public static void CreateFile()
    {
        string path = Application.persistentDataPath + "/score.dat";
        if (!File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            ScoreData data = new ScoreData(0);

            formatter.Serialize(stream, data);
            stream.Close();
        }
    }

    public static void SaveScore(int score)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/score.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        ScoreData data = new ScoreData(score);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static int LoadScore()
    {
        string path = Application.persistentDataPath + "/score.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ScoreData data = formatter.Deserialize(stream) as ScoreData;
            stream.Close();

            return data.score;
        }
        else
        {
            return 0;
        }
    }
    
}

[System.Serializable]
public class ScoreData
{
    public int score;

    public ScoreData(int score)
    {
        this.score = score;
    }
}
