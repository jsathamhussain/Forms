using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MyFormsApplication
{
    public partial class Form2 : Form
    {
        BindingList<Person> objPerson = new BindingList<Person>();
        Person objPersons = new Person();

        public Form2()
        {
            InitializeComponent();
            txtFirstName.Focus();
            dgDetails.RowStateChanged += new DataGridViewRowStateChangedEventHandler(dgDetails_RowStateChanged);
            objPerson.ListChanged += new ListChangedEventHandler(objPerson_ListChanged);
        }

        private void dgDetails_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected)
                return;

            if (dgDetails.SelectedRows.Count != 0)
            {
                DataGridViewRow dr = this.dgDetails.SelectedRows[0];
                string id = dr.Cells["colId"].Value.ToString();
                string fName = dr.Cells["colFName"].Value.ToString();
                string lName = dr.Cells["colLName"].Value.ToString();
                string age = dr.Cells["colAge"].Value.ToString();
                string address = dr.Cells["colAddress"].Value.ToString();

                lblID.Text = id;
                txtFirstName.Text = fName;
                txtLastName.Text = lName;
                txtAge.Text = age;
                txtAddress.Text = address;
                txtFirstName.Focus();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            BindingList<Person> lstPerson = new BindingList<Person>();

            bool isEdit = (lblID.Text.ToString() == "") ? false : true;
            int id = (lblID.Text.ToString() == "") ? 1 : Int32.Parse(lblID.Text.ToString());
            int ID = getMaxID(objPerson);

            if (id == ID)
            {
                if (isEdit == false)
                {
                    ID = id + 1;
                }
                else
                {
                    ID = id;
                }
            }
            else
            {
                if (ID <= 0)
                {
                    ID = 1;
                    lblID.Text = ID.ToString();
                }
                else
                {
                    if (isEdit == false)
                    {
                        ID++;
                    }
                    else
                    {
                        ID = id;
                    }            
                }                    
            }
            
            string FName = txtFirstName.Text.Trim();
            string LName = txtLastName.Text.Trim();
            int Age = Int32.Parse(txtAge.Text.Trim());
            string Address = txtAddress.Text.Trim();
            string status = string.Empty;
            objPersons.SaveDetails(ID, FName, LName, Age, Address, isEdit, ref objPerson, ref status);
            if (objPerson.Count > 0)
            {
                dgDetails.AutoGenerateColumns = false;
                dgDetails.DataSource = null;
                dgDetails.DataSource = objPerson;

                string successMsg = string.Empty;
                string headerMsg = "Personal Info";

                if (status == "Add")
                {
                    successMsg = "Record Added Successfully";
                }
                else if (status == "Update")
                {
                    successMsg = "Record Updated Successfully";
                }
                else
                {
                    successMsg = "Error Processing Request";
                }

                MessageBox.Show(successMsg, headerMsg);

                lblID.Text = string.Empty;
                txtFirstName.Text = string.Empty;
                txtLastName.Text = string.Empty;
                txtAge.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtFirstName.Focus();
            }
        }
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool isDeleteSuccess = false;
            string status = string.Empty;
            string successMsg = string.Empty;
            string headerMsg = "Personal Info";

            if (lblID.Text == "" || objPerson.Count  == 0)
            {
                successMsg = "No Rows To Delete";
                MessageBox.Show(successMsg, headerMsg);
                txtFirstName.Focus();
                return;
            }

            int id = Int32.Parse(lblID.Text.ToString());

            foreach (Person _objPerson in objPerson)
            {
                if (_objPerson.ID == id)
                {
                    objPersons.DeleteDetails(id, ref objPerson, ref status);
                    dgDetails.AutoGenerateColumns = false;
                    dgDetails.DataSource = null;
                    dgDetails.DataSource = (objPerson.Count > 0) ? objPerson : null;

                    if (status == "Delete")
                    {
                        successMsg = "Record Deleted Successfully";

                        isDeleteSuccess = true;
                        lblID.Text = string.Empty;
                        txtFirstName.Text = string.Empty;
                        txtLastName.Text = string.Empty;
                        txtAge.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                        MessageBox.Show(successMsg, headerMsg);
                    }
                    
                    if (objPerson.Count == 0)
                    {
                        Thread.Sleep(500);
                        successMsg = "No Rows To Display";
                        MessageBox.Show(successMsg, headerMsg);
                    }

                    return;
                }
            }

            if (isDeleteSuccess == false)
            {
                successMsg = "Record Doesn't Exist !!!";
                MessageBox.Show(successMsg, headerMsg);
            }

            txtFirstName.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lblID.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtAge.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtFirstName.Focus();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form1 obj = new Form1();
            if (obj == null)
            {
                obj.Parent = this;
            }
            obj.Show();
            this.Hide();
        }

        public void objPerson_ListChanged(object sender, ListChangedEventArgs e)
        {
            MessageBox.Show(e.ListChangedType.ToString());
        }

        int getMaxID(BindingList<Person> list)
        {
            int maxNo = int.MinValue;
            if (list.Count > 0)
            {
                foreach (Person type in list)
                {
                    if (type.ID > maxNo)
                    {
                        maxNo = type.ID;
                    }
                }
            }
            return maxNo;
        }

        class Person
        {
            public int ID { get; set; }
            public string FName { get; set; }
            public string LName { get; set; }
            public int Age { get; set; }
            public string Address { get; set; }
            public bool isEdit { get; set; }

            public void SaveDetails(int id, string fName, string lName, int age, string address, bool isedit, ref BindingList<Person> lstPerson, ref string status)
            {
                Person _objPerson = new Person();

                if (isedit == true)
                {
                    if (lstPerson.Count > 0)
                    {
                        foreach (Person per in lstPerson)
                        {
                            if (per.ID == id)
                            {
                                per.ID = id;
                                per.FName = fName;
                                per.LName = lName;
                                per.Age = age;
                                per.Address = address;
                                status = "Update";
                            }
                        }
                    }
                }
                else
                {
                    _objPerson.ID = id;
                    _objPerson.FName = fName;
                    _objPerson.LName = lName;
                    _objPerson.Age = age;
                    _objPerson.Address = address;
                    _objPerson.isEdit = isedit;
                    lstPerson.Add(_objPerson);
                    status = "Add";
                }                
            }

            public void DeleteDetails(int id, ref BindingList<Person> lstPerson, ref string status)
            {
                var itemToRemove = lstPerson.Where(x => x.ID == id).ToList();
                foreach (var item in itemToRemove)
                {
                    lstPerson.Remove(item);
                }
                status = "Delete";
            }
        }
    }
}