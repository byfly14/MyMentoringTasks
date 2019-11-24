namespace KeyGenerator
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
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>E
        protected override void Dispose(bool disposing)
        {
            int num1 = 2;
            while (true)
            {
                int num2 = 0;
                switch (num1)
                {
                    case 0:
                        num2 = 4;
                        num1 = num2;
                        continue;
                    case 1:
                        goto label_12;
                    case 2:
                        switch (0)
                        {
                            case 0:
                                goto label_3;
                            default:
                                continue;
                        }
                    case 3:
                    label_7:
                        this.eval_b.Dispose();
                        num2 = 1;
                        num1 = num2;
                        continue;
                    case 4:
                        num2 = 1;
                        if (num2 == 0)
                            ;
                        if (this.eval_b != null)
                        {
                            num2 = 3;
                            num1 = num2;
                            continue;
                        }
                        goto label_12;
                    default:
                    label_3:
                        if (disposing)
                        {
                            num2 = 13490;
                            int num3 = num2;
                            num2 = 13490;
                            int num4 = num2;
                            switch (num3 == num4 ? 1 : 0)
                            {
                                case 0:
                                case 2:
                                    goto label_7;
                                default:
                                    num2 = 0;
                                    if (num2 == 0)
                                        ;
                                    num2 = 0;
                                    num1 = num2;
                                    continue;
                            }
                        }
                        else
                            goto label_12;
                }
            }
        label_12:
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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";
        }

        #endregion
    }
}

