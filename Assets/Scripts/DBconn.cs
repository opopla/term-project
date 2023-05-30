using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mono.Data.SqliteClient;
using System.IO;
using System.Data;

public class Rank
{
    public int No;
    public string Name;
    public float Time;


    public Rank(int no, string name, float time)
    {
        No = no;
        Name = name;
        Time = time;
    }
}


public class DBconn : MonoBehaviour
{
    public InputField inputField;
    public List<Rank> RankList = new List<Rank>();

    void Start()
    {
        StartCoroutine(Main());
    }

    IEnumerator Main()
    {
        yield return StartCoroutine(RankDBParsing("RankDB.sqlite"));
    }

    //코루틴
    IEnumerator RankDBParsing(string p)
    {
        string Filepath = Application.persistentDataPath + "/" + p;
        if (!File.Exists(Filepath))
        {
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);
            while (!loadDB.isDone) { }
            File.WriteAllBytes(Filepath, loadDB.bytes);
        }
        string connectionString = "URI=FILE:" + Filepath;

        RankList.Clear();

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM RankTable";

                dbCmd.CommandText = sqlQuery;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Debug.Log(reader.GetString(2));

                        RankList.Add(new Rank(reader.GetInt32(0), reader.GetString(1), reader.GetFloat(0)));

                    }
                    dbConnection.Close();
                    reader.Close();
                }
                
            }
        }
        yield return null;
    }
    //SQLite 저장과 불러오기 법!#4참조
    public void SaveBtn()
    {
        StartCoroutine(SaveDb("RankDB.sqlite"));

    }
   
    // 코루틴 .
    IEnumerator SaveDb(string p)
    {


        string Filepath = Application.persistentDataPath + "/" + p;

        if (!File.Exists(Filepath))
        {
            Debug.LogWarning("File \"" + Filepath + "\" does not exist. Attempting to create from \"" +
                             Application.dataPath + "!/assets/" + p);

            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);
            while (!loadDB.isDone) { }
            File.WriteAllBytes(Filepath, loadDB.bytes);
        }

        string connectionString = "URI=file:" + Filepath;


        RankList.Clear();

        // using을 사용함으로써 비정상적인 예외가 발생할 경우에도 반드시 파일을 닫히도록 할 수 있다.
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())  // EnterSqL에 명령 할 수 있다. 
            {


                //수정
              
                string Name = inputField.text;
                string Time = GameManager.endingtime;

                /* sqlQuery = "UPDATE  ItemTable  SET " +
                    "Name =" + "'" + Name + "'"
                    + ",Desc =" + "'" + Discription + "'";*/

                string sqlQuery = "INSERT INTO RankTable  (Name,Time) VALUES(@Name,@Time)";


                //string sqlQuery = "DELETE FROM ItemTable Where ID =5";
                // WHere을 붙인 이유는 테이블 전체를 돌기 때문에 해당 아이디만 수정하게 선택한것.
                Debug.Log(sqlQuery);
                //UPDATE UserInfo  SET  Money = 11, Scene ='dd', Pos ='0,0,1', Car ='0,0,1'

                dbCmd.CommandText = sqlQuery;
                using (IDataReader reader = dbCmd.ExecuteReader()) // 테이블에 있는 데이터들이 들어간다. 
                {
                    dbConnection.Close();
                    reader.Close();
                }




            }
        }



        yield return null;
    }

    public void ShowBtn()
    {
        StartCoroutine(ShowDb("RankDB.sqlite"));

    }
    IEnumerator ShowDb(string p)
    {
        string Filepath = Application.persistentDataPath + "/" + p;
        if (!File.Exists(Filepath))
        {
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);
            while (!loadDB.isDone) { }
            File.WriteAllBytes(Filepath, loadDB.bytes);
        }
        string connectionString = "URI=FILE:" + Filepath;

        RankList.Clear();

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM RankTable ORDER BY DESC Time";

                dbCmd.CommandText = sqlQuery;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Debug.Log(reader.GetString(2));

                        RankList.Add(new Rank(reader.GetInt32(0), reader.GetString(1), reader.GetFloat(0)));

                    }
                    dbConnection.Close();
                    reader.Close();
                }

            }
        }
        yield return null;
    }


}
