using System;
using System.Linq;
using System.Windows.Forms;
using UI = Autodesk.Revit.UI;
using DB = Autodesk.Revit.DB;

namespace RevitPlugin
{
    // 簡易介面，用於設定構件接合順序並啟動接合
    public class JoinObjectsForm : Form
    {
        private readonly UI.UIDocument _uidoc;
        private ListBox _orderList;
        private ComboBox _categoryCombo;
        private ListView _resultView;
        private System.Collections.Generic.Dictionary<string, DB.BuiltInCategory> _categoryMap;

        public JoinObjectsForm(UI.UIDocument uidoc)
        {
            _uidoc = uidoc;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "構件接合";
            Width = 380;
            Height = 260;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            _categoryMap = new System.Collections.Generic.Dictionary<string, DB.BuiltInCategory>();
            foreach (DB.Category cat in _uidoc.Document.Settings.Categories)
            {
                int id = cat.Id.IntegerValue;
                if (Enum.IsDefined(typeof(DB.BuiltInCategory), id))
                {
                    var bic = (DB.BuiltInCategory)id;
                    if (!_categoryMap.ContainsKey(cat.Name))
                        _categoryMap.Add(cat.Name, bic);
                }
            }

            _orderList = new ListBox { Top = 10, Left = 10, Width = 200, Height = 200 };
            _orderList.Items.AddRange(new object[] { "結構柱", "結構構件", "牆", "樓板" });

            _categoryCombo = new ComboBox { Top = 10, Left = 200, Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            _categoryCombo.Items.AddRange(_categoryMap.Keys.OrderBy(k => k).ToArray());
            var addButton = new Button { Text = "新增", Top = 40, Left = 200, Width = 60 };
            addButton.Click += (s, e) =>
            {
                var name = _categoryCombo.SelectedItem as string;
                if (!string.IsNullOrEmpty(name) && !_orderList.Items.Contains(name))
                    _orderList.Items.Add(name);
            };

            var upButton = new Button { Text = "上移", Top = 70, Left = 200, Width = 60 };
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

            var downButton = new Button { Text = "下移", Top = 100, Left = 200, Width = 60 };
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

            var removeButton = new Button { Text = "刪除", Top = 130, Left = 200, Width = 60 };
            removeButton.Click += (s, e) =>
            {
                int i = _orderList.SelectedIndex;
                if (i >= 0)
                    _orderList.Items.RemoveAt(i);
            };

            var startButton = new Button { Text = "進行接合", Top = 200, Left = 200, Width = 80 };
            startButton.Click += StartJoin;

            var cancelButton = new Button { Text = "取消接合", Top = 200, Left = 290, Width = 80 };
            cancelButton.Click += (s, e) => Close();

            _resultView = new ListView { Visible = false };
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
                _orderList, _categoryCombo, addButton,
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

            string message = _resultView.Items.Count == 0
                ? "全部接合完成"
                : $"有{_resultView.Items.Count}個元素無法接合";
            UI.TaskDialog.Show("構件接合", message);
            Close();
        }

        private DB.BuiltInCategory? GetCategory(string name)
        {
            if (_categoryMap.TryGetValue(name, out var bic))
                return bic;
            return null;
        }
    }
}