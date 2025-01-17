﻿using System;
using System.Reflection;
using System.Diagnostics;
using Codeer.Friendly.Windows;
using Codeer.Friendly.Dynamic;
using System.Windows.Forms;
using System.Linq;

namespace VoiceroidDaemon
{
    /// <summary>
    /// VOICEROID2エディタにDLLインジェクションするクラス
    /// </summary>
    public class Injecter
    {
        /// <summary>
        /// 認証コードを取得する。
        /// VOICEROID2エディタが実行中である必要がある。
        /// </summary>
        /// <returns>認証コードのシード値</returns>
        public static string GetKey()
        {
            // VOICEROIDエディタのプロセスを検索する
            Process[] voiceroid_processes = Process.GetProcessesByName("VoiceroidEditor");
            if (voiceroid_processes.Length == 0)
            {
                return null;
            }
            Process process = voiceroid_processes[0];

            // プロセスに接続する
            WindowsAppFriend app = new WindowsAppFriend(process);
            WindowsAppExpander.LoadAssembly(app, typeof(Injecter).Assembly);
            dynamic injected_program = app.Type(typeof(Injecter));
            try
            {
                String key = injected_program.InjectedGetKey();
                // 認証コードを読み取って返す
                return key;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        
        /// <summary>
        /// 認証コードの取得のためにDLLインジェクション先で実行されるコード
        /// </summary>
        /// <returns></returns>
        private static string InjectedGetKey()
        {            
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
            {
                // if (assembly.GetName().Name == "AI.Framework.App")
                if (assembly.GetName().Name == "AI.Talk.Editor.Core"){
					Type type = assembly.GetType("AI.Talk.Editor.Settings.AppSettings");
					var property = type.GetProperty("Current");
					dynamic current = property.GetValue(type);
                    return (string)current.LicenseKey;
                }
            }
            return null;
            
        }
    }
}
