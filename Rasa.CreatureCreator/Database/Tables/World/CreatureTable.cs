using System.Collections.Generic;

using MySql.Data.MySqlClient;

namespace Rasa.Database.Tables.World
{
    using System;
    using Structures;

    public class CreatureTable
    {
        private static readonly MySqlCommand AddCreatureCommand = new MySqlCommand("INSERT INTO `creatures` (`classId`, `faction`, `level`, `maxhitPoints`, `nameId`, `comment`) VALUES (@ClassId, @Faction, @Level, @MaxHitPoints, @NameId, @Comment)");
        private static readonly MySqlCommand GetCreatureCommand = new MySqlCommand("SELECT * FROM creatures WHERE dbId = @DbId");
        private static readonly MySqlCommand LoadCreaturesCommand = new MySqlCommand("SELECT * FROM creatures");

        public static void Initialize()
        {

            AddCreatureCommand.Connection = GameDatabaseAccess.WorldConnection;
            AddCreatureCommand.Parameters.Add("@Classid", MySqlDbType.UInt32);
            AddCreatureCommand.Parameters.Add("@Faction", MySqlDbType.UInt32);
            AddCreatureCommand.Parameters.Add("@Level", MySqlDbType.UInt32);
            AddCreatureCommand.Parameters.Add("@MaxHitPoints", MySqlDbType.UInt32);
            AddCreatureCommand.Parameters.Add("@NameId", MySqlDbType.UInt32);
            AddCreatureCommand.Parameters.Add("@Comment", MySqlDbType.String);
            AddCreatureCommand.Prepare();

            GetCreatureCommand.Connection = GameDatabaseAccess.WorldConnection;
            GetCreatureCommand.Parameters.Add("@DbId", MySqlDbType.UInt32);
            GetCreatureCommand.Prepare();

            LoadCreaturesCommand.Connection = GameDatabaseAccess.WorldConnection;
            LoadCreaturesCommand.Prepare();
        }

        public static List<CreaturesEntry> LoadCreatures()
        {
            lock (GameDatabaseAccess.WorldLock)
            {
                var creatures = new List<CreaturesEntry>();

                using (var reader = LoadCreaturesCommand.ExecuteReader())
                    while (reader.Read())
                        creatures.Add(CreaturesEntry.Read(reader));

                return creatures;
            }
        }

        public static bool AddCreature(CreaturesEntry creature)
        {
            try
            {
                lock (GameDatabaseAccess.WorldLock)
                {
                    AddCreatureCommand.Parameters["@ClassId"].Value = creature.ClassId;
                    AddCreatureCommand.Parameters["@Faction"].Value = creature.Faction;
                    AddCreatureCommand.Parameters["@Level"].Value = creature.Level;
                    AddCreatureCommand.Parameters["@MaxHitPoints"].Value = creature.MaxHitPoints;
                    AddCreatureCommand.Parameters["@NameId"].Value = creature.NameId;
                    AddCreatureCommand.Parameters["@Comment"].Value = creature.Comment;
                    AddCreatureCommand.ExecuteNonQuery();

                    creature.DbId = (uint)AddCreatureCommand.LastInsertedId;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static CreaturesEntry GetCreature(uint dbId)
        {
            lock (GameDatabaseAccess.WorldLock)
            {
                GetCreatureCommand.Parameters["@DbId"].Value = dbId;
                using (var reader = GetCreatureCommand.ExecuteReader())
                    return CreaturesEntry.Read(reader);
            }
        }
    }
}
