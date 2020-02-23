namespace UiTets
{
    partial class ViewTetsForm
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
            this.page_box = new System.Windows.Forms.TextBox();
            this.page_lable = new System.Windows.Forms.Label();
            this.description_box = new System.Windows.Forms.TextBox();
            this.description_lable = new System.Windows.Forms.Label();
            this.btn_ok = new System.Windows.Forms.Button();
            this.test_name_box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // page_box
            // 
            this.page_box.Location = new System.Drawing.Point(83, 62);
            this.page_box.Name = "page_box";
            this.page_box.ReadOnly = true;
            this.page_box.Size = new System.Drawing.Size(199, 20);
            this.page_box.TabIndex = 13;
            // 
            // page_lable
            // 
            this.page_lable.AutoSize = true;
            this.page_lable.Location = new System.Drawing.Point(8, 66);
            this.page_lable.Name = "page_lable";
            this.page_lable.Size = new System.Drawing.Size(54, 13);
            this.page_lable.TabIndex = 12;
            this.page_lable.Text = "StartPage";
            // 
            // description_box
            // 
            this.description_box.Location = new System.Drawing.Point(83, 98);
            this.description_box.Multiline = true;
            this.description_box.Name = "description_box";
            this.description_box.ReadOnly = true;
            this.description_box.Size = new System.Drawing.Size(199, 76);
            this.description_box.TabIndex = 11;
            // 
            // description_lable
            // 
            this.description_lable.AutoSize = true;
            this.description_lable.Location = new System.Drawing.Point(8, 98);
            this.description_lable.Name = "description_lable";
            this.description_lable.Size = new System.Drawing.Size(60, 13);
            this.description_lable.TabIndex = 10;
            this.description_lable.Text = "Description";
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(288, 192);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 9;
            this.btn_ok.Text = "Create";
            this.btn_ok.UseVisualStyleBackColor = true;
            // 
            // test_name_box
            // 
            this.test_name_box.Location = new System.Drawing.Point(83, 26);
            this.test_name_box.Name = "test_name_box";
            this.test_name_box.ReadOnly = true;
            this.test_name_box.Size = new System.Drawing.Size(199, 20);
            this.test_name_box.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "TestName";
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 230);
            this.Controls.Add(this.page_box);
            this.Controls.Add(this.page_lable);
            this.Controls.Add(this.description_box);
            this.Controls.Add(this.description_lable);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.test_name_box);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ViewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ViewForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox page_box;
        private System.Windows.Forms.Label page_lable;
        private System.Windows.Forms.TextBox description_box;
        private System.Windows.Forms.Label description_lable;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.TextBox test_name_box;
        private System.Windows.Forms.Label label1;
    }
}