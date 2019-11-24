using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyGenerator
{
    public partial class Form1 : Form
    {
        [CompilerGenerated]
        private sealed class eval_A
        {
            public byte[] a;

            public int[] b;

            [NonSerialized]
            private const string eval_c = "32";

            internal int eval_a(byte A_0, int A_1)
            {
                int num = 7823;
                int num2 = num;
                num = 7823;
                int num3;
                int num4;
                switch (num2 == num)
                {
                    default:
                    {
                        num = 1;
                        num3 = num;
                        goto IL_004c;
                    }
                        IL_004c:
                        if (num3 == 0)
                        {
                            goto IL_004e;
                        }
                        goto IL_004e;
                        IL_004e:
                        if (0 == 0)
                        {
                            goto IL_006c;
                        }
                        goto IL_006c;
                        IL_006c:
                        num = 0;
                        num4 = num;
                        goto IL_0078;
                        IL_0078:
                        return A_0 ^ this.a[A_1];
                }
                //IL_005f:
                //num = 0;
                //num4 = num;
                //goto IL_0078;
                //IL_0040:
                //num = 0;
                //num3 = num;
                //goto IL_004c;
            }

            internal int eval_a(int A_0, int A_1)
            {
                int num = -18476;
                int num2 = num;
                num = -18476;
                int num3;
                int num4;
                switch (num2 == num)
                {
                    default:
                    {
                        num = 1;
                        num3 = num;
                        goto IL_004c;
                    }
                        IL_004c:
                        if (num3 == 0)
                        {
                            goto IL_004e;
                        }
                        goto IL_004e;
                        IL_004e:
                        if (0 == 0)
                        {
                            goto IL_006c;
                        }
                        goto IL_006c;
                        IL_006c:
                        num = 0;
                        num4 = num;
                        goto IL_0078;
                        IL_0078:
                        return A_0 - this.b[A_1];
                }
                //IL_005f:
                //num = 0;
                //num4 = num;
                //goto IL_0078;
                //IL_0040:
                //num = 0;
                //num3 = num;
                //goto IL_004c;
            }
        }

        private string[] a;
        private IContainer eval_b;
        private Button eval_c;
        private TextBox eval_d;
        private Label eval_e;
        private Label eval_f;
        [NonSerialized]
        string eval_g;

        public Form1()
        {
            this.eval_a();
        }

        private void eval_a(object A_0, EventArgs A_1)
        {
            int num1 = 0;
            int num2;
            while (true)
            {
                switch (num1)
                {
                    case 0:
                    label_1:
                        switch (0)
                        {
                            case 0:
                                break;
                            default:
                                continue;
                        }
                        break;
                    case 1:
                        goto label_10;
                    case 2:
                        num2 = 1;
                        if (num2 == 0)
                            ;
                        num2 = 0;
                        num2 = -26500;
                        int num3 = num2;
                        num2 = -26500;
                        int num4 = num2;
                        switch (num3 == num4 ? 1 : 0)
                        {
                            case 0:
                            case 2:
                                goto label_1;
                            default:
                                goto label_7;
                        }
                }
                if (string.IsNullOrEmpty(this.eval_d.Text))
                {
                    num2 = 2;
                    num1 = num2;
                }
                else
                {
                    this.a = this.eval_d.Text.Split('-');
                    num2 = 1;
                    num1 = num2;
                }
            }
        label_7:
            num2 = 0;
            if (num2 == 0)
                ;
            this.eval_f.Text = "Key cannot be empty";
            return;
        label_10:
            this.eval_f.Text = this.eval_a(this.a) ? "Correct key" : "Wrong key";
            this.eval_f.Visible = true;
        }

        private bool eval_a(string[] A_0_1)
        {
            int num1;
            // ISSUE: variable of a compiler-generated type
            Form1.eval_A evalA;
            NetworkInterface networkInterface;
            switch (0)
            {
                case 0:
                label_2:
                    // ISSUE: object of a compiler-generated type is created
                    evalA = new Form1.eval_A();
                    networkInterface = ((IEnumerable<NetworkInterface>)NetworkInterface.GetAllNetworkInterfaces()).FirstOrDefault<NetworkInterface>();
                    num1 = 0;
                    goto default;
                default:
                    while (true)
                    {
                        switch (num1)
                        {
                            case 0:
                                switch (true ? 1 : 0)
                                {
                                    case 0:
                                    case 2:
                                        goto label_8;
                                    default:
                                        if (false)
                                            ;
                                        if (true)
                                            ;
                                        num1 = networkInterface != null ? 1 : 2;
                                        continue;
                                }
                            case 1:
                                goto label_7;
                            case 2:
                            label_8:
                                num1 = 3;
                                continue;
                            case 3:
                                goto label_9;
                            default:
                                goto label_2;
                        }
                    }
                label_7:
                    byte[] addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
                    // ISSUE: reference to a compiler-generated field
                    evalA.a = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());
                    // ISSUE: reference to a compiler-generated method
                    Func<byte, int, int> selector1 = new Func<byte, int, int>(evalA.eval_a);
                    int[] array = ((IEnumerable<byte>)addressBytes).Select<byte, int>(selector1).Select<int, int>((Func<int, int>)(A_0_2 =>
                    {
                        if (A_0_2 <= 999)
                        {
                            int num2 = 17361;
                            int num3 = num2;
                            num2 = 17361;
                            int num4 = num2;
                            switch (num3 == num4 ? 1 : 0)
                            {
                                case 0:
                                case 2:
                                    break;
                                default:
                                    num2 = 0;
                                    if (num2 == 0)
                                        ;
                                    num2 = 0;
                                    num2 = 1;
                                    if (num2 == 0)
                                        ;
                                    return A_0_2 * 10;
                            }
                        }
                        return A_0_2;
                    })).ToArray<int>();
                    // ISSUE: reference to a compiler-generated field
                    evalA.b = ((IEnumerable<string>)A_0_1).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>();
                    // ISSUE: reference to a compiler-generated method
                    Func<int, int, int> selector2 = new Func<int, int, int>(evalA.eval_a);
                    return ((IEnumerable<int>)array).Select<int, int>(selector2).All<int>((Func<int, bool>)(A_0_2 =>
                    {
                        int num2 = 862;
                        int num3 = num2;
                        num2 = 862;
                        int num4 = num2;
                        switch (num3 == num4)
                        {
                            case true:
                                int num5 = 1;
                                if (num5 == 0)
                                    ;
                                num5 = 0;
                                if (num5 == 0)
                                    ;
                                num5 = 0;
                                return A_0_2 == 0;
                            default:
                                goto case true;
                        }
                    }));
                label_9:
                    return false;
            }
        }

        private void eval_a()
        {
            int num1 = 15225;
            int num2 = num1;
            num1 = 15225;
            int num3 = num1;
            switch (num2 == num3)
            {
                case true:
                    int num4 = 0;
                    if (num4 == 0)
                        ;
                    num4 = 1;
                    if (num4 == 0)
                        ;
                    num4 = 0;
                    this.eval_c = new Button();
                    this.eval_d = new TextBox();
                    this.eval_e = new Label();
                    this.eval_f = new Label();
                    this.SuspendLayout();
                    this.eval_c.Location = new Point(268, 51);
                    this.eval_c.Name = "bt_check";
                    this.eval_c.Size = new Size(75, 23);
                    this.eval_c.TabIndex = 0;
                    this.eval_c.Text = "Check";
                    this.eval_c.UseVisualStyleBackColor = true;
                    this.eval_c.Click += new EventHandler(this.eval_a);
                    this.eval_d.Location = new Point(35, 25);
                    this.eval_d.Name = "tb_key";
                    this.eval_d.Size = new Size(308, 20);
                    this.eval_d.TabIndex = 1;
                    this.eval_e.AutoSize = true;
                    this.eval_e.Location = new Point(32, 9);
                    this.eval_e.Name = "label1";
                    this.eval_e.Size = new Size(107, 13);
                    this.eval_e.TabIndex = 2;
                    this.eval_e.Text = "Please, enter the key";
                    this.eval_f.AutoSize = true;
                    this.eval_f.Location = new Point(35, 52);
                    this.eval_f.Name = "lb_status";
                    this.eval_f.Size = new Size(35, 13);
                    this.eval_f.TabIndex = 3;
                    this.eval_f.Text = "label2";
                    this.eval_f.Visible = false;
                    this.AutoScaleDimensions = new SizeF(6f, 13f);
                    this.AutoScaleMode = AutoScaleMode.Font;
                    this.ClientSize = new Size(369, 86);
                    this.Controls.Add((Control)this.eval_f);
                    this.Controls.Add((Control)this.eval_e);
                    this.Controls.Add((Control)this.eval_d);
                    this.Controls.Add((Control)this.eval_c);
                    this.Name = nameof(Form1);
                    this.Text = "Crack me";
                    this.ResumeLayout(false);
                    this.PerformLayout();
                    break;
                default:
                    goto case true;
            }
        }
    }
}
