using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace payloadchecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
         byte[] payload_v2 = new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                        0x59, 0xdd, 0xb4, 0x1f, 0x41, 0x28, 0xff, 0x4e, 0x70, 0xd2, 0x48, 0xcc, 0x1f, 0x7a, 0xd6, 0xab };
		 byte[] payload_v1 = new byte[]{0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                                        0x12, 0x8f, 0xfe, 0x1d, 0x86, 0xdc, 0x44, 0x2a, 0xad, 0x49, 0xe7, 0x24, 0x1a, 0x76, 0xc6, 0x6a};

		private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

          
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ofd.Dispose();
                FileStream stream = File.OpenRead(ofd.FileName);

                byte[] BOOT0 = new byte[32];

                stream.Position = 0x3F0000;
                stream.Read(BOOT0, 0, 0x20);

                if (BOOT0.SequenceEqual(payload_v1))
                {
                    MessageBox.Show("Spacecraft 0.1.0 Release\nDo not use this chip on an OLED");
                }
                else if (BOOT0.SequenceEqual(payload_v2))
                {
                    MessageBox.Show("Spacecraft 0.2.0 Release\nYou can use this chip on an OLED");
                }
                else
                {
                    MessageBox.Show("Unknown Payload. Please contact the developer");

                }
            }
         
            

         
        }

    }
}