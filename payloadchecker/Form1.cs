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
        uint payload_v1 = 0xd90f1907, payload_v2 = 0x3d36ffac, gateway_payload = 0x3b25Bab4, payload_v2_improved_sd_BOOT2OFW = 0xf67893ad, stock_oled = 0x8bdd4e3e;
        private void button1_Click(object sender, EventArgs e)
        {
	
			OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                
                FileStream stream = File.OpenRead(ofd.FileName);

                byte[] BOOT0 = new byte[0xFDC0];

                stream.Position = 0x3F0000;
                stream.Read(BOOT0, 0, 0xFDC0);

                uint crc32BOOT0 = Crc32Algorithm.Compute(BOOT0);

                if (crc32BOOT0 == payload_v1)
                {
                    MessageBox.Show("Spacecraft 0.1.0 Release\nDo not use this chip on an OLED");
                }
                else if (crc32BOOT0 == payload_v2)
                {
                    MessageBox.Show("Spacecraft 0.2.0 Release\nYou can use this chip on an OLED");
                }
                else if (crc32BOOT0 == gateway_payload)
                {
                    MessageBox.Show("Original TX Payload. Update your chip to Spacecraft v2 before you use it on an OLED");
                }
                else if (crc32BOOT0 == payload_v2_improved_sd_BOOT2OFW)
                {
                    MessageBox.Show("Improved Spacecraft v2 with better SD card compatibility/BOOT2OFW functionality\nYou can use this chip on an OLED");
                }
                else if (crc32BOOT0 == stock_oled)
                {
                    MessageBox.Show("Spacecraft v2 on stock OLED chip.\nYou can use this chip on an OLED");
                }
                else
                {
                    MessageBox.Show("Unknown Payload. Please contact the developer");

                }
		ofd.Dispose();
            }
         
            

         
        }

    }
}
