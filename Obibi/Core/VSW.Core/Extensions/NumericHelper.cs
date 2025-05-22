using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace VSW.Core
{
    public static class NumericHelper
    {
        public static decimal Round(this decimal number, int decimalDigits = 0, MidpointRounding midpoint = MidpointRounding.AwayFromZero)
        {
            return Math.Round(number, decimalDigits, midpoint);
        }

        public static double Round(this double number, int decimalDigits = 0, MidpointRounding midpoint = MidpointRounding.AwayFromZero)
        {
            return Math.Round(number, decimalDigits, midpoint);
        }

        public static decimal ToPercent(this decimal rate)
        {
            return rate * 100;
        }

        public static double ToPercent(this double rate)
        {
            return rate * 100;
        }

        public static double ToRate(this double percent, int decimalDigits = 0, MidpointRounding midpoint = MidpointRounding.AwayFromZero)
        {
            return (percent / 100).Round(decimalDigits, midpoint);
        }

        public static decimal ToRate(this decimal percent, int decimalDigits = 0, MidpointRounding midpoint = MidpointRounding.AwayFromZero)
        {
            return (percent / 100).Round(decimalDigits, midpoint);
        }

        public static double ToRate(this double value, double baseValue, int decimalDigits = 0, MidpointRounding midpoint = MidpointRounding.AwayFromZero)
        {
            return (value / baseValue).Round(decimalDigits, midpoint);
        }

        public static decimal ToRate(this decimal value, decimal baseValue, int decimalDigits = 0, MidpointRounding midpoint = MidpointRounding.AwayFromZero)
        {
            return (value / baseValue).Round(decimalDigits, midpoint);
        }

        public static int Floor(this int left, int right)
        {
            return left / right;
        }

        public static int Ceiling(this int left, int right)
        {
            var decLeft = Convert.ToDecimal(left);
            var decRight = Convert.ToDecimal(right);
            return Convert.ToInt32(Math.Ceiling(decLeft / decRight));
        }

        public static string FormatString(int decimalDigits = 0)
        {
            if(decimalDigits >= 0)
                return "N" + decimalDigits.ToString();

            return "N";
        }

        public static string ToFormattedString(this int number, CultureInfo culture)
        {
            var format = FormatString(0);
            return number.ToString(format, culture);
        }

        public static string ToFormattedString(this decimal number, int decimalDigits = 0, CultureCode cultureCode = CultureCode.vi_VN)
        {
            var culture = CultureHelper.Get(cultureCode);
            var format = FormatString(decimalDigits);
            return number.ToString(format, culture);
        }

        public static string ToFormattedString(this double number, int decimalDigits = 0, CultureCode cultureCode = CultureCode.vi_VN)
        {
            var culture = CultureHelper.Get(cultureCode);
            var format = FormatString(decimalDigits);
            return number.ToString(format, culture);
        }

        public static string ToFormattedString(this int number, CultureCode cultureCode = CultureCode.vi_VN)
        {
            var culture = CultureHelper.Get(cultureCode);
            var format = FormatString(0);
            return number.ToString(format, culture);
        }

        #region Đọc số tiếng Việt
        public static string ToVNString(this double value)
        {
            return ReadNumber(Convert.ToInt64(value));
        }

        public static string ToVNString(this decimal value)
        {
            return ReadNumber(Convert.ToInt64(value));
        }

        public static string ToVNString(this int value)
        {
            return ReadNumber(Convert.ToInt64(value));
        }

        public static string GetFormatString(int decimalDigit = 2)
        {
            var d = decimalDigit == 0 ? "" : ("." + "#".PadLeft(decimalDigit, '#'));
            return "{0:#,##0" + d + "}";
        }

        public static string ToFormatString(this decimal money, int decimalDigit = 2, string currencySymbol = "")
        {
            string format = GetFormatString(decimalDigit);
            var s = string.Format(CultureInfo.InvariantCulture, format, money);
            if (currencySymbol.IsNotEmpty())
            {
                s += " " + currencySymbol.ToUpper();
            }

            return s;
        }

        private static string[] numberVNToken = { "Không", "Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín" };
        private static string ReadNumber(long so)
        {
            //Int64 so = Convert.ToInt64(soInput);
            bool soAm = false;
            if (so < 0)
            {
                so = -so;
                soAm = true;
            }


            string chuoi = "";
            List<string> lstToken = new List<string>();

            if (so == 0) { return numberVNToken[0] + " đồng"; };

            string hauto = "";
            long ty;
            do
            {
                ty = so % 1000000000;
                so = so / 1000000000;
                if (so > 0)
                {
                    var lstTokenTemp = ReadMillion(ty, true);

                    if (!hauto.IsEmpty())
                        lstToken.Insert(0, hauto);

                    if (lstTokenTemp.Count > 0)
                        lstToken.InsertRange(0, lstTokenTemp);
                }
                else
                {
                    var lstTokenTemp = ReadMillion(ty, false);
                    if (!hauto.IsEmpty())
                        lstToken.Insert(0, hauto);

                    if (lstTokenTemp.Count > 0)
                        lstToken.InsertRange(0, lstTokenTemp);

                    lstToken.Add("Đồng");
                }
                hauto = "Tỷ";
            } while (so > 0);

            chuoi = lstToken.Join(" ").ToLower().Trim();

            if (chuoi.Length > 0)
            {
                string first = chuoi.Substring(0, 1);
                first = first.ToUpper();
                chuoi = first + chuoi.Substring(1);
            }

            if (soAm)
            {
                chuoi = "Âm " + chuoi;
            }
            return chuoi;
        }
        private static List<string> ReadMillion(long so, bool isHasBlockCha)
        {
            List<string> lstRs = new List<string>();
            long trieu = so / 1000000;
            so = so % 1000000;
            if (trieu > 0)
            {
                lstRs.AddRange(ReadBlock(trieu, isHasBlockCha));
                lstRs.Add("Triệu");
                isHasBlockCha = true;
            }

            long nghin = so / 1000;
            so = so % 1000;
            if (nghin > 0)
            {
                lstRs.AddRange(ReadBlock(nghin, isHasBlockCha));
                lstRs.Add("Nghìn");
                isHasBlockCha = true;
            }

            if (so > 0)
            {
                lstRs.AddRange(ReadBlock(so, isHasBlockCha));
            }
            return lstRs;
        }

        private static List<string> ReadBlock(long so, bool isHasBlockCha)
        {
            List<string> lstRs = new List<string>();
            long tram = so / 100;
            so = so % 100;

            if (tram > 0 || isHasBlockCha)
            {
                lstRs.Add(numberVNToken[tram]);
                lstRs.Add("Trăm");
            }

            lstRs.AddRange(ReadDecimalBase(so, tram > 0 || isHasBlockCha));
            return lstRs;
        }

        private static List<string> ReadDecimalBase(long so, bool isHasBlockCha)
        {
            List<string> lstRs = new List<string>();
            long chuc = so / 10;
            long donvi = so % 10;
            if (chuc > 1)
            {
                lstRs.Add(numberVNToken[chuc]);
                lstRs.Add("Mươi");
            }
            else if (chuc == 1)
            {
                lstRs.Add("Mười");
            }
            else if (chuc == 0 && donvi > 0 && isHasBlockCha)
            {
                lstRs.Add("Lẻ");
            }

            if (donvi == 5 && chuc >= 1)
            {
                lstRs.Add("Lăm");
            }
            else if (donvi == 1 && chuc > 1)
            {
                lstRs.Add("Mốt");
            }
            else if (donvi > 0)
            {
                lstRs.Add(numberVNToken[donvi]);
            }

            return lstRs;
        }
        #endregion

        #region Đọc số nước ngoài
        private static string[] numberUSDToken = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Night" };
        private static string[] numberUSD = { "Ten", "Eleven", "Twelve", "Thir", "", "fif", "", "", "Eigh", "Nigh" };
        public static string ToUSDString(this decimal value, string CurrencyCode = "")
        {
            return ReadNumberUSD(value, CurrencyCode);
        }

        private static string ReadNumberUSD(decimal number, string CurrencyCode)
        {
            bool soAm = false;
            long so = (long)Math.Truncate(number);
            if (so < 0)
            {
                so = -so;
                soAm = true;
            }

            long thapPhan = (long)((number - so) * 100);
            string chuoi = "";
            List<string> lstToken = new List<string>();
            var CurrencyName = GetCurrencyName(CurrencyCode);

            if (so == 0) 
            {
                if (thapPhan == 0)
                {
                    return $"{numberVNToken[0]} {CurrencyName}";
                }
                else
                {
                    return $"Nought {ReadDecimalUSD(thapPhan).Join(" ").ToLower().Trim()} {CurrencyName}";
                }
            };

            string hauto = "";
            long ty;
            bool IsReadThapPhan = false;
            do
            {
                ty = so % 1000000000;
                so = so / 1000000000;
                if (so > 0)
                {
                    IsReadThapPhan = true;
                    var lstTokenTemp = ReadMillionUSD(ty, thapPhan);

                    if (!hauto.IsEmpty())
                        lstToken.Insert(0, hauto);

                    if (lstTokenTemp.Count > 0)
                        lstToken.InsertRange(0, lstTokenTemp);
                }
                else
                {
                    var lstTokenTemp = ReadMillionUSD(ty, IsReadThapPhan ? 0 : thapPhan);
                    if (!hauto.IsEmpty())
                        lstToken.Insert(0, hauto);

                    if (lstTokenTemp.Count > 0)
                        lstToken.InsertRange(0, lstTokenTemp);

                    lstToken.Add($"{CurrencyName}");
                }
                hauto = "Billion";
            } while (so > 0);

            chuoi = lstToken.Join(" ").ToLower().Trim();

            if (chuoi.Length > 0)
            {
                string first = chuoi.Substring(0, 1);
                first = first.ToUpper();
                chuoi = first + chuoi.Substring(1);
            }

            if (soAm)
            {
                chuoi = "Negative " + chuoi;
            }
            return chuoi;
        }

        private static List<string> ReadMillionUSD(long so, long thapPhan = 0)
        {
            List<string> lstRs = new List<string>();
            long trieu = so / 1000000;
            so = so % 1000000;
            if (trieu > 0)
            {
                lstRs.AddRange(ReadBlockUSD(trieu));
                lstRs.Add("Million");
            }

            long nghin = so / 1000;
            so = so % 1000;
            if (nghin > 0)
            {
                lstRs.AddRange(ReadBlockUSD(nghin));
                lstRs.Add("Thousand");
            }

            if (so > 0)
            {
                lstRs.AddRange(ReadBlockUSD(so));
            }

            if (thapPhan > 0)
            {
                lstRs.AddRange(ReadDecimalUSD(thapPhan));
            }
            return lstRs;
        }

        private static List<string> ReadBlockUSD(long so)
        {
            List<string> lstRs = new List<string>();
            long tram = so / 100;
            so = so % 100;

            if (tram > 0)
            {
                lstRs.Add(numberUSDToken[tram]);
                lstRs.Add("Hundred");
            }

            var lst = ReadDecimalUSDBase(so);
            if (lst.IsNotEmpty())
            {
                if (tram > 0)
                {
                    lstRs.Add("And");
                }
                lstRs.AddRange(ReadDecimalUSDBase(so));
            }

            return lstRs;
        }

        private static List<string> ReadDecimalUSDBase(long so)
        {
            List<string> lstRs = new List<string>();
            long chuc = so / 10;
            long donvi = so % 10;

            if (chuc == 1)
            {
                if (donvi <= 2)
                {
                    lstRs.Add(numberUSD[donvi]);
                }
                else
                {
                    var number = "teen";
                    if (donvi == 3 || donvi == 5 || donvi == 8 || donvi == 9)
                    {
                        number = numberUSD[donvi] + number;
                    }
                    else
                    {
                        number = numberUSDToken[chuc] + number;
                    }
                    lstRs.Add(number);
                }
            }
            else if(chuc > 1)
            {
                var number = "ty";
                if (chuc == 2)
                {
                    number = "Twen" + number;
                }
                else if (chuc == 3 || chuc == 5 || chuc == 8 || chuc == 9)
                {
                    number = numberUSD[chuc] + number;
                }
                else
                {
                    number = numberUSDToken[chuc] + number;
                }

                lstRs.Add(number);

                if (donvi > 0)
                {
                    lstRs.Add(numberUSDToken[donvi]);
                }
            }
            else
            {
                if (donvi > 0)
                {
                    lstRs.Add(numberUSDToken[donvi]);
                }
            }

            return lstRs;
        }

        private static List<string> ReadDecimalUSD(long so)
        {
            List<string> lstRs = new List<string>();
            long chuc = so / 10;
            long donvi = so % 10;
            if (chuc == 0 && donvi == 0)
            {
                return lstRs;
            }

            lstRs.Add("Point");

            if (chuc == 0)
            {
                lstRs.Add("Oh");
                lstRs.Add(numberUSDToken[donvi]);
            }
            else
            {
                if (chuc == 1)
                {
                    if (donvi == 0)
                    {
                        lstRs.Add(numberUSDToken[chuc]);
                    }
                    else if (donvi <= 2)
                    {
                        lstRs.Add(numberUSD[donvi]);
                    }
                    else
                    {
                        var number = "teen";
                        if (donvi == 3 || donvi == 5 || donvi == 8 || donvi == 9)
                        {
                            number = numberUSD[donvi] + number;
                        }
                        else
                        {
                            number = numberUSDToken[chuc] + number;
                        }
                        lstRs.Add(number);
                    }
                }
                else if (chuc > 1)
                {
                    var number = "ty";
                    if (chuc == 2)
                    {
                        number = "Twen" + number;
                    }
                    else if (chuc == 3 || chuc == 5 || chuc == 8 || chuc == 9)
                    {
                        number = numberUSD[chuc] + number;
                    }
                    else
                    {
                        number = numberUSDToken[chuc] + number;
                    }

                    lstRs.Add(number);

                    if (donvi > 0)
                    {
                        lstRs.Add(numberUSDToken[donvi]);
                    }
                }
            }

            return lstRs;
        }
        private static string GetCurrencyName(string LoaiTien)
        {
            if (LoaiTien == "USD")
            {
                return "Dollar";
            }
            else if (LoaiTien == "EUR")
            {
                return "Euro";
            }
            else if (LoaiTien == "CHF")
            {
                return "Swiss Franc";
            }
            else if (LoaiTien == "CNY")
            {
                return "Yuan";
            }
            else if (LoaiTien == "GBP")
            {
                return "British pound";
            }
            else if (LoaiTien == "JNY")
            {
                return "Yen";
            }
            else if (LoaiTien == "MYR")
            {
                return "Ringgit";
            }
            else if (LoaiTien == "AUD")
            {
                return "Australian dollar";
            }
            else if (LoaiTien == "SGD")
            {
                return "Singaporean Dollar";
            }
            else if (LoaiTien == "CAD")
            {
                return "Canadian dollar";
            }
            else if (LoaiTien == "KRW")
            {
                return "Won";
            }

            return "Dollar";
        }
        #endregion
    }
}
