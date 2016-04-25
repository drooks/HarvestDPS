using System;
using System.IO;
using HarvestDPS.Models;

namespace HarvestDPS.Extensions
{
    /// <summary>
    /// drooks@gmail.com - This class reads and writes to TripleDes encrypted flat file. Persisted models are properties of the PersistedContent model.
    /// </summary>
    public class DataStorage
    {

        #region Singleton

        // There can only be one, not unlike highlanders
        private static readonly Lazy<DataStorage> _instance = new Lazy<DataStorage>(() => new DataStorage()); //double-check locking via Lazy class

        private DataStorage() { }

        public static DataStorage Instance
        {
            get { return _instance.Value; }
        }

        #endregion

        private const string PassPhrase = "44616e6e794973417765736f6d65"; //This should be something relatively unique saved at install time
        private string FlatFileName { get {
                
                return @"C:\Temp\MyHarvest.dat";
            } }
        private bool _useCryptography = false;
        private PersistedContent _persistedContent;

        /// <summary>
        /// Object persisted in encrypted storage. Object is persisted in memory till WriteChanges() is called
        /// </summary>
        public PersistedContent PersistedContent
        {
            get { return _persistedContent ?? (_persistedContent = ReadFromStorage()); }
        }

        /// <summary>
        /// Writes PersistedContent Object to File.
        /// </summary>
        public void WriteChanges()
        {
            var content = "";
            if (_useCryptography)
            {
                //todo something wrong, fix
                var Cryptography = new Cryptography.TripleDesCrypto();
                content = Cryptography.Encrypt(PersistedContent, PassPhrase);
            }
            else
            {
                content = Newtonsoft.Json.JsonConvert.SerializeObject(PersistedContent);
            }

            File.WriteAllText(FlatFileName, content);
        }

        private PersistedContent ReadFromStorage()
        {
            if (!File.Exists(FlatFileName)) return new PersistedContent();


            var content = File.ReadAllText(FlatFileName);

            if (_useCryptography)
            {
                var Cryptography = new Cryptography.TripleDesCrypto();
                return Cryptography.Decrypt<PersistedContent>(content, PassPhrase);
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<PersistedContent>(content);
            }
        }
    }
}
