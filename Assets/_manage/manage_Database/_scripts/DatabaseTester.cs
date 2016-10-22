﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace EA4S.Db.Management
{
    public class DatabaseTester : MonoBehaviour
    {
#if UNITY_EDITOR
        public DatabaseLoader DBLoader;
#endif
        private DatabaseManager db;

        public Text OutputText;
        public TextMeshProUGUI OutputTextArabic;

        void Awake()
        {
            string playerId = "1";
            this.db = new DatabaseManager(playerId);
        }

        #region Main Actions

        public void ImportAll()
        {
#if UNITY_EDITOR
            DBLoader.LoadDatabase();
            DumpAllDataCounts();
#endif
        }

        public void RegenerateRuntimeDB()
        {
            db.RegenerateRuntimeDB();
        }

        #endregion

        #region Specific Logs

        public void DumpAllDataCounts()
        {
            string output = "";
            output += ("N letters: " + db.FindAllLetterData().Count) + "\n";
            output += ("N words: " + db.FindAllWordData().Count) + "\n";
            output += ("N phrases: " + db.FindAllPhraseData().Count) + "\n";
            output += ("N minigames: " + db.FindAllMiniGameData().Count) + "\n";
            output += ("N stages: " + db.FindAllStageData().Count) + "\n";
            output += ("N playsessions: " + db.FindAllPlaySessionData().Count) + "\n";
            output += ("N localizations: " + db.FindAllLocalizationData().Count) + "\n";
            output += ("N rewards: " + db.FindAllRewardData().Count) + "\n";
            PrintOutput(output);
        }

        public void DumpAllLetterData()
        {
            DumpAllData(db.FindAllLetterData());
        }

        public void DumpAllWordData()
        {
            DumpAllData(db.FindAllWordData());
        }

        public void DumpAllPhraseData()
        {
            DumpAllData(db.FindAllPhraseData());
        }

        public void DumpAllPlaySessionData()
        {
            DumpAllData(db.FindAllPlaySessionData());
        }

        public void DumpAllStageData()
        {
            DumpAllData(db.FindAllStageData());
        }

        public void DumpAllLocalizationData()
        {
            DumpAllData(db.FindAllLocalizationData());
        }

        public void DumpAllMiniGameData()
        {
            DumpAllData(db.FindAllMiniGameData());
        }


        public void DumpAllLogData()
        {
            DumpAllData(db.FindAllLogData());
        }

        public void DumpLetterById(string id)
        {
            IData data = db.GetLetterDataById(id);
            DumpDataById(id, data);
        }

        public void DumpWordById(string id)
        {
            var data = db.GetWordDataById(id);
            var arabic_text = data.Arabic;
            PrintArabicOutput(arabic_text);
            DumpDataById(id, data);
        }

        public void DumpPhraseById(string id)
        {
            IData data = db.GetPhraseDataById(id);
            DumpDataById(id, data);
        }

        public void DumpMiniGameById(string id)
        {
            IData data = db.GetMiniGameDataById(id);
            DumpDataById(id, data);
        }

        public void DumpStageById(string id)
        {
            IData data = db.GetStageDataById(id);
            DumpDataById(id, data);
        }

        public void DumpPlaySessionById(string id)
        {
            IData data = db.GetPlaySessionDataById(id);
            DumpDataById(id, data);
        }

        public void DumpLocalizationById(string id)
        {
            IData data = db.GetLocalizationDataById(id);
            DumpDataById(id, data);
        }

        public void DumpRewardById(string id)
        {
            IData data = db.GetRewardDataById(id);
            DumpDataById(id, data);
        }

        public void DumpLogDataById(string id)
        {
            IData data = db.GetLogDataById(id);
            DumpDataById(id, data);
        }



        public void DumpArabicWord(string id)
        {
            var data = db.GetWordDataById(id);
            var arabic_text = data.Arabic;
            PrintArabicOutput(arabic_text);
        }

        public void DumpActiveMinigames()
        {
            var all_minigames = db.FindAllMiniGameData();
            var active_minigames = db.FindAllActiveMinigames();
            PrintOutput(active_minigames.Count + " active minigames out of " + all_minigames.Count);
        }

        #endregion

        #region Log

        public void TestInsertLogData()
        {
            LogInfoData newData = new LogInfoData();
            newData.Id = UnityEngine.Random.Range(0f, 999).ToString();
            newData.Time = Time.time.ToString();
            newData.Session = UnityEngine.Random.Range(0, 10).ToString();
            newData.PlayerID = 1;
            newData.Score = UnityEngine.Random.Range(0f, 10f);

            this.db.InsertLogData(newData);

            PrintOutput("Inserted new LogData: " + newData.ToString());
        }

        public void TestLINQLogData()
        {
            List<LogInfoData> list = this.db.FindAllLogData(x => x.Score > 5f);
            DumpAllData(list);
        }

        #endregion

        #region Inner Dumps

        public void DumpAllData<T>(List<T> list) where T : IData
        {
            string output = "";
            foreach (var data in list) {
                output += (data.GetId() + ": " + data.ToString()) + "\n";
            }
            PrintOutput(output); ;
        }

        public void DumpDataById(string id, IData data)
        {
            string output = "";
            if (data != null) {
                output += (data.GetId() + ": " + data.ToString());
            } else {
                output += "No data with ID " + id;
            }
            PrintOutput(output);
        }

        #endregion

        #region Utilities

        void PrintOutput(string output)
        {
            Debug.Log(output);
            OutputText.text = output;
        }

        void PrintArabicOutput(string output)
        {
            //Debug.Log(fixed_output);
            OutputTextArabic.text = ArabicAlphabetHelper.PrepareStringForDisplay(output);
        }

        #endregion
    }
}
