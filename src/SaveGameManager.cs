using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using MysticChronicles.Models;

namespace MysticChronicles
{
    [DataContract]
    public class SaveData
    {
        [DataMember]
        public string HeroName { get; set; }
        
        [DataMember]
        public int Level { get; set; }
        
        [DataMember]
        public int CurrentHP { get; set; }
        
        [DataMember]
        public int MaxHP { get; set; }
        
        [DataMember]
        public int CurrentMP { get; set; }
        
        [DataMember]
        public int MaxMP { get; set; }
        
        [DataMember]
        public int Attack { get; set; }
        
        [DataMember]
        public int Defense { get; set; }
        
        [DataMember]
        public int Magic { get; set; }
        
        [DataMember]
        public int Speed { get; set; }
        
        [DataMember]
        public int Experience { get; set; }
        
        [DataMember]
        public int X { get; set; }
        
        [DataMember]
        public int Y { get; set; }

        public static SaveData FromCharacter(Character character)
        {
            return new SaveData
            {
                HeroName = character.Name,
                Level = character.Level,
                CurrentHP = character.CurrentHP,
                MaxHP = character.MaxHP,
                CurrentMP = character.CurrentMP,
                MaxMP = character.MaxMP,
                Attack = character.Attack,
                Defense = character.Defense,
                Magic = character.Magic,
                Speed = character.Speed,
                Experience = character.Experience,
                X = character.X,
                Y = character.Y
            };
        }

        public Character ToCharacter()
        {
            return new Character
            {
                Name = HeroName,
                Level = Level,
                CurrentHP = CurrentHP,
                MaxHP = MaxHP,
                CurrentMP = CurrentMP,
                MaxMP = MaxMP,
                Attack = Attack,
                Defense = Defense,
                Magic = Magic,
                Speed = Speed,
                Experience = Experience,
                X = X,
                Y = Y
            };
        }
    }

    public static class SaveGameManager
    {
        private const string SaveFileName = "savegame.dat";

        public static async Task<bool> SaveGame(Character character)
        {
            try
            {
                var saveData = SaveData.FromCharacter(character);
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.CreateFileAsync(SaveFileName, CreationCollisionOption.ReplaceExisting);

                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    var serializer = new DataContractSerializer(typeof(SaveData));
                    serializer.WriteObject(stream, saveData);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<SaveData> LoadGame()
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync(SaveFileName);

                using (var stream = await file.OpenStreamForReadAsync())
                {
                    var serializer = new DataContractSerializer(typeof(SaveData));
                    return (SaveData)serializer.ReadObject(stream);
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<bool> SaveExists()
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                await folder.GetFileAsync(SaveFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> DeleteSave()
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync(SaveFileName);
                await file.DeleteAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
