using System;
using System.Linq;
using System.Windows.Forms;
using UI = Autodesk.Revit.UI;
using DB = Autodesk.Revit.DB;

namespace RevitPlugin
{
    // 簡易介面，用於設定建築接合順序並啟動接合
    public class JoinObjectsForm : Form
    {
        private readonly UI.UIDocument _uidoc;
        private ListBox _orderList;
        private TextBox _newItemText;
        private ListView _resultView;

        public JoinObjectsForm(UI.UIDocument uidoc)
        {
            _uidoc = uidoc;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "建築接合";
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
                        var el = _uidoc.Document.GetElement(new DB.ElementId(id));
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

        // 依設定順序執行接合，僅列出無法接合的元素
        private void StartJoin(object sender, EventArgs e)
        {
            var order = _orderList.Items.Cast<string>().ToList();
            string sequence = string.Join(" -> ", order);
            var result = UI.TaskDialog.Show(
                "確認接合順序",
                $"接合順序：{sequence}\n是否開始接合？",
                UI.TaskDialogCommonButtons.Yes | UI.TaskDialogCommonButtons.No);
            if (result != UI.TaskDialogResult.Yes)
                return;

            _resultView.Items.Clear();
            var doc = _uidoc.Document;
            using (var t = new DB.Transaction(doc, "Join Elements"))
            {
                t.Start();
                for (int i = 0; i < order.Count - 1; i++)
                {
                    var cat1 = GetCategory(order[i]);
                    var cat2 = GetCategory(order[i + 1]);
                    if (cat1 == null || cat2 == null)
                        continue;

                    var first = new DB.FilteredElementCollector(doc)
                        .WhereElementIsNotElementType()
                        .OfCategory(cat1.Value)
                        .FirstOrDefault();
                    var seconds = new DB.FilteredElementCollector(doc)
                        .WhereElementIsNotElementType()
                        .OfCategory(cat2.Value)
                        .ToList();

                    if (first == null)
                        continue;

                    foreach (var s in seconds)
                    {
                        try
                        {
                            if (!DB.JoinGeometryUtils.AreElementsJoined(doc, first, s))
                                DB.JoinGeometryUtils.JoinGeometry(doc, first, s);
                        }
                        catch
                        {
                            var item = new ListViewItem(new[] { s.Name, s.Id.IntegerValue.ToString() });
                            _resultView.Items.Add(item);
                        }
                    }
                }
                t.Commit();
            }

            if (_resultView.Items.Count == 0)
            {
                UI.TaskDialog.Show("建築接合", "全部接合完成");
            }
            _resultView.Visible = _resultView.Items.Count > 0;
        }

        private static DB.BuiltInCategory? GetCategory(string name)
        {
            return name switch
            {
                "柱" => DB.BuiltInCategory.OST_StructuralColumns,
                "梁" => DB.BuiltInCategory.OST_StructuralFraming,
                "牆" => DB.BuiltInCategory.OST_Walls,
                "板" => DB.BuiltInCategory.OST_Floors,
                _ => null
            };
        }
    }
}