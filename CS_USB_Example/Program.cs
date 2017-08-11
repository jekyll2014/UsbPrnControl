//This document contains programming examples.
//Custom S.P.A. grants you a nonexclusive copyright license to use all programming code examples from which you can generate similar function tailored to your own specific needs.
//All sample code is provided by Custom S.P.A. for illustrative purposes only. These examples have not been thoroughly tested under all conditions. 
//Custom S.P.A., therefore, cannot guarantee or imply reliability, serviceability, or function of these programs.
//In no event shall Custom S.P.A. be liable for any direct, indirect, incidental, special, exemplary, or consequential damages (including, but not limited to, procurement of substitute goods or services; loss of use, data, or profits; or business interruption) however caused and on any theory of liability, whether in contract, strict liability, or tort 
//(including negligence or otherwise) arising in any way out of the use of this software, even if advised of the possibility of such damage.
//All programs contained herein are provided to you "as is" without any warranties of any kind. 
//The implied warranties of non-infringement, merchantability and fitness for a particular purpose are expressly disclaimed.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UsbPrnControl
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
