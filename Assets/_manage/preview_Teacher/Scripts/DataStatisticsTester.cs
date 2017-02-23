﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.DeInspektor.Attributes;
using EA4S.UI;
using UnityEngine;
using UnityEngine.UI;
using EA4S.Assessment;
using EA4S.Audio;
using EA4S.Database;

namespace EA4S.Teacher.Test
{
    /// <summary>
    /// Helper class to test DataBase contents, useful to pinpoint critical data.
    /// </summary>
    public class DataStatisticsTester : MonoBehaviour
    {
        private VocabularyHelper _vocabularyHelper;
        private DatabaseManager _databaseManager;
        //private List<PlaySessionData> _playSessionDatas;
        private List<LetterData> _letterDatas;
        private List<WordData> _wordDatas;
        private List<PhraseData> _phraseDatas;

        //private LetterFilters _letterFilters;
        private WordFilters _wordFilters;

        void Awake()
        {
            _databaseManager = AppManager.I.DB;
            _vocabularyHelper = AppManager.I.VocabularyHelper;

            //_playSessionDatas = _databaseManager.GetAllPlaySessionData();
            _letterDatas = _databaseManager.GetAllLetterData();
            _wordDatas = _databaseManager.GetAllWordData();
            _phraseDatas = _databaseManager.GetAllPhraseData();

            //_letterFilters = new LetterFilters();
            _wordFilters = new WordFilters();
        }


        [DeMethodButton("Letters Frequency")]
        public void DoLettersFrequency()
        {
            int threshold = 3;

            DoStatsList("Frequency of letters in words", _letterDatas,
                data => _vocabularyHelper.GetWordsWithLetter(_wordFilters, data.Id).Count < threshold,
                data => _vocabularyHelper.GetWordsWithLetter(_wordFilters, data.Id).Count.ToString());
        }

        [DeMethodButton("Word Length")]
        public void DoWordLength()
        {
            DoStatsList("Frequency of word length", _wordDatas,
                data => false,
                data => data.Letters.Length.ToString());
        }

        [DeMethodButton("Letter Audio")]
        public void DoLetterAudio()
        {
            DoStatsList("Letters with audio", _letterDatas,
                data => AudioManager.I.GetAudioClip(data) == null,
                data => AudioManager.I.GetAudioClip(data) == null ? "NO" : "ok");
        }

        [DeMethodButton("Word Audio")]
        public void DoWordAudio()
        {
            DoStatsList("Words with audio", _wordDatas,
                data => AudioManager.I.GetAudioClip(data) == null,
                data => AudioManager.I.GetAudioClip(data) == null ? "NO" : "ok");
        }

        [DeMethodButton("Phrase Audio")]
        public void DoPhraseAudio()
        {
            DoStatsList("Phrases with audio", _phraseDatas,
                data => AudioManager.I.GetAudioClip(data) == null,
                data => AudioManager.I.GetAudioClip(data) == null ? "NO" : "ok");
        }

        #region Internals

        void DoStatsList<T>(string title, List<T> dataList, Predicate<T> problematicCheck, Func<T, string> valueFunc)
        {
            var problematicEntries = new List<string>();

            string data_s = "\n\n";
            foreach (var data in dataList)
            {
                bool isProblematic = problematicCheck(data);

                string entryS = string.Format("{0}: \t{1}", data, valueFunc(data));
                if (isProblematic)
                {
                    data_s += "\n" + "<color=red>" + entryS + "</color>";
                    problematicEntries.Add(data.ToString());
                }
                else
                {
                    data_s += "\n" + entryS;
                }
            }

            string final_s = "---- " + title;
            if (problematicEntries.Count == 0)
            {
                final_s += "\nAll is fine!\n";
            }
            else
            {
                final_s += "\nProblematic: (" + problematicEntries.Count + ") \n";
                foreach (var entry in problematicEntries)
                    final_s += "\n" + entry;
            }

            final_s += data_s;
            PrintReport(final_s);
        }

        void PrintReport(string s)
        {
            Debug.Log(s);
        }

        #endregion

    }

}