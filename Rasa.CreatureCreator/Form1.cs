using System;
using System.Windows.Forms;

namespace Rasa.CreatureCreator
{
    using Data;
    using Database.Tables.World;
    using Structures;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CreateCreatureButton_Click(object sender, EventArgs e)
        {
            var creature = new CreaturesEntry();

            // check classId
            if (uint.TryParse(ClassId_textBox.Text, out var classId))
            {
                if (Program.LoadedCreatures.ContainsKey(classId))
                    creature.ClassId = classId;
                else
                {
                    MessageBox.Show("Given classId is not creature");
                    return;
                }
            }
            else
            {
                MessageBox.Show("classId parameter is empty");
                return;
            }

            // check for faction
            if (uint.TryParse(Faction_textBox.Text, out var faction))
            {
                creature.Faction = faction;
            }
            else
            {
                MessageBox.Show("faction parameter is empty");
                return;
            }

            // check for level
            if (uint.TryParse(Level_textBox.Text, out var level))
            {
                if (level > 50 || level < 1)
                {
                    MessageBox.Show("invalid level parameter");
                    return;
                }

                creature.Level = level;
            }
            else
            {
                MessageBox.Show("level parameter is empty");
                return;
            }

            // check for maxHitPoints
            if (uint.TryParse(MaxHitPoints_textBox.Text, out var maxHitPoints))
            {
                if (maxHitPoints < 1)
                {
                    MessageBox.Show("invalid maxHitPoints parameter");
                    return;
                }

                creature.MaxHitPoints = maxHitPoints;
            }
            else
            {
                MessageBox.Show("maxHitPoints parameter is empty");
                return;
            }

            // check for nameId
            if (uint.TryParse(NameId_textBox.Text, out var nameId))
            {
                creature.NameId = nameId;
            }
            else
            {
                MessageBox.Show("nameId parameter is empty");
                return;
            }

            // check for comment
            if (Comment_textBox.Text == "" || Comment_textBox.Text == "0")
            {
                MessageBox.Show("Please enter some comment about creature");
                return;
            }
            else
                creature.Comment = Comment_textBox.Text;

            if (!CreatureTable.AddCreature(creature))
            {
                MessageBox.Show("TechnicalDifficulty");
                return;
            }

            MessageBox.Show($"created creature with:DbId {creature.DbId}");
        }

        private void KeyPressCheckIsNumber(object sender, KeyPressEventArgs e)
        {
            var ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void KeyPressCheckForDecimal(object sender, KeyPressEventArgs e)
        {
            var ch = e.KeyChar;
            var origin = sender as System.Windows.Forms.TextBox;

            if (ch == 46 && origin.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void PickClassId_MouseClick(object sender, MouseEventArgs e)
        {
            if (PickClassIdList.Visible == true)
            {
                if (PickClassIdList.SelectedItem != null)
                {
                    var selected = PickClassIdList.SelectedItem.ToString().Split(new char[0]);

                    ClassId_textBox.Text = selected[0];
                    PickedClassId.Text = selected[1];
                }

                PickClassId_button.Text = "Pick ClassId";
                PickClassIdList.Items.Clear();
                PickClassIdList.Hide();
                return;
            }

            foreach (var creature in Program.LoadedCreatures)
            {
                var isNpc = true;

                if (IsNpc_checkBox.CheckState == CheckState.Checked)
                {
                    isNpc = false;

                    foreach (var aug in creature.Value.Augmentations)
                    {
                        if (aug == AugmentationType.NPC)
                            isNpc = true;
                    }

                    if (!isNpc)
                        continue;
                }
                PickClassIdList.Items.Add(creature.Value.ClassId.ToString() + " " + creature.Value.ClassName);
            }

            PickClassIdList.Show();
            PickClassId_button.Text = "Select ClassId";
        }

        private void ClassId_textBox_Leave(object sender, EventArgs e)
        {
            if (uint.TryParse(ClassId_textBox.Text, out var classId))
            {
                if (Program.LoadedCreatures.ContainsKey(classId))
                    PickedClassId.Text = Program.LoadedCreatures[classId].ClassName;
                else
                    PickedClassId.Text = "class not found";
            }
            else
                PickedClassId.Text = "class not found";
        }

        private void LetTest(object sender, EventArgs e)
        {
            var selected = PickClassIdList.SelectedItem.ToString().Split(new char[0]);

            ClassId_textBox.Text = selected[0];
            PickedClassId.Text = selected[1];

            PickClassId_button.Text = "Pick ClassId";
            PickClassIdList.Items.Clear();
            PickClassIdList.Hide();
            return;
        }

        private void PickCreatureFromDb(object sender, EventArgs e)
        {
            var dbId = uint.Parse(PickCreature_textBox.Text);
            var creature = CreatureTable.GetCreature(dbId);

            if (creature != null)
            {
                ManageCreature_ClassId_TextBox.Text = creature.ClassId.ToString();
                ManageCreature_Faction_TextBox.Text = creature.Faction.ToString();
                ManageCreature_Level_TextBox.Text = creature.Level.ToString();
                ManageCreature_MaxHitPoints_TextBox.Text = creature.Level.ToString();
                ManageCreature_NameId_TextBox.Text = creature.NameId.ToString();
                ManageCreature_Comment_TextBox.Text = creature.Comment;
                ManageCreature_ClassName_Label.Text = Program.LoadedCreatures[creature.ClassId].ClassName;
            }
            else
                MessageBox.Show("There is no creature with that dbId in database!");
        }
    }
}
