using System;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace RevitPlugin
{
    // 簡易介面，用於設定物件接合順序並啟動接合
    public class JoinObjectsForm : Form
    {
        private readonly UIDocument _uidoc;
        private ListBox _orderList;
        private TextBox _newItemText;
        private ListView _resultView;

        public JoinObjectsForm(UIDocument uidoc)
        {
            _uidoc = uidoc;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "物件接合";
            Width = 360;
            Height = 480;

            _orderList = new ListBox { Top = 10, Left = 10, Width = 200, Height = 150 };
            _orderList.Items.AddRange(new object[] { "柱", "梁", "版", "牆" });

            _newItemText = new TextBox { Top = 170, Left = 10, Width = 120 };
            var addButton = new Button { Text = "新增", Top = 170, Left = 140, Width = 60 };
            addButton.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(_newItemText.Text))
                {
                    _orderList.Items.Add(_newItemText.Text.Trim());
                    _newItemText.Clear();
                }
            };

            var upButton = new Button { Text = "上移", Top = 200, Left = 10, Width = 60 };
            upButton.Click += (s, e) =>
            {
                int i = _orderList.SelectedIndex;
                if (i > 0)
                {
                    var item = _orderList.Items[i];
                    _orderList.Items.RemoveAt(i);
                    _orderList.Items.Insert(i - 1, item);
                    _orderList.SelectedIndex = i - 1;
                }
            };

            var downButton = new Button { Text = "下移", Top = 200, Left = 80, Width = 60 };
            downButton.Click += (s, e) =>
            {
                int i = _orderList.SelectedIndex;
                if (i >= 0 && i < _orderList.Items.Count - 1)
                {
                    var item = _orderList.Items[i];
                    _orderList.Items.RemoveAt(i);
                    _orderList.Items.Insert(i + 1, item);
                    _orderList.SelectedIndex = i + 1;
                }
            };

            var removeButton = new Button { Text = "移除", Top = 200, Left = 150, Width = 60 };
            removeButton.Click += (s, e) =>
            {
                int i = _orderList.SelectedIndex;
                if (i >= 0)
                    _orderList.Items.RemoveAt(i);
            };

            var startButton = new Button { Text = "開始接合", Top = 240, Left = 10, Width = 100 };
            startButton.Click += StartJoin;

            var cancelButton = new Button { Text = "取消", Top = 240, Left = 120, Width = 60 };
            cancelButton.Click += (s, e) => Close();

            _resultView = new ListView { Top = 280, Left = 10, Width = 320, Height = 150, Visible = false };
            _resultView.View = View.Details;
            _resultView.FullRowSelect = true;
            _resultView.Columns.Add("名稱", 200);
            _resultView.Columns.Add("Id", 80);
            _resultView.DoubleClick += (s, e) =>
            {
                if (_resultView.SelectedItems.Count > 0)
                {
                    if (int.TryParse(_resultView.SelectedItems[0].SubItems[1].Text, out int id))
                    {
                        var el = _uidoc.Document.GetElement(new ElementId(id));
                        if (el != null)
                            _uidoc.ShowElements(el.Id);
                    }
                }
            };

            Controls.AddRange(new Control[]
            {
                _orderList, _newItemText, addButton,
                upButton, downButton, removeButton,
                startButton, cancelButton, _resultView
            });
        }

        // 示意性接合實作，僅顯示所設定的順序並列出未接合元素
        private void StartJoin(object sender, EventArgs e)
        {
            // 真正的接合邏輯因情境而異，此處僅示範流程
            string order = string.Join(" -> ", _orderList.Items.Cast<string>());
            TaskDialog.Show("接合順序", order);

            // 產生示例未接合項目，可依實際情況替換
            _resultView.Items.Clear();
            var collector = new FilteredElementCollector(_uidoc.Document)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Wall))
                .Take(3);
            foreach (var el in collector)
            {
                var item = new ListViewItem(new[] { el.Name, el.Id.IntegerValue.ToString() });
                _resultView.Items.Add(item);
            }
            _resultView.Visible = true;
        }
    }
}
