using System.Text.RegularExpressions;
using DRG.Core.Types;

namespace DRG.Core.Drg
{
    public class DrgLogic
    {
        public DrgLogic(int id, string ord, string drg, string rtc, string icd, string mdc, string pdgProp, string orprop, string procPro1, string dgCat1, string agelim, string compl, string sex, string dgprop1, string dgprop2, string dgprop3, string dgprop4, string secproc1, string disch, string dur, string locDrg)
        {
            Id = id;
            Ord = ord;
            Drg = drg;
            Rtc = rtc;
            Icd = new Icd(icd);
            Mdc = new Mdc(mdc);
            Pdg = new Pdg(pdgProp);
            OrProp = new OrProp(orprop);
            DgCat1 = new DgCat(dgCat1);
            Agelim = new Age(agelim);
            Compl = new Compl(compl);
            Sex = ParseGender(sex);
            Dgprop1 = new DgProp(dgprop1);
            Dgprop2 = new DgProp(dgprop2);
            Dgprop3 = new DgProp(dgprop3);
            Dgprop4 = new DgProp(dgprop4);
            SecPro = new SecPro(secproc1);
            Disch = new Disch(disch);
            Dur = new Duration(dur);
            LocDrg = locDrg;
            ProcPro = new ProcPro(procPro1);
        }

        private Gender ParseGender(string sex)
        {
            var gender = Gender.Null;

            if (!string.IsNullOrEmpty(sex))
            { 
                switch (sex.ToUpper())
                {
                    case "F":
                        return Gender.Female;
                    case "M":
                        return Gender.Male;
                    case "-":
                        return Gender.Minus;
                }
            }

            return gender;
        }

        public int Id { get; private set; }
        public string Ord { get; private set; }
        public string Drg { get; private set; }
        public string Rtc { get; private set; }
        public Icd Icd { get; private set; }
        public Mdc Mdc { get; private set; }
        public Pdg Pdg { get; private set; }
        public OrProp OrProp { get; private set; }
        public ProcPro ProcPro { get; private set; }
        public DgCat DgCat1 { get; private set; }
        public Age Agelim { get; private set; }
        public Compl Compl { get; private set; }
        public Gender Sex { get; private set; }
        public DgProp Dgprop1 { get; private set; }
        public DgProp Dgprop2 { get; private set; }
        public DgProp Dgprop3 { get; private set; }
        public DgProp Dgprop4 { get; private set; }
        public SecPro SecPro { get; private set; }
        public Disch Disch { get; private set; }
        public Duration Dur { get; private set; }
        public string LocDrg { get; private set; }
    }
}