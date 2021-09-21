using Cryptware.NCryptoki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.mkysoft.nChiper
{
    public class Encryption
    {
        private Cryptoki _Cryptoki = null;
        private Token _Token = null;
        private Session _Session = null;
        private RSAPublicKey _RSAPublicKey = null;

        public Encryption(string OCSPassword, string _KeyName)
        {
            _Cryptoki = new Cryptoki("cknfast-64.dll");
            int err = _Cryptoki.Initialize(true);
            if (err != 0)
                throw new Exception("Cryptoki Initialize error: " + err);

            _Token = _Cryptoki.Slots[1].Token;

            _Session = _Token.OpenSession(Session.CKF_SERIAL_SESSION, null, null);

            err = _Session.Login(Session.CKU_USER, OCSPassword);
            if (err != 0)
                throw new Exception("Cryptoki Session Login error: " + err);

            var template = new CryptokiCollection();
            template.Add(new ObjectAttribute(ObjectAttribute.CKA_CLASS, CryptokiObject.CKO_PUBLIC_KEY));
            template.Add(new ObjectAttribute(ObjectAttribute.CKA_KEY_TYPE, Key.CKK_RSA));
            template.Add(new ObjectAttribute(ObjectAttribute.CKA_ENCRYPT, true));
            template.Add(new ObjectAttribute(ObjectAttribute.CKA_LABEL, _KeyName));
            CryptokiCollection objects = _Session.Objects.Find(template, 1);
            if (objects.Count == 0)
                throw new Exception(string.Format("Public Key '{0}' was not found!", _KeyName));
            _RSAPublicKey = (RSAPublicKey)objects[0];
        }

        public byte[] Encrypt(byte[] text)
        {
            int err = _Session.EncryptInit(Mechanism.RSA_PKCS, _RSAPublicKey);
            if (err != 0)
                throw new Exception("Cryptoki Session EncryptInit error: " + err);
            return _Session.Encrypt(text);
        }
    }
}
