using System;
using System.Globalization;
using Convert = VSW.Core.Global.Convert;

namespace VSW.Lib.Global
{
    public class ReadPrice
    {
        private static string Chu(string gNumber)
        {
            var result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;

                case "1":
                    result = "một";
                    break;

                case "2":
                    result = "hai";
                    break;

                case "3":
                    result = "ba";
                    break;

                case "4":
                    result = "bốn";
                    break;

                case "5":
                    result = "năm";
                    break;

                case "6":
                    result = "sáu";
                    break;

                case "7":
                    result = "bảy";
                    break;

                case "8":
                    result = "tám";
                    break;

                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }

        private static string Unit(string number)
        {
            var result = "";

            if (number.Equals("1"))
                result = "";
            if (number.Equals("2"))
                result = "nghìn";
            if (number.Equals("3"))
                result = "triệu";
            if (number.Equals("4"))
                result = "tỷ";
            if (number.Equals("5"))
                result = "nghìn tỷ";
            if (number.Equals("6"))
                result = "triệu tỷ";
            if (number.Equals("7"))
                result = "tỷ tỷ";

            return result;
        }

        private static string Split(string splitPossition)
        {
            var result = "";

            if (splitPossition.Equals("000"))
                return "";

            if (splitPossition.Length == 3)
            {
                var tr = splitPossition.Trim().Substring(0, 1).Trim();
                var ch = splitPossition.Trim().Substring(1, 1).Trim();
                var dv = splitPossition.Trim().Substring(2, 1).Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    result = " không trăm lẻ " + Chu(dv.Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    result = Chu(tr.Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    result = Chu(tr.Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt(ch) > 1 && Convert.ToInt(dv) > 0 && !dv.Equals("5"))
                    result = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt(ch) > 1 && dv.Equals("0"))
                    result = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt(ch) > 1 && dv.Equals("5"))
                    result = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt(dv) > 0 && !dv.Equals("5"))
                    result = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    result = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    result = " không trăm mười lăm ";
                if (Convert.ToInt(tr) > 0 && Convert.ToInt(ch) > 1 && Convert.ToInt(dv) > 0 && !dv.Equals("5"))
                    result = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt(tr) > 0 && Convert.ToInt(ch) > 1 && dv.Equals("0"))
                    result = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt(tr) > 0 && Convert.ToInt(ch) > 1 && dv.Equals("5"))
                    result = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt(tr) > 0 && ch.Equals("1") && Convert.ToInt(dv) > 0 && !dv.Equals("5"))
                    result = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    result = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    result = Chu(tr.Trim()).Trim() + " trăm mười lăm ";
            }

            return result;
        }

        public static string ConvertNumberToText(int number)
        {
            if (number == 0)
                return "Không đồng";

            var lsoChu = "";
            var tachMod = "";
            var tachConlai = "";
            var num = Math.Round((double)number, 0);
            var gN = Convert.ToString(num);
            var m = Convert.ToInt(gN.Length / 3);
            var mod = gN.Length - m * 3;

            // Dau [+ , - ]
            if (number < 0)
                return "[-]";
            const string dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tachMod = "00" + Convert.ToString(num.ToString(CultureInfo.InvariantCulture).Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tachMod = "0" + Convert.ToString(num.ToString(CultureInfo.InvariantCulture).Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tachMod = "000";
            // Tach hang con lai sau mod :
            if (num.ToString(CultureInfo.InvariantCulture).Length > 2)
                tachConlai = Convert.ToString(num.ToString(CultureInfo.InvariantCulture).Trim().Substring(mod, num.ToString(CultureInfo.InvariantCulture).Length - mod)).Trim();

            //don vi hang mod
            var im = m + 1;
            if (mod > 0)
                lsoChu = Split(tachMod).Trim() + " " + Unit(im.ToString().Trim());
            // Tach 3 trong tach_conlai

            var i = m;
            var n = m;
            var j = 1;

            while (i > 0)
            {
                var splitPossition = tachConlai.Trim().Substring(0, 3).Trim();
                lsoChu = lsoChu.Trim() + " " + Split(splitPossition.Trim()).Trim();
                m = n + 1 - j;
                if (!splitPossition.Equals("000"))
                    lsoChu = lsoChu.Trim() + " " + Unit(m.ToString().Trim()).Trim();
                tachConlai = tachConlai.Trim().Substring(3, tachConlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lsoChu.Trim().Substring(0, 1).Equals("k"))
                lsoChu = lsoChu.Trim().Substring(10, lsoChu.Trim().Length - 10).Trim();
            if (lsoChu.Trim().Substring(0, 1).Equals("l"))
                lsoChu = lsoChu.Trim().Substring(2, lsoChu.Trim().Length - 2).Trim();
            if (lsoChu.Trim().Length > 0)
                lsoChu = dau.Trim() + " " + lsoChu.Trim().Substring(0, 1).Trim().ToUpper() + lsoChu.Trim().Substring(1, lsoChu.Trim().Length - 1).Trim() + " đồng.";

            return lsoChu.Trim();
        }
    }
}