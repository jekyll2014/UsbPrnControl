//This document contains programming examples.
//Custom S.P.A. grants you a nonexclusive copyright license to use all programming code examples from which you can generate similar function tailored to your own specific needs.
//All sample code is provided by Custom S.P.A. for illustrative purposes only. These examples have not been thoroughly tested under all conditions. 
//Custom S.P.A., therefore, cannot guarantee or imply reliability, serviceability, or function of these programs.
//In no event shall Custom S.P.A. be liable for any direct, indirect, incidental, special, exemplary, or consequential damages (including, but not limited to, procurement of substitute goods or services; loss of use, data, or profits; or business interruption) however caused and on any theory of liability, whether in contract, strict liability, or tort 
//(including negligence or otherwise) arising in any way out of the use of this software, even if advised of the possibility of such damage.
//All programs contained herein are provided to you "as is" without any warranties of any kind. 
//The implied warranties of non-infringement, merchantability and fitness for a particular purpose are expressly disclaimed.

using System.Windows.Forms;

namespace UsbPrnControl
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBox_Printer = new System.Windows.Forms.ComboBox();
            this.label_UsbPrnList = new System.Windows.Forms.Label();
            this.button_Open = new System.Windows.Forms.Button();
            this.button_Refresh = new System.Windows.Forms.Button();
            this.button_closeport = new System.Windows.Forms.Button();
            this.button_Send = new System.Windows.Forms.Button();
            this.textBox_command = new System.Windows.Forms.TextBox();
            this.textBox_terminal = new System.Windows.Forms.TextBox();
            this.checkBox_hexCommand = new System.Windows.Forms.CheckBox();
            this.checkBox_autoscroll = new System.Windows.Forms.CheckBox();
            this.checkBox_hexTerminal = new System.Windows.Forms.CheckBox();
            this.checkBox_hexParam = new System.Windows.Forms.CheckBox();
            this.textBox_param = new System.Windows.Forms.TextBox();
            this.button_Clear = new System.Windows.Forms.Button();
            this.checkBox_saveInput = new System.Windows.Forms.CheckBox();
            this.textBox_saveTo = new System.Windows.Forms.TextBox();
            this.button_openFile = new System.Windows.Forms.Button();
            this.textBox_fileName = new System.Windows.Forms.TextBox();
            this.checkBox_hexFileOpen = new System.Windows.Forms.CheckBox();
            this.button_sendFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBox_delay = new System.Windows.Forms.TextBox();
            this.textBox_sendNum = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.radioButton_stream = new System.Windows.Forms.RadioButton();
            this.radioButton_byByte = new System.Windows.Forms.RadioButton();
            this.radioButton_byString = new System.Windows.Forms.RadioButton();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBox_strDelay = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_printer = new System.Windows.Forms.CheckBox();
            this.checkBox_scanner = new System.Windows.Forms.CheckBox();
            this.checkBox_saveTime = new System.Windows.Forms.CheckBox();
            this.checkBox_saveOutput = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // comboBox_Printer
            // 
            this.comboBox_Printer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Printer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Printer.FormattingEnabled = true;
            this.comboBox_Printer.Location = new System.Drawing.Point(12, 25);
            this.comboBox_Printer.Name = "comboBox_Printer";
            this.comboBox_Printer.Size = new System.Drawing.Size(560, 21);
            this.comboBox_Printer.TabIndex = 1;
            // 
            // label_UsbPrnList
            // 
            this.label_UsbPrnList.AutoSize = true;
            this.label_UsbPrnList.Location = new System.Drawing.Point(12, 9);
            this.label_UsbPrnList.Name = "label_UsbPrnList";
            this.label_UsbPrnList.Size = new System.Drawing.Size(64, 13);
            this.label_UsbPrnList.TabIndex = 1;
            this.label_UsbPrnList.Text = "Devices list:";
            // 
            // button_Open
            // 
            this.button_Open.Location = new System.Drawing.Point(12, 52);
            this.button_Open.Name = "button_Open";
            this.button_Open.Size = new System.Drawing.Size(70, 25);
            this.button_Open.TabIndex = 2;
            this.button_Open.Text = "Open";
            this.button_Open.UseVisualStyleBackColor = true;
            this.button_Open.Click += new System.EventHandler(this.button_OPEN_Click);
            // 
            // button_Refresh
            // 
            this.button_Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Refresh.Location = new System.Drawing.Point(502, 52);
            this.button_Refresh.Name = "button_Refresh";
            this.button_Refresh.Size = new System.Drawing.Size(70, 25);
            this.button_Refresh.TabIndex = 0;
            this.button_Refresh.Text = "Refresh";
            this.button_Refresh.UseVisualStyleBackColor = true;
            this.button_Refresh.Click += new System.EventHandler(this.button_REFRESH_Click);
            // 
            // button_closeport
            // 
            this.button_closeport.Enabled = false;
            this.button_closeport.Location = new System.Drawing.Point(88, 52);
            this.button_closeport.Name = "button_closeport";
            this.button_closeport.Size = new System.Drawing.Size(70, 25);
            this.button_closeport.TabIndex = 16;
            this.button_closeport.Text = "Close";
            this.button_closeport.UseVisualStyleBackColor = true;
            this.button_closeport.Click += new System.EventHandler(this.button_CLOSE_Click);
            // 
            // button_Send
            // 
            this.button_Send.Enabled = false;
            this.button_Send.Location = new System.Drawing.Point(12, 83);
            this.button_Send.Name = "button_Send";
            this.button_Send.Size = new System.Drawing.Size(70, 47);
            this.button_Send.TabIndex = 7;
            this.button_Send.Text = "Send";
            this.button_Send.UseVisualStyleBackColor = true;
            this.button_Send.Click += new System.EventHandler(this.button_WRITE_Click);
            // 
            // textBox_command
            // 
            this.textBox_command.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_command.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_command.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_command.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_command.Location = new System.Drawing.Point(186, 83);
            this.textBox_command.Name = "textBox_command";
            this.textBox_command.Size = new System.Drawing.Size(386, 20);
            this.textBox_command.TabIndex = 4;
            this.textBox_command.Leave += new System.EventHandler(this.textBox_command_Leave);
            // 
            // textBox_terminal
            // 
            this.textBox_terminal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_terminal.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_terminal.Location = new System.Drawing.Point(12, 198);
            this.textBox_terminal.MaxLength = 3276700;
            this.textBox_terminal.Multiline = true;
            this.textBox_terminal.Name = "textBox_terminal";
            this.textBox_terminal.ReadOnly = true;
            this.textBox_terminal.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_terminal.Size = new System.Drawing.Size(560, 21);
            this.textBox_terminal.TabIndex = 17;
            // 
            // checkBox_hexCommand
            // 
            this.checkBox_hexCommand.AutoSize = true;
            this.checkBox_hexCommand.Checked = true;
            this.checkBox_hexCommand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_hexCommand.Location = new System.Drawing.Point(88, 89);
            this.checkBox_hexCommand.Name = "checkBox_hexCommand";
            this.checkBox_hexCommand.Size = new System.Drawing.Size(92, 17);
            this.checkBox_hexCommand.TabIndex = 3;
            this.checkBox_hexCommand.Text = "hex command";
            this.checkBox_hexCommand.UseVisualStyleBackColor = true;
            this.checkBox_hexCommand.CheckedChanged += new System.EventHandler(this.checkBox_hexCommand_CheckedChanged);
            // 
            // checkBox_autoscroll
            // 
            this.checkBox_autoscroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_autoscroll.AutoSize = true;
            this.checkBox_autoscroll.Checked = true;
            this.checkBox_autoscroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_autoscroll.Location = new System.Drawing.Point(12, 233);
            this.checkBox_autoscroll.Name = "checkBox_autoscroll";
            this.checkBox_autoscroll.Size = new System.Drawing.Size(75, 17);
            this.checkBox_autoscroll.TabIndex = 12;
            this.checkBox_autoscroll.Text = "Autoscroll;";
            this.checkBox_autoscroll.UseVisualStyleBackColor = true;
            // 
            // checkBox_hexTerminal
            // 
            this.checkBox_hexTerminal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_hexTerminal.AutoSize = true;
            this.checkBox_hexTerminal.Checked = true;
            this.checkBox_hexTerminal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_hexTerminal.Location = new System.Drawing.Point(90, 233);
            this.checkBox_hexTerminal.Name = "checkBox_hexTerminal";
            this.checkBox_hexTerminal.Size = new System.Drawing.Size(48, 17);
            this.checkBox_hexTerminal.TabIndex = 13;
            this.checkBox_hexTerminal.Text = "Hex;";
            this.checkBox_hexTerminal.UseVisualStyleBackColor = true;
            // 
            // checkBox_hexParam
            // 
            this.checkBox_hexParam.AutoSize = true;
            this.checkBox_hexParam.Checked = true;
            this.checkBox_hexParam.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_hexParam.Location = new System.Drawing.Point(88, 112);
            this.checkBox_hexParam.Name = "checkBox_hexParam";
            this.checkBox_hexParam.Size = new System.Drawing.Size(93, 17);
            this.checkBox_hexParam.TabIndex = 5;
            this.checkBox_hexParam.Text = "hex parameter";
            this.checkBox_hexParam.UseVisualStyleBackColor = true;
            this.checkBox_hexParam.CheckedChanged += new System.EventHandler(this.checkBox_hexParam_CheckedChanged);
            // 
            // textBox_param
            // 
            this.textBox_param.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_param.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_param.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_param.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_param.Location = new System.Drawing.Point(186, 110);
            this.textBox_param.Name = "textBox_param";
            this.textBox_param.Size = new System.Drawing.Size(386, 20);
            this.textBox_param.TabIndex = 6;
            this.textBox_param.Leave += new System.EventHandler(this.textBox_param_Leave);
            // 
            // button_Clear
            // 
            this.button_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Clear.Location = new System.Drawing.Point(502, 225);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(70, 25);
            this.button_Clear.TabIndex = 15;
            this.button_Clear.Text = "Clear";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // checkBox_saveInput
            // 
            this.checkBox_saveInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_saveInput.AutoSize = true;
            this.checkBox_saveInput.Checked = true;
            this.checkBox_saveInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_saveInput.Location = new System.Drawing.Point(233, 233);
            this.checkBox_saveInput.Name = "checkBox_saveInput";
            this.checkBox_saveInput.Size = new System.Drawing.Size(49, 17);
            this.checkBox_saveInput.TabIndex = 98;
            this.checkBox_saveInput.Text = "input";
            this.checkBox_saveInput.UseVisualStyleBackColor = true;
            this.checkBox_saveInput.CheckedChanged += new System.EventHandler(this.checkBox_saveTo_CheckedChanged);
            // 
            // textBox_saveTo
            // 
            this.textBox_saveTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_saveTo.Enabled = false;
            this.textBox_saveTo.Location = new System.Drawing.Point(350, 231);
            this.textBox_saveTo.Name = "textBox_saveTo";
            this.textBox_saveTo.Size = new System.Drawing.Size(62, 20);
            this.textBox_saveTo.TabIndex = 14;
            this.textBox_saveTo.Text = "usb_rx.txt";
            // 
            // button_openFile
            // 
            this.button_openFile.Location = new System.Drawing.Point(12, 136);
            this.button_openFile.MinimumSize = new System.Drawing.Size(70, 25);
            this.button_openFile.Name = "button_openFile";
            this.button_openFile.Size = new System.Drawing.Size(70, 25);
            this.button_openFile.TabIndex = 8;
            this.button_openFile.Text = "Select file:";
            this.button_openFile.UseVisualStyleBackColor = true;
            this.button_openFile.Click += new System.EventHandler(this.button_openFile_Click);
            // 
            // textBox_fileName
            // 
            this.textBox_fileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_fileName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_fileName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_fileName.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_fileName.Location = new System.Drawing.Point(186, 139);
            this.textBox_fileName.Name = "textBox_fileName";
            this.textBox_fileName.Size = new System.Drawing.Size(310, 20);
            this.textBox_fileName.TabIndex = 9;
            this.textBox_fileName.TextChanged += new System.EventHandler(this.textBox_fileName_TextChanged);
            // 
            // checkBox_hexFileOpen
            // 
            this.checkBox_hexFileOpen.AutoSize = true;
            this.checkBox_hexFileOpen.Checked = true;
            this.checkBox_hexFileOpen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_hexFileOpen.Location = new System.Drawing.Point(88, 141);
            this.checkBox_hexFileOpen.Name = "checkBox_hexFileOpen";
            this.checkBox_hexFileOpen.Size = new System.Drawing.Size(87, 17);
            this.checkBox_hexFileOpen.TabIndex = 10;
            this.checkBox_hexFileOpen.Text = "hex text data";
            this.checkBox_hexFileOpen.UseVisualStyleBackColor = true;
            this.checkBox_hexFileOpen.CheckedChanged += new System.EventHandler(this.checkBox_hexFileOpen_CheckedChanged);
            // 
            // button_sendFile
            // 
            this.button_sendFile.Enabled = false;
            this.button_sendFile.Location = new System.Drawing.Point(12, 167);
            this.button_sendFile.MinimumSize = new System.Drawing.Size(70, 25);
            this.button_sendFile.Name = "button_sendFile";
            this.button_sendFile.Size = new System.Drawing.Size(70, 25);
            this.button_sendFile.TabIndex = 11;
            this.button_sendFile.Text = "Send file:";
            this.button_sendFile.UseVisualStyleBackColor = true;
            this.button_sendFile.Click += new System.EventHandler(this.button_sendFile_ClickAsync);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // textBox_delay
            // 
            this.textBox_delay.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_delay.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_delay.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_delay.Location = new System.Drawing.Point(475, 170);
            this.textBox_delay.MaxLength = 5;
            this.textBox_delay.Name = "textBox_delay";
            this.textBox_delay.Size = new System.Drawing.Size(40, 20);
            this.textBox_delay.TabIndex = 105;
            this.textBox_delay.Text = "1000";
            this.textBox_delay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox_sendNum
            // 
            this.textBox_sendNum.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_sendNum.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_sendNum.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_sendNum.Location = new System.Drawing.Point(389, 170);
            this.textBox_sendNum.MaxLength = 5;
            this.textBox_sendNum.Name = "textBox_sendNum";
            this.textBox_sendNum.Size = new System.Drawing.Size(40, 20);
            this.textBox_sendNum.TabIndex = 106;
            this.textBox_sendNum.Text = "1";
            this.textBox_sendNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(521, 173);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(54, 13);
            this.label24.TabIndex = 107;
            this.label24.Text = "ms. delay;";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(435, 173);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(34, 13);
            this.label23.TabIndex = 108;
            this.label23.Text = "times;";
            // 
            // radioButton_stream
            // 
            this.radioButton_stream.AutoSize = true;
            this.radioButton_stream.Checked = true;
            this.radioButton_stream.Location = new System.Drawing.Point(88, 171);
            this.radioButton_stream.Name = "radioButton_stream";
            this.radioButton_stream.Size = new System.Drawing.Size(59, 17);
            this.radioButton_stream.TabIndex = 112;
            this.radioButton_stream.TabStop = true;
            this.radioButton_stream.Text = "stream;";
            this.radioButton_stream.UseVisualStyleBackColor = true;
            this.radioButton_stream.CheckedChanged += new System.EventHandler(this.radioButton_stream_CheckedChanged);
            // 
            // radioButton_byByte
            // 
            this.radioButton_byByte.AutoSize = true;
            this.radioButton_byByte.Location = new System.Drawing.Point(220, 171);
            this.radioButton_byByte.Name = "radioButton_byByte";
            this.radioButton_byByte.Size = new System.Drawing.Size(62, 17);
            this.radioButton_byByte.TabIndex = 113;
            this.radioButton_byByte.TabStop = true;
            this.radioButton_byByte.Text = "by byte;";
            this.radioButton_byByte.UseVisualStyleBackColor = true;
            // 
            // radioButton_byString
            // 
            this.radioButton_byString.AutoSize = true;
            this.radioButton_byString.Location = new System.Drawing.Point(150, 171);
            this.radioButton_byString.Name = "radioButton_byString";
            this.radioButton_byString.Size = new System.Drawing.Size(67, 17);
            this.radioButton_byString.TabIndex = 114;
            this.radioButton_byString.TabStop = true;
            this.radioButton_byString.Text = "by string;";
            this.radioButton_byString.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(502, 139);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(70, 19);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 111;
            // 
            // textBox_strDelay
            // 
            this.textBox_strDelay.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_strDelay.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_strDelay.Enabled = false;
            this.textBox_strDelay.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_strDelay.Location = new System.Drawing.Point(286, 170);
            this.textBox_strDelay.MaxLength = 5;
            this.textBox_strDelay.Name = "textBox_strDelay";
            this.textBox_strDelay.Size = new System.Drawing.Size(40, 20);
            this.textBox_strDelay.TabIndex = 109;
            this.textBox_strDelay.Text = "10";
            this.textBox_strDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(332, 173);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 110;
            this.label1.Text = "ms. delay;";
            // 
            // checkBox_printer
            // 
            this.checkBox_printer.AutoSize = true;
            this.checkBox_printer.Checked = true;
            this.checkBox_printer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_printer.Location = new System.Drawing.Point(82, 8);
            this.checkBox_printer.Name = "checkBox_printer";
            this.checkBox_printer.Size = new System.Drawing.Size(55, 17);
            this.checkBox_printer.TabIndex = 115;
            this.checkBox_printer.Text = "printer";
            this.checkBox_printer.UseVisualStyleBackColor = true;
            this.checkBox_printer.CheckedChanged += new System.EventHandler(this.checkBox_printer_CheckedChanged);
            // 
            // checkBox_scanner
            // 
            this.checkBox_scanner.AutoSize = true;
            this.checkBox_scanner.Location = new System.Drawing.Point(143, 8);
            this.checkBox_scanner.Name = "checkBox_scanner";
            this.checkBox_scanner.Size = new System.Drawing.Size(64, 17);
            this.checkBox_scanner.TabIndex = 115;
            this.checkBox_scanner.Text = "scanner";
            this.checkBox_scanner.UseVisualStyleBackColor = true;
            this.checkBox_scanner.CheckedChanged += new System.EventHandler(this.checkBox_scanner_CheckedChanged);
            // 
            // checkBox_saveTime
            // 
            this.checkBox_saveTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_saveTime.AutoSize = true;
            this.checkBox_saveTime.Checked = true;
            this.checkBox_saveTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_saveTime.Location = new System.Drawing.Point(182, 233);
            this.checkBox_saveTime.Name = "checkBox_saveTime";
            this.checkBox_saveTime.Size = new System.Drawing.Size(45, 17);
            this.checkBox_saveTime.TabIndex = 116;
            this.checkBox_saveTime.Text = "time";
            this.checkBox_saveTime.UseVisualStyleBackColor = true;
            // 
            // checkBox_saveOutput
            // 
            this.checkBox_saveOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_saveOutput.AutoSize = true;
            this.checkBox_saveOutput.Checked = true;
            this.checkBox_saveOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_saveOutput.Location = new System.Drawing.Point(288, 233);
            this.checkBox_saveOutput.Name = "checkBox_saveOutput";
            this.checkBox_saveOutput.Size = new System.Drawing.Size(56, 17);
            this.checkBox_saveOutput.TabIndex = 98;
            this.checkBox_saveOutput.Text = "output";
            this.checkBox_saveOutput.UseVisualStyleBackColor = true;
            this.checkBox_saveOutput.CheckedChanged += new System.EventHandler(this.checkBox_saveTo_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(144, 235);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 117;
            this.label2.Text = "Save";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 262);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox_saveTime);
            this.Controls.Add(this.checkBox_scanner);
            this.Controls.Add(this.checkBox_printer);
            this.Controls.Add(this.radioButton_stream);
            this.Controls.Add(this.radioButton_byByte);
            this.Controls.Add(this.radioButton_byString);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.textBox_strDelay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_delay);
            this.Controls.Add(this.textBox_sendNum);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.button_openFile);
            this.Controls.Add(this.textBox_fileName);
            this.Controls.Add(this.checkBox_hexFileOpen);
            this.Controls.Add(this.button_sendFile);
            this.Controls.Add(this.textBox_saveTo);
            this.Controls.Add(this.checkBox_saveOutput);
            this.Controls.Add(this.checkBox_saveInput);
            this.Controls.Add(this.button_Clear);
            this.Controls.Add(this.textBox_param);
            this.Controls.Add(this.checkBox_hexParam);
            this.Controls.Add(this.checkBox_hexTerminal);
            this.Controls.Add(this.checkBox_autoscroll);
            this.Controls.Add(this.checkBox_hexCommand);
            this.Controls.Add(this.textBox_terminal);
            this.Controls.Add(this.textBox_command);
            this.Controls.Add(this.button_closeport);
            this.Controls.Add(this.button_Send);
            this.Controls.Add(this.button_Refresh);
            this.Controls.Add(this.button_Open);
            this.Controls.Add(this.label_UsbPrnList);
            this.Controls.Add(this.comboBox_Printer);
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UsbPrnControl (c) Andrey Kalugin (jekyll@mail.ru), 2016";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox comboBox_Printer;
        private Label label_UsbPrnList;
        private Button button_Open;
        private Button button_Refresh;
        private Button button_closeport;
        private Button button_Send;
        private TextBox textBox_command;
        private TextBox textBox_terminal;
        private CheckBox checkBox_hexCommand;
        private CheckBox checkBox_autoscroll;
        private CheckBox checkBox_hexTerminal;
        private CheckBox checkBox_hexParam;
        private TextBox textBox_param;
        private Button button_Clear;
        private CheckBox checkBox_saveInput;
        private TextBox textBox_saveTo;
        private Button button_openFile;
        private TextBox textBox_fileName;
        private CheckBox checkBox_hexFileOpen;
        private Button button_sendFile;
        private OpenFileDialog openFileDialog1;
        private TextBox textBox_delay;
        private TextBox textBox_sendNum;
        private Label label24;
        private Label label23;
        private RadioButton radioButton_stream;
        private RadioButton radioButton_byByte;
        private RadioButton radioButton_byString;
        private ProgressBar progressBar1;
        private TextBox textBox_strDelay;
        private Label label1;
        private CheckBox checkBox_printer;
        private CheckBox checkBox_scanner;
        private CheckBox checkBox_saveTime;
        private CheckBox checkBox_saveOutput;
        private Label label2;
        ToolTip ToolTipTerminal = new ToolTip();
        private Timer timer1;
    }
}

