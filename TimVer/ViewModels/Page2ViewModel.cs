namespace TimVer.ViewModels
{
    class Page2ViewModel
    {
        public string MachName => GetInfo.CimQuerySys("Name");

        public string LastBoot => GetInfo.CimQueryOS("LastBootUpTime");

        public string SystemDevice => GetInfo.CimQueryOS("SystemDevice");

        public string BootDevice => GetInfo.CimQueryOS("BootDevice");

        public string Manufacturer => GetInfo.CimQuerySys("Manufacturer");

        public string Model => GetInfo.CimQuerySys("Model");

    }
}
