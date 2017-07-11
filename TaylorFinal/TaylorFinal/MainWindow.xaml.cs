using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
//using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
namespace TaylorFinal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] Races = { "Human", "Elf" ,"Dwarf", "Halfling","Yeti"};
        private string[] AppearanceID = { "APP_1", "APP_2","APP_3","APP_4" };
        private List<string> classIDSTracker = new List<string>();
        private List<string> scriptIDsTracker = new List<string>();
        XmlDocument doc;
        XmlDataProvider XmlDP;
        public MainWindow()
        {

            InitializeComponent();
            //populate combo boxes
            doc = new XmlDocument();
            XmlDP = (XmlDataProvider)FindResource("XDP");
            doc.Load("NPCData.xml");
            XmlDP.Document = doc;
            //add npc IDs
            npcIDNum.Items.Add("New NPC");
            npcIDNum.SelectedValue = "New NPC";
            foreach (XmlNode id in doc.SelectNodes("Data/NPCs/NPC/@ID"))
            {
                npcIDNum.Items.Add(id);
      
            }

            //add ability ids
            abilityIDNum.Items.Add("New Ability");
            abilityIDNum.SelectedValue = "New Ability";
            foreach (XmlNode abilityIDS  in doc.SelectNodes("Data/Abilities/Ability/@ID"))
            {
                abilityIDNum.Items.Add(abilityIDS);
                availableIDBoxes.Items.Add(abilityIDS);

            }

            //add class ids
            classIDComboBox.Items.Add("New Class");
            classIDComboBox.SelectedValue = "New Class";
            foreach (XmlNode classIDS in doc.SelectNodes("Data/Classes/Class/@ID"))
            {
                classIDComboBox.Items.Add(classIDS);
                classIDNum.Items.Add(classIDS);
                classIDSTracker.Add(classIDS.InnerText);
               
            }

            foreach (string item in Races)
            {
                raceComboBox.Items.Add(item);
                
            }
            foreach(string item in AppearanceID)
            {
                appearanceIDNum.Items.Add(item);
            }

            for(int k=0;k<= doc.SelectNodes("Data/Abilities/Ability/@ID").Count;k++){
                XmlNode nodeToAdd=doc.SelectSingleNode("Data/Abilities/Ability[@ID ='ABI_0"+k+ "']" + "/ScriptID");
                if (nodeToAdd!=null&&nodeToAdd.InnerText != null)
                {
                    string itemToAdd = nodeToAdd.InnerText;
                    scriptID.Items.Add(itemToAdd);
                    scriptIDsTracker.Add(itemToAdd);
                }
            }
        }

        private void npcIDNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            npcIDChanged();
        }
        private void npcIDChanged()
        {
            if (npcIDNum.SelectedIndex == 0)
            {
                levelText.Text = "1";
                npcNameText.Text = "Input Name";
                raceComboBox.SelectedIndex = 0;
                appearanceIDNum.SelectedIndex = 0;
                isAttackbleBool.IsChecked = false;
            }
            else
            {
                if (npcIDNum.SelectedValue!= null)
                {
                    npcNameText.Text = doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/DisplayName").InnerText;

                    levelText.Text = doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/Level").InnerText;

                    for (int i = 0; i < Races.Length; i++)
                    {
                        if (doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/Race").InnerText == Races[i])
                        {
                            raceComboBox.SelectedIndex = i;
                        }
                    }

                    for (int j = 0; j < classIDNum.Items.Count; j++)
                    {
                        if (doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/ClassID") != null)
                        {
                            classIDNum.SelectedIndex = classIDSTracker.IndexOf(doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/ClassID").InnerText);

                        }
                    }

                    string isNPCAttackable = doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/isAttackable").InnerText;
                    if (isNPCAttackable == "true")
                    {
                        isAttackbleBool.IsChecked = true;
                    }
                    else
                    {
                        isAttackbleBool.IsChecked = false;
                    }

                    for (int l = 0; l < AppearanceID.Length; l++)
                    {
                        if (doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/AppearanceID").InnerText == AppearanceID[l])
                        {
                            appearanceIDNum.SelectedIndex = l;
                        }
                    }


                    maxDamageNum.Text = doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/Stats/Stat[@ID ='ATK']").InnerText;
                    armorNum.Text = doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/Stats/Stat[@ID ='DEF']").InnerText;
                }
            }
        }
        private void classIDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //class data
            if (classIDComboBox.SelectedIndex == 0)
            {
                classDisplayNameText.Text = "Input Display Name";
                abilitiesUsedByClassList.Items.Clear();

            }
            else
            {
                classDisplayNameText.Text = doc.SelectSingleNode("Data/Classes/Class[@ID ='" + classIDComboBox.SelectedValue.ToString() + "']" + "/DisplayName").InnerText;
                abilitiesUsedByClassList.Items.Clear();
                foreach (XmlNode classAbilities in doc.SelectSingleNode("Data/Classes/Class[@ID ='" + classIDComboBox.SelectedValue.ToString() + "']" + "/Abilities"))
                {
                    abilitiesUsedByClassList.Items.Add(classAbilities.InnerText);
                }
            }
        }

        private void abilityIDNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (abilityIDNum.SelectedIndex == 0)
            {
                abilityDisplayNameText.Text = "Input Display Name";
                flavorTextInput.Text = "Input Display Text";
            }
            else
            {
                abilityDisplayNameText.Text = doc.SelectSingleNode("Data/Abilities/Ability[@ID ='" + abilityIDNum.SelectedValue.ToString() + "']" + "/DisplayName").InnerText;
                flavorTextInput.Text = doc.SelectSingleNode("Data/Abilities/Ability[@ID ='" + abilityIDNum.SelectedValue.ToString() + "']" + "/FlavorText").InnerText;

                for (int j = 0; j <= scriptID.Items.Count; j++)
                {
                    if (doc.SelectSingleNode("Data/Abilities/Ability[@ID ='" + abilityIDNum.SelectedValue.ToString() + "']" + "/ScriptID") != null)
                    {
                        scriptID.SelectedIndex = scriptIDsTracker.IndexOf(doc.SelectSingleNode("Data/Abilities/Ability[@ID ='" + abilityIDNum.SelectedValue.ToString() + "']" + "/ScriptID").InnerText);

                    }
                }
            }
        }

        //Adds ability to loist
        private void addAbilityID_Click(object sender, RoutedEventArgs e)
        {
            if (abilitiesUsedByClassList.Items.Contains(availableIDBoxes.SelectedValue) == false)
            {
                abilitiesUsedByClassList.Items.Add(availableIDBoxes.SelectedValue);
            }
        }

        private void saveNPCButton_Click(object sender, RoutedEventArgs e)
        {
            if (npcIDNum.SelectedItem.ToString() == "New NPC")
            {//create new npc nodes
                XmlNode npcRoot = doc.SelectSingleNode("Data/NPCs");
                XmlElement npcAttributeID = doc.CreateElement("NPC");
                npcAttributeID.SetAttribute("ID", "NPC_0" + npcIDNum.Items.Count);
              //  string newNPCID = "ID=\"NPC_0" + npcIDNum.Items.Count + "\"";
                //XmlAttribute newNPC = doc.CreateAttribute(newNPCID);
                npcRoot.AppendChild(npcAttributeID);
                XmlElement newNPCDisplayName = doc.CreateElement("DisplayName");
                newNPCDisplayName.InnerText = npcNameText.Text;
                npcAttributeID.AppendChild(newNPCDisplayName);
                XmlElement newNPCRace = doc.CreateElement("Race");
                newNPCRace.InnerText = raceComboBox.Text;
                npcAttributeID.AppendChild(newNPCRace);
                XmlElement newNPCClass = doc.CreateElement("ClassID");
                newNPCClass.InnerText = classIDNum.Text;
                npcAttributeID.AppendChild(newNPCClass);
                XmlElement newNPCLevel = doc.CreateElement("Level");
                newNPCLevel.InnerText = levelText.Text;
                npcAttributeID.AppendChild(newNPCLevel);
                XmlElement newNPCAttackable = doc.CreateElement("isAttackable");

                if (isAttackbleBool.IsChecked == true)
                {
                    newNPCAttackable.InnerText = "true";
                }
                else
                {
                    newNPCAttackable.InnerText = "false";
                }
                npcAttributeID.AppendChild(newNPCAttackable);
                XmlElement newNPCAppearanceID = doc.CreateElement("AppearanceID");
                newNPCAppearanceID.InnerText = appearanceIDNum.Text;
                npcAttributeID.AppendChild(newNPCAppearanceID);

                XmlElement newNPCStats = doc.CreateElement("Stats");
                npcAttributeID.AppendChild(newNPCStats);
                XmlElement atkStatID = doc.CreateElement("Stat");
                atkStatID.SetAttribute("ID","ATK");
                newNPCStats.AppendChild(atkStatID);
                XmlElement newNPCAtkValue = doc.CreateElement("Value");
                newNPCAtkValue.InnerText = "0";
                atkStatID.AppendChild(newNPCAtkValue);
                XmlElement defStatID = doc.CreateElement("Stat");
                defStatID.SetAttribute("ID", "DEF");
                newNPCStats.AppendChild(defStatID);
                XmlElement newNPCDefValue = doc.CreateElement("Value");
                newNPCDefValue.InnerText = "0";
                defStatID.AppendChild(newNPCDefValue);

                doc.Save("NPCData.xml");
                npcIDNum.Items.Add("NPC_0" + npcIDNum.Items.Count);
             //   npcIDNum.SelectedIndex = npcIDNum.Items.Count - 1;
              //  npcIDNum.SelectedItem = npcIDNum.Items.Count - 1;
               // npcIDChanged();

                levelText.Text = "1";
                npcNameText.Text = "Input Name";
                raceComboBox.SelectedIndex = 0;
                appearanceIDNum.SelectedIndex = 0;
                isAttackbleBool.IsChecked = false;
            }
            else if(npcIDNum.SelectedIndex>=1)
            {
                doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/DisplayName").InnerText = npcNameText.Text;

                doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/Level").InnerText = levelText.Text;

                doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/Race").InnerText = raceComboBox.Text;

                doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/ClassID").InnerText = classIDNum.Text;

                if (isAttackbleBool.IsChecked==true)
                {
                    doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/isAttackable").InnerText="true";
                }
                else
                {
                    doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/isAttackable").InnerText = "false";
                }

                doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/AppearanceID").InnerText = appearanceIDNum.Text;

                doc.Save("NPCData.xml");
            }
        }

        private void saveStatsButton_Click(object sender, RoutedEventArgs e)
        {
            if(npcIDNum.Text!="New NPC"&&npcIDNum.SelectedIndex>=-1)
            {
                doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/Stats/Stat[@ID ='ATK']").InnerText= maxDamageNum.Text;
                doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']" + "/Stats/Stat[@ID ='DEF']").InnerText = armorNum.Text;
                doc.Save("NPCData.xml");
            }
        }

        private void saveClassIDButton_Click(object sender, RoutedEventArgs e)
        {
            if(classIDComboBox.Text!="New Class"&&classIDComboBox.SelectedIndex>=-1)
            {
                doc.SelectSingleNode("Data/Classes/Class[@ID ='" + classIDComboBox.SelectedValue.ToString() + "']" + "/DisplayName").InnerText = classDisplayNameText.Text;
                //TODO:fix this part
                abilitiesUsedByClassList.Items.Clear();
                foreach (XmlNode classAbilities in doc.SelectSingleNode("Data/Classes/Class[@ID ='" + classIDComboBox.SelectedValue.ToString() + "']" + "/Abilities"))
                {
                    abilitiesUsedByClassList.Items.Add(classAbilities.InnerText);
                }
                doc.Save("NPCData.xml");
            }
            else
            {
                XmlNode classRoot = doc.SelectSingleNode("Data/Classes");
                XmlElement classID = doc.CreateElement("Class");
                classID.SetAttribute("ID", classDisplayNameText.Text);
                classRoot.AppendChild(classID);
                XmlElement className = doc.CreateElement("DisplayName");
                className.InnerText = classDisplayNameText.Text;
                classID.AppendChild(className);
                XmlElement classAbilities = doc.CreateElement("Abilities");
                classID.AppendChild(classAbilities);
                foreach(string item in abilitiesUsedByClassList.Items)
                {
                    XmlElement ability = doc.CreateElement("Ability");
                    ability.InnerText = item;
                    classAbilities.AppendChild(ability);
                }
                classIDNum.Items.Add(classDisplayNameText.Text);
                classIDComboBox.Items.Add(classDisplayNameText.Text);
                doc.Save("NPCData.xml");
            }
        }

        private void saveAbilityButton_Click(object sender, RoutedEventArgs e)
        {
            if(abilityIDNum.Text!="New Ability" && abilityIDNum.SelectedIndex > -1)
            {
                doc.SelectSingleNode("Data/Abilities/Ability[@ID ='" + abilityIDNum.SelectedValue.ToString() + "']" + "/DisplayName").InnerText = abilityDisplayNameText.Text;
                doc.SelectSingleNode("Data/Abilities/Ability[@ID ='" + abilityIDNum.SelectedValue.ToString() + "']" + "/FlavorText").InnerText = flavorTextInput.Text;
                doc.SelectSingleNode("Data/Abilities/Ability[@ID ='" + abilityIDNum.SelectedValue.ToString() + "']" + "/ScriptID").InnerText = scriptID.Text;
                doc.Save("NPCData.xml");
            }
            else
            {
                XmlNode abilityRoot = doc.SelectSingleNode("Data/Abilities");
                XmlElement abilityIDNode = doc.CreateElement("Ability");
                abilityIDNode.SetAttribute("ID", "ABI_0"+abilityIDNum.Items.Count);
                abilityRoot.AppendChild(abilityIDNode);
                XmlElement abilityName = doc.CreateElement("DisplayName");
                abilityName.InnerText = abilityDisplayNameText.Text;
                abilityIDNode.AppendChild(abilityName);
                XmlElement flavorTextDisplay = doc.CreateElement("FlavorText");
                flavorTextDisplay.InnerText = flavorTextInput.Text;
                abilityIDNode.AppendChild(flavorTextDisplay);
                XmlElement scriptIDName = doc.CreateElement("ScriptID");
                scriptIDName.InnerText = scriptID.Text;
                abilityIDNode.AppendChild(scriptIDName);
                doc.Save("NPCData.xml");
                abilityIDNum.Items.Add("ABI_0" + abilityIDNum.Items.Count);
                availableIDBoxes.Items.Add("ABI_0" + abilityIDNum.Items.Count);
            }
        }

        private void removeNpcSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            if(npcIDNum.SelectedItem.ToString() != "New NPC")
            {
                XmlNode npcRoot = doc.SelectSingleNode("Data/NPCs");
                XmlNode npcToRemove = doc.SelectSingleNode("Data/NPCs/NPC[@ID ='" + npcIDNum.SelectedValue.ToString() + "']");
                npcRoot.RemoveChild(npcToRemove);
                npcIDNum.SelectedIndex = npcIDNum.SelectedIndex - 1;
                npcIDNum.SelectedValue = npcIDNum.Items[npcIDNum.SelectedIndex];
                npcIDNum.Items.RemoveAt(npcIDNum.SelectedIndex+1);
                doc.Save("NPCData.xml");
            }
        }

        private void removeSelectedClassButton_Click(object sender, RoutedEventArgs e)
        {
            if (classIDComboBox.SelectedItem.ToString() != "New Class")
            {
                XmlNode classRoot = doc.SelectSingleNode("Data/Classes");
                XmlNode classToRemove = doc.SelectSingleNode("Data/Classes/Class[@ID ='" + classIDComboBox.SelectedValue.ToString() + "']");
                classRoot.RemoveChild(classToRemove);
                classIDComboBox.SelectedIndex = classIDComboBox.SelectedIndex - 1;
                classIDComboBox.SelectedValue = classIDComboBox.Items[classIDComboBox.SelectedIndex];
                classIDComboBox.Items.RemoveAt(classIDNum.SelectedIndex + 1);
                classIDNum.Items.RemoveAt(classIDNum.SelectedIndex + 1);
              
                doc.Save("NPCData.xml");
            }
        }

        private void removeAbilityButton_Click(object sender, RoutedEventArgs e)
        {

            if (classIDComboBox.SelectedItem.ToString() != "New Class")
            {
                XmlNode abilityRoot = doc.SelectSingleNode("Data/Abilities");
                XmlNode abilityToRemove = doc.SelectSingleNode("Data/Abilities/Ability[@ID ='" + abilityIDNum.SelectedValue.ToString() + "']");
                abilityRoot.RemoveChild(abilityToRemove);
                abilityIDNum.SelectedIndex = abilityIDNum.SelectedIndex - 1;
                availableIDBoxes.Items.RemoveAt(abilityIDNum.SelectedIndex + 1);
                abilityIDNum.Items.RemoveAt(abilityIDNum.SelectedIndex+1);

                doc.Save("NPCData.xml");
            }
        }
    }
}
