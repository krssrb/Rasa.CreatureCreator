using System;
using System.Collections.Generic;
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
            CC_ComboBox_Load();
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
            if (uint.TryParse(Level_ComboBox.Text, out var level))
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

            if (CC_SetAppearence_CheckButton.Checked == true)
            {

                if (CC_Helmet_ComboBox.SelectedItem is ComboBoxItem helmet)
                    CreatureAppearanceTable.SetCreatureAppearance(creature.DbId, 1, helmet.Value, new Color(CC_Helmet_Panel.BackColor).Hue);
                if (CC_Shoes_ComboBox.SelectedItem is ComboBoxItem shoes)
                    CreatureAppearanceTable.SetCreatureAppearance(creature.DbId, 2, shoes.Value, new Color(CC_Shoes_Panel.BackColor).Hue);
                if (CC_Gloves_ComboBox.SelectedItem is ComboBoxItem gloves)
                    CreatureAppearanceTable.SetCreatureAppearance(creature.DbId, 3, gloves.Value, new Color(CC_Helmet_Panel.BackColor).Hue);
                if (CC_Weapon_ComboBox.SelectedItem is ComboBoxItem weapon)
                    CreatureAppearanceTable.SetCreatureAppearance(creature.DbId, 13, weapon.Value, 0);   // weapons don't need color
                if (CC_Hair_ComboBox.SelectedItem is ComboBoxItem hair)
                    CreatureAppearanceTable.SetCreatureAppearance(creature.DbId, 14, hair.Value, new Color(CC_Hair_Panel.BackColor).Hue);
                if (CC_Torso_ComboBox.SelectedItem is ComboBoxItem torso)
                    CreatureAppearanceTable.SetCreatureAppearance(creature.DbId, 15, torso.Value, new Color(CC_Torso_Panel.BackColor).Hue);
                if (CC_Legs_ComboBox.SelectedItem is ComboBoxItem legs)
                    CreatureAppearanceTable.SetCreatureAppearance(creature.DbId, 16, legs.Value, new Color(CC_Legs_Panel.BackColor).Hue);
                if (CC_Face_ComboBox.SelectedItem is ComboBoxItem face)
                    CreatureAppearanceTable.SetCreatureAppearance(creature.DbId, 17, face.Value, new Color(CC_Face_Panel.BackColor).Hue);
            }

            MessageBox.Show($"Created Creature {creature.DbId}");
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

        private void UpdateCreature_Button_OnClick(object sender, EventArgs e)
        {
            var creature = new CreaturesEntry
            {
                DbId = uint.Parse(PickCreature_textBox.Text)
            };

            // check classId
            if (uint.TryParse(ManageCreature_ClassId_TextBox.Text, out var classId))
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
            if (uint.TryParse(ManageCreature_Faction_TextBox.Text, out var faction))
            {
                creature.Faction = faction;
            }
            else
            {
                MessageBox.Show("faction parameter is empty");
                return;
            }

            // check for level
            if (uint.TryParse(ManageCreature_Level_TextBox.Text, out var level))
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
            if (uint.TryParse(ManageCreature_MaxHitPoints_TextBox.Text, out var maxHitPoints))
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
            if (uint.TryParse(ManageCreature_NameId_TextBox.Text, out var nameId))
            {
                creature.NameId = nameId;
            }
            else
            {
                MessageBox.Show("nameId parameter is empty");
                return;
            }

            // check for comment
            if (ManageCreature_Comment_TextBox.Text == "" || ManageCreature_Comment_TextBox.Text == "0")
            {
                MessageBox.Show("Please enter some comment about creature");
                return;
            }
            else
                creature.Comment = ManageCreature_Comment_TextBox.Text;

            CreatureTable.UpdateCreature(creature);

            MessageBox.Show($"Update creature with: DbId {creature.DbId}");
        }

        private void PickColorHue(object sender, EventArgs e)
        {
            var panel = sender as Panel;

            if (CC_ColorDialog.ShowDialog() == DialogResult.OK)
            {
                var hue = new Color(CC_ColorDialog.Color).Hue;
                panel.BackColor = CC_ColorDialog.Color;
            }
        }

        private void CC_SetAppearence_CheckButton_CheckedChanged(object sender, EventArgs e)
        {
            var button = sender as CheckBox;

           // CC_Panel_Main.AutoScrollPosition = new System.Drawing.Point(0, 50);

            if (button.CheckState == CheckState.Checked)
            {
                CC_SetAppearence_Panel.Show();
            }
            else
                CC_SetAppearence_Panel.Hide();
        }

        private void CC_ComboBox_Load()
        {
            foreach (var equipment in Program.LoadedEquipment)
                switch(equipment.Value.Equipable.SlotId)
                {
                    case 1:
                        CC_Helmet_ComboBox.Items.Add(new ComboBoxItem(equipment.Value.ClassName, equipment.Value.ClassId));
                        break;
                    case 2:
                        CC_Shoes_ComboBox.Items.Add(new ComboBoxItem(equipment.Value.ClassName, equipment.Value.ClassId));
                        break;
                    case 3:
                        CC_Gloves_ComboBox.Items.Add(new ComboBoxItem(equipment.Value.ClassName, equipment.Value.ClassId));
                        break;
                    case 13:
                        CC_Weapon_ComboBox.Items.Add(new ComboBoxItem(equipment.Value.ClassName, equipment.Value.ClassId));
                        break;
                    case 14:
                        CC_Hair_ComboBox.Items.Add(new ComboBoxItem(equipment.Value.ClassName, equipment.Value.ClassId));
                        break;
                    case 15:
                        CC_Torso_ComboBox.Items.Add(new ComboBoxItem(equipment.Value.ClassName, equipment.Value.ClassId));
                        break;
                    case 16:
                        CC_Legs_ComboBox.Items.Add(new ComboBoxItem(equipment.Value.ClassName, equipment.Value.ClassId));
                        break;
                    case 17:
                        CC_Face_ComboBox.Items.Add(new ComboBoxItem(equipment.Value.ClassName, equipment.Value.ClassId));
                        break;
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 25:
                    case 26:
                    case 30:
                    case 10000001:
                        // ToDo
                        break;
                    default:
                        MessageBox.Show($"not hadled {equipment.Value.Equipable.SlotId}");
                        break;
                }
        }
    }
}
