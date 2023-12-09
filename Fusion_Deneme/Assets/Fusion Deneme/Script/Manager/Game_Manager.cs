using System;
using System.Net;
using UnityEngine;
using System.Net.Http;
using System.Globalization;
using UnityEngine.SceneManagement;

public class Game_Manager : Singletion<Game_Manager>
{
    #region OnValidate
    //public bool isValid = false;
    //[SerializeField] private List<string> soldierName = new List<string>();
    //[SerializeField] private List<Transform> soldiers = new List<Transform>();
    //private void OnValidate()
    //{
    //    if (isValid)
    //    {
    //        for (int i = 0; i < soldiers.Count; i++)
    //        {
    //            //string[] isname = soldiers[i].name.Split(' ');
    //            //string newName = "";
    //            //for (int e = 0; e < isname.Length; e++)
    //            //{
    //            //    if (e != 0)
    //            //    {
    //            //        newName += "_" + isname[e];
    //            //    }
    //            //    newName = isname[e];
    //            //    soldierName.Add(newName);
    //            //}
    //            string dd = soldiers[i].name.Replace(' ', '_');
    //            soldierName.Add(dd);
    //        }
    //        isValid = false;
    //    }
    //} 
    #endregion

    // Public or inspector values
    [SerializeField] private int mapSize = 50;
    [SerializeField] private Transform mapGround;

    #region Genel
    public override void OnAwake()
    {
        mapGround.localScale = new Vector3(mapSize, mapSize, 0);
        Utils.SetMapSize(mapSize);
        Utils.SetCamera(Camera.main);
    }
    private void Start()
    {
        LearnDate();
    }
    public void RestartMenu()
    {
        SceneManager.LoadScene("Game");
    }
    #endregion

    #region LearnDate
    private DateTime today = DateTime.UtcNow;

    [ContextMenu("Days")]
    private void Dayler()
    {
        // Yerel saati veriyor.
        try
        {
            using (var response = WebRequest.Create("http://www.google.com").GetResponse())
            {
                //string todaysDates =  response.Headers["date"];
                Debug.Log(DateTime.ParseExact(response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal));
            }
        }
        catch (WebException)
        {
            Debug.Log(DateTime.Now);
        }
        // UTC yi veriyor.
        using (var client = new HttpClient())
        {
            try
            {
                var result = client.GetAsync("https://google.com",
                      HttpCompletionOption.ResponseHeadersRead).Result;
                Debug.Log(result.Headers.Date);
            }
            catch
            {
                Debug.Log(DateTime.Now);
            }
        }
    }

    private void LearnDate()
    {
        if (CanLearnDateTime())
        {
            today = LearnDateTime();
        }
    }
    public bool CanLearnDateTime()
    {
        bool canLearnDate = false;
        try
        {
            using (var response = WebRequest.Create("http://www.google.com").GetResponse())
            {
                today = DateTime.ParseExact(response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
            }
            Canvas_Manager_Game.Instance.OnlineConnection(true);
            canLearnDate = true;
        }
        catch (WebException)
        {
            // Internetini yok  panelini açtýr.
            Canvas_Manager_Game.Instance.OnlineConnection(false);
            canLearnDate = false;
        }
        return canLearnDate;
    }
    public DateTime LearnDateTime()
    {
        return today;
    }
    #endregion
}