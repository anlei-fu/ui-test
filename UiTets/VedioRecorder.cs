using System;
using System.Diagnostics;
using System.IO;

namespace UiTest
{
    public class VedioRecorder
    {
        private readonly object _lock = new object();

        private bool _isRunning;

        private string _outputFolder;

        private string _ffmpeg;

        public VedioRecorder(string outputFolder,string ffmpeg)
        {
            _outputFolder =new DirectoryInfo(outputFolder).FullName;
            _ffmpeg = ffmpeg;
        }
        public string StartRecord()
        {
            lock (_lock)
            {
                if (_isRunning)
                    return null;

                _isRunning = true;
            }

            var fileName = $"{_outputFolder}/{DateTime.Now:yyyyMMddhhmmss}.mp4";

            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;//不显示程序窗口
                p.Start();//启动程序
                p.StandardInput.WriteLine($"start {_ffmpeg} -f gdigrab -i desktop -f mp4 {fileName}");
            }catch
            {
                return null;
            }

            return fileName;
        }

        public void Stop()
        {
            var ps=  Process.GetProcessesByName("mmfpeg");
            if (ps.Length != 0)
            {
                try
                {
                    foreach (var item in ps)
                    {
                        item.Kill();
                    }
                }
                catch
                {

                }
            }

            _isRunning = false;
        }
    }
}
