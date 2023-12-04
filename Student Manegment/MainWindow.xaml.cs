using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Homework8_Ex0_StudentManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            Student student = GetNewStudent();
            AddToStudentsStackPanel(student);
        }

        private void AddToStudentsStackPanel(Student student)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Tag = student;
            checkBox.Content = $"{student.FirstName} {student.LastName} ({student.ClassStanding})";

            StudentsStackPanel.Children.Add(checkBox);
        }

        private Student GetNewStudent()
        {
            Student student = new Student();

            student.FirstName = FirstNameTextBox.Text;
            student.LastName = LastNameTextBox.Text;

            // Nullable value, so we use .Value property to get the actual value
            student.DateOfBirth = DateOfBirthDatePicker.SelectedDate.Value;

            student.ClassStanding = (ClassStandingComboBox.SelectedItem as ComboBoxItem).Content.ToString();

            // Address
            student.Address = AddressTextBox.Text;
            student.City = (CityComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            student.PostCode = PostCodeTextBox.Text;

            return student;
        }

        private void ImportStudentsButton_Click(object sender, RoutedEventArgs e)
        {
            // Load students from the file.
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string fileContent = File.ReadAllText(openFileDialog.FileName);
                List<Student> importedStudents = JsonConvert.DeserializeObject<List<Student>>(fileContent);

                foreach (Student student in importedStudents)
                    AddToStudentsStackPanel(student);
            }
        }

        private void ExportStudentsButton_Click(object sender, RoutedEventArgs e)
        {
            // Export the selected students where the user is asking
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                List<Student> students = GetSelectedStudents();

                if (students.Count == 0)
                {
                    MessageBox.Show("Please select at least 1 student.", "No Student Selected", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string serializedStudents = JsonConvert.SerializeObject(students);

                    File.WriteAllText(saveFileDialog.FileName, serializedStudents);
                }
            }
        }

        private List<Student> GetSelectedStudents()
        {
            List<Student> students = new List<Student>();

            foreach (CheckBox studentCheckBox in StudentsStackPanel.Children)
            {
                if (studentCheckBox.IsChecked == true)
                {
                    Student selectedStudent = studentCheckBox.Tag as Student;
                    students.Add(selectedStudent);
                }
            }

            return students;
        }
    }
}