using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayOfWeek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            labelDayOfWeek.Text = "";
        }

        // テキストを整数値に変換するメソッド
        // (仮引数) text : 変換する文字列、val : 変換した整数値
        // (返却値) (int) val
        
        private void textToValue(string text, out int val)
        {
            if (int.TryParse(text, out val) == false)
                val = -1; // ここで-1にすることでZeller()の中で"西暦年エラー"をだしている。
        }
        

        // 閏年かどうか判定するメソッド
        // (仮引数) 西暦年
        // (戻り値) 閏年なら true, そうでなければ false
        // 「4で割り切れ、かつ100で割り切れないか、または、400で割り切れる年」は閏年
        
        private bool leapyearJudge(int year)
        {
            if (year % 400 == 0)
                return true;
            else if (year % 4 == 0 && year % 100 != 0)
                return true;

            return false;
        }

        // 年月日の妥当性チェックを行うメソッド
        // (仮引数) year：年、month : 月、day：日
        // (戻り値) 妥当ならば 空文字列, そうでなければ あり得ない日付
        
        private bool MonthandDayJudge(int year, int month, int day)
        {
            bool result = true; // 判定結果(妥当ならture, そうでなければfalse）

            switch (month)
            {
                // 1月、3月、5月、7月、8月、10月、12月は31日まで
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    if (day > 31)
                        result = true;
                    break;
                // 4月、6月、9月、11月は31日まで
                case 4:
                case 6:
                case 9:
                case 11:
                    if (day > 30)
                        result = false;
                    break;
                // 2月は28日まで、閏年ならば29日まで
                case 2:      
                    if (day == 29 && leapyearJudge(year))  // もし閏年ならば
                        result = true;
                    else if (day > 28)
                        result = false;
                    break;
                default:
                    result = true;
                    break;
            }
            return result;
        }

        // 西暦年、月、日から曜日を求めるメソッド(Zellerの公式)
        // (仮引数) year : 西暦年、month : 月、day : 日
        // (戻り値) dayofweek : 曜日（日曜=0, 月曜=1, 火曜=2,・・・）
        
        private string Zeller(int year, int month, int day)
        {
            int w;
            string dayofweek;

            if (year < 0)
                return "西暦年エラー";

            if (month == 1 || month == 2) // 1月と2月は前年の13月と14月として計算する
            {
                year -= 1;
                month += 12;
            }

            w = (5 * year / 4 - year / 100 + year / 400 + (26 * month + 16) / 10 + day) % 7; // Zellerの公式

            switch (w)
            {
                case 0:
                    dayofweek = "日曜日です";
                    break;
                case 1:
                    dayofweek = "月曜日です";
                    break;
                case 2:
                    dayofweek = "火曜日です";
                    break;
                case 3:
                    dayofweek = "水曜日です";
                    break;
                case 4:
                    dayofweek = "木曜日です";
                    break;
                case 5:
                    dayofweek = "金曜日です";
                    break;
                case 6:
                    dayofweek = "土曜日です";
                    break;
                default: // これがないと、外部でdayofweekに何も代入していない場合、
                         // 関数として空の変数を返す可能性があるから"未割り当てのローカル変数が使用される"というエラー発生
                    dayofweek = "";
                    break;
            }
            return dayofweek;
        }

        private void buttonDayOfWeekCul_Click_1(object sender, EventArgs e)
        {
            // 西暦年を整数値に変換
            int year;
            int month = (int)numericUpDownMonth.Value;
            int day = (int)numericUpDownDay.Value;

            textToValue(textBoxYear.Text, out year); // textを整数値に変換
            if (MonthandDayJudge(year, month, day))
                labelDayOfWeek.Text = Zeller(year, month, day);
            else
                labelDayOfWeek.Text = "あり得ない日付";
        }
    }
}
