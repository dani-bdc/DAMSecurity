using DAMSecurityLib.Certificates;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DAMSecurityGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btSelectPdf_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                txtPdfFile.Text = fileDialog.FileName;
            }
        }

        private void btSelectPfx_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                txtPfxFile.Text = fileDialog.FileName;
            }
        }

        private void btSign_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtPdfFile.Text))
            {
                MessageBox.Show("Pdf file is empty");
                return;
            }
            if (String.IsNullOrEmpty(this.txtPfxFile.Text))
            {
                MessageBox.Show("Pfx file is empty");
                return;
            }
            if (String.IsNullOrEmpty(this.txtOutFile.Text))
            {
                MessageBox.Show("Out file is empty");
                return;
            }

            DAMSecurityLib.Crypto.Sign sign;

            try
            {
                sign = new DAMSecurityLib.Crypto.Sign();
                sign.InitCertificate(this.txtPfxFile.Text, this.txtPfxPassword.Text);
                sign.SignPdf(this.txtPdfFile.Text, this.txtOutFile.Text, this.chkShowSignature.IsChecked == true);
                MessageBox.Show("Signed pdf was generated");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't sign pdf file");
                MessageBox.Show(ex.ToString());
            }
        }

        private void btSelectOut_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                txtOutFile.Text = saveFileDialog.FileName;
            }
        }

        private void btSelectPfxCert_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                txtPfxFileCert.Text = saveFileDialog.FileName;
            }
        }

        private void btGenerateCert_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtCertName.Text))
            {
                MessageBox.Show("Certificate name is mandatory");
                return;
            }
            if (String.IsNullOrEmpty(txtCertOrganization.Text))
            {
                MessageBox.Show("Certificate organization is mandatory");
                return;
            }
            if (String.IsNullOrEmpty(txtCertLocality.Text))
            {
                MessageBox.Show("Certificate locality is mandatory");
                return;
            }
            CertificateInfo certificateInfo = new CertificateInfo();
      
            certificateInfo.CommonName = txtCertName.Text;
            certificateInfo.Organization=txtCertOrganization.Text;
            certificateInfo.Locality=txtCertLocality.Text;

            try
            {
                Autosigned.GeneratePfx(txtPfxFileCert.Text, txtPfxPasswordCert.Text, certificateInfo);
                MessageBox.Show("Certificated generated successefully");
            } catch (Exception ex)
            {
                MessageBox.Show("Couldn't generate certificate");
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
