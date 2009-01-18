using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using Routrek.PKI;

namespace SSHClient
{

    public partial class KeyGenForm : Form
    {
        private MouseEventHandler _mmhandler;
        private bool _gotKey;
        private SSH2UserAuthKey _key = null;
        private String _OpenSSHstring;

        public KeyGenForm()
        {
            InitializeComponent();
            buttonSave.Enabled = false;
            algorithmBox.SelectedIndex = 0;
            bitCountBox.SelectedIndex = 0;
            labelfingerprint.Hide();
            labelpublicKey.Hide();
            labelRandomness.Hide();
            labelTag.Hide();
            textBoxTag.Hide();
            progressBarGenerate.Hide();
            publicKeyBox.Hide();
            fingerprintBox.Hide();
            _gotKey = false;
        }

        public string KeyTag
        {
        	get
        	{
        		return textBoxTag.Text;
        	}
        	set
        	{
        		textBoxTag.Text = value;
        	}
        }
        public SSH2UserAuthKey Key
        {
        	get
        	{
        		return _key;
        	}
        }
        public bool needMoreEntropy
        {
            get
            {
                return _gotKey==false;
            }
        }      

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            PublicKeyAlgorithm algorithm;
            if (algorithmBox.Text == "RSA")
                algorithm= PublicKeyAlgorithm.RSA;
            else
                algorithm= PublicKeyAlgorithm.DSA;
            labelfingerprint.Hide();
            labelpublicKey.Hide();
            labelTag.Hide();
            textBoxTag.Hide();
            publicKeyBox.Hide();
            fingerprintBox.Hide();
            labelRandomness.Show();
            progressBarGenerate.Show();
            _gotKey = false;
            _key = null;
            _OpenSSHstring = "";
            KeyGenThread t = new KeyGenThread(this, algorithm, Int32.Parse(bitCountBox.Text));
            _mmhandler = t.OnMouseMove;
            progressBarGenerate.MouseMove += _mmhandler;
            t.Start();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
        	_key = null;
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
        	//Settings.AddSSHKey(textBoxTag.Text,_key);
            Close();
        }

        public void hideGenKey()
        {
            progressBarGenerate.Value = progressBarGenerate.Maximum;
            progressBarGenerate.Value = progressBarGenerate.Maximum;
            progressBarGenerate.Hide();
            labelRandomness.Hide();
        }

        public void showKey()
        {
            _OpenSSHstring = _key.PublicPartInOpenSSHStyle();
            String alg;
            if (_key.Algorithm == PublicKeyAlgorithm.DSA)
                alg = "dsa";
            else
                alg = "rsa";
            DateTime n = DateTime.Now;
            String comment = alg + "-key-" + n.ToString("yyyyMMdd");
            labelpublicKey.Show();
            publicKeyBox.Text = _OpenSSHstring + " " + textBoxTag.Text;
            publicKeyBox.Show();
            //labelfingerprint.Show();
            //fingerprintBox.Text = _key.
            //fingerprintBox.Show();
            labelTag.Show();
            textBoxTag.Show();
            textBoxTag.Text = comment;
        }

        public void SetProgressValue(int v)
        {
            if (v < progressBarGenerate.Maximum)
                progressBarGenerate.Value = v;
            if (_key != null)
            {
                hideGenKey();
                showKey();
                progressBarGenerate.MouseMove -= _mmhandler;
                buttonSave.Enabled = true;
                _gotKey = true;
            }
        }

        public void SetResultKey(SSH2UserAuthKey k)
        {
            _key = k;
        }

        private void textBoxTag_TextChanged(object sender, EventArgs e)
        {
            if (textBoxTag.Text == "")
            {
                buttonSave.Enabled = false;
            }
            else
            {
                publicKeyBox.Text = _OpenSSHstring + " " + textBoxTag.Text;
                if(buttonSave.Enabled)
                    buttonSave.Enabled = true;
            }
        }

    }

    internal class KeyGenThread
    {
        private KeyGenForm _parent;
        PublicKeyAlgorithm _algorithm;
        Int32 _bits;
        private KeyGenRandomGenerator _rnd;
        private int _mouseMoveCount;

        public KeyGenThread(KeyGenForm p, PublicKeyAlgorithm a, Int32 b)
        {
            _parent = p;
            _algorithm = a;
            _bits = b;
            _rnd = new KeyGenRandomGenerator();
        }

        public void Start()
        {
            Thread t = new Thread(new ThreadStart(EntryPoint));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        public void SetAbortFlag()
        {
            _rnd.SetAbortFlag();
        }

        private void EntryPoint()
        {
            try
            {
                _mouseMoveCount = 0;
                KeyPair kp;
                if (_algorithm == PublicKeyAlgorithm.DSA)
                    kp = DSAKeyPair.GenerateNew(_bits, _rnd);
                else
                    kp = RSAKeyPair.GenerateNew(_bits, _rnd);
                _parent.SetResultKey(new SSH2UserAuthKey(kp));
            }
            catch (Exception ex)
            {
            	// TODO - make a delegate for linking logging
                //Terminals.Logging.Log.Debug(ex.StackTrace);
            }
        }

        public void OnMouseMove(object sender, MouseEventArgs args)
        {
            if (_parent.needMoreEntropy)
            {
                int n = (int)System.DateTime.Now.Ticks;
                n ^= (args.X << 16);
                n ^= args.Y;
                n ^= (int)0x31031293;

                _rnd.Refresh(n);
                _parent.SetProgressValue(++_mouseMoveCount);
            }
        }

        private class KeyGenRandomGenerator : Random
        {
            private Random _internal;
            public int _doubleCount;
            private int _internalAvailableCount;
            private bool _abortFlag;

            public KeyGenRandomGenerator()
            {
                _internalAvailableCount = 0;
                _abortFlag = false;
            }

            public override double NextDouble()
            {

                while (_internalAvailableCount == 0)
                {
                    Thread.Sleep(100);
                    if (_abortFlag) throw new Exception("key generation aborted");
                }
                _internalAvailableCount--;
                _doubleCount++;
                return _internal.NextDouble();
            }


            public void Refresh(int seed)
            {
                _internal = new Random(seed);
                _internalAvailableCount = 50;
            }
            public void RefreshFinal(int seed)
            {
                _internal = new Random(seed);
                _internalAvailableCount = Int32.MaxValue;
            }

            public void SetAbortFlag()
            {
                _abortFlag = true;
            }
        }
    }
}
