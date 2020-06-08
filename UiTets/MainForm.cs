using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace UiTest
{
    public partial class MainForm : Form
    {
        private const string CONFIG_FILE = "./config.json";

        private EventCapture _eventCapture = new EventCapture();

        private TestRunner _testRunner = new TestRunner();

        private TreeViewRender _treeViewRender;

        private RichTextBoxConsole _mainConsole;

        private RichTextBoxConsole _infoConsole;

        private SnapShoter _snapShoter;

        private Queue<Project> _testQueue = new Queue<Project>();

        private ResultSaver _resultSaver;

        private MutipleResultSaver _mutipleResultSaver;

        private Config config;

        private bool _isMutipleTest;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            Init();

        }

        /// <summary>
        /// Initialize all components and bind events handler
        /// </summary>
        private void Init()
        {
            // read and init config
            LoadConfig();

            treeView1.AfterSelect += (x, y) =>
            {
                var project = y.Node.Tag as Project;
                SetTestInfo(project);
            };

            treeView1.AfterCheck += (x, y) =>
            {
                var node = y.Node;

                if (node.Nodes.Count > 0)
                {
                    foreach (var item in node.Nodes)
                    {
                        var subNode = item as TreeNode;
                        CheckMany(subNode, node.Checked);
                    }
                }
            };

            // capture hotkey event and handle them
            _eventCapture.Start += Start;
            _eventCapture.Stop += Stop;
            _eventCapture.Resume += Resume;
            _eventCapture.Restart += Restart;
            _eventCapture.Pause += Pause;
            _eventCapture.Position += x =>
            {
                ClipBoardUtil.SetText($"{x.X},{x.Y}");
            };

            // init test runner dependencies and event callback handlers
            _testRunner.Printer = Log;
            _testRunner.SnapShoter = SnapShot;
            _testRunner.SetClipBoard = SetClipBoard;
            _testRunner.Infoer = Info;
            _testRunner.SnapShoter = SnapShot;
            _testRunner.Finished += TestFinished;

            // init output consoles
            _mainConsole = new RichTextBoxConsole(main_box);
            _infoConsole = new RichTextBoxConsole(info_box);

            // init treeview render
            _treeViewRender = new TreeViewRender(treeView1);

            // init snapshoter
            _snapShoter = new SnapShoter(config.SnapshotDir);

            // mount project
            LoadProjects();

            // begin capture hotkey event
            _eventCapture.StartCapture();
           
        }


        private void CheckMany(TreeNode node ,bool _checked)
        {
            node.Checked = _checked;

            foreach (var item in node.Nodes)
            {
                var subNode = item as TreeNode;
                CheckMany(subNode, _checked);
            }
        }

        private void LoadConfig()
        {
            if (!File.Exists(CONFIG_FILE))
            {
                File.WriteAllText(CONFIG_FILE, "{}");
            }

            // read config file
            try
            {
                config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText(CONFIG_FILE));
            }
            catch(Exception ex)
            {
                _infoConsole.Error($"incorrect config file\r\n{ex}");
                config = new Config();
            }

            // set default config if absent or incorrect path
            if (config.WorkDir == null||!Directory.Exists(config.WorkDir))
            {
                Directory.CreateDirectory("./project");
                config.WorkDir = "./project";
                UpdateConfig(config);
            }

            if (config.SnapshotDir == null||!Directory.Exists(config.SnapshotDir))
            {
                Directory.CreateDirectory("./snapshot");
                config.SnapshotDir = "./snapshot";
                UpdateConfig(config);
            }

            if (config.OutputDir == null||!Directory.Exists(config.OutputDir))
            {
                Directory.CreateDirectory("./output");
                config.OutputDir = "./output";
                UpdateConfig(config);
            }
        }

        /// <summary>
        /// Update config
        /// </summary>
        /// <param name="config"></param>
        private void UpdateConfig(Config config)
        {
            File.WriteAllText(CONFIG_FILE, Newtonsoft.Json.JsonConvert.SerializeObject(config));
        }

        /// <summary>
        ///  Run test
        /// </summary>
        private void Start()
        {
            Trace.Write("begin");
            if (_testRunner.IsRunning)
            {
                _infoConsole.Warning("Test still running ,stop it and retry");
            }

            _testQueue.Clear();

            var tests = GetTests();
            foreach (var item in tests)
            {
                _testQueue.Enqueue(item);
            }

            _isMutipleTest = _testQueue.Count > 1;

            Run();

        }

        /// <summary>
        /// Test runner finished callback
        /// </summary>
        /// <param name="project"></param>
        private void TestFinished(string project)
        {
            //_infoConsole.Info($"{project} test finished");
            //var file = _resultSaver.Generate();

            //if (_isMutipleTest)
            //{
            //    _mutipleResultSaver.Add(project, file);
            //    if (_testQueue.Count == 0)
            //        ShowResult(_mutipleResultSaver.Generate());
            //}
            //else
            //{
            //    ShowResult(file);
            //}

            //_resultSaver.Clear();

        }

        /// <summary>
        /// Run test
        /// </summary>
        private void Run()
        {
            if (_testQueue.Count > 0)
            {
                _mainConsole.Clear();

                var project = _testQueue.Dequeue();
                SetTestInfo(project);
                ChangeTest(project.Path);
                _mainConsole.Info($"{project.Name} test started");
                _testRunner.Run(project);
            }
        }

        /// <summary>
        /// Update test info on ui
        /// </summary>
        /// <param name="project"></param>
        private void SetTestInfo(Project project)
        {
            lb_test_title.Text = project.Name;
            lb_test_description.Text = project.Description??"";

        }

        /// <summary>
        /// Stop test, menu click or hotkey callback
        /// </summary>
        private void Stop()
        {
            if (_testRunner.IsRunning)
            {
                _testRunner.Stop();
                _mainConsole.Info($"test finished");
            }
        }

        /// <summary>
        /// Resume test, menu click or hotkey callback
        /// </summary>
        private void Resume()
        {
            if (_testRunner.IsRunning)
            {
                _testRunner.Resume();
                _mainConsole.Info($"test resumed");
            }
        }

        /// <summary>
        /// Pause test ,menu click , hotkey callback
        /// </summary>
        private void Pause()
        {
            if (_testRunner.IsRunning)
            {
                _testRunner.Pause();
                _mainConsole.Info($"test paused");
            }
        }

        /// <summary>
        /// Restart test, menu click , hotkey callback
        /// </summary>
        private void Restart()
        {
            _infoConsole.Info("restarting ");
            Stop();
            Start();
            _infoConsole.Info("restarted ");
        }

        /// <summary>
        /// Reload project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_OpenClick(object sender, System.EventArgs e)
        {
            if (_testRunner.IsRunning)
            {
                _infoConsole.Warning("test is still running,stop it and retry");
                return;
            }

            FolderBrowserDialog dl = new FolderBrowserDialog();
            if (dl.ShowDialog() == DialogResult.OK)
            {
                UpdateWorkDir(dl.SelectedPath);
                ReloadProject();
            }
        }

        private void ReloadProject()
        {
            _infoConsole.Info("reloading project");
            _treeViewRender.Clear();
            LoadProjects();
            _infoConsole.Info("reloaded");
        }

        /// <summary>
        /// Update workdir
        /// </summary>
        /// <param name="dir"></param>
        private void UpdateWorkDir(string dir)
        {
            config.WorkDir = dir;
            UpdateConfig(config);
        }

        /// <summary>
        /// Test runner log event handler
        /// </summary>
        /// <param name="msg"></param>
        private void Log(string msg)
        {
             _mainConsole.Log(msg);
        }

        /// <summary>
        /// Test runner info event handler
        /// </summary>
        /// <param name="msg"></param>
        private void Info(string msg)
        {
          _mainConsole.Info(msg);
        }

        /// <summary>
        /// Test runner clipboard event handler
        /// </summary>
        /// <param name="msg"></param>
        private void SetClipBoard(string msg)
        {
            this.InvokeAfterHandleCreated(() => ClipBoardUtil.SetText(msg));

        }

        /// <summary>
        /// Test runner snapshot event handler
        /// </summary>
        /// <param name="description"></param>
        private void SnapShot(string description)
        {
            var file = _snapShoter.SnapShot();
            Info($"snapshot {description}\r\n save to file {file} ");
          //  _resultSaver.Add(file, description);
        }

        /// <summary>
        /// Update treeview selected node when test item changed,
        /// project path is the key of treenode
        /// </summary>
        /// <param name="path"></param>
        private void ChangeTest(string path)
        {
            treeView1.SelectedImageKey = path;
        }

        /// <summary>
        /// Read all project file and render treeview
        /// </summary>
        private void LoadProjects()
        {
            var workDir = config.WorkDir;

            var project = new Project();
            project.SubProjects = new List<Project>();
            project.Path = workDir;
            LoadProjectCore(workDir, project);
            _treeViewRender.Render(ConvertToTree(project));
        }

        /// <summary>
        /// Recursively load project file
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="project"></param>
        private void LoadProjectCore(string dir, Project project)
        {
            if (!Directory.Exists(dir))
            {
                return;
            }

            var folder = new DirectoryInfo(dir);

            foreach (var file in folder.GetFiles())
            {
                LoadSubProject(file.FullName, project);
            }

            foreach (var item in folder.GetDirectories())
            {
                var subProject = new Project();
                subProject.Path = item.FullName;
                subProject.Name = item.Name;
                subProject.SubProjects = new List<Project>();
                project.SubProjects.Add(subProject);
                LoadProjectCore(item.FullName, subProject);
            }

        }

        /// <summary>
        /// Load subProject
        /// </summary>
        /// <param name="file"></param>
        /// <param name="project"></param>
        private void LoadSubProject(string file, Project project)
        {
            try
            {
                var subProject = Newtonsoft.Json.JsonConvert.DeserializeObject<Project>(File.ReadAllText(file));
                subProject.Path = file;
                project.SubProjects.Add(subProject);
            }
            catch (Exception ex)
            {
                _infoConsole.Error($"load file {file} failed\r\n {ex}");
            }
        }

        /// <summary>
        /// Convert project to tree model
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        private Tree ConvertToTree(Project project)
        {
            var tree = new Tree();
            ConvertToTreeCore(tree, project);
            return tree;
        }

        /// <summary>
        /// Recursively convert
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="project"></param>
        private void ConvertToTreeCore(Tree tree, Project project)
        {
            tree.Text = project.Name;
            tree.Data = project;
            tree.Key = project.Path;
            if (project.SubProjects != null)
            {
                foreach (var item in project.SubProjects)
                {
                    var subTree = new Tree();
                    ConvertToTreeCore(subTree, item);
                    tree.Children.Add(subTree);
                }
            }

        }

        /// <summary>
        /// Get all tests which treenode is checked
        /// </summary>
        /// <returns></returns>
        private List<Project> GetTests()
        {
            List<TreeNode> nodes = _treeViewRender.GetCheckedNodes();
            var projects = new List<Project>();
            foreach (var item in nodes)
            {
                if (item.Tag != null)
                {
                    projects.Add(item.Tag as Project);
                }
            }

            return projects;
        }



        /// <summary>
        /// Open browser to display result of test which render with html
        /// </summary>
        /// <param name="file"></param>
        private void ShowResult(string file)
        {
            try
            {
                Process.Start(file);
            }
            catch(Exception ex)
            {
                _infoConsole.Error($"open file '{file}' failed\r\n{ex}");
            }
        }
    }
}
