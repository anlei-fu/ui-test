using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace UiTets
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Serialie event record
        /// </summary>
        private Serializer _serializer = new Serializer();

        /// <summary>
        /// Projects
        /// </summary>
        private List<Project> _projects=new List<Project>();

        /// <summary>
        /// Event capture
        /// </summary>
        private EventCapture _eventCapture = new EventCapture();

        /// <summary>
        /// Test runner
        /// </summary>
        private TestRunner _testRunner = new TestRunner();

        /// <summary>
        /// New project form
        /// </summary>
        private NewProjectForm _newProjectForm = new NewProjectForm();

        /// <summary>
        /// New test form
        /// </summary>
        private NewTestForm _newTestForm = new NewTestForm();

        /// <summary>
        /// Use to edit a project or a test
        /// </summary>
        private EditForm _editForm = new EditForm();

        /// <summary>
        /// Use to view a project or a test
        /// </summary>
        private ViewTetsForm _viewForm = new ViewTetsForm();

        /// <summary>
        /// Use to rename a project or a test
        /// </summary>
        private ReNameForm _renameForm = new ReNameForm();

        /// <summary>
        /// Is capturing
        /// </summary>
        private bool _isCapturing => _eventCapture.IsCapturing;

        /// <summary>
        /// Is running a test
        /// </summary>
        private bool _isRunning => _testRunner.IsRunning;

        /// <summary>
        /// Record current project tree node
        /// </summary>
        private TreeNode _currentProjectNode;

        /// <summary>
        /// Record current test tree node
        /// </summary>
        private TreeNode _currentTestNode;

        /// <summary>
        ///  Render record in code box
        /// </summary>
        private RichTextBoxRender _richTextBoxRender;

        /// <summary>
        /// Use to record keystatus
        /// </summary>
        private EventRecorder _eventRecorder=new EventRecorder();

        /// <summary>
        /// Use to mark is recording
        /// </summary>
        private bool _isRecording;


        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            Init();
        }

        /// <summary>
        /// Do initialize
        /// </summary>
        private void Init()
        {
            Directory.CreateDirectory("./output");

            // load projects
            if (File.Exists("./output/all.project"))
            {
                var projectFile = File.ReadAllText("./output/all.project");
                _projects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Project>>(projectFile);

                // build tree
                foreach (var project in _projects)
                {
                    var node = new TreeNode()
                    {
                        Text = project.Name,
                        Tag = project,
                    };

                    foreach (var test in project.Tests)
                    {
                        var testNode = new TreeNode()
                        {
                            Text = test.Name,
                            Tag = test,
                        };
                        node.Nodes.Add(testNode);
                    }

                    project_tree.Nodes.Add(node);
                }
            }

            WinformUtils.SetLineHeight(code_box, 22);
            _richTextBoxRender = new RichTextBoxRender(code_box);

            _eventCapture.OnRecord += x =>
            {

                if (_currentTestNode != null&&_isRecording)
                    AppendRecord(x);

                _eventRecorder.AddEvent(x);

                if (x.EventType == EventType.KeyDown)
                {
                    if (_eventRecorder.IsKeyDown(Keys.Control)
                       && _eventRecorder.IsKeyDown(Keys.Shift)
                       && _eventRecorder.IsKeyDown(Keys.S)
                       && !_isRecording)
                    {
                        _isRecording = true;
                    }else if(_eventRecorder.IsKeyDown(Keys.LControlKey)
                       && _eventRecorder.IsKeyDown(Keys.Shift)
                       && _eventRecorder.IsKeyDown(Keys.E)
                       && _isRecording)
                    {
                        _isRecording = false;
                    }
                }
            };

            // callback biding 
            _newTestForm.DoCreateTest = CreateNewTest;
            _newProjectForm.DoCreateProject = CreateNewProject;
            _editForm.DoEditProject = EditProject;
            _editForm.DoEditTest = EditTest;
            _renameForm.OnNewName =Rename;

            _eventCapture.StartCapture();

            // size changed
            splitContainer2.Panel1.SizeChanged += (x, y) =>
            {
                project_tree.Size = new Size(splitContainer2.Panel1.Width-10,splitContainer2.Height-project_tree.Location.Y);
            };

            splitContainer2.Panel2.SizeChanged += (x, y) =>
            {
                code_box.Size = new Size(splitContainer2.Panel2.Width - 45, splitContainer2.Panel2.Height - code_box.Location.Y);
            };
        }

        /// <summary>
        /// Render a test
        /// </summary>
        /// <param name="test"></param>
        private void Render(Test test)
        {
            code_box.Clear();
            code_box.Rtf = test.Rtf;
        }

        /// <summary>
        /// Append new event record into code box
        /// </summary>
        /// <param name="record"></param>
        private void AppendRecord(EventRecord record)
        {
            code_box.AppendText(_serializer.Serialize(record));
        }

        /// <summary>
        /// Create a new project, and add into project tree
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        private bool CreateNewProject(string projectName,string description)
        {
            foreach (var item in _projects)
            {
                if (item.Name == projectName)
                    return false;
            }

            var project = new Project()
            {
                Name = projectName,
                Description=description,
                Folder=$"./output/{projectName}",
            };

            _projects.Add(project);
            Directory.CreateDirectory($"./output/{projectName}");

            var node = new TreeNode()
            {
                Text=projectName,
                Tag=project
            };

            project_tree.Nodes.Add(node);

            return true;
        }

        /// <summary>
        /// Create a new test ,and add its' node into project tree
        /// </summary>
        /// <param name="testName"></param>
        /// <param name="page"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        private bool CreateNewTest(string testName,string page,string description)
        {
            var project = _currentProjectNode.Tag as Project;

            foreach (var item in project.Tests)
            {
                if (item.Name == testName)
                    return false;
            }

            var test = new Test()
            {
                Name = testName,
                Description = description,
                Page = page,
                Path=$"./output/{project.Name}/{testName}.rtf"
            };

            project.Tests.Add(test);

            File.WriteAllText(test.Path, "");

            var node = new TreeNode()
            {
                Tag = test,
                Text = testName,
            };

            _currentProjectNode.Nodes.Add(node);

            return true;
        }

        /// <summary>
        /// Start to record event
        /// </summary>
        private void StartCapture()
        {
            if (_currentTestNode == null)
            {
                MessageBox.Show("Select a test first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _eventCapture.StopCapture();
        }

        /// <summary>
        /// Stop record event
        /// </summary>
        private void StopCapture()
        {
            _eventCapture.StartCapture();
        }

        /// <summary>
        /// Delete a project and remove from project tree
        /// </summary>
        private void DeleteProject()
        {
            project_tree.Nodes.Remove(_currentProjectNode);
            var project = _currentProjectNode.Tag as Project;
            _projects.Remove(project);
            _currentProjectNode = null;
            Directory.Delete(project.Folder);
        }

        /// <summary>
        /// Delete a test and remove from project tree
        /// </summary>
        private void DeleteTest()
        {
            _currentTestNode.Parent.Nodes.Remove(_currentTestNode);
            var test = _currentTestNode.Tag as Test;
            File.Delete(test.Path);
            var project = _currentProjectNode.Tag as Project;
            project.Tests.Remove(test);
            code_box.Clear();
        }

        /// <summary>
        /// Edit project properties
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        private void EditProject(string name,string description)
        {
            _currentProjectNode.Text = name;
            var project = _currentProjectNode.Tag as Project;
            project.Name = name;
            project.Description = description;
        }

        /// <summary>
        /// Edit test properties
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="description"></param>
        private void EditTest(string name, string page, string description)
        {
            _currentTestNode.Text = name;
            var test = _currentTestNode.Tag as Test;
            test.Description = description;
            test.Name = name;
            test.Page = page;
        }
        /// <summary>
        /// Rename project
        /// </summary>
        /// <param name="newName"></param>
        private void Rename(string newName)
        {
            if(project_tree.SelectedNode.Tag is Test test)
            {
                test.Name = newName;
                _currentTestNode.Text = newName;
            }
            else
            {
                var project = _currentProjectNode.Tag as Project;
                project.Name = newName;
                _currentProjectNode.Text = newName;
            }
        }

        /// <summary>
        /// Run test
        /// </summary>
        private void Test()
        {

        }

        /// <summary>
        /// Stop test
        /// </summary>
        private void StopTest()
        {

        }

        /// <summary>
        /// Output some into output-box
        /// </summary>
        private void Log(string msg)
        {
            output_box.Text += msg + "\r\n";
        }

        /// <summary>
        /// Do something when app close
        /// </summary>
        private void CloseCore()
        {

        }

        /// <summary>
        /// Menu new project callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_NewProject(object sender, System.EventArgs e)
        {
            _newProjectForm.ShowDialog();
        }

        /// <summary>
        /// Menu save callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Save(object sender, System.EventArgs e)
        {
            
        }

        /// <summary>
        /// Menu rename callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Rename(object sender, System.EventArgs e)
        {
            _renameForm.ShowDialog();
        }

        /// <summary>
        /// Menu view callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_View(object sender, System.EventArgs e)
        {
            if (project_tree.SelectedNode.Tag is Test test)
            {
                _viewForm.ViewTest(test.Name, test.Page, test.Description);
            }
            else
            {
                var project = project_tree.SelectedNode.Tag as Project;
                _viewForm.ViewProject(project.Name, project.Description);
            }
        }

        /// <summary>
        /// Menu edit callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Edit(object sender, System.EventArgs e)
        {
            if (project_tree.SelectedNode.Tag is Test test)
            {
                _editForm.EditTest(test.Name, test.Page, test.Description);
            }
            else
            {
                var project = project_tree.SelectedNode.Tag as Project;
                _editForm.EditProject(project.Name, project.Description);
            }
        }

        /// <summary>
        /// Menu delete callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Delete(object sender, System.EventArgs e)
        {
            if(project_tree.SelectedNode.Tag is Test)
            {
                DeleteTest();
            }
            else
            {
                DeleteProject();
            }
        }

        /// <summary>
        /// Menu new test callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_NewTest(object sender, System.EventArgs e)
        {
            _newTestForm.ShowDialog();
        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void project_tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (project_tree.SelectedNode.Tag is Test test)
            {
                if (test.Rtf == null)
                    test.Rtf = File.ReadAllText(test.Path);

                Render(test);
                _currentTestNode = project_tree.SelectedNode;
                context_menu.Items[0].Visible = false;
            }
            else
            {
                _currentProjectNode = project_tree.SelectedNode;
                context_menu.Items[0].Visible = true;
            }
        }

        private void project_tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (project_tree.SelectedNode != null && project_tree.SelectedNode.Tag is Test test)
            {
                test.Rtf = code_box.Rtf;
            }
        }
    }
}
