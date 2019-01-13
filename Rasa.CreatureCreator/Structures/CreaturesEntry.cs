using MySql.Data.MySqlClient;

namespace Rasa.Structures
{
    public class CreaturesEntry
    {
        public uint DbId { get; set; }
        public string Comment { get; set; }
        public uint ClassId { get; set; }
        public uint Faction { get; set; }
        public uint Level { get; set; }
        public uint MaxHitPoints { get; set; }
        public uint NameId { get; set; }

        public static CreaturesEntry Read(MySqlDataReader reader)
        {
            if(!reader.Read())
                return null;

            return new CreaturesEntry
            {
                DbId = reader.GetUInt32("dbId"),
                Comment = reader.GetString("comment"),
                ClassId = reader.GetUInt32("classId"),
                Faction = reader.GetUInt32("faction"),
                Level = reader.GetUInt32("level"),                
                MaxHitPoints = reader.GetUInt32("maxHitPoints"),
                NameId = reader.GetUInt32("nameId")
            };
        }
    }
}