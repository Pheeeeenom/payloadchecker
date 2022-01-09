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
using Force.Crc32;

namespace payloadchecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        uint payload_v1 = 0xd90f1907, payload_v2 = 0x3d36ffac, gateway_payload = 0x3b25Bab4, payload_v2_improved_sd_BOOT2OFW = 0xf67893ad, stock_oled = 0x8bdd4e3e, hwliteg3 = 0x13e8eea5;

        uint spacecraftv1 = 0x5104d6f4, spacecraftv2 = 0x5074125e;
        uint spacecraftv1_len = 0xF9C0, spacecraftv2_len = 0xFC60;
        private void button1_Click(object sender, EventArgs e)
        {
	
			OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                
                FileStream stream = File.OpenRead(ofd.FileName);
                #region spacecraft v1 check
                byte[] BOOT0 = new byte[0xF9C0];
                
                stream.Position = 0x3F0000;
                stream.Read(BOOT0, 0, 0xF9C0);

                uint crc32BOOT0 = Crc32Algorithm.Compute(BOOT0);

                if (crc32BOOT0 == spacecraftv1)
                {
                    MessageBox.Show("Do NOT use on an OLED", "");
                }
                #endregion
                #region spacecraft v2 check
                byte[] BOOT0_2nd = new byte[0xFC60];

                stream.Position = 0x3F0000;
                stream.Read(BOOT0_2nd, 0, 0xFC60);

                uint crc32BOOT0_ = Crc32Algorithm.Compute(BOOT0_2nd);

                if (crc32BOOT0_ == spacecraftv2)
                {
                    MessageBox.Show("Do use on an OLED", "");
                }
                #endregion

                if((crc32BOOT0 != spacecraftv1) &&(crc32BOOT0_ != spacecraftv2))
                {
                    MessageBox.Show("Unknown Payload/SX Payload?", "");
                }

                ofd.Dispose();
            }
         
        }

    }
}
