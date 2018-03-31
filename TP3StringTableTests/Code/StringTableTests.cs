using Microsoft.VisualStudio.TestTools.UnitTesting;
using TP3.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3.Code.Tests
{
    //Les tests de chargement de la classe StringTable ont été réalisés correctement.
        //Fichier vide
        //Fichier avec entrées correctes
        //Fichier avec contenu incorrect
        //Fichier avec une ou plusieurs items manquants #1 										
        //Fichier avec une ou plusieurs items manquants #2										
        //Fichier avec une ou plusieurs items manquants #3										
    //Les tests de la méthode GetValue de la classe StringTable ont été réalisés correctement.											
        //Identifiant valide
        //Identifiant invalide

    /// <summary>
    /// Classe de test pour StringTable.
    /// </summary>
    [TestClass()]
    public class StringTableTests
    {
        // Test empty file
        [TestMethod]
        public void EmptyTextFileTest()
        {
            // Mise en place des données
            string[] content =
            {
                "",
            };
            // Appel de la méthode à tester
            ErrorCode errorCode = StringTable.GetInstance().Parse(content);
            // Validation des résultats
            Assert.AreEqual(errorCode, ErrorCode.MISSING_FIELD);
            // Clean-up
            StringTable.GetInstance().ResetStringTable();
        }

        // Test file with correct data
        [TestMethod]
        public void TextFileCorrectDataTest()
        {
            // Mise en place des données
            string[] content =
            {
                "ID_TOTAL_TIME==>Temps total---Total time",
                "ID_LIFE==>Vie---Life",
                "ID_SCORE==>Points---Score",
                "ID_PAUSE==>En pause---Pause",
                "ID_CONTROLLER_ERROR==>Svp, veuillez brancher une manette Xbox 360---Please plug in an Xbox 360 controller",
                "ID_START_PROMPT==>Appuyez sur \"start\" pour continuer---Press \"start\" to continues",
                "ID_BOMBS==>Bombes---Bombs",
            };
            // Appel de la méthode à tester
            ErrorCode errorCode = StringTable.GetInstance().Parse(content);
            // Validation des résultats
            Assert.AreEqual(errorCode, ErrorCode.OK);
            // Clean-up
            StringTable.GetInstance().ResetStringTable();
        }

        // Test file with bad format
        [TestMethod]
        public void TextFileBadFormatTest()
        {
            // Mise en place des données
            string[] content =
            {
                "ID_TOTAL_TIME==>Temps total--Total time",
                "ID_LIFE==>Vie---Life",
                "ID_SCORE==>Points---Score",
                "ID_PAUSE=>En pause---Pause",
                "ID_CONTROLLER_ERROR==>Svp, veuillez brancher une manette Xbox 360---Please plug in an Xbox 360 controller",
                "ID_START_PROMPT==>Appuyez sur \"start\" pour continuer--Press \"start\" to continues",
                "ID_BOMBS==Bombes--Bombs",
            };
            // Appel de la méthode à tester
            ErrorCode errorCode = StringTable.GetInstance().Parse(content);
            // Validation des résultats
            Assert.AreEqual(errorCode, ErrorCode.BAD_FILE_FORMAT);
            // Clean-up
            StringTable.GetInstance().ResetStringTable();
        }

        // Missing bombs
        [TestMethod]
        public void TextFileMissingData1Test()
        {
            // Mise en place des données
            string[] content =
            {
                "ID_TOTAL_TIME==>Temps total---Total time",
                "ID_LIFE==>Vie---Life",
                "ID_SCORE==>Points---Score",
                "ID_PAUSE==>En pause---Pause",
                "ID_CONTROLLER_ERROR==>Svp, veuillez brancher une manette Xbox 360---Please plug in an Xbox 360 controller",
                "ID_START_PROMPT==>Appuyez sur \"start\" pour continuer---Press \"start\" to continues",
            };
            // Appel de la méthode à tester
            ErrorCode errorCode = StringTable.GetInstance().Parse(content);
            // Validation des résultats
            Assert.AreEqual(errorCode, ErrorCode.MISSING_FIELD);
            // Clean-up
            StringTable.GetInstance().ResetStringTable();
        }

        // Missing time
        [TestMethod]
        public void TextFileMissingData2Test()
        {
            // Mise en place des données
            string[] content =
            {
                "ID_LIFE==>Vie---Life",
                "ID_SCORE==>Points---Score",
                "ID_PAUSE==>En pause---Pause",
                "ID_CONTROLLER_ERROR==>Svp, veuillez brancher une manette Xbox 360---Please plug in an Xbox 360 controller",
                "ID_START_PROMPT==>Appuyez sur \"start\" pour continuer---Press \"start\" to continues",
                "ID_BOMBS==>Bombes---Bombs",
            };
            // Appel de la méthode à tester
            ErrorCode errorCode = StringTable.GetInstance().Parse(content);
            // Validation des résultats
            Assert.AreEqual(errorCode, ErrorCode.MISSING_FIELD);
            // Clean-up
            StringTable.GetInstance().ResetStringTable();
        }

        // Missing score and pause
        [TestMethod]
        public void TextFileMissingData3Test()
        {
            // Mise en place des données
            string[] content =
            {
                "ID_TOTAL_TIME==>Temps total---Total time",
                "ID_LIFE==>Vie---Life",
                "ID_CONTROLLER_ERROR==>Svp, veuillez brancher une manette Xbox 360---Please plug in an Xbox 360 controller",
                "ID_START_PROMPT==>Appuyez sur \"start\" pour continuer---Press \"start\" to continues",
                "ID_BOMBS==>Bombes---Bombs",
            };
            // Appel de la méthode à tester
            ErrorCode errorCode = StringTable.GetInstance().Parse(content);
            // Validation des résultats
            Assert.AreEqual(errorCode, ErrorCode.MISSING_FIELD);
            // Clean-up
            StringTable.GetInstance().ResetStringTable();
        }

        // English score test
        [TestMethod]
        public void GetValueEnglishTest()
        {
            // Mise en place des données
            string[] content =
            {
                "ID_TOTAL_TIME==>Temps total---Total time",
                "ID_LIFE==>Vie---Life",
                "ID_SCORE==>Points---Score",
                "ID_PAUSE==>En pause---Pause",
                "ID_CONTROLLER_ERROR==>Svp, veuillez brancher une manette Xbox 360---Please plug in an Xbox 360 controller",
                "ID_START_PROMPT==>Appuyez sur \"start\" pour continuer---Press \"start\" to continues",
                "ID_BOMBS==>Bombes---Bombs",
            };
            // Appel de la méthode à tester
            ErrorCode errorCode = StringTable.GetInstance().Parse(content);
            string score = StringTable.GetInstance().GetValue(Language.English, "ID_SCORE");
            // Validation des résultats
            Assert.AreEqual(errorCode, ErrorCode.OK);
            Assert.AreEqual(score, "Score");
            // Clean-up
            StringTable.GetInstance().ResetStringTable();
        }

        // French total time test
        [TestMethod]
        public void GetValueFrenchTest()
        {
            // Mise en place des données
            string[] content =
            {
                "ID_TOTAL_TIME==>Temps total---Total time",
                "ID_LIFE==>Vie---Life",
                "ID_SCORE==>Points---Score",
                "ID_PAUSE==>En pause---Pause",
                "ID_CONTROLLER_ERROR==>Svp, veuillez brancher une manette Xbox 360---Please plug in an Xbox 360 controller",
                "ID_START_PROMPT==>Appuyez sur \"start\" pour continuer---Press \"start\" to continues",
                "ID_BOMBS==>Bombes---Bombs",
            };
            // Appel de la méthode à tester
            ErrorCode errorCode = StringTable.GetInstance().Parse(content);
            string totalTime = StringTable.GetInstance().GetValue(Language.French, "ID_TOTAL_TIME");
            // Validation des résultats
            Assert.AreEqual(errorCode, ErrorCode.OK);
            Assert.AreEqual(totalTime, "Temps total");
            // Clean-up
            StringTable.GetInstance().ResetStringTable();
        }

        // Invalid Id
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetValueInvalidIdTest()
        {
            // Mise en place des données
            string[] content =
            {
                "ID_TOTAL_TIME==>Temps total---Total time",
                "ID_LIFE==>Vie---Life",
                "ID_SCORE==>Points---Score",
                "ID_PAUSE==>En pause---Pause",
                "ID_CONTROLLER_ERROR==>Svp, veuillez brancher une manette Xbox 360---Please plug in an Xbox 360 controller",
                "ID_START_PROMPT==>Appuyez sur \"start\" pour continuer---Press \"start\" to continues",
                "ID_BOMBS==>Bombes---Bombs",
            };
            // Appel de la méthode à tester
            ErrorCode errorCode = StringTable.GetInstance().Parse(content);
            string totalTime = StringTable.GetInstance().GetValue(Language.French, "ID_total_time");
            // Validation des résultats
            // Clean-up
            StringTable.GetInstance().ResetStringTable();
        }
    }
}