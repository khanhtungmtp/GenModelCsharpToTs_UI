using ScintillaNET;

namespace CtoTS
{
    partial class AppCToTS
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppCToTS));
            btnGen = new Button();
            lbcsharp = new Label();
            lbTypescript = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            inputTextBoxCsharp = new Scintilla();
            inputTextBoxTypescript = new Scintilla();
            btnCopy = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnGen
            // 
            btnGen.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnGen.Location = new Point(342, 616);
            btnGen.Name = "btnGen";
            btnGen.Size = new Size(116, 47);
            btnGen.TabIndex = 1;
            btnGen.Text = "Convert TS";
            btnGen.UseVisualStyleBackColor = true;
            btnGen.Click += btnGen_Click;
            // 
            // lbcsharp
            // 
            lbcsharp.AutoSize = true;
            lbcsharp.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            lbcsharp.Location = new Point(174, 102);
            lbcsharp.Name = "lbcsharp";
            lbcsharp.Size = new Size(38, 30);
            lbcsharp.TabIndex = 5;
            lbcsharp.Text = "C#";
            // 
            // lbTypescript
            // 
            lbTypescript.AutoSize = true;
            lbTypescript.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            lbTypescript.Location = new Point(567, 102);
            lbTypescript.Name = "lbTypescript";
            lbTypescript.Size = new Size(106, 30);
            lbTypescript.TabIndex = 6;
            lbTypescript.Text = "Typescript";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(284, 9);
            label1.Name = "label1";
            label1.Size = new Size(240, 30);
            label1.TabIndex = 7;
            label1.Text = "Convert C# to Typescript";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(359, 42);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(81, 66);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // inputTextBoxCsharp
            // 
            inputTextBoxCsharp.AutoCMaxHeight = 9;
            inputTextBoxCsharp.Margins[0].Width = 20;
            inputTextBoxCsharp.BiDirectionality = BiDirectionalDisplayType.Disabled;
            inputTextBoxCsharp.LexerName = "cpp";
            inputTextBoxCsharp.Location = new Point(3, 135);
            inputTextBoxCsharp.Name = "inputTextBoxCsharp";
            inputTextBoxCsharp.ScrollWidth = 1;
            inputTextBoxCsharp.Size = new Size(392, 475);
            inputTextBoxCsharp.TabIndents = true;
            inputTextBoxCsharp.TabIndex = 0;
            inputTextBoxCsharp.UseRightToLeftReadingLayout = false;
            inputTextBoxCsharp.WrapMode = WrapMode.None;
            // Assuming 'inputTextBox' is an instance of ScintillaNET.Scintilla
            inputTextBoxCsharp.SetKeywords(0, "public class DateTime List DateOnly decimal int float double string void bool"); // Set your custom keywords

            // Optionally, you can enable syntax highlighting
            inputTextBoxCsharp.Styles[ScintillaNET.Style.Default].ForeColor = System.Drawing.Color.Silver;
            inputTextBoxCsharp.Styles[ScintillaNET.Style.Cpp.Word].ForeColor = System.Drawing.Color.Blue;
            inputTextBoxCsharp.Styles[ScintillaNET.Style.Cpp.Word2].ForeColor = System.Drawing.Color.Red;
            // 
            // inputTextBoxTypescript
            // 
            inputTextBoxTypescript.AutoCMaxHeight = 9;
            inputTextBoxTypescript.Margins[0].Width = 20;
            inputTextBoxTypescript.BiDirectionality = BiDirectionalDisplayType.Disabled;
            inputTextBoxTypescript.LexerName = "cpp";
            inputTextBoxTypescript.Location = new Point(401, 135);
            inputTextBoxTypescript.Name = "inputTextBoxTypescript";
            inputTextBoxTypescript.ScrollWidth = 1;
            inputTextBoxTypescript.Size = new Size(402, 475);
            inputTextBoxTypescript.TabIndents = true;
            inputTextBoxTypescript.TabIndex = 0;
            inputTextBoxTypescript.UseRightToLeftReadingLayout = false;
            inputTextBoxTypescript.WrapMode = WrapMode.None;
            // Assuming 'inputTextBox' is an instance of ScintillaNET.Scintilla
            inputTextBoxTypescript.SetKeywords(0, "export interface any number int string void bool null Date boolean"); // Set your custom keywords

            // Optionally, you can enable syntax highlighting
            inputTextBoxTypescript.Styles[ScintillaNET.Style.Default].ForeColor = System.Drawing.Color.Silver;
            inputTextBoxTypescript.Styles[ScintillaNET.Style.Cpp.Word].ForeColor = System.Drawing.Color.Blue;
            inputTextBoxTypescript.Styles[ScintillaNET.Style.Cpp.Word2].ForeColor = System.Drawing.Color.Red;
            // To hide the caret
            inputTextBoxTypescript.CaretWidth = 0;
            inputTextBoxTypescript.Click += inputTextBoxTypescript_Click;
            // 
            // btnCopy
            // 
            btnCopy.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnCopy.Location = new Point(731, 96);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(71, 36);
            btnCopy.TabIndex = 9;
            btnCopy.Text = "Copy";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;
            // 
            // AppCToTS
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(804, 673);
            Controls.Add(btnCopy);
            Controls.Add(inputTextBoxCsharp);
            Controls.Add(inputTextBoxTypescript);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Controls.Add(lbTypescript);
            Controls.Add(lbcsharp);
            Controls.Add(btnGen);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AppCToTS";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Csharp to Typescript";
            Load += AppCToTS_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Scintilla inputTextBoxCsharp;
        private Scintilla inputTextBoxTypescript;
        private Button btnGen;
        private Label lbcsharp;
        private Label lbTypescript;
        private Label label1;
        private PictureBox pictureBox1;
        private Button btnCopy;
    }
}