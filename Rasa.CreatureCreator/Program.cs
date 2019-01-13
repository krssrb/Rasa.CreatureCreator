using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Rasa.CreatureCreator
{
    using Config;
    using Data;
    using Database;
    using Database.Tables.World;
    using Rasa.Structures;

    static class Program
    {
        public static Config Config { get; private set; }
        public static Dictionary<uint, EntityClass> LoadedCreatures = new Dictionary<uint, EntityClass>();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Configuration.OnLoad += ConfigLoaded;
            Configuration.OnReLoad += ConfigReLoaded;
            Configuration.Load();

            GameDatabaseAccess.Initialize(Config.WorldDatabaseConnectionString, Config.CharDatabaseConnectionString);

            // load Creatures
            LoadEntityClasses();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void ConfigReLoaded()
        {

            // Totally reload the configuration, because it's automatic reload case can only handle one reload. Our code's bug?
            Configuration.Load();
        }

        private static void ConfigLoaded()
        {
            var oldConfig = Config;

            Config = new Config();
            Configuration.Bind(Config);
        }

        private static void LoadEntityClasses()
        {
            var entityClassList = EntityClassTable.LoadEntityClass();

            foreach (var entityClass in entityClassList)
            {
                // Parse AugmentationList
                var augList = new List<AugmentationType>();
                var augmentations = Regex.Split(entityClass.Augmentations, @"\D+");
                var isCreature = false;

                foreach (var value in augmentations)
                    if (int.TryParse(value, out var augmentation))
                    {
                        augList.Add((AugmentationType)augmentation);
                        if ((AugmentationType)augmentation == AugmentationType.Creature)
                            isCreature = true;

                    };

                if (isCreature)
                    LoadedCreatures.Add(entityClass.ClassId, new EntityClass(
                    entityClass.ClassId,
                    entityClass.ClassName,
                    entityClass.MeshId,
                    entityClass.ClassCollisionRole,
                    augList,
                    entityClass.TargetFlag
                    ));
            };
        }
    }
}
