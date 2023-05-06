using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AozoraBunko2Voiceroid2 {
	public class TextConverter {
		public string ImportFilePath { get; set; }
		public string ConvertFilePath { get; set; }
		public string ExportuserDicPath { get; set; }
		public TextConverter(string importFilePath, string convertFilePath, string exportuserDicPath) {
			ImportFilePath = importFilePath;
			ConvertFilePath = convertFilePath;
			ExportuserDicPath = exportuserDicPath;
		}

		public void Convert() {

			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			Dictionary<string, string> dic = new Dictionary<string, string>();
			var buf = new StringBuilder();
			var sjis = Encoding.GetEncoding("shift_jis");
			int headerCount = 0;
			foreach (string origin in File.ReadLines(ImportFilePath, sjis)) {
				string l = sjis.GetString(sjis.GetBytes(origin));

				// 空行スキップ
				if (l == "") {
					continue;
				}
				// ヘッダ開始/終了部スキップ
				if (l[0] == '-' && l[l.Length - 1] == '-' && l.Count(t => t.Equals('-')) == l.Length) {
					// Console.WriteLine("ヘッダ識別行検出");
					headerCount++;
					continue;
				}
				// ヘッダ部分スキップ
				if (headerCount == 1) {
					// Console.WriteLine("ヘッダ部分スキップ");
					continue;
				}
				// 漢字注釈部スキップ
				while (l.Contains("［")) {
					int startIdx = l.IndexOf("［");
					int endIdx = l.IndexOf("］");
					l = l.Remove(startIdx, endIdx - startIdx + 1);
				}
				// ルビ関連
				while (l.Contains("｜")) {
					int splitIdx = l.IndexOf("｜");
					int startIdx = l.IndexOf("《", splitIdx);
					int endIdx = l.IndexOf("》", startIdx);
					string target = l.Substring(splitIdx, endIdx - splitIdx + 1);
					string kanji = l.Substring(splitIdx + 1, startIdx - splitIdx - 1);
					string kana = l.Substring(startIdx + 1, endIdx - startIdx - 1);

					// ユーザー辞書へ追加
					dic.TryAdd(kanji, kana);

					// 漢字+ルビを漢字のみに変換
					l = l.Replace(target, kanji);
					// 漢字+ルビをルビのみに変換
					// l = l.Replace(target, kana);
				}

				// ルビ関連
				var matches = Regex.Matches(l, @"([^０-９ぁ-んァ-ヶ、。「」―\s]+)《([ぁ-んァ-ヶ]+)》");
				foreach (Match match in matches) {
					if (match.Success) {
						// ユーザー辞書へ追加
						dic.TryAdd(match.Groups[1].Value, match.Groups[2].Value);
						// 漢字+ルビを漢字のみに変換
						l = l.Replace(match.Groups[0].Value, match.Groups[1].Value);
					}
				}

				buf.AppendLine(l);
			}

			File.WriteAllText(ConvertFilePath, buf.ToString(), sjis);

			buf.Clear();
			buf.Append("# ComponentName=\"AITalk SDK\" ComponentVersion=\"1.0.0.1\" UpdateDateTime=\"");
			buf.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
			buf.Append("\" Type=\"Word\" Version=\"4.1\" Language=\"Japanese\" Dialect=\"Kansai\" Count=\"");
			buf.Append(dic.Count).Append("\"");
			buf.AppendLine();

			foreach (var d in dic) {
				buf.Append("名詞-一般;");
				buf.Append(d.Key);
				buf.Append(";2000;");
				buf.Append(Microsoft.VisualBasic.Strings.StrConv(d.Value, Microsoft.VisualBasic.VbStrConv.Katakana, 0x411));
				buf.Append(";1-0-1:*");
				buf.AppendLine();
			}

			File.WriteAllText(ExportuserDicPath, buf.ToString(), sjis);
		}
	}
}
