﻿namespace GNFS_Winforms
{
	partial class MainForm
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
			this.tbOutput = new System.Windows.Forms.TextBox();
			this.tbN = new System.Windows.Forms.TextBox();
			this.tbBase = new System.Windows.Forms.TextBox();
			this.tbBound = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnCreateGnfs = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.tbDegree = new System.Windows.Forms.TextBox();
			this.btnFindRelations = new System.Windows.Forms.Button();
			this.btnFindSquares = new System.Windows.Forms.Button();
			this.btnConstructPoly = new System.Windows.Forms.Button();
			this.btnMatrix = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbOutput
			// 
			this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbOutput.Location = new System.Drawing.Point(4, 124);
			this.tbOutput.Multiline = true;
			this.tbOutput.Name = "tbOutput";
			this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbOutput.Size = new System.Drawing.Size(703, 356);
			this.tbOutput.TabIndex = 0;
			this.tbOutput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbOutput_KeyUp);
			// 
			// tbN
			// 
			this.tbN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbN.Location = new System.Drawing.Point(20, 4);
			this.tbN.Name = "tbN";
			this.tbN.Size = new System.Drawing.Size(687, 20);
			this.tbN.TabIndex = 1;
			// 
			// tbBase
			// 
			this.tbBase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbBase.Location = new System.Drawing.Point(100, 24);
			this.tbBase.Name = "tbBase";
			this.tbBase.Size = new System.Drawing.Size(608, 20);
			this.tbBase.TabIndex = 2;
			// 
			// tbBound
			// 
			this.tbBound.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbBound.Enabled = false;
			this.tbBound.Location = new System.Drawing.Point(100, 80);
			this.tbBound.Name = "tbBound";
			this.tbBound.Size = new System.Drawing.Size(180, 20);
			this.tbBound.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(15, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "N";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 28);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(83, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Polynomial base";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(0, 84);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(99, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Smoothness Bound";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// btnCreateGnfs
			// 
			this.btnCreateGnfs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCreateGnfs.Location = new System.Drawing.Point(348, 48);
			this.btnCreateGnfs.Name = "btnCreateGnfs";
			this.btnCreateGnfs.Size = new System.Drawing.Size(288, 20);
			this.btnCreateGnfs.TabIndex = 7;
			this.btnCreateGnfs.Text = "Create/Load";
			this.btnCreateGnfs.UseVisualStyleBackColor = true;
			this.btnCreateGnfs.Click += new System.EventHandler(this.btnCreateGnfs_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(4, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(93, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Polynomial degree";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tbDegree
			// 
			this.tbDegree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbDegree.Location = new System.Drawing.Point(100, 52);
			this.tbDegree.Name = "tbDegree";
			this.tbDegree.Size = new System.Drawing.Size(180, 20);
			this.tbDegree.TabIndex = 8;
			// 
			// btnFindRelations
			// 
			this.btnFindRelations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFindRelations.Enabled = false;
			this.btnFindRelations.Location = new System.Drawing.Point(348, 92);
			this.btnFindRelations.Name = "btnFindRelations";
			this.btnFindRelations.Size = new System.Drawing.Size(96, 23);
			this.btnFindRelations.TabIndex = 10;
			this.btnFindRelations.Text = "Find Relations";
			this.btnFindRelations.UseVisualStyleBackColor = true;
			this.btnFindRelations.Click += new System.EventHandler(this.btnFindRelations_Click);
			// 
			// btnFindSquares
			// 
			this.btnFindSquares.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFindSquares.Enabled = false;
			this.btnFindSquares.Location = new System.Drawing.Point(540, 92);
			this.btnFindSquares.Name = "btnFindSquares";
			this.btnFindSquares.Size = new System.Drawing.Size(96, 23);
			this.btnFindSquares.TabIndex = 11;
			this.btnFindSquares.Text = "Find Squares";
			this.btnFindSquares.UseVisualStyleBackColor = true;
			this.btnFindSquares.Click += new System.EventHandler(this.btnFindSquares_Click);
			// 
			// btnConstructPoly
			// 
			this.btnConstructPoly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnConstructPoly.Enabled = false;
			this.btnConstructPoly.Location = new System.Drawing.Point(368, 68);
			this.btnConstructPoly.Name = "btnConstructPoly";
			this.btnConstructPoly.Size = new System.Drawing.Size(224, 23);
			this.btnConstructPoly.TabIndex = 12;
			this.btnConstructPoly.Text = "Construct Polynomials";
			this.btnConstructPoly.UseVisualStyleBackColor = true;
			this.btnConstructPoly.Click += new System.EventHandler(this.btnConstructPoly_Click);
			// 
			// btnMatrix
			// 
			this.btnMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMatrix.Enabled = false;
			this.btnMatrix.Location = new System.Drawing.Point(444, 92);
			this.btnMatrix.Name = "btnMatrix";
			this.btnMatrix.Size = new System.Drawing.Size(96, 23);
			this.btnMatrix.TabIndex = 13;
			this.btnMatrix.Text = "Matrix Solve";
			this.btnMatrix.UseVisualStyleBackColor = true;
			this.btnMatrix.Click += new System.EventHandler(this.btnMatrix_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(710, 482);
			this.Controls.Add(this.btnMatrix);
			this.Controls.Add(this.btnConstructPoly);
			this.Controls.Add(this.btnFindSquares);
			this.Controls.Add(this.btnFindRelations);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.tbDegree);
			this.Controls.Add(this.btnCreateGnfs);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbBound);
			this.Controls.Add(this.tbBase);
			this.Controls.Add(this.tbN);
			this.Controls.Add(this.tbOutput);
			this.MinimumSize = new System.Drawing.Size(500, 300);
			this.Name = "MainForm";
			this.Text = "GNFS";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbOutput;
		private System.Windows.Forms.TextBox tbN;
		private System.Windows.Forms.TextBox tbBase;
		private System.Windows.Forms.TextBox tbBound;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnCreateGnfs;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbDegree;
		private System.Windows.Forms.Button btnFindRelations;
		private System.Windows.Forms.Button btnFindSquares;
		private System.Windows.Forms.Button btnConstructPoly;
		private System.Windows.Forms.Button btnMatrix;
	}
}

