namespace MDTracer
{
    partial class Form_Registry
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
            groupBox1 = new GroupBox();
            dataGridView_cpu = new DataGridView();
            groupBox2 = new GroupBox();
            dataGridView_vdp = new DataGridView();
            groupBox5 = new GroupBox();
            dataGridView_call_stack = new DataGridView();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_cpu).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_vdp).BeginInit();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_call_stack).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dataGridView_cpu);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(213, 557);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "cpu";
            // 
            // dataGridView_cpu
            // 
            dataGridView_cpu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_cpu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView_cpu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_cpu.Location = new Point(6, 22);
            dataGridView_cpu.Name = "dataGridView_cpu";
            dataGridView_cpu.RowHeadersVisible = false;
            dataGridView_cpu.RowTemplate.Height = 25;
            dataGridView_cpu.Size = new Size(200, 529);
            dataGridView_cpu.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dataGridView_vdp);
            groupBox2.Location = new Point(231, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(292, 557);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "vdp";
            // 
            // dataGridView_vdp
            // 
            dataGridView_vdp.AllowUserToAddRows = false;
            dataGridView_vdp.AllowUserToDeleteRows = false;
            dataGridView_vdp.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_vdp.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView_vdp.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_vdp.Location = new Point(6, 22);
            dataGridView_vdp.Name = "dataGridView_vdp";
            dataGridView_vdp.ReadOnly = true;
            dataGridView_vdp.RowHeadersVisible = false;
            dataGridView_vdp.RowTemplate.Height = 25;
            dataGridView_vdp.Size = new Size(280, 529);
            dataGridView_vdp.TabIndex = 1;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(dataGridView_call_stack);
            groupBox5.Location = new Point(529, 12);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(236, 256);
            groupBox5.TabIndex = 5;
            groupBox5.TabStop = false;
            groupBox5.Text = "call stack";
            // 
            // dataGridView_call_stack
            // 
            dataGridView_call_stack.AllowUserToAddRows = false;
            dataGridView_call_stack.AllowUserToDeleteRows = false;
            dataGridView_call_stack.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_call_stack.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView_call_stack.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_call_stack.Location = new Point(5, 22);
            dataGridView_call_stack.MultiSelect = false;
            dataGridView_call_stack.Name = "dataGridView_call_stack";
            dataGridView_call_stack.ReadOnly = true;
            dataGridView_call_stack.RowHeadersVisible = false;
            dataGridView_call_stack.RowTemplate.Height = 25;
            dataGridView_call_stack.Size = new Size(225, 228);
            dataGridView_call_stack.TabIndex = 33;
            // 
            // Form_Registry
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(890, 581);
            Controls.Add(groupBox5);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Form_Registry";
            Text = "Register View";
            FormClosing += Form_Registry_FormClosing;
            Shown += Form_Registry_Shown;
            ResizeEnd += Form_Registry_ResizeEnd;
            Paint += Form_Registry_Paint;
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_cpu).EndInit();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_vdp).EndInit();
            groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_call_stack).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private GroupBox groupBox1;
        private DataGridView dataGridView_cpu;
        private GroupBox groupBox2;
        private DataGridView dataGridView_vdp;
        private GroupBox groupBox5;
        private DataGridView dataGridView_call_stack;
    }
}