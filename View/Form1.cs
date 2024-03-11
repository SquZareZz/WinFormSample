using Dapper;
using NLog;
using Oracle.ManagedDataAccess.Client;
using ShippingOrderCOAFilter.Controller;
using ShippingOrderCOAFilter.Model;
using ShippingOrderCOAFilter.View;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Mail;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace ShippingOrderCOAFilter
{
    public partial class Form1 : Form
    {
        #region Initial Values

        // ��l�� ToolTip ���
        private System.Windows.Forms.ToolTip toolTip1 = new System.Windows.Forms.ToolTip();
        // ��l������ Form �Ϊ� Model
        // �Ҧ��A�Ψ� Form ���ѼƴX�G����b�o��
        private FormFormatModel FormModel = new FormFormatModel();
        // �ŧi Controller
        MainFormHandler LogicAgent;
        // ��l������ log ���
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Delegate Samples

        // �b Form1 ���إߩe�������
        // About TextBox
        // ��s
        public UpdateTextBoxDelegate UpdateShippingOrderNoTextBoxDelegate;
        public UpdateTextBoxDelegate UpdateCOA_textBoxDelegate;
        public UpdateTextBoxDelegate UpdateFilterResult_TextBoxDelegate;
        public UpdateTextBoxDelegate UpdateCheckResult_TextBoxDelegate;
        // �ܦ�
        public UpdateTextBoxColorDelegate UpdateFilterResult_TextBoxColorDelegate;
        public UpdateTextBoxColorDelegate UpdateCheckResult_TextBoxColorDelegate;
        // About ListView
        public UpdateListViewDelegate UpdateListViewInfoDelegate;
        public RefreshListViewDelegate RefreshListViewInfoDelegate;
        // About ClearEvent
        public ClearDelegate ClearAllItemsDelegate;
        public ClearPartialDelegate ClearPartialShippingOrderDelegate;
        // About Logger
        public UpdateLoggerDelegate UpdateLoggerInfoDelegate;

        #endregion

        #endregion

        #region Delegate Initialize

        #region ���� TextBox ���e��

        /// <summary>
        /// �e���G��l�Ʃe������A�P�����P�@��
        /// </summary>
        /// <returns></returns>
        public delegate void UpdateTextBoxDelegate(string text);
        /// <summary>
        /// �e���GShippingOrderNoTextBox �ܧ�
        /// </summary>
        /// <returns></returns>
        private void UpdateShippingOrderNoTextBox(string text)
        {
            ShippingOrderNoTextBox.Text = text;
        }
        /// <summary>
        /// �e���GCOA_textBox �ܧ�
        /// </summary>
        /// <returns></returns>
        private void UpdateCOA_textBox(string text)
        {
            COA_textBox.Text = text;
        }
        /// <summary>
        /// �e���GFilterResult_TextBox �ܧ�
        /// </summary>
        /// <returns></returns>
        private void UpdateFilterResult_TextBox(string text)
        {
            FilterResult_TextBox.Text = text;
        }
        /// <summary>
        /// �e���GCheckResult_TextBox �ܧ�
        /// </summary>
        /// <returns></returns>
        private void UpdateCheckResult_TextBox(string text)
        {
            CheckResult_TextBox.Text = text;
        }

        #endregion

        #region ���� TextBox �ܦ⪺�e��

        /// <summary>
        /// �e���G��l�Ʃe������A�P�����P�@��
        /// </summary>
        /// <returns></returns>
        public delegate void UpdateTextBoxColorDelegate(Color ChangedColor);
        /// <summary>
        /// �e���GFilterResult_TextBox �ܦ�
        /// </summary>
        /// <returns></returns>
        private void UpdateFilterResult_TextBoxColor(Color ChangedColor)
        {
            FilterResult_TextBox.BackColor = ChangedColor;
        }
        /// <summary>
        /// �e���GFilterResult_TextBox �ܦ�
        /// </summary>
        /// <returns></returns>
        private void UpdateCheckResult_TextBoxColor(Color ChangedColor)
        {
            CheckResult_TextBox.BackColor = ChangedColor;
        }

        #endregion

        #region ���� listView1 ���e��

        /// <summary>
        /// �e���G��l�Ʃe������A�P�����P�@��
        /// </summary>
        /// <returns></returns>
        public delegate void UpdateListViewDelegate(ListViewItem item);
        public delegate void RefreshListViewDelegate(ListViewItem item, string[] RefreshedItems);
        /// <summary>
        /// �e���GlistView1Item �ܧ�
        /// </summary>
        /// <returns></returns>
        private void UpdateListViewItem(ListViewItem item)
        {
            // �N��Ӷ��زK�[��ListView��
            listView1.Items.Add(item);
        }
        /// <summary>
        /// �e���GlistView1Item ��s
        /// </summary>
        /// <returns></returns>
        private void RefreshListViewItem(ListViewItem TargetItem, string[] RefreshedItems)
        {
            for (int i = 0; i < RefreshedItems.Length; i++)
            {
                TargetItem.SubItems[i].Text = RefreshedItems[i];
            }
            listView1.Refresh();
        }

        #endregion

        #region ���� Logger ���e��

        /// <summary>
        /// �e���G��l�Ʃe������A�P�����P�@��
        /// </summary>
        /// <returns></returns>
        public delegate void UpdateLoggerDelegate(string Level, string Record);
        /// <summary>
        /// �e���GLogger �g�J
        /// </summary>
        /// <returns></returns>
        private void UpdateLogger(string Level, string Record)
        {
            // �N��Ӷ��زK�[��ListView��
            WriteLog_LocalFile(Level, Record, "MainFormHandler");
        }

        #endregion

        #region ����M�z�ƥ󪺩e��

        /// <summary>
        /// �e���G��l�Ʃe������A�P�����P�@��
        /// </summary>
        /// <returns></returns>
        public delegate void ClearDelegate();
        public delegate void ClearPartialDelegate(List<string> RemoveList);
        /// <summary>
        /// �e���G�M�z�ƥ�
        /// </summary>
        /// <returns></returns>
        private void ClearAll_Delegate()
        {
            ClearAll();
        }
        private void ClearPartialShippingOrder_Delegate(List<string> RemoveList)
        {
            ClearPartialShippingOrder(RemoveList);
        }

        #endregion

        #endregion

        /// <summary>
        /// ��l�Ƶe��
        /// </summary>
        /// <returns></returns>
        public Form1()
        {
            InitializeComponent();
            // �b Form1 ���غc�禡����l�Ʃe��
            InitializeDelegate();
        }

        /// <summary>
        /// ��l�Ʃe���C��
        /// </summary>
        /// <returns></returns>
        private void InitializeDelegate()
        {
            // TextBox
            UpdateShippingOrderNoTextBoxDelegate = new UpdateTextBoxDelegate(UpdateShippingOrderNoTextBox);
            UpdateCOA_textBoxDelegate = new UpdateTextBoxDelegate(UpdateCOA_textBox);
            UpdateFilterResult_TextBoxDelegate = new UpdateTextBoxDelegate(UpdateFilterResult_TextBox);
            UpdateCheckResult_TextBoxDelegate = new UpdateTextBoxDelegate(UpdateCheckResult_TextBox);
            UpdateFilterResult_TextBoxColorDelegate = new UpdateTextBoxColorDelegate(UpdateFilterResult_TextBoxColor);
            UpdateCheckResult_TextBoxColorDelegate = new UpdateTextBoxColorDelegate(UpdateCheckResult_TextBoxColor);
            // ListView
            UpdateListViewInfoDelegate = new UpdateListViewDelegate(UpdateListViewItem);
            RefreshListViewInfoDelegate = new RefreshListViewDelegate(RefreshListViewItem);
            listView1.MouseClick += new MouseEventHandler(this.listView1_ItemActivate);
            // Logger
            UpdateLoggerInfoDelegate = new UpdateLoggerDelegate(UpdateLogger);
            // Clear
            ClearAllItemsDelegate = new ClearDelegate(ClearAll_Delegate);
            ClearPartialShippingOrderDelegate = new ClearPartialDelegate(ClearPartialShippingOrder_Delegate);
        }

        /// <summary>
        /// ��l�i�J���禡�A��l�Ʃ�b�o��
        /// </summary>
        /// <returns></returns>
        private void Form1_Load(object sender, EventArgs e)
        {
            WriteLog_LocalFile("info", "�{���Ұ�", MethodBase.GetCurrentMethod()?.Name);

            WriteLog_LocalFile("info", "�]�w���ܤ�r", MethodBase.GetCurrentMethod()?.Name);
            toolTip1.SetToolTip(SendMail, "�o�e�ֹﵲ�G�����ܳ��i");
            toolTip1.SetToolTip(Clear, "�M���X�f���T�P�ֹﵲ�G");
            toolTip1.SetToolTip(ScanShipoutRecipt, "�q��Ʈw�j�M�X�f���T");
            toolTip1.SetToolTip(OpenDirectory, "�}�Ҧs�� COA ����Ƨ�");
            toolTip1.SetToolTip(COA_Preview, "�}�� COA ��ƹw���e���A�Ӧ� DB �� COA ���");
            toolTip1.SetToolTip(ShippingOrderPreview, "�}�ҥX�f���ƹw���e���A�i�H�W�ߧR���X�f��");
            toolTip1.SetToolTip(FilterShippingOrder, "�̾ڥX�f��帹�ˬd��Ƨ����ɦW�O�_�ۦP");
            toolTip1.SetToolTip(Check_COA_with_Order, "�ֹ�X�f��P COA ��T�O�_�@�P");
            toolTip1.SetToolTip(Output_COA, "��z���� COA �ɮ׷h����w��Ƨ�");

            WriteLog_LocalFile("info", "��l�ƦC���", MethodBase.GetCurrentMethod()?.Name);
            // �K�[���
            listView1.Columns.Add("�X�f�渹");
            listView1.Columns.Add("CustomerLotNo");
            listView1.Columns.Add("����");
            listView1.Columns.Add("�U���M��");
            listView1.Columns.Add("�t�ӥN��");
            // �������i�H�Q���
            listView1.FullRowSelect = true;
            // �]�w�C�۰ʽվ�
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            // ��l������ Form �Ϊ� Handler(Agent)
            // **�n�b Form �Q�إߥH��~��إߡA�]�����n�غc�@���B�~�� Form�A�p�G����l�]�w�|�y���L���`��
            WriteLog_LocalFile("info", "�غc LogicAgent", MethodBase.GetCurrentMethod()?.Name);
            LogicAgent = new MainFormHandler();
        }

        #region ���o����W����

        /// <summary>
        /// ���o COA_TextBox �W����
        /// </summary>
        /// <returns></returns>
        public string ReturnCOA_TextBoxText()
        {
            return COA_textBox.Text;
        }

        /// <summary>
        /// ���o ListView1 �W�� Items
        /// </summary>
        /// <returns></returns>
        public ListViewItemCollection ReturnListView1Items()
        {
            return listView1.Items;
        }

        #endregion

        /// <summary>
        /// ��� ShippingOrderNoTextBox �� Enter �P�����s���\��
        /// </summary>
        /// <returns></returns>
        private void ShippingOrderNoTextBox_TextChanged(object sender, KeyPressEventArgs e)
        {            
            // �ˬd���U������O�_�O Enter ��]ASCII 13�^
            if (e.KeyChar == (char)Keys.Enter && sender == ShippingOrderNoTextBox)
            {
                ScanShipoutRecipt_Click(sender, e);
                // ���� Enter �䪺�w�]�ʧ@�]�����^
                e.Handled = true;
            }
        }

        /// <summary>
        /// �u�j�M�X�f��v���s���U��޵o���ƥ�
        /// </summary>
        /// <returns></returns>
        private void ScanShipoutRecipt_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("���B��J�X�f�渹�ADEMO ���T�w�H 123 �|��", "DEMO ���ƨ�");
                if (!String.IsNullOrEmpty(ShippingOrderNoTextBox.Text))
                {
                    WriteLog_LocalFile("info", "�j�M�X�f��", MethodBase.GetCurrentMethod()?.Name);
                    ShippingOrderNoTextBox.Text = "123";
                    FormModel = LogicAgent.ScanShipoutRecipt(FormModel, ShippingOrderNoTextBox.Text, this);
                }
                else
                {
                    MessageBox.Show("�п�J�ݨD�渹", "�ťջݨD�渹");
                    WriteLog_LocalFile("info", "�ťջݨD�渹", MethodBase.GetCurrentMethod()?.Name);
                }
                // �]�w�C�۰ʽվ�
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                ShippingOrderNoTextBox.Text = "";
            }
            catch (Exception ex)
            {
                WriteLog_LocalFile("error", "�j�M�X�f�沧�`�A�H�U�^����IT \r\n {ex}", MethodBase.GetCurrentMethod()?.Name);
                MessageBox.Show($"�j�M�X�f�沧�`�A�H�U�^����IT \r\n {ex}", "�j�M�X�f�沧�`");
                return;
            }
        }

        /// <summary>
        /// �}�Ҧs�� COA ����Ƨ�
        /// </summary>
        /// <returns></returns>
        private void OpenDirectory_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLog_LocalFile("info", "�}�Ҹ�Ƨ�", MethodBase.GetCurrentMethod()?.Name);
                using (var fbd = new FolderBrowserDialog())
                {
                    // �]�w�w�]�}�Ҫ����|
                    //fbd.SelectedPath = @"";
                    DialogResult _result = fbd.ShowDialog();
                    string COA_Dir = fbd.SelectedPath;
                    COA_textBox.Text = COA_Dir;
                }
                DialogResult result = MessageBox.Show("�ݭn�ഫ��l COA �ɮסA�@���ɮ׭n 5~10 ��\r\n" +
                    "�Э@�ߵ��ݴ��ܵ��X�{�A�~��@�~", "�T�{", MessageBoxButtons.YesNo);

                // �ھڨϥΪ̪���ܰ��椣�P���޿�
                if (result == DialogResult.Yes)
                {
                    // �ϥΪ̿�ܬO�A����U�@�Ө禡                    
                    WriteLog_LocalFile("info", "���o COA �ɮת����", MethodBase.GetCurrentMethod()?.Name);
                    FormModel = LogicAgent.GetCOA_FileInfo(FormModel, this);
                    WriteLog_LocalFile("info", "���o�Ӧ� DB �� COA ���", MethodBase.GetCurrentMethod()?.Name);
                    FormModel = LogicAgent.GetCOA_From_DB(FormModel, this);
                    MessageBox.Show("�ഫ�����I", "�T�{");
                }
                else
                {
                    COA_textBox.Text = "";
                    // �ϥΪ̿�ܧ_�A�^��D�e��
                    // �b�o�̥i�H�[�J�^��D�e���������޿�
                    return;
                }

            }
            catch (Exception ex)
            {
                WriteLog_LocalFile("error", ex.ToString(), MethodBase.GetCurrentMethod()?.Name);
                return;
            }
        }

        /// <summary>
        /// �}�� COA ��ƹw���e���A�Ӧ� DB �� COA ���
        /// </summary>
        /// <returns></returns>
        private void COA_Preview_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLog_LocalFile("info", "�}�� COA ��ƹw���e��", MethodBase.GetCurrentMethod()?.Name);
                if (String.IsNullOrEmpty(COA_textBox.Text))
                {
                    MessageBox.Show("�п�ܥ��T�ɮ׸��|", "�ť� COA �ɮ׸��|");
                }
                else
                {
                    // ��Ҥ� SecondForm
                    SubForm secondForm = new SubForm(COA_textBox.Text, FormModel.COA_List);
                    // �]�w MainForm �� SecondForm ���D����
                    secondForm.Owner = this;
                    // ��� SecondForm
                    secondForm.Show();
                }
            }
            catch (Exception ex)
            {
                WriteLog_LocalFile("warn", $"�}�� COA ��ƹw���e�����`�A�H�U�^����IT\r\n {ex}", MethodBase.GetCurrentMethod()?.Name);
                MessageBox.Show($"�}�� COA ��ƹw���e�����`�A�H�U�^����IT\r\n {ex}", "�}�ҵ������`");
                return;
            }
        }

        /// <summary>
        /// �}�ҥX�f���ƹw���e��
        /// </summary>
        /// <returns></returns>
        private void ShippingOrderPreview_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLog_LocalFile("info", "�}�ҥX�f��w���e��", MethodBase.GetCurrentMethod()?.Name);
                if (FormModel.ShippingOrderNums.Count == 0)
                {
                    MessageBox.Show("�|�L�X�f����", "�����X�f��");
                }
                else
                {
                    // ��Ҥ� SecondForm
                    SubForm2 secondForm = new SubForm2(FormModel, this);
                    // �]�w MainForm �� SecondForm ���D����
                    secondForm.Owner = this;
                    // ��� SecondForm
                    secondForm.Show();
                }
            }
            catch (Exception ex)
            {
                WriteLog_LocalFile("warn", $"�}�ҥX�f���ƹw���e�����`�A�H�U�^����IT\r\n {ex}", MethodBase.GetCurrentMethod()?.Name);
                MessageBox.Show($"�}�ҥX�f���ƹw���e�����`�A�H�U�^����IT\r\n {ex}", "�}�ҵ������`");
                return;
            }
        }

        /// <summary>
        /// �̾ڥX�f��帹�ˬd��Ƨ����ɦW�O�_�ۦP
        /// </summary>
        /// <returns></returns>
        private void Filter_Click(object sender, EventArgs e)
        {
            // ShippingOrderList => �X�f���� + ������T
            // COA_LocalFile_List => ���a�� COA �ɮ� + ������T
            WriteLog_LocalFile("info", "�̾ڥX�f��帹�ˬd��Ƨ����ɦW�O�_�ۦP", MethodBase.GetCurrentMethod()?.Name);
            try
            {
                FormModel = LogicAgent.CompareShippingOrderListWithLocal_COA_File(FormModel, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"���X�f��P���a COA �ɮײ��`�A�H�U�^����IT\r\n {ex}", "���X�f��P���a COA �ɮײ��`");
                WriteLog_LocalFile("error", $"���X�f��P���a COA �ɮײ��`�A�H�U�^����IT\r\n {ex}", MethodBase.GetCurrentMethod()?.Name);
                return;
            }
        }

        /// <summary>
        /// �ֹ�X�f��P COA ��T
        /// </summary>
        /// <returns></returns>
        private void Check_COA_with_Order_Click(object sender, EventArgs e)
        {
            // ShippingOrderList => �X�f���� + ������T
            // COA_List => ���a�� COA �ɮ� + ������T
            WriteLog_LocalFile("info", "�ֹ�X�f��P COA ��T", MethodBase.GetCurrentMethod()?.Name);
            try
            {
                FormModel = LogicAgent.Compare_COA_with_Order(FormModel, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"���X�f��P COA ���e���`�A�H�U�^����IT\r\n {ex}", "���X�f��P COA ���e���`");
                WriteLog_LocalFile("error", $"���X�f��P COA ���e���`�A�H�U�^����IT\r\n {ex}", MethodBase.GetCurrentMethod()?.Name);
                return;
            }
        }

        /// <summary>
        /// ��z���� COA �ɮ׷h����w��Ƨ�
        /// </summary>
        /// <returns></returns>
        private void Output_COA_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLog_LocalFile("info", "��z���� COA �ɮ׷h����w��Ƨ�", MethodBase.GetCurrentMethod()?.Name);
                FormModel = LogicAgent.MoveFilesToOrderedDirectory(FormModel, this);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"��X�ɮײ��`�A�H�U�^����IT\r\n {ex}", "��X�ɮײ��`");
                WriteLog_LocalFile("error", $"��X�ɮײ��`�A�H�U�^����IT\r\n {ex}", MethodBase.GetCurrentMethod()?.Name);
                return;
            }
        }

        /// <summary>
        /// �H�e���G Mail
        /// </summary>
        /// <returns></returns>
        private void SendMail_Click(object sender, EventArgs e)
        {
            try
            {
                // �ϥ� MessageBox ��ܽT�{��ܮ�
                DialogResult result = MessageBox.Show("�O�_�o�e Mail�H", "�T�{", MessageBoxButtons.YesNo);

                // �ھڨϥΪ̪���ܰ��椣�P���޿�
                if (result == DialogResult.Yes)
                {
                    // �ϥΪ̿�ܬO�A����U�@�Ө禡
                    WriteLog_LocalFile("info", "�H�e���G Mail", MethodBase.GetCurrentMethod()?.Name);
                    LogicAgent.SendMailEvent(FormModel, this);
                }
                else
                {
                    // �ϥΪ̿�ܧ_�A�^��D�e��
                    // �b�o�̥i�H�[�J�^��D�e���������޿�
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"�H�e Mail ���`�A�H�U�^����IT\r\n {ex}", "�H�e Mail ���`");
                WriteLog_LocalFile("error", $"�H�e Mail ���`�A�H�U�^����IT\r\n {ex}", MethodBase.GetCurrentMethod()?.Name);
                return;
            }
        }

        /// <summary>
        /// �M�����sĲ�o�᪺�ƥ�
        /// </summary>
        /// <returns></returns>
        private void Clear_Click(object sender, EventArgs e)
        {
            try
            {
                WriteLog_LocalFile("info", "�M�z�D�e������", MethodBase.GetCurrentMethod()?.Name);
                ClearAll();
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// �M���Ҧ��w��J���
        /// </summary>
        /// <returns></returns>
        private void ClearAll()
        {
            try
            {
                WriteLog_LocalFile("info", "�M���Ҧ��w��J���", MethodBase.GetCurrentMethod()?.Name);
                listView1.Items.Clear();
                ShippingOrderNoTextBox.Text = "";
                COA_textBox.Text = "";
                //��l�� FormFormatModel
                FormModel = new FormFormatModel();
                TestBoxReset();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�M����Ʋ��`�A�H�U�^����IT \r\n {ex}", "�M����Ʋ��`");
                throw;
            }
        }

        /// <summary>
        /// �M���X�f�泡�����
        /// </summary>
        /// <returns></returns>
        private void ClearPartialShippingOrder(List<string> RemoveList)
        {
            try
            {
                WriteLog_LocalFile("info", "�M�������ݨD��", MethodBase.GetCurrentMethod()?.Name);
                foreach (var BeRemoved in RemoveList)
                {
                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (item.Tag.ToString().Contains(BeRemoved))
                        {
                            // �R�� Form2 ��������
                            listView1.Items.Remove(item);
                            FormModel.ShippingOrderList.Remove(item.Tag.ToString());
                            FormModel.ExistingKeysOfShippingOrderAndCustPN.Remove(item.Tag.ToString());
                            FormModel.ShippingOrderNums.Remove(BeRemoved);
                        }
                    }
                }
                FormModel.OutputList = new HashSet<string>();
                FormModel.CustomerList = new HashSet<string>();
                FormModel.COA_Mapping_Lot_List = new Dictionary<string, string>();
                TestBoxReset();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"�M����Ʋ��`�A�H�U�^����IT \r\n {ex}", "�M����Ʋ��`");
                throw;
            }
        }

        /// <summary>
        /// �g�{���B�檺 log
        /// </summary>
        /// <returns></returns>
        public static void WriteLog_LocalFile(string level, string Message, string? layer)
        {
            try
            {
                switch (level)
                {
                    case "info":
                        logger.Info($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - [{layer}] - {Message}");
                        break;
                    case "warn":
                        logger.Warn($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - [{layer}] - {Message}");
                        break;
                    case "error":
                        logger.Error($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - [{layer}] - {Message}");
                        break;
                    default:
                        throw new Exception("���䴩��Log�榡");
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// �I�� listView1 �W�� item �۰ʽƻs��ŶKï
        /// </summary>
        /// <returns></returns>
        private void listView1_ItemActivate(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.ListView listview = (System.Windows.Forms.ListView)sender;
            ListViewItem lstrow = listview.GetItemAt(e.X, e.Y);
            ListViewItem.ListViewSubItem lstcol = lstrow.GetSubItemAt(e.X, e.Y);
            string strText = lstcol.Text;
            // �b�o�̳B�z ListView �� Item �Q�E�����ƥ��޿�
            try
            {
                Clipboard.SetDataObject(strText);
                //�ɥ[�� => ��m�ץ�
                toolTip1.Show($"�w�ƻs {strText}", this, e.X + 500, e.Y + 100);

                // ���� 0.3 ���۰�����ToolTip
                Thread.Sleep(300);
                toolTip1.Hide(this);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ��l�� TestBox
        /// </summary>
        /// <returns></returns>
        private void TestBoxReset()
        {
            FilterResult_TextBox.Text = FormModel.InitialFilterTextBox();
            CheckResult_TextBox.Text = FormModel.InitialCheckTextBox();
            FilterResult_TextBox.BackColor = Color.White;
            CheckResult_TextBox.BackColor = Color.White;
        }
    }
}