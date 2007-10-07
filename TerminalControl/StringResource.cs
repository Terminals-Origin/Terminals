using System;
using System.Globalization;
using System.Resources;
using System.Diagnostics;
using System.Reflection;

namespace Routrek.SSHC
{
    /// <summary>
    /// StringResource の概要の説明です。
    /// </summary>
    internal class StringResources
    {
        private string _resourceName;
        private ResourceManager _resMan;

        public StringResources(string name, Assembly asm)
        {
            _resourceName = name;
            LoadResourceManager(name, asm);
        }

        public string GetString(string id)
        {
            return _resMan.GetString(id); //もしこれが遅いようならこのクラスでキャッシュでもつくればいいだろう
        }

        private void LoadResourceManager(string name, Assembly asm)
        {
            //当面は英語・日本語しかしない
            CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentUICulture;
            //if(ci.Name.StartsWith("ja"))
            //_resMan = new ResourceManager(name+"_ja", asm);
            //else
            _resMan = new ResourceManager(name, asm);
        }
    }
}