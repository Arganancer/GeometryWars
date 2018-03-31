using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3.Code
{
    /// <summary>
    /// Classe qui gère la localisation.
    /// </summary>
    public class StringTable
    {
        private static StringTable stringTable = null;
        private Dictionary<string, string> english;
        private Dictionary<string, string> french;
        private string[] ids =
        {
            "ID_TOTAL_TIME",
            "ID_LIFE",
            "ID_SCORE",
            "ID_PAUSE",
            "ID_CONTROLLER_ERROR",
            "ID_START_PROMPT",
            "ID_BOMBS",
        };

        /// <summary>
        /// Constructeur privé de la classe StringTable.
        /// </summary>
        private StringTable()
        {
            english = new Dictionary<string, string>();
            french = new Dictionary<string, string>();
        }

        /// <summary>
        /// Méthode qui retourne la seule instance de la classe StringTable.
        /// </summary>
        /// <returns> retourne la seule instance de la classe StringTable. </returns>
        public static StringTable GetInstance()
        {
            if (stringTable == null)
            {
                stringTable = new StringTable();
                return stringTable;
            }
            return stringTable;
        }

        /// <summary>
        /// Méthode qui retourne une valeur spécifiée à partir d'une langue et d'une clé.
        /// </summary>
        /// <param name="language"> Le langage que l'on souhaite avoir. </param>
        /// <param name="key"> La clé réfère à une entrée dans le dictionnaire. </param>
        /// <returns> Retourne une valeur spécifiée à partir d'une langue et d'une clé. </returns>
        public string GetValue(Language language, string key)
        {
            if (language == Language.French)
            {
                return french[key];
            }
            return english[key];
        }

        /// <summary>
        /// Méthode qui fait l'analyse du tableau de chaines de caractères passé en paramètre, 
        /// et qui remplit les dictionnaires avec les valeurs extraites.
        /// </summary>
        /// <param name="content"> Tableau de chaines de caractères contenant les informations de localisation. </param>
        /// <returns> Retourne le message d'erreur approprié selon les résultats de l'extraction des données. </returns>
        public ErrorCode Parse(string[] content)
        {
            ErrorCode errorCode = ErrorCode.MISSING_FIELD;
            string key = " ";
            foreach (var id in ids) 
            {
                foreach (var line in content)
                {
                    errorCode = ErrorCode.MISSING_FIELD;
                    if (line.Contains(id))
                    {
                        errorCode = ErrorCode.OK;
                        break;
                    }
                }
                if (errorCode != ErrorCode.OK)
                {
                    return errorCode;
                }
            }
            foreach (var line in content)
            {
                if (!line.Contains("---") || !line.Contains("==>"))
                {
                    errorCode = ErrorCode.BAD_FILE_FORMAT;
                    return errorCode;
                }
                key = line.Substring(0, line.IndexOf('='));
                french.Add(key, line.Substring(line.IndexOf('>') + 1, line.IndexOf('-') - line.IndexOf('>') - 1));
                english.Add(key, line.Substring(line.LastIndexOf('-') + 1));
            }
            return errorCode;
        }

        /// <summary>
        /// Méthode de débogage pour réinitialiser la classe StringTable.
        /// </summary>
        public void ResetStringTable()
        {
            stringTable = null;
            french = new Dictionary<string, string>();
            english = new Dictionary<string, string>();
        }
    }
}
