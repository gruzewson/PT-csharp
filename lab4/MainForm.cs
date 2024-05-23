using System;
using System.ComponentModel;
using System.Linq;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace lab4
{
    public partial class MainForm : Form
    {
        private DataGridView dataGridView;
        private Button addButton;
        private Button deleteButton;
        private Button editButton;
        private Panel buttonPanel;
        private ToolStrip toolStrip;
        private ToolStripComboBox propertyComboBox;
        private ToolStripTextBox searchTextBox;
        private ToolStripButton searchButton;
        private SortableBindingList<Car> sortableCars;

        public MainForm()
        {
            InitializeControls();
            SetupDataGridView();
            SetupLayout();
            SetupEvents();
            SetupSortableBindingList();
        }

        private void InitializeControls()
        {
            dataGridView = new DataGridView
            {
                Dock = DockStyle.Top,
                AutoGenerateColumns = true,
                Height = 400,
                AllowUserToAddRows = false
            };

            addButton = new Button { Text = "Add Car", Dock = DockStyle.Left };
            deleteButton = new Button { Text = "Delete Car", Dock = DockStyle.Left };
            editButton = new Button { Text = "Edit Car", Dock = DockStyle.Left };

            buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40
            };

            buttonPanel.Controls.Add(addButton);
            buttonPanel.Controls.Add(deleteButton);
            buttonPanel.Controls.Add(editButton);

            toolStrip = new ToolStrip();
            propertyComboBox = new ToolStripComboBox();
            searchTextBox = new ToolStripTextBox();
            searchButton = new ToolStripButton("Search");

            toolStrip.Items.Add(propertyComboBox);
            toolStrip.Items.Add(searchTextBox);
            toolStrip.Items.Add(searchButton);
        }

        private void SetupDataGridView()
        {
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ReadOnly = true;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.MultiSelect = false;
            dataGridView.ColumnHeaderMouseClick += DataGridView_ColumnHeaderMouseClick;
        }

        private void SetupLayout()
        {
            this.Controls.Add(dataGridView);
            this.Controls.Add(toolStrip);
            this.Controls.Add(buttonPanel);
            this.Text = "Car Collection";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = 750;
            this.Height = 600;
        }

        private void SetupEvents()
        {
            addButton.Click += AddButton_Click;
            deleteButton.Click += DeleteButton_Click;
            editButton.Click += EditButton_Click;
            propertyComboBox.Enter += PropertyComboBox_Enter;
            searchButton.Click += SearchButton_Click;
        }

        private void PropertyComboBox_Enter(object sender, EventArgs e)
        {
            propertyComboBox.Items.Clear();
            var properties = typeof(Car).GetProperties()
                .Where(p => p.PropertyType == typeof(string) || p.PropertyType == typeof(int))
                .Select(p => p.Name)
                .ToArray();
            propertyComboBox.Items.AddRange(properties);
        }

        private void SetupSortableBindingList()
        {
            sortableCars = new SortableBindingList<Car>(SharedData.myCars);
            dataGridView.DataSource = sortableCars;
        }

        private void DataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnName = dataGridView.Columns[e.ColumnIndex].DataPropertyName;
            PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(Car)).Find(columnName, true);

            if (prop != null)
            {
                ListSortDirection direction = ListSortDirection.Ascending;
                if (dataGridView.Tag is KeyValuePair<string, ListSortDirection> lastSort && lastSort.Key == columnName)
                {
                    direction = lastSort.Value == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
                }

                sortableCars.ApplySort(prop, direction);
                dataGridView.Tag = new KeyValuePair<string, ListSortDirection>(columnName, direction);
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string selectedProperty = propertyComboBox.SelectedItem?.ToString();
            string searchValue = searchTextBox.Text;

            if (string.IsNullOrEmpty(selectedProperty) || string.IsNullOrEmpty(searchValue))
            {
                MessageBox.Show("Please select a property and enter a search value.");
                return;
            }

            PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(typeof(Car)).Find(selectedProperty, true);
            if (propertyDescriptor == null)
            {
                MessageBox.Show("Invalid property selected.");
                return;
            }

            var filteredCars = SharedData.myCars.Where(car =>
            {
                var value = propertyDescriptor.GetValue(car);
                if (value == null) return false;

                if (propertyDescriptor.PropertyType == typeof(string))
                {
                    return value.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase);
                }
                if (propertyDescriptor.PropertyType == typeof(int))
                {
                    return value.ToString() == searchValue;
                }
                return false;
            }).ToList();

            dataGridView.DataSource = new SortableBindingList<Car>(filteredCars);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Car newCar = new Car("New Model", new Engine(2.0, 150, "Type"), 2024);
            newCar = EnterNewData(newCar);
            SharedData.myCars.Add(newCar);
            dataGridView.DataSource = new SortableBindingList<Car>(SharedData.myCars);
        }

        private string PromptInput(string prompt)
        {
            return Microsoft.VisualBasic.Interaction.InputBox(prompt, "Input", "");
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                SharedData.myCars.RemoveAt(dataGridView.SelectedRows[0].Index);
                dataGridView.DataSource = new SortableBindingList<Car>(SharedData.myCars);
            }
            else
            {
                MessageBox.Show("Please select a car to delete.");
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                Car selectedCar = SharedData.myCars[dataGridView.SelectedRows[0].Index];
                Car car = EnterNewData(selectedCar);
                selectedCar.Model = car.Model;
                selectedCar.Year = car.Year;
                selectedCar.Motor = car.Motor;
                dataGridView.Refresh();
            }
            else
            {
                MessageBox.Show("Please select a car to edit.");
            }
        }

        private Car EnterNewData(Car car)
        {
            string model = PromptInput("Enter the car model:");
            if (model == null) model = car.Model;

            int year;
            if (!int.TryParse(PromptInput("Enter the car year:"), out year))
            {
                MessageBox.Show("Invalid year format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                year = car.Year;
            }

            double displacement;
            if (!double.TryParse(PromptInput("Enter the engine displacement:"), out displacement))
            {
                MessageBox.Show("Invalid displacement format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                displacement = car.Motor.Displacement;
            }

            double horsepower;
            if (!double.TryParse(PromptInput("Enter the engine horsepower:"), out horsepower))
            {
                MessageBox.Show("Invalid horsepower format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                horsepower = car.Motor.HorsePower;
            }

            string engineType = PromptInput("Enter the engine type:");
            if (engineType == null) engineType = car.Motor.Model;

            Car newCar = new Car(model, new Engine(displacement, horsepower, engineType), year);
            return newCar;
        }
    }
}

