using BhavFinder.DBPF;
using BhavFinder.DBPF.Bhav;
using BhavFinder.DBPF.GLOB;
using BhavFinder.DBPF.OBJD;
using BhavFinder.DBPF.STR;
using BhavFinder.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace BhavFinder
{
    public partial class BhavFinderForm : Form
    {
        private SortedDictionary<String, String> semiGlobalsByName = new SortedDictionary<string, string>();
        private SortedDictionary<String, String> semiGlobalsByGroup = new SortedDictionary<string, string>();

        static private SortedDictionary<String, String> globalObjectsByGroupID = new SortedDictionary<string, string>();
        static private SortedDictionary<String, String> localObjectsByGroupID = new SortedDictionary<string, string>();

        static private SortedDictionary<uint, uint> semiglobalsByGroupID = new SortedDictionary<uint, uint>();

        private MruList MyMruList;

        private TextBox[] operands = new TextBox[16];
        private TextBox[] masks = new TextBox[16];

        private Regex Hex2Regex = new Regex(@"^([0-9A-F][0-9A-F]?)$");
        private Regex HexGroupRegex = new Regex(@"^(0[xX])?([0-9A-Fa-f]+)");
        private Regex HexGUIDRegex = new Regex(@"^(0[xX])?([0-9A-Fa-f]+)");
        private Regex HexOpCodeRegex = new Regex(@"^(0[xX])?([0-9A-Fa-f]+)");
        private Regex HexInstanceRegex = new Regex(@"^(0[xX])?([0-9A-Fa-f]+)");

        private BhavData bhavData = new BhavData();

        public BhavFinderForm()
        {
            InitializeComponent();

            operands[0] = textOperand0;
            operands[1] = textOperand1;
            operands[2] = textOperand2;
            operands[3] = textOperand3;
            operands[4] = textOperand4;
            operands[5] = textOperand5;
            operands[6] = textOperand6;
            operands[7] = textOperand7;
            operands[8] = textOperand8;
            operands[9] = textOperand9;
            operands[10] = textOperand10;
            operands[11] = textOperand11;
            operands[12] = textOperand12;
            operands[13] = textOperand13;
            operands[14] = textOperand14;
            operands[15] = textOperand15;

            ClearOperands();

            masks[0] = textMask0;
            masks[1] = textMask1;
            masks[2] = textMask2;
            masks[3] = textMask3;
            masks[4] = textMask4;
            masks[5] = textMask5;
            masks[6] = textMask6;
            masks[7] = textMask7;
            masks[8] = textMask8;
            masks[9] = textMask9;
            masks[10] = textMask10;
            masks[11] = textMask11;
            masks[12] = textMask12;
            masks[13] = textMask13;
            masks[14] = textMask14;
            masks[15] = textMask15;

            ResetMasks();

            gridBhavs.DataSource = bhavData;

            parseXml("Resources/XML/semiglobals.xml", "semiglobal", semiGlobalsByName, semiGlobalsByGroup);
#if DEBUG
            Console.WriteLine("Loaded " + semiGlobalsByName.Count + " semiglobals");
#endif

            this.comboBhavInGroup.Items.Add("");
            this.comboBhavInGroup.Items.Add("0xFFFFFFFF Custom");
            this.comboBhavInGroup.Items.Add("0x7FD46CD0 Globals");
            this.comboBhavInGroup.Items.Add("0x7FE59FD0 Behaviour");

            this.comboOpCodeInGroup.Items.Add("");

            foreach (KeyValuePair<String, String> kvp in semiGlobalsByName)
            {
				String group = "0x" + kvp.Value.ToUpper() + " " + kvp.Key;
				
                this.comboBhavInGroup.Items.Add(group);
                this.comboOpCodeInGroup.Items.Add(group);
            }

            SortedDictionary<String, String> primitivesByOpCode = new SortedDictionary<string, string>();

            parseXml("Resources/XML/primitives.xml", "primitive", primitivesByOpCode);
#if DEBUG
            Console.WriteLine("Loaded " + primitivesByOpCode.Count + " primitives");
#endif

            this.comboOpCode.Items.Add("");

            foreach (KeyValuePair<String, String> kvp in primitivesByOpCode)
            {
                this.comboOpCode.Items.Add(kvp.Key + " " + kvp.Value);
            }

            SortedDictionary<String, String> textlistsByInstance = new SortedDictionary<string, string>();

            parseXml("Resources/XML/textlists.xml", "textlist", textlistsByInstance);
#if DEBUG
            Console.WriteLine("Loaded " + textlistsByInstance.Count + " textlists");
#endif

            this.comboUsingSTR.Items.Add("");

            foreach (KeyValuePair<String, String> kvp in textlistsByInstance)
            {
                this.comboUsingSTR.Items.Add(kvp.Key + " " + kvp.Value);
            }
        }

        private void ClearOperands()
        {
            foreach (TextBox operand in operands)
            {
                operand.Text = "";
                toolTip.SetToolTip(operand, "");
            }
        }

        private void ResetMasks()
        {
            String strDefaultMask = "FF";

            foreach (TextBox mask in masks)
            {
                mask.Text = strDefaultMask;
                toolTip.SetToolTip(mask, "Binary: 1111 1111");
            }
        }

        private void UpdateForm()
        {
            btnGO.Enabled = (textFilePath.Text.Length > 0 && comboOpCode.Text.Length > 0);
            bhavData.Clear();
            lblProgress.Visible = false;
        }

        private void OnFilePathChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void OnClearOperandsClicked(object sender, EventArgs e)
        {
            ClearOperands();
            UpdateForm();
        }

        private void OnOperandChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Text.Length > 0)
            {
                if (!Hex2Regex.IsMatch(tb.Text))
                {
                    tb.Text = "";
                    toolTip.SetToolTip(tb, "");
                }
                else
                {
                    toolTip.SetToolTip(tb, "Decimal: " + Convert.ToUInt32(tb.Text, 16));
                }
            }
            else
            {
                toolTip.SetToolTip(tb, "");
            }
        }

        private void OnResetMasksClicked(object sender, EventArgs e)
        {
            ResetMasks();
            UpdateForm();
        }

        private void OnMaskChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb.Text.Length > 0)
            {
                if (!Hex2Regex.IsMatch(tb.Text))
                {
                    tb.Text = "";
                    toolTip.SetToolTip(tb, "");
                }
                else
                {
                    String binStr = Convert.ToString(Convert.ToUInt32(tb.Text, 16), 2);
                    binStr = "0000000" + binStr;
                    binStr = binStr.Substring(binStr.Length-8, 8);
                    toolTip.SetToolTip(tb, "Binary: " + binStr.Substring(0, 4) + " " + binStr.Substring(4, 4));
                }
            }
            else
            {
                toolTip.SetToolTip(tb, "");
            }
        }

        private void OnKeyPress_HexOnly(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 'a' && e.KeyChar <= 'f')
            {
                e.KeyChar = (char)(e.KeyChar - 'a' + 'A');
            }

            if (e.KeyChar == 'X' || e.KeyChar == 'x')
            {
                e.KeyChar = 'x';

                if (((Control) sender).Text.Equals("0"))
                {
                    return;
                }
            }

            if (!(
                    Char.IsControl(e.KeyChar) ||
                    (e.KeyChar >= '0' && e.KeyChar <= '9') || 
                    (e.KeyChar >= 'A' && e.KeyChar <= 'F')
                ))
            {
                e.Handled = true;
            }
        }

        private void OnSelectClicked(object sender, EventArgs e)
        {
            if (selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                textFilePath.Text = selectFileDialog.FileName;
            }
        }

        private void OnClearMatchingClicked(object sender, EventArgs e)
        {
            comboUsingOperand.SelectedIndex = 0;
            comboUsingSTR.SelectedIndex = 0;
            textUsingRegex.Text = "";
            UpdateForm();
        }

        private void OnClearGroupsClicked(object sender, EventArgs e)
        {
            comboBhavInGroup.SelectedIndex = 0;
            comboOpCodeInGroup.SelectedIndex = 0;
            UpdateForm();
        }

        private void OnClearOpCodeClicked(object sender, EventArgs e)
        {
            comboOpCode.SelectedIndex = 0;
            comboVersion.SelectedIndex = 0;
            UpdateForm();
        }

        private void OnOpCodeChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            if (cb.Text.Length > 0)
            {
                Match m = HexOpCodeRegex.Match(cb.Text);
                if (m.Success)
                {
                    lblOpCodeInGroup.Visible = comboOpCodeInGroup.Visible = (Convert.ToUInt32(m.Groups[2].Value, 16) >= 0x2000);
                }
                else
                {
                    cb.Text = "";

                    lblOpCodeInGroup.Visible = comboOpCodeInGroup.Visible = false;
                }
            }
            else
            {
                lblOpCodeInGroup.Visible = comboOpCodeInGroup.Visible = false;
            }

            UpdateForm();
        }

        private void OnKeyPress_Ignore(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            BhavFinderAbout about = new BhavFinderAbout();

            about.tbProduct.Text = BhavFinderApp.AppProduct;
            about.tbCopyright.Text = BhavFinderApp.AppCopyright;

            about.StartPosition = FormStartPosition.CenterParent;
            
            about.ShowDialog();
        }

        private void OnGroupChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            if (cb.Text.Length > 0)
            {
                if (!HexGroupRegex.IsMatch(cb.Text))
                {
                    cb.Text = "";
                }
            }

            UpdateForm();
        }

        private void OnStrIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            if (cb.Text.Length > 0)
            {
                if (!HexInstanceRegex.IsMatch(cb.Text))
                {
                    cb.Text = "";
                }
            }

            UpdateForm();
        }

        private void OnConfigClicked(object sender, EventArgs e)
        {
            Form config = new BhavFinderConfig();

            config.StartPosition = FormStartPosition.CenterParent;

            if (config.ShowDialog() == DialogResult.OK)
            {
                UpdateGlobalObjects();
            }
        }

        private void parseXml(String xml, String element, SortedDictionary<String, String> byValue)
        {
            parseXml(xml, element, null, byValue);
        }

        private void parseXml(String xml, String element, SortedDictionary<String, String> byName, SortedDictionary<String, String> byValue)
        {
            XmlReader reader = XmlReader.Create(xml);

            String value = null;
            String name = null;

            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("value"))
                    {
                        reader.Read();
                        value = reader.Value;
                    }
                    else if (reader.Name.Equals("name"))
                    {
                        reader.Read();
                        name = reader.Value;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals(element))
                {
                    if (byName != null) byName.Add(name, value);
                    byValue.Add(value, name);
                }
            }
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadFormSettings(BhavFinderApp.RegistryKey, this);
            checkShowNames.Checked = bool.Parse(RegistryTools.GetSetting(BhavFinderApp.RegistryKey, checkShowNames.Name, checkShowNames.Checked.ToString()).ToString());
            OnSwitchGroupChanged(checkShowNames, null);

            MyMruList = new MruList(BhavFinderApp.RegistryKey, recentPackagesToolStripMenuItem, 8);
            MyMruList.FileSelected += MyMruList_FileSelected;

            UpdateGlobalObjects();
        }

        private void UpdateGlobalObjects()
        {
            globalObjectsByGroupID.Clear();
            semiglobalsByGroupID.Clear();

            String sims2Path = RegistryTools.GetSetting(BhavFinderApp.RegistryKey, "Sims2Path", "") as String;

            if (sims2Path.Length > 0)
            {
                try
                {
                    DBPFFile package = new DBPFFile(sims2Path + @"\TSData\Res\Objects\objects.package");

                    foreach (var entry in package.GetEntriesByType(Glob.TYPE))
                    {
                        Glob glob = new Glob(entry.GroupID, package.GetIoBuffer(entry));
                        semiglobalsByGroupID.Add(entry.GroupID, glob.SemiGlobalGroup);
                    }

                    foreach (var entry in package.GetEntriesByType(Objd.TYPE))
                    {
                        Objd objd = new Objd(entry.GroupID, package.GetIoBuffer(entry));
                        String group = SimPe.Helper.HexString(entry.GroupID);

                        if (globalObjectsByGroupID.ContainsKey(group))
                        {
                            if (entry.InstanceID == 0x41A7)
                            {
                                globalObjectsByGroupID.Remove(group);
                                globalObjectsByGroupID.Add(group, objd.FileName);
                            }
                        }
                        else
                        {
                            globalObjectsByGroupID.Add(group, objd.FileName);
                        }
                    }
                }
                catch (Exception )
                {
                    // TODO - tell the user about this
                    RegistryTools.DeleteSetting(BhavFinderApp.RegistryKey, "Sims2Path");
                }
            }

#if DEBUG
            Console.WriteLine("Loaded " + globalObjectsByGroupID.Count + " game objects");
            Console.WriteLine("Loaded " + semiglobalsByGroupID.Count + " semi-global references");
#endif

            UpdateForm();
        }

        private void UpdateLocalObjects()
        {
            localObjectsByGroupID.Clear();

            try
            {
                DBPFFile package = new DBPFFile(textFilePath.Text);

                foreach (var entry in package.GetEntriesByType(Objd.TYPE))
                {
                    Objd objd = new Objd(entry.GroupID, package.GetIoBuffer(entry));
                    String group = SimPe.Helper.HexString(entry.GroupID);

                    if (localObjectsByGroupID.ContainsKey(group))
                    {
                        if (entry.InstanceID == 0x41A7)
                        {
                            localObjectsByGroupID.Remove(group);
                            localObjectsByGroupID.Add(group, objd.FileName);
                        }
                    }
                    else
                    {
                        localObjectsByGroupID.Add(group, objd.FileName);
                    }
                }
            }
            catch (Exception)
            {
                // TODO - tell the user about this
            }

#if DEBUG
            Console.WriteLine("Loaded " + localObjectsByGroupID.Count + " local objects");
#endif
        }

        private void MyMruList_FileSelected(String package)
        {
            textFilePath.Text = package;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SaveFormSettings(BhavFinderApp.RegistryKey, this);
            RegistryTools.SaveSetting(BhavFinderApp.RegistryKey, checkShowNames.Name, checkShowNames.Checked.ToString());
        }

        private void OnSwitchGroupChanged(object sender, EventArgs e)
        {
            this.gridBhavs.Columns["colBhavGroupInstance"].Visible = !checkShowNames.Checked;
            this.gridBhavs.Columns["colBhavGroupName"].Visible = checkShowNames.Checked;
        }

        private void OnGoClicked(object sender, EventArgs e)
        {
            if (bhavFinderWorker.IsBusy)
            {
                // This is the Cancel action
                Debug.Assert(bhavFinderWorker.WorkerSupportsCancellation == true);

                // Cancel the asynchronous operation.
                bhavFinderWorker.CancelAsync();
            }
            else
            {
                Dictionary<int, HashSet<uint>> strLookupByIndexLocal = null;
                Dictionary<int, HashSet<uint>> strLookupByIndexGlobal = null;

                UpdateLocalObjects();

                // This is the Search action
                bhavData.Clear();
                btnGO.Text = "Cancel";

                lblProgress.Text = "Progress:";
                lblProgress.Visible = true;
                progressBar.Visible = true;
                progressBar.Value = 0;

                if (comboUsingOperand.Text.Length > 0 &&
                    comboUsingSTR.Text.Length > 0 && HexOpCodeRegex.IsMatch(comboUsingSTR.Text) &&
                    textUsingRegex.Text.Length > 0)
                {
                    lblProgress.Refresh();
                    progressBar.Refresh();

                    try
                    {
                        Regex regex = new Regex(textUsingRegex.Text);
                        int operand = Convert.ToInt32(comboUsingOperand.Text, 10);
                        Match m = HexInstanceRegex.Match(comboUsingSTR.Text);
                        uint instance = Convert.ToUInt32(m.Groups[2].ToString(), 16);

                        String sims2Path = RegistryTools.GetSetting(BhavFinderApp.RegistryKey, "Sims2Path", "") as String;
                        if (sims2Path.Length > 0)
                        {
                            strLookupByIndexGlobal = BuildStrLookupTable(sims2Path + @"\TSData\Res\Objects\objects.package", instance, regex);
                        }

                        strLookupByIndexLocal = BuildStrLookupTable(textFilePath.Text, instance, regex);
                    }
                    catch (Exception)
                    {
                        // TODO - tell the user about this
                    }
                }

                BhavFilter filter = GetFilters(strLookupByIndexLocal, strLookupByIndexGlobal);

                bhavFinderWorker.RunWorkerAsync(filter);
            }
        }

        private void bhavFinderWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            BhavFilter filter = e.Argument as BhavFilter;

            DBPFFile package = new DBPFFile(textFilePath.Text);

            List<DBPFEntry> bhavs = package.GetEntriesByType(Bhav.TYPE);
            int done = 0;
            int found = 0;

            foreach (var entry in bhavs)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    Bhav bhav = new Bhav(entry.GroupID, package.GetIoBuffer(entry));

                    int percentComplete = percentComplete = (int)((++done / (float)bhavs.Count) * 100.0);

                    if (filter.isWanted(bhav))
                    {
                        String group = entry.GroupID.ToString("X8");
                        String groupName;

                        

                        if (entry.GroupID == 0x7FD46CD0)
                        {
                            groupName = "(global)";
                        }
                        else if (entry.GroupID == 0x7FE59FD0)
                        {
                            groupName = "(behaviour)";
                        }
                        else if (semiGlobalsByGroup.TryGetValue(group, out groupName))
                        {
                            groupName += " (semi-global)";
                        }
                        else if (globalObjectsByGroupID.TryGetValue(group, out groupName))
                        {
                            groupName += " (game object)";
                        }
                        else if (localObjectsByGroupID.TryGetValue(group, out groupName))
                        {
                            groupName += " (local object)";
                        }
                        else if (entry.GroupID == 0xFFFFFFFF)
                        {
                            groupName = "(local)";
                        }
                        else
                        {
                            groupName = "0x" + group;
                        }

                        DataRow row = bhavData.NewRow();
                        row["Instance"] = "0x" + entry.InstanceID.ToString("X4");
                        row["Name"] = bhav.FileName;
                        row["GroupInstance"] = "0x" + entry.GroupID.ToString("X8");
                        row["GroupName"] = groupName;

                        worker.ReportProgress(percentComplete, row);
#if DEBUG
                        if (bhavs.Count < 30) System.Threading.Thread.Sleep(300);
#endif
                        ++found;
                    }
                    else
                    {
                        worker.ReportProgress(percentComplete, null);
                    }
                }
            }

            e.Result = found;
        }

        private void bhavFinderWorker_Progress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 0)
            {
                progressBar.Value = e.ProgressPercentage;
            }

            if (e.UserState != null)
            {
                bhavData.Append(e.UserState as DataRow);
            }
        }

        private void bhavFinderWorker_Completed(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            lblProgress.Visible = false;
            progressBar.Visible = false;

            if (e.Error != null)
            {
                MyMruList.RemoveFile(textFilePath.Text);
                textFilePath.Text = "";

                // TODO - tell the user about this
            }
            else
            {
                MyMruList.AddFile(textFilePath.Text);

                if (e.Cancelled == true)
                {
                }
                else
                {
                    lblProgress.Text = "Total: " + Convert.ToInt32(e.Result);
                    lblProgress.Visible = true;
                }
            }

            btnGO.Text = "FIND BHAVs";
        }

        private Dictionary<int, HashSet<uint>> BuildStrLookupTable(String packagePath, uint instanceID, Regex regex)
        {
            Dictionary<int, HashSet<uint>> lookup = new Dictionary<int, HashSet<uint>>();

            DBPFFile package = new DBPFFile(packagePath);

            foreach (var entry in package.GetEntriesByType(Str.TYPE))
            {
                if (entry.InstanceID == instanceID)
                {
                    Str str = new Str(entry.GroupID, package.GetIoBuffer(entry));
                    StrItemList entries = str.LanguageItems(SimPe.Data.MetaData.Languages.English);

                    for (int i = 0; i < entries.Length; ++i)
                    {
                        if (regex.IsMatch(entries[i].Title))
                        {
                            HashSet<uint> groups;

                            if (!lookup.TryGetValue(i, out groups))
                            {
                                groups = new HashSet<uint>();
                                lookup.Add(i, groups);
                            }

                            groups.Add(entry.GroupID);
                        }
                    }
                }
            }

            return lookup;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 || (e.Modifiers == Keys.Control && e.KeyCode == Keys.R))
            {
                OnGoClicked(btnGO, null);
            }
            else if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.N)
            {
                checkShowNames.Checked = !checkShowNames.Checked;
            }
            else if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.X)
            {
                OnClearOpCodeClicked(btnClearOpCode, null);
                OnClearOperandsClicked(btnClearOperands, null);
                OnResetMasksClicked(btnResetMasks, null);
                OnClearGroupsClicked(btnClearGroups, null);
                OnClearMatchingClicked(btnUsingClear, null);

                comboOpCode.Focus();
            }
        }

        private void OnContextMenuOperandsOpening(object sender, CancelEventArgs e)
        {
            mnuItemPasteGUID.Enabled = Clipboard.ContainsText() && HexGUIDRegex.IsMatch(Clipboard.GetText(TextDataFormat.Text));
        }

        private void menuPasteGUIDClicked(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                int index = Array.IndexOf(operands, ((sender as ToolStripMenuItem).Owner as ContextMenuStrip).SourceControl as TextBox);

                if (index >= 0 && index <= 12)
                {
                    Match m = HexGUIDRegex.Match(Clipboard.GetText(TextDataFormat.Text));

                    UInt32 GUID = Convert.ToUInt32(m.Groups[2].Value, 16);

                    for (int i = 0; i < 4; ++i) {
                        operands[index++].Text = (GUID % 256).ToString("X");
                        GUID /= 256;
                    }
                }
            }
        }
    }
}
